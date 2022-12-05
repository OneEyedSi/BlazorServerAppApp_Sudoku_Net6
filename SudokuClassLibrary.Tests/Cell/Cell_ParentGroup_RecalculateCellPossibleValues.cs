using System;
using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace SudokuClassLibrary.Tests.Cell
{
    public class Cell_ParentGroup_RecalculateCellPossibleValues
    {
        [Fact]
        public void Should_Set_PossibleValues_To_Intersection_Of_ParentGroups_AvailableValues()
        {
            // Arrange
            Sudoku.CellGroup parentGroup1 = new(CellGroupType.Row, 1);
            SetCellGroupAvailableValues(parentGroup1, new int[] { 1, 2, 3 });

            Sudoku.CellGroup parentGroup2 = new(CellGroupType.Column, 1);
            SetCellGroupAvailableValues(parentGroup2, new int[] { 2, 3, 4 });

            Sudoku.Cell cell = new(1, 1);
            cell.ParentGroups.Add(parentGroup1);
            cell.ParentGroups.Add(parentGroup2);

            EventArgs args = new();

            // Act
            cell.ParentGroup_RecalculateCellPossibleValues(parentGroup1, args);

            // Assert
            cell.GetPossibleValues().Should().NotBeNullOrEmpty()
                .And.HaveCount(2)
                .And.OnlyContain(pv => (pv >= 2 && pv <= 3));
        }
        [Fact]
        public void Should_Set_PossibleValues_When_ParentGroup_AvailableValues_Changes()
        {
            // Arrange
            Sudoku.CellGroup parentGroup = new(CellGroupType.Row, 1);
            Sudoku.Cell cell = new(1, 1);
            parentGroup.AddCell(cell);

            // Act
            SetCellGroupAvailableValues(parentGroup, new int[] { 1, 2, 3 });

            // Assert
            cell.GetPossibleValues().Should().NotBeNullOrEmpty()
                .And.HaveCount(3)
                .And.OnlyContain(pv => (pv >= 1 && pv <= 3));
        }

        private void SetCellGroupAvailableValues(Sudoku.CellGroup cellGroup, 
            IEnumerable<int> availableValues)
        {
            // AvailableValues calculated as the values left from the range 1..9 after any 
            // cell Values have been removed.  So to set the AvailableValues set cell Values to 
            // all the values we don't want to keep.
            IEnumerable<int> valuesToSet = Enumerable.Range(1, 9);
            valuesToSet = valuesToSet.Except(availableValues);
            foreach(int valueToSet in valuesToSet) 
            {
                Sudoku.Cell cell = new(9, valueToSet);
                cell.Value = valueToSet;
                cellGroup.AddCell(cell);
            }

        }
    }
}
