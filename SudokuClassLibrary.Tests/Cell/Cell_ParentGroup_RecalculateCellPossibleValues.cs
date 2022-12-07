using System;
using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using SudokuClassLibrary.Tests.CellGroup;

namespace SudokuClassLibrary.Tests.Cell
{
    public class Cell_ParentGroup_RecalculateCellPossibleValues
    {
        [Fact]
        public void Should_Set_PossibleValues_To_Intersection_Of_ParentGroups_AvailableValues()
        {
            // Arrange
            int cellRow = 1;
            int cellColumn = 1;

            Sudoku.CellGroup parentGroup1 = new(CellGroupType.Row, cellRow);
            int minValue1 = 1;
            int maxValue1 = 3;
            parentGroup1.SetAvailableValueRange(minValue1, maxValue1);

            Sudoku.CellGroup parentGroup2 = new(CellGroupType.Column, cellColumn);
            int minValue2 = 2;
            int maxValue2 = 4;
            parentGroup2.SetAvailableValueRange(minValue2, maxValue2);

            Sudoku.Cell cell = new(cellRow, cellColumn);
            cell.ParentGroups.Add(parentGroup1);
            cell.ParentGroups.Add(parentGroup2);

            EventArgs args = new();

            // Act
            cell.ParentGroup_RecalculateCellPossibleValues(parentGroup1, args);

            // Assert
            cell.GetPossibleValuesDictionary().ShouldHaveExpectedValuesSetToRange(2, 3);
        }
        [Fact]
        public void Should_Set_PossibleValues_When_ParentGroup_AvailableValues_Changes()
        {
            // Arrange
            int cellRow = 1;
            int cellColumn = 1;

            Sudoku.CellGroup parentGroup = new(CellGroupType.Row, cellRow);
            Sudoku.Cell cell = new(cellRow, cellColumn);
            parentGroup.AddCell(cell);

            // Act
            parentGroup.SetAvailableValueRange(2, 4);

            // Assert
            cell.GetPossibleValuesDictionary().ShouldHaveExpectedValuesSetToRange(2, 4);
        }
    }
}
