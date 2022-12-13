using SudokuClassLibrary;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SudokuWebApp.Shared.Classes
{
    public class GameState
    {
        public event Action? RefreshRequested;

        private Stopwatch _timer = new Stopwatch();

        public Grid GameGrid { get; } = new();

        public TimeSpan ElapsedRunningTime => _timer.Elapsed;

        private GameStatus _status = GameStatus.Setup;
        public GameStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                PreviousStatus = _status;
                _status = value;

                SwitchTimer(PreviousStatus, _status);
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

        public void RequestRefresh()
        {
            RefreshRequested?.Invoke();
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
