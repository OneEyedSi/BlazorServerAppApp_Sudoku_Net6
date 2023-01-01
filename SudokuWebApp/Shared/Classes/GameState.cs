using classlib = SudokuClassLibrary;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SudokuWebApp.Shared.Classes;

namespace SudokuWebApp.Shared.Classes
{
    public class GameStatusChangedEventArgs : EventArgs
    {
        public GameStatusChangedEventArgs(GameStatus currentStatus)
        {
            CurrentStatus = currentStatus;
        }
        public GameStatus CurrentStatus { get; set; }
    }

    public class GameState
    {
        public event Action? RefreshRequested;
        public event EventHandler<GameStatusChangedEventArgs>? StatusChanged;

        private ILogger _logger;

        public GameState(ILogger<GameState> logger)
        {
            _logger = logger;
            _logger.LogInformation("Initializing GameState.");
            foreach (var cell in GameGrid.GetEnumerableCells())
            {
                cell.CellValueChanged += Cell_CellValueChanged;
            }
        }

        public classlib.Grid GameGrid { get; } = new();

        public classlib.History History { get; } = new();

        private Stack<GameStatus> _statusHistoryStack = new();
        private GameStatus _status = GameStatus.Initial;
        public GameStatus Status
        {
            get
            {
                _logger.LogDebug("Getting Game Status: {_status}", _status);
                return _status;
            }
            set
            {
                SetStatus(value);
            }
        }

        private void SetStatus(GameStatus newValue, bool isRecursive = false)
        {
            _logger.LogDebug("Setting game Status: PreviousStatus: {_status}, new Status: {value}...",
                    _status, newValue);
            GameStatus previousStatus = _status;
            _status = newValue;

            // Don't want to push dialog box status onto history stack.
            // It's just a temporary status, pausing before the user clicks a button 
            //  to close the dialog.
            if (previousStatus != GameStatus.ModalDialogOpen)
            {
                _statusHistoryStack.Push(previousStatus);
            }

            // If status is Running, need to call OnStatusChanged to check if game has completed
            // and, if so, update status to Completed.  Otherwise, call OnStatusChanged if
            // status has changed.
            if (_status == GameStatus.Running || _status != previousStatus)
            {
                _logger.LogDebug("Setting game Status: Calling OnStatusChanged()...");
                OnStatusChanged();
            }
        }

        public GameStatus PreviousStatus => _statusHistoryStack.Any() 
                                                ? _statusHistoryStack.Peek()
                                                : GameStatus.Initial;

        public void ResetToPreviousStatus()
        {
            GameStatus oldStatus = _status;
            GameStatus statusToSet = _statusHistoryStack.Pop();
            _logger.LogDebug("Resetting game Status to: {previousStatus}...", statusToSet);
            _status = statusToSet;
            if (oldStatus != _status)
            {
                _logger.LogDebug("Resetting game Status: Calling OnStatusChanged()...");
                OnStatusChanged();
            }
        }

        public bool IsKillerSudoku
        {
            get
            {
                return GameGrid.IsKillerSudoku;
            }

            set
            {
                GameGrid.IsKillerSudoku = value;
            }
        }

        public bool ShowPossibleValues { get; set; }
        public bool HighlightSinglePossibleValue { get; set; }
        public bool HighlightUniquePossibleValueInGroup { get; set; }

        public bool GameIsComplete => GameGrid.GameIsComplete;

        public bool CanUndo => History.CanUndo;
        public bool CanRedo => History.CanRedo;

        public void RequestRefresh()
        {
            RefreshRequested?.Invoke();
        }

        public void OnUndo()
        {
            ReplayHistory(isUndo: true);
        }

        public void OnRedo()
        {
            ReplayHistory(isUndo: false);
        }

