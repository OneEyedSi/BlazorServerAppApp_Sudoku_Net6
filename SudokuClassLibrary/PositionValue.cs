using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuClassLibrary
{
    public class PositionValue
    {
        public PositionValue(int row, int column, int? value) : this(new Position(row, column), value)
        { }

        public PositionValue(Position position, int? value)
        {
            Position = position;
            Value = value;
        }

        public Position Position { get; set; }
        public int? Value { get; set; }
        public int Row { get { return Position.Row; } }
        public int Column { get { return Position.Column; } }
    }
}
