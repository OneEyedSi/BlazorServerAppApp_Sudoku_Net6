using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuClassLibrary
{
    public class HistoryValue
    {
        public HistoryValue(int row, int column, int? previousValue, int? newValue)
            : this(new Position(row, column), previousValue, newValue)
        { }

        public HistoryValue(Position position, int? previousValue, int? newValue)
        {
            Position = position;
            PreviousValue = previousValue;
            NewValue = newValue;
        }

        public Position Position { get; set; }
        public int? PreviousValue { get; set; }
        public int? NewValue { get; set; }
        public int Row { get { return Position.Row; } }
        public int Column { get { return Position.Column; } }
    }
}
