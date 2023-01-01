using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuDataAccess.Models.Reference
{
    public class Position
    {
        public int PositionId { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }

    #region Configuration *************************************************************************

    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            List<Position> positions = new();

            int id = 1;
            for (int row = 0; row <= 8; row++) 
            {
                for (int column = 0; column <= 8; column++)
                {
                    positions.Add(new Position { PositionId = id, Row = row, Column = column });
                    id++;
                }
            }

            builder.HasData(positions);
        }
    }

    #endregion
}
