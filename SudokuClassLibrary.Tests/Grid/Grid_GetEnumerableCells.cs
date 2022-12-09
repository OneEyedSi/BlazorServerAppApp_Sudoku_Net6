using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System.Collections;

namespace SudokuClassLibrary.Tests.Grid
{
    public class Grid_GetEnumerableCells
    {
        [Fact]
        public void Should_Return_81_Cells()
        {
            // Arrange
            Sudoku.Grid grid = new();

            // Act
            var enumerableCells = grid.GetEnumerableCells();

            // Assert
            enumerableCells.Should().NotBeNullOrEmpty().And.HaveCount(81);
        }
        [Fact]
        public void Should_Return_Unique_Cells()
        {
            // Arrange
            Sudoku.Grid grid = new();

            // Act
            var enumerableCells = grid.GetEnumerableCells();

            // Assert
            enumerableCells.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void Should_Return_Cells_With_Correct_Row_And_Column_Values()
        {
            // Arrange
            Sudoku.Grid grid = new();

            // Act
            var enumerableCells = grid.GetEnumerableCells();

            // Assert
            enumerableCells.Should()
                .OnlyContain(c => c.Row >= 0 && c.Row <= 8
                            && c.Column >= 0 && c.Column <= 8);
        }
    }
}
