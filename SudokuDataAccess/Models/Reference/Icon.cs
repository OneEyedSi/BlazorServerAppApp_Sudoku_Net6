using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuDataAccess.Models.Reference
{
    public class Icon
    {
        public const string DefaultTitle = "Guest";
        public const int DefaultId = 1;

        public int IconId { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
    }

    #region Configuration *************************************************************************

    public class IconConfiguration : IEntityTypeConfiguration<Icon>
    {
        public void Configure(EntityTypeBuilder<Icon> builder)
        {
            List<Icon> icons = new()
            {
                new Icon { IconId = Icon.DefaultId, Title = Icon.DefaultTitle, Path = "img/1669707620smiley-classic-icon_grey.svg" },
                new Icon { IconId = 2, Title = "Brown", Path = "img/1669707620smiley-classic-icon_brown.svg" },
                new Icon { IconId = 3, Title = "Pink", Path = "img/1669707620smiley-classic-icon_pink.svg" },
                new Icon { IconId = 4, Title = "Orange", Path = "img/1669707620smiley-classic-icon_orange.svg" },
                new Icon { IconId = 5, Title = "Yellow", Path = "img/1669707620smiley-classic-icon_yellow.svg" },
                new Icon { IconId = 6, Title = "Green", Path = "img/1669707620smiley-classic-icon_green.svg" },
                new Icon { IconId = 7, Title = "Cyan", Path = "img/1669707620smiley-classic-icon_cyan.svg" },
                new Icon { IconId = 8, Title = "Purple", Path = "img/1669707620smiley-classic-icon_purple.svg" }
            };

            builder.HasData(icons);
        }
    }

    #endregion
}
