using SudokuClassLibrary;
using System.Diagnostics;

namespace SudokuWebApp.Shared.Classes
{
    public class GameState
    {
        private Stopwatch _timer = new Stopwatch();

        public Grid GameGrid { get; } = new();

        public TimeSpan ElapsedRunningTime => _timer.Elapsed;

        private GameStatus _status = GameStatus.PreStart;
        public GameStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                var previousStatus = _status;
                _status = value;

                SwitchTimer(previousStatus, _status);
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

        private void SwitchTimer(GameStatus previousStatus, GameStatus newStatus)
        {
            if (newStatus == GameStatus.Running)
            {
                switch (previousStatus)
                {
                    case GameStatus.Starting:
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
