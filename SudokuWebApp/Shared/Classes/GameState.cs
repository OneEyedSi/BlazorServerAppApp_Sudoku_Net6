using Sudoku = SudokuClassLibrary;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SudokuClassLibrary;

namespace SudokuWebApp.Shared.Classes
{
    public class GameState
    {
        public event Action? RefreshRequested;
        public event Action? StatusChanged;

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

        private Stopwatch _timer = new Stopwatch();

        public Sudoku.Grid GameGrid { get; } = new();

        public Sudoku.History History { get; } = new();

        public TimeSpan ElapsedRunningTime => _timer.Elapsed;

        private Stack<GameStatus> _statusHistoryStack = new();
        private GameStatus _status = GameStatus.Initial;
        public GameStatus Status
        {
            get
            {
                if (_status == GameStatus.Initial && GameGrid.HasInitialValue)
                {
                    _statusHistoryStack.Push(_status);
                    _status = GameStatus.Setup;
                }
                else if (_status == GameStatus.GameStart && _timer.ElapsedTicks > 0)
                {
                    _statusHistoryStack.Push(_status);
                    _status = GameStatus.Running;
                }
                else if (_status == GameStatus.Running && GameGrid.GameIsComplete)
                {
                    _statusHistoryStack.Push(_status);
                    _status = GameStatus.Completed;
                }

                if (_status != PreviousStatus)
                {
                    OnStatusChanged();
                }

                return _status;
            }
            set
            {
                GameStatus previousStatus = _status;
                _status = value;

                // Don't want to push dialog box status onto history stack.
                // It's just a temporary status, pausing before the user clicks a button 
                //  to close the dialog.
                if (previousStatus != GameStatus.ModalDialogOpen)
                {
                    _statusHistoryStack.Push(previousStatus);
                }

                if (_status != previousStatus)
                {
                    OnStatusChanged();
                }
            }
        }

        public GameStatus PreviousStatus => _statusHistoryStack.Any() 
                                                ? _statusHistoryStack.Peek()
                                                : GameStatus.Initial;

        public void ResetToPreviousStatus()
        {
            GameStatus previousStatus = _statusHistoryStack.Pop();
            _status = previousStatus;
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
            HistoryValue? changeDetails = isUndo 
                                            ? History.GetPreviousChange() 
                                            : History.GetNextChange();
            if (changeDetails == null)
            {
                return;
            }

            var cell = GameGrid.GetCellByPosition(changeDetails.Position);
            if (cell == null)
            {
                return;
            }

            int? newCellValue = isUndo
                                    ? changeDetails.PreviousValue
                                    : changeDetails.NewValue;

            switch (_status)
            {
                case GameStatus.Initial:
                case GameStatus.Setup:
                    cell.SetInitialValue(newCellValue, isReplayingHistory: true);
                    break;
                case GameStatus.GameStart:
                case GameStatus.Running:
                    cell.SetValue(newCellValue, isReplayingHistory: true);
                    break;
            }
        }

        public void OnStatusChanged()
        {
            switch (_status)
            {
                case GameStatus.Initial:
                    OnInitialStatusSet();
                    break;
                case GameStatus.GameStart:
                    OnGameStartStatusSet();
                    break;
                case GameStatus.Paused: 
                    OnPausedStatusSet();
                    break;
                case GameStatus.Running:
                    OnRunningStatusSet();
                    break;
                case GameStatus.Completed:
                    OnCompletedStatusSet();
                    break;
                case GameStatus.ModalDialogOpen:
                    OnModalDialogOpenSet();
                    break;
            }

            StatusChanged?.Invoke();
        }

        private void OnInitialStatusSet()
        {
            // Won't clear IsKillerSudoku but that's what we want.  If user set IsKillerSudoku 
            // previously assume they want to keep it until they manually clear it.
            GameGrid.ResetGame();
            History.Clear();
            ClearTimerAndLeaveStopped();
        }

        private void OnGameStartStatusSet()
        {
            GameGrid.RestartGame();
            // When start game clear history because don't want user to be able to undo setting 
            // of initial values while game is running.
            History.Clear();
            ClearAndRestartTimer();
        }

        private void OnPausedStatusSet()
        {
            PauseTimer();
        }

        private void OnRunningStatusSet()
        {
            // Won't cause a problem if timer already running, but will start running again if 
            // previously paused.
            ContinueTimer();
        }

        private void OnCompletedStatusSet()
        {
            History.Clear();
            StopTimer();
        }

        private void OnModalDialogOpenSet()
        {
            PauseTimer();
        }

        private void ClearTimerAndLeaveStopped()
        {
            _timer.Reset();
        }

        private void ClearAndRestartTimer()
        {
            _timer.Restart();
        }

        private void PauseTimer()
        {
            // Exactly same as stop.  Only difference in the game is the workflow:
            //  Pause --> Continue
            //  Stop --> Clear
            StopTimer();
        }

        private void ContinueTimer()
        {
            // Starts timer running from previously stopped value; doesn't clear it.
            // Will not throw if already running.
            _timer.Start();
        }

        private void StopTimer()
        {
            // Will not throw if not running.
            _timer.Stop();
        }

        private void Cell_CellValueChanged(object? sender, Sudoku.CellValueChangedEventArgs eventArgs)
        {
            // Should be illegal - this method should only run on CellValueChanged event for a
            // cell.
            if (sender is not Sudoku.Cell changedCell)
            {
                return;
            }

            // If replaying history don't want to add it to the history - end up toggling between
            // Undo and Redo otherwise.
            if (!eventArgs.IsReplayingHistory)
            {
                Sudoku.Position changedCellPosition = changedCell.Position;
                HistoryValue changeDetails = new(changedCellPosition, eventArgs.PreviousValue, eventArgs.NewValue);
                History.AddChange(changeDetails);
            }
        }
    }
}