        private void ReplayHistory(bool isUndo)
        {
            _logger.LogDebug($"{(isUndo ? "Undo" : "Redo")}ing change...");

            classlib.HistoryValue? changeDetails = isUndo 
                                            ? History.GetPreviousChange() 
                                            : History.GetNextChange();
            if (changeDetails == null)
            {
                return;
            }

            var cell = GameGrid.GetCellByPosition(changeDetails.Position);
            if (cell == null)
            {
                _logger.LogDebug("No cell specified, aborting change.");
                return;
            }

            int? newCellValue = isUndo
                                    ? changeDetails.PreviousValue
                                    : changeDetails.NewValue;

            _logger.LogDebug($"{(isUndo ? "Undo" : "Redo")}: Changing cell[{cell.Row}, {cell.Column}] value to {newCellValue}.");

            switch (_status)
            {
                case GameStatus.Initial:
                case GameStatus.Setup:
                    cell.SetInitialValue(newCellValue, isReplayingHistory: true);
                    break;
                case GameStatus.GameStart:
                case GameStatus.Running:
                    cell.SetGameValue(newCellValue, isReplayingHistory: true);
                    break;
            }
        }

        public void OnStatusChanged()
        {
            _logger.LogDebug("GameState OnStatusChanged called...");
            switch (_status)
            {
                case GameStatus.Initial:
                    OnInitialStatusSet();
                    break;
                case GameStatus.GameStart:
                    OnGameStartStatusSet();
                    break;
                case GameStatus.Running:
                    OnRunningStatusSet();
                    break;
                case GameStatus.Completed:
                    OnCompletedStatusSet();
                    break;
            }

            var eventArgs = new GameStatusChangedEventArgs(_status);
            StatusChanged?.Invoke(this, eventArgs);
        }

        private void OnInitialStatusSet()
        {
            _logger.LogDebug("GameState OnInitialStatusSet called: Resetting game and clearing History.");
            // Won't clear IsKillerSudoku but that's what we want.  If user set IsKillerSudoku 
            // previously assume they want to keep it until they manually clear it.
            GameGrid.ResetGame();
            History.Clear();
            Status = GameStatus.Setup;
        }

        private void OnGameStartStatusSet()
        {
            // Want to clear any game values (but not initial values) and the history but then
            // move status to Running.

            _logger.LogDebug("GameState OnGameStartStatusSet called: Restarting game and clearing History.");
            GameGrid.RestartGame();
            // When start game clear history because don't want user to be able to undo setting 
            // of initial values while game is running.
            History.Clear();
            Status = GameStatus.Running;
        }

        private void OnRunningStatusSet()
        {
            _logger.LogDebug("GameState OnRunningStatusSet called, checking if game is complete.");
            if (GameGrid.GameIsComplete)
            {
                Status = GameStatus.Completed;
            }
        }

        private void OnCompletedStatusSet()
        {
            _logger.LogDebug("GameState OnCompletedStatusSet called, clearing History.");
            History.Clear();
        }

        private void Cell_CellValueChanged(object? sender, classlib.CellValueChangedEventArgs eventArgs)
        {
            _logger.LogDebug("GameState Cell_CellValueChanged called...");

            // Should be illegal - this method should only run on CellValueChanged event for a
            // cell.
            if (sender is not classlib.Cell changedCell)
            {
                _logger.LogDebug("GameState Cell_CellValueChanged called but no changedCell set.");
                return;
            }

            // If replaying history don't want to add it to the history - end up toggling between
            // Undo and Redo otherwise.
            if (!eventArgs.IsReplayingHistory && eventArgs.PreviousValue != eventArgs.NewValue)
            {
                _logger.LogDebug("GameState Cell_CellValueChanged: Recording change in History.");
                classlib.Position changedCellPosition = changedCell.Position;
                classlib.HistoryValue changeDetails = new(changedCellPosition, eventArgs.PreviousValue, eventArgs.NewValue);
                History.AddChange(changeDetails);
            }

            // Set a cell value during the Initial state: Move to Setup state.
            if (_status == GameStatus.Initial && eventArgs.NewValue != null)
            {
                Status = GameStatus.Setup;
            }
            // If setting the cell value completed the game, change the status.
            else if (_status == GameStatus.Running && GameGrid.GameIsComplete)
            {
                Status = GameStatus.Completed;
            }

            if (eventArgs.PossibleValuesChanged)
            {
                RequestRefresh();
            }
        }
    }
}
