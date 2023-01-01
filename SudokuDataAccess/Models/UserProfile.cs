using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SudokuDataAccess.Models.Reference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuDataAccess.Models
{
    public class UserProfile
    {
        public const string DefaultName = "Guest";
        public const int DefaultId = 1;

        public int UserProfileId { get; set; }
        public string Name { get; set; }

        #region Parents ***************************************************************************

        public int? IconId { get; set; }
        public Icon? Icon { get; set; }

        #endregion

        #region Children **************************************************************************

        public List<Game> Games { get; set; }

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
