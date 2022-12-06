using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System.Linq;

namespace SudokuClassLibrary.Tests.Grid
{
    public class Grid_IsKillerSudoku
    {
        [Fact]
        public void Should_Add_CellGroups_For_Diagonals_When_IsKillerSudoku_Set()
        {
            // Arrange
            Sudoku.Grid grid = new();

            // Act
            grid.IsKillerSudoku = true;

            // Assert
            var groups = grid.Groups.Where(cg => cg.GroupType == CellGroupType.Diagonal);
            groups.Should().NotBeNullOrEmpty()
                .And.HaveCount(2);
        }

        [Fact]
        public void Should_NotAdd_NewCellGroups_For_Diagonals_When_IsKillerSudoku_Set_And_DiagonalGroups_AlreadyExist()
        {
            // Arrange
            Sudoku.Grid grid = new(isKillerSodoku: true);

            // Act
            grid.IsKillerSudoku = true;

            // Assert
            var groups = grid.Groups.Where(cg => cg.GroupType == CellGroupType.Diagonal);
            groups.Should().NotBeNullOrEmpty()
                .And.HaveCount(2);
        }

        [Fact]
        public void Should_NotAdd_NewCellGroups_For_Diagonals_When_IsKillerSudoku_False()
        {
            // Arrange
            Sudoku.Grid grid = new();

            // Act
            grid.IsKillerSudoku = false;

            // Assert
            var group = grid.Groups.FirstOrDefault(cg => cg.GroupType == CellGroupType.Diagonal);
            group.Should().BeNull();
        }

        [Fact]
        public void Should_Remove_ExistingCellGroups_For_Diagonals_When_IsKillerSudoku_False()
        {
            // Arrange
            Sudoku.Grid grid = new(isKillerSodoku: true);

            // Act
            grid.IsKillerSudoku = false;

            // Assert
            var group = grid.Groups.FirstOrDefault(cg => cg.GroupType == CellGroupType.Diagonal);
            group.Should().BeNull();
        }
    }
}
