using System;
using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System.Collections.ObjectModel;

namespace SudokuClassLibrary.Tests.Cell
{
    public class Cell_HasSinglePossibleValue
    {
        [Fact]
        public void Should_Clear_HasSinglePossibleValue_When_Multiple_PossibleValues()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);
            var possibleValues = new[] { 1, 2, 3 };
            cell.SetPossibleValues(possibleValues);

            // Act

            // Assert
            cell.HasSinglePossibleValue.Should().BeFalse();
        }

        [Fact]
        public void Should_Set_HasSinglePossibleValue_When_Single_PossibleValue()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);
            var possibleValues = new[] { 1 };
            cell.SetPossibleValues(possibleValues);

            // Act

            // Assert
            cell.HasSinglePossibleValue.Should().BeTrue();
        }

        [Fact]
        public void Should_Set_HasSinglePossibleValue_When_Value_Set()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);
            cell.Value = 4;

            // Act

            // Assert
            cell.HasSinglePossibleValue.Should().BeTrue();
        }
        [Fact]
        public void Should_Clear_HasSinglePossibleValue_When_Value_Cleared()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);
            cell.Value = 4;

            // Act
            cell.Value = null;

            // Assert
            cell.HasSinglePossibleValue.Should().BeFalse();
        }
    }
}
