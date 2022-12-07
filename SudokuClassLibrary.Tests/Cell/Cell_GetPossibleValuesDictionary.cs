using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuClassLibrary.Tests.Cell
{
    public class Cell_GetPossibleValuesDictionary
    {
        [Fact]
        public void Should_Return_ReadOnlyDictionary()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);

            // Act
            var possibleValues = cell.GetPossibleValuesDictionary();

            // Assert
            (possibleValues is IReadOnlyDictionary<int, bool>).Should().BeTrue();
        }

        [Fact]
        public void Should_Return_All_PossibleValues_Set_After_Cell_Initialized()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);

            // Act
            var possibleValues = cell.GetPossibleValuesDictionary();

            // Assert
            possibleValues.ShouldHaveExpectedValuesSetToRange(minValue: 1, maxValue: 9);
        }

        [Fact]
        public void Should_Return_Correct_PossibleValues_Set_When_MultiplePossibleValues()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);
            int minValue = 2;
            int numberOfValues = 5;
            cell.SetPossibleValueRange(minValue, numberOfValues);

            // Act
            var possibleValues = cell.GetPossibleValuesDictionary();

            // Assert
            possibleValues.ShouldHaveExpectedValuesSetToRange(minValue, numberOfValues);
        }

        [Fact]
        public void Should_Return_Single_PossibleValue_Set_With_Key_EqualTo_Value_When_Value_Set()
        {
            // Arrange
            int? newValue = 4;
            Sudoku.Cell cell = new(1, 1);
            int expectedValue = newValue.Value;

            // Act
            cell.Value = newValue;
            var possibleValues = cell.GetPossibleValuesDictionary();

            // Assert
            possibleValues.ShouldHaveExpectedValueSet(expectedValue);
        }
    }
}
