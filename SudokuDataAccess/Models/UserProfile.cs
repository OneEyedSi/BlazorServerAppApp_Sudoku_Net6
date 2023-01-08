using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SudokuDataAccess.Models.Reference;
using System.ComponentModel.DataAnnotations;

namespace SudokuDataAccess.Models
{
    public class UserProfile
    {
        public const string DefaultName = "Guest";
        public const int DefaultId = 1;

        public int UserProfileId { get; set; }

        [Required(ErrorMessage = "Profile name is required.")]
        public string Name { get; set; }

        #region Parents ***************************************************************************

        [Required(ErrorMessage = "An icon is required for the profile.")]
        public int? IconId { get; set; }
        public Icon? Icon { get; set; }

        #endregion

        #region Children **************************************************************************

        public List<UserGame> Games { get; set; }

        #endregion
    }

    #region Configuration *************************************************************************

    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            List<UserProfile> userProfiles = new()
            {
                new UserProfile 
                { 
                    UserProfileId = UserProfile.DefaultId, 
                    Name = UserProfile.DefaultName, 
                    IconId = Icon.DefaultId 
                }
            };

            builder.HasData(userProfiles);
        }
    }

    #endregion
}
