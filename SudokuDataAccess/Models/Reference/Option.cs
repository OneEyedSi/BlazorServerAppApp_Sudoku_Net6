using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuDataAccess.Models.Reference
{
    public class Option
    {
        public int OptionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    #region Configuration *************************************************************************

    public class OptionConfiguration : IEntityTypeConfiguration<Option>
    {
        public void Configure(EntityTypeBuilder<Option> builder)
        {
            List<Option> options = new()
            {
                new Option { OptionId = 1, 
                            Name = "ShowPossibleValues", 
                            Description = "Show possible values for each square" },
                new Option { OptionId = 2, 
                            Name = "HighlightSinglePossibleValue", 
                            Description = "Highlight squares with only one possible value in pale yellow" },
                new Option { OptionId = 3, 
                            Name = "HighlightUniquePossibleValueInGroup", 
                            Description = "Highlight squares with possible value unique in row, column, square or diagonal in pale orange" }
            };

            builder.HasData(options);
        }
    }

    #endregion
}
