using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System.Linq;

namespace SudokuClassLibrary.Tests.Cell
{
    public class Cell_GetNumberOfPossibleValues
    {
        [Fact]
        public void Should_Return_9_After_Cell_Initialized()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);
            int expectedNumber = 9;

            // Act
            int actualNumberOfPossibleValues = cell.GetNumberOfPossibleValues();

            // Assert
            actualNumberOfPossibleValues.Should().Be(expectedNumber);
        }

        [Fact]
        public void Should_Return_Correct_Number_When_MultiplePossibleValues()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);
            int minValue = 2;
            int maxValue = 5;
            cell.SetPossibleValueRange(minValue, maxValue);
            int expectedNumber = maxValue - minValue + 1;

            // Act
            int actualNumberOfPossibleValues = cell.GetNumberOfPossibleValues();

            // Assert
            actualNumberOfPossibleValues.Should().Be(expectedNumber);
        }

        [Fact]
        public void Should_Return_1_After_Cell_Value_Set()
        {
            // Arrange
            int? newValue = 4;
            Sudoku.Cell cell = new(1, 1);
            int expectedNumber = 1;

            // Act
            cell.Value = newValue;
            int actualNumberOfPossibleValues = cell.GetNumberOfPossibleValues();

            // Assert
            actualNumberOfPossibleValues.Should().Be(expectedNumber);
        }
    }
}
