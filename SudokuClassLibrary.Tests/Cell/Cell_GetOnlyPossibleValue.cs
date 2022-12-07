using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;

namespace SudokuClassLibrary.Tests.Cell
{
    public class Cell_GetOnlyPossibleValue
    {
        [Fact]
        public void Should_Return_Null_When_Multiple_PossibleValues()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);
            int minValue = 2;
            int maxValue = 5;
            cell.SetPossibleValueRange(minValue, maxValue);

            // Act
            int? onlyPossibleValue = cell.GetOnlyPossibleValue();

            // Assert
            onlyPossibleValue.HasValue.Should().BeFalse();
        }

        [Fact]
        public void Should_Return_PossibleValue_When_Single_PossibleValue()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);
            int possibleValue = 4;
            cell.SetPossibleValue(possibleValue);

            // Act
            int? onlyPossibleValue = cell.GetOnlyPossibleValue();

            // Assert
            onlyPossibleValue.Should().Be(possibleValue);
        }

        [Fact]
        public void Should_Return_Value_When_Value_Set()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);
            int newValue = 4;
            cell.Value = newValue;

            // Act
            int? onlyPossibleValue = cell.GetOnlyPossibleValue();

            // Assert
            onlyPossibleValue.Should().Be(newValue);
        }

        [Fact]
        public void Should_Return_Null_When_Value_Cleared()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);
            cell.Value = 4;

            // Act
            cell.Value = null;
            int? onlyPossibleValue = cell.GetOnlyPossibleValue();

            // Assert
            onlyPossibleValue.HasValue.Should().BeFalse();
        }
    }
}
