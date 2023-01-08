using Microsoft.EntityFrameworkCore;
using SudokuClassLibrary;
using SudokuDataAccess;
using SudokuDataAccess.Models.Reference;

namespace SudokuWebApp.Data
{
    public class IconService : IIconService
    {
        private readonly IDbContextFactory<DataContext> _dbContextFactory;
        private readonly IUserProfileService _userProfileService;

        public IconService(IUserProfileService userProfileService,
            IDbContextFactory<DataContext> dbContextFactory)
        {
            _userProfileService = userProfileService;
            _dbContextFactory = dbContextFactory;
        }

        public IEnumerable<Icon>? GetUnusedAndCurrentIcons(int currentProfileIconId = 0)
        {
            var unusedIcons = GetUnusedIcons();

            // Should be only one current icon but easier to work with IEnumerable.
            IEnumerable<Icon>? currentIcons = null;
            if (currentProfileIconId > 0)
            {
                currentIcons = GetFilteredIcons(i => i.IconId == currentProfileIconId);
            }

            if (unusedIcons == null)
            {
                if (currentIcons == null)
                {
                    return null;
                }

                return currentIcons;
            }

            if (currentIcons == null)
            {
                return unusedIcons;
            }

            return unusedIcons.Concat(currentIcons).OrderBy(i => i.IconId);
        }

        public IEnumerable<Icon>? GetUnusedIcons()
        {
            var userProfiles = _userProfileService.GetAllUserProfiles();

            // There must be at least one user profile.  If not, then something went wrong.
#pragma warning disable CS8604 // Possible null reference argument: Ignore because IsNullOrEmpty 
            // extension method can handle a null IEnumerable.
            if (userProfiles.IsNullOrEmpty())
            {
                return null;
            }
#pragma warning restore CS8604 // Possible null reference argument.

            var usedIconIds = userProfiles.Select(up => up.IconId);

            return GetFilteredIcons(i => !usedIconIds.Contains(i.IconId));
        }

        public IEnumerable<Icon>? GetAllIcons()
        {
            return GetFilteredIcons(i => true);
        }

        public IEnumerable<Icon>? GetFilteredIcons(Func<Icon, bool> filter)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            if (dbContext == null)
            {
                return null;
            }

            return dbContext.Icons
                .Where(filter)
                .ToList();
        }
    }
}
