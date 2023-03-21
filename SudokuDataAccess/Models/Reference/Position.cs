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

        public static int GetPositionIdFromRowAndColumn(int row, int column)
        {
            if (row < 0 || row > 8)
            {
                throw new ArgumentException($"Invalid row value: {row}.  Must be between 0 and 8.", "row");
            }
            if (column < 0 || column > 8)
            {
                throw new ArgumentException($"Invalid column value: {column}.  Must be between 0 and 8.", "column");
            }

            return row * 9 + column + 1;
        }
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
