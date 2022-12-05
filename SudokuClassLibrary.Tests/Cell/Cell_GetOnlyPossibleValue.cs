using System;
using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System.Collections.ObjectModel;

namespace SudokuClassLibrary.Tests.Cell
{
    public class Cell_GetOnlyPossibleValue
    {
        [Fact]
        public void Should_Return_Null_When_Multiple_PossibleValues()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);
            var possibleValues = new[] { 1, 2, 3 };
            cell.SetPossibleValues(possibleValues);

            // Act

            // Assert
            cell.GetOnlyPossibleValue().HasValue.Should().BeFalse();
        }

        [Fact]
        public void Should_Return_PossibleValue_When_Single_PossibleValue()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);
            var possibleValues = new[] { 1 };
            cell.SetPossibleValues(possibleValues);

            // Act

            // Assert
            cell.GetOnlyPossibleValue().Should().Be(1);
        }

        [Fact]
        public void Should_Return_Value_When_Value_Set()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);
            cell.Value = 4;

            // Act

            // Assert
            cell.GetOnlyPossibleValue().Should().Be(4);
        }

        [Fact]
        public void Should_Return_Null_When_Value_Cleared()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);
            cell.Value = 4;

            // Act
            cell.Value = null;

            // Assert
            cell.GetOnlyPossibleValue().HasValue.Should().BeFalse();
        }
    }
}
