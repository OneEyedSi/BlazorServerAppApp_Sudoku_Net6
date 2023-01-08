using SudokuDataAccess.Models;
using SudokuDataAccess.Models.Reference;
using SudokuWebApp.Data;

namespace SudokuWebApp.Data
{
    public interface IUserProfileService
    {
        event Action? ProfileListUpdated;
        Task<int?> AddOrUpdateUserProfileAsync(int userProfileId, string name, int iconId);
        Task<int?> AddOrUpdateUserProfileAsync(UserProfile userProfile);
        UserProfile? GetDefaultUserProfile();
        UserProfile? GetUserProfileById(int userProfileId);
        IEnumerable<UserProfile>? GetAllUserProfiles();
        Task<bool> RemoveUserProfileAsync(int userProfileId);
        Task<bool> RemoveUserProfileAsync(UserProfile userProfile);
    }
}