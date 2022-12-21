namespace SudokuWebApp.Shared.Classes
{
    public enum GameStatus
    {
        Unknown = 0,
        Initial,
        Setup,
        GameStart,
        Running,
        Paused,
        Completed, 
        ModalDialogOpen
    }
}
