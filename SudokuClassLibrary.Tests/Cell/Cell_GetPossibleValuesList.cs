using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System;
using System.Linq;

namespace SudokuClassLibrary.Tests.Cell
{
    public class Cell_GetPossibleValuesList
    {
        [Fact]
        public void Should_Return_ReadOnlyCollection()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);

            // Act
            var possibleValues = cell.GetPossibleValuesList();

            // Assert
            (possibleValues is IReadOnlyCollection<int>).Should().BeTrue();
        }

        [Fact]
        public void Should_Return_All_PossibleValues_Set_After_Cell_Initialized()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);

            // Act
            var possibleValues = cell.GetPossibleValuesList();

            // Assert
            possibleValues.ShouldHaveExpectedValuesSetToRange(1, 9);
        }

        [Fact]
        public void Should_Return_Correct_PossibleValues_When_MultiplePossibleValues()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);
            int minValue = 2;
            int maxValue = 5;
            cell.SetPossibleValueRange(minValue, maxValue);

            // Act
            var possibleValues = cell.GetPossibleValuesList();

            // Assert
            possibleValues.ShouldHaveExpectedValuesSetToRange(minValue, maxValue);
        }

        [Fact]
        public void Should_Return_Single_PossibleValue_EqualTo_Value_When_Value_Set()
        {
            // Arrange
            int? newValue = 4;
            Sudoku.Cell cell = new(1, 1);
            int expectedValue = newValue.Value;

            // Act
            cell.SetValue(newValue);
            var possibleValues = cell.GetPossibleValuesList();

            // Assert
            possibleValues.ShouldHaveExpectedValueSet(expectedValue);
        }
    }
}
