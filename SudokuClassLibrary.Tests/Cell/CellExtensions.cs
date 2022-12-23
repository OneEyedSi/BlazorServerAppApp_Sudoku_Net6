using Microsoft.VisualBasic;
using SudokuClassLibrary.Tests.CellGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using Sudoku = SudokuClassLibrary;

namespace SudokuClassLibrary.Tests.Cell
{
    public static class CellExtensions
    {
        public static void SetPossibleValue(this Sudoku.Cell cell, int value)
        {
            IEnumerable<int> values = new int[] { value };
            cell.SetPossibleValues(values);
        }

        public static void SetPossibleValueRange(this Sudoku.Cell cell,
            int minValue, int maxValue)
        {
            int numberOfValues = maxValue - minValue + 1;
            IEnumerable<int> values = Enumerable.Range(minValue, numberOfValues);
            cell.SetPossibleValues(values);
        }

        public static void SetPossibleValues(this Sudoku.Cell cell,
            IEnumerable<int> valuesToSet)
        {
            // Resets possible values and also AvailableValues of any parent groups.
            cell.ResetPossibleValues();

            // Creates a parent group if there isn't one already.
            var parentGroup = GetCellFirstParentGroup(cell);

            // Possible values are calculated as the intersection of the AvailableValues of
            // the cell's ParentGroups.
            parentGroup.SetAvailableValues(valuesToSet);
        }

        /// <summary>
        /// Resets the Possible values for the cell, setting all values between 1 and 9 as 
        /// Possible values.
        /// </summary>
        /// <param name="cell"></param>
        /// <remarks>Possible values are calculated as the intersection of the 
        /// AvailableValues of the cell's ParentGroups.  So have to reset the AvailableValues 
        /// for every ParentGroup.  To do that, clear the Value of every child Cell of every 
        /// ParentGroup.</remarks>
        public static void ResetPossibleValues(this Sudoku.Cell cell)
        {
            if (cell == null)
            {
                return;
            }

            var parentGroups = cell.ParentGroups;
            if (!(parentGroups?.Any() ?? false)) 
            {
                var parentGroup = GetCellFirstParentGroup(cell);
                parentGroups = new()
                {
                    parentGroup
                };
            }

            foreach ( var group in parentGroups)
            {
                foreach(var groupCell in group.Cells)
                {
                    groupCell.SetValue(null);
                }
            }
        }

        private static Sudoku.CellGroup GetCellFirstParentGroup(Sudoku.Cell cell)
        {
            var parentGroup = cell.ParentGroups?.FirstOrDefault();
            if (parentGroup != null)
            {
                return parentGroup;
            }

            int cellRow = cell.Position.Row;
            parentGroup = new(CellGroupType.Row, cellRow);
            parentGroup.AddCell(cell);
            return parentGroup;
        }
    }
}
