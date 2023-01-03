using SudokuDataAccess.Models;

namespace SudokuClassLibrary.DataServices
{
    public interface IUserProfileService
    {
        event Action? ProfileListUpdated;
        Task<int?> AddOrUpdateUserProfileAsync(int userProfileId, string name, int iconId);
        UserProfile? GetDefaultUserProfile();
        UserProfile? GetUserProfileById(int userProfileId);
        IEnumerable<UserProfile>? GetAllUserProfiles();
        Task<bool> RemoveUserProfileAsync(int userProfileId);
        Task<bool> RemoveUserProfileAsync(UserProfile userProfile);
    }
}