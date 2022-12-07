using Sudoku = SudokuClassLibrary;
using System.Collections.Generic;
using System.Linq;
using SudokuClassLibrary.Tests.Cell;
using System;

namespace SudokuClassLibrary.Tests.CellGroup
{
    public static class CellGroupExtensions
    {
        public static void SetAvailableValue(this Sudoku.CellGroup cellGroup, int value)
        {
            IEnumerable<int> values = new int[] { value };
            cellGroup.SetAvailableValues(values);
        }

        public static void SetAvailableValueRange(this Sudoku.CellGroup cellGroup,
            int minValue, int maxValue)
        {
            int numberOfValues = maxValue - minValue + 1;
            IEnumerable<int> values = Enumerable.Range(minValue, numberOfValues);
            cellGroup.SetAvailableValues(values);
        }

        public static void SetAvailableValues(this Sudoku.CellGroup cellGroup,
            IEnumerable<int> availableValues)
        {
            // AvailableValues calculated as the values left from the range 1..9 after any 
            // cell Values have been removed.  So to set the AvailableValues set cell Values to 
            // all the values we don't want to keep.
            IEnumerable<int> valuesToSet = Enumerable.Range(start: 1, count: 9);
            valuesToSet = valuesToSet.Except(availableValues);
            foreach (int valueToSet in valuesToSet)
            {
                Sudoku.Cell cell = new(1, valueToSet);
                cell.Value = valueToSet;
                cellGroup.AddCell(cell);
            }

        }
    }
}
