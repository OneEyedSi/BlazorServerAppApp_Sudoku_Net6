namespace SudokuWebApp.Shared.Classes
{
    public enum GameStatus
    {
        Unknown = 0,
        PreStart,
        Starting,
        InitializingCells,
        Running,
        Paused,
        Cancelling,
        Cancelled,
        Finished
    }
}
