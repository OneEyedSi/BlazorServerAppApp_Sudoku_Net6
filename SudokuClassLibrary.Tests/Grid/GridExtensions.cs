using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Sudoku = SudokuClassLibrary;

namespace SudokuClassLibrary.Tests.Grid
{
    public static class GridExtensions
    {
        public static IEnumerable<Sudoku.Cell> GetCellsThatShouldBeInRow(this Sudoku.Grid grid,
            int rowIndex)
        {
            var cells = grid.GetEnumerableCells().Where(c => c.Row == rowIndex);
            return cells;
        }

        public static IEnumerable<Sudoku.Cell> GetCellsThatShouldBeInColumn(this Sudoku.Grid grid,
            int columnIndex)
        {
            var cells = grid.GetEnumerableCells().Where(c => c.Column == columnIndex);
            return cells;
        }

        public static IEnumerable<Sudoku.Cell> GetCellsThatShouldBeInSquare(this Sudoku.Grid grid,
            int squareIndex)
        {
            Func<Sudoku.Cell, bool> cellShouldBeInSquare =
                c => Sudoku.CellGroup.GetSquareIndexForCell(c) == squareIndex;

            var cells = grid.GetEnumerableCells().Where(c => cellShouldBeInSquare(c));
            return cells;
        }

        public static IEnumerable<Sudoku.Cell> GetCellsThatShouldBeInDiagonal(this Sudoku.Grid grid,
            int diagonalIndex)
        {
            if (diagonalIndex < 0 || diagonalIndex > 1)
            {
                return Enumerable.Empty<Sudoku.Cell>();
            }

            Func<Sudoku.Cell, bool> cellIsOnDiagonal =
                c => 
                {
                    // Primary diagonal.
                    if (diagonalIndex == 0)
                    {
                        return Sudoku.CellGroup.IsCellInPrimaryDiagonal(c);
                    }

                    return Sudoku.CellGroup.IsCellInSecondaryDiagonal(c);
                };

            var cells = grid.GetEnumerableCells().Where(c => cellIsOnDiagonal(c));
            return cells;
        }

        public static Sudoku.CellGroup GetRowGroup(this Sudoku.Grid grid, int groupIndex)
        {
            return grid.GetGroup(CellGroupType.Row, groupIndex);
        }

        public static Sudoku.CellGroup GetColumnGroup(this Sudoku.Grid grid, int groupIndex)
        {
            return grid.GetGroup(CellGroupType.Column, groupIndex);
        }

        public static Sudoku.CellGroup GetSquareGroup(this Sudoku.Grid grid, int groupIndex)
        {
            return grid.GetGroup(CellGroupType.Square, groupIndex);
        }

        public static Sudoku.CellGroup GetDiagonalGroup(this Sudoku.Grid grid, int groupIndex)
        {
            return grid.GetGroup(CellGroupType.Diagonal, groupIndex);
        }

        public static Sudoku.CellGroup GetGroup(this Sudoku.Grid grid, 
            CellGroupType groupType, int groupIndex)
        {
            if (!(grid.Groups?.Any() ?? false))
            {
                throw new ApplicationException($"No {groupType} CellGroup exists for index {groupIndex}.");
            }

            var groups = grid.Groups.Where(g => g.GroupType == groupType && g.Index == groupIndex);

            int numberOfGroups = groups.Count();
            switch (numberOfGroups)
            {
                case 0:
                    throw new ApplicationException($"No {groupType} CellGroup exists for index {groupIndex}.");
                case 1:
                    return groups.First();
                case > 1:
                    throw new ApplicationException($"Multiple {groupType} CellGroups exist for index {groupIndex}.");
                default:
                    throw new ApplicationException($"Invalid CellGroups Count for {groupType} CellGroups with index {groupIndex}.");
            }
        }
    }
}
