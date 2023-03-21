using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuDataAccess.Models.Reference
{
    public class Level
    {
        public int LevelId { get; set; }
        public string Title { get; set; }
        public int MinimumNumberOfInitialValues { get; set; }
        public int MaximumNumberOfInitialValues { get; set; }
    }

    #region Configuration *************************************************************************

    public class LevelConfiguration : IEntityTypeConfiguration<Level>
    {
        public void Configure(EntityTypeBuilder<Level> builder)
        {
            List<Level> levels = new()
            {
                new Level 
                { 
                    LevelId = 1, 
                    Title = "Novice",
                    MinimumNumberOfInitialValues = 36, 
                    MaximumNumberOfInitialValues = 81 
                },
                new Level
                {
                    LevelId = 2,
                    Title = "Moderate",
                    MinimumNumberOfInitialValues = 32,
                    MaximumNumberOfInitialValues = 35
                },
                new Level
                {
                    LevelId = 3,
                    Title = "Hard",
                    MinimumNumberOfInitialValues = 28,
                    MaximumNumberOfInitialValues = 31
                },
                new Level
                {
                    LevelId = 4,
                    Title = "Very Hard",
                    MinimumNumberOfInitialValues = 24,
                    MaximumNumberOfInitialValues = 27
                },
                new Level
                {
                    LevelId = 5,
                    Title = "Genius",
                    MinimumNumberOfInitialValues = 1,
                    MaximumNumberOfInitialValues = 23
                }
            };

            builder.HasData(levels);
        }
    }

    #endregion
}
