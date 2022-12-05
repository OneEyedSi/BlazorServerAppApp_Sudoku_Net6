using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuClassLibrary
{
    public enum CellGroupType
    {
        Invalid = 0,
        Row,
        Column,
        Square,
        Diagonal
    }
}
