using SudokuDataAccess.Models;

namespace SudokuClassLibrary.DataServices
{
    public interface IUserProfileService
    {
        Task<int?> AddOrUpdateUserProfileAsync(int userProfileId, string name, int iconId);
        UserProfile? GetDefaultUserProfile();
        UserProfile? GetUserProfileById(int userProfileId);
        Task<bool> RemoveUserProfileAsync(int userProfileId);
        Task<bool> RemoveUserProfileAsync(UserProfile userProfile);
    }
}