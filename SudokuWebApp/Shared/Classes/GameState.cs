using SudokuClassLibrary;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SudokuWebApp.Shared.Classes
{
    public class GameState
    {
        public event Action? RefreshRequested;
        public event Action? StatusChanged;

        private Stopwatch _timer = new Stopwatch();

        public Grid GameGrid { get; } = new();

        public TimeSpan ElapsedRunningTime => _timer.Elapsed;

        private GameStatus _status = GameStatus.Initial;
        public GameStatus Status
        {
            get
            {
                if (_status == GameStatus.Initial && GameGrid.HasInitialValue)
                {
                    PreviousStatus = _status;
                    _status = GameStatus.Setup;
                }
                else if (_status == GameStatus.GameStart && _timer.ElapsedTicks > 0)
                {
                    PreviousStatus = _status;
                    _status = GameStatus.Running;
                }
                else if (_status == GameStatus.Running && GameGrid.GameIsComplete)
                {
                    PreviousStatus = _status;
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

                if (previousStatus != GameStatus.ModalDialogOpen 
                    && _status != GameStatus.ModalDialogOpen)
                {
                    PreviousStatus = previousStatus;
                }

                if (_status != previousStatus)
                {
                    OnStatusChanged();
                }
            }
        }

        public GameStatus PreviousStatus { get; private set; }

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

        public bool GameIsComplete => GameGrid.GameIsComplete;

        public void RequestRefresh()
        {
            RefreshRequested?.Invoke();
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
            ClearTimerAndLeaveStopped();
        }

        private void OnGameStartStatusSet()
        {
            GameGrid.RestartGame();
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

        private void SwitchTimer(GameStatus previousStatus, GameStatus newStatus)
        {
            if (newStatus == GameStatus.Running)
            {
                switch (previousStatus)
                {
                    case GameStatus.Setup:
                        // Sets timer to 0 then starts it.
                        _timer.Restart();
                        break;
                    case GameStatus.Paused:
                        // Continues from previous timer Elapsed time; doesn't reset it to 0.
                        _timer.Start();
                        break;
                }
            }
            else if (_timer.IsRunning)
            {
                _timer.Stop();
            }
        }
    }
}
