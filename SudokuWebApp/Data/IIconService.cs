using SudokuDataAccess.Models.Reference;

namespace SudokuWebApp.Data
{
    public interface IIconService
    {
        IEnumerable<Icon>? GetAllIcons();
        IEnumerable<Icon>? GetFilteredIcons(Func<Icon, bool> filter);
        IEnumerable<Icon>? GetUnusedAndCurrentIcons(int currentProfileIconId = 0);
        IEnumerable<Icon>? GetUnusedIcons();
    }
}