using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System.Linq;

namespace SudokuClassLibrary.Tests.Grid
{
    public class Grid_IsKillerSudoku
    {
        [Fact]
        public void Should_Add_2_CellGroups_For_Diagonals_When_IsKillerSudoku_Set()
        {
            // Arrange
            Sudoku.Grid grid = new();

            // Act
            grid.IsKillerSudoku = true;

            // Assert
            grid.Groups.Where(cg => cg.GroupType == CellGroupType.Diagonal)
                .Should().HaveCount(2);
        }

        [Fact]
        public void Should_Add_Single_CellGroup_For_PrimaryDiagonal_When_IsKillerSudoku_Set()
        {
            // Arrange
            Sudoku.Grid grid = new();

            // Act
            grid.IsKillerSudoku = true;

            // Assert
            grid.Groups.Where(cg => cg.GroupType == CellGroupType.Diagonal && cg.Index == 0)
                .Should().ContainSingle();
        }

        [Fact]
        public void Should_Have_CorrectCells_In_CellGroup_For_PrimaryDiagonal_When_IsKillerSudoku_Set()
        {
            // Arrange
            Sudoku.Grid grid = new();

            // Act
            grid.IsKillerSudoku = true;

            // Assert
            grid.ShouldHaveCorrectCellsInDiagonalCellGroup(0);
        }

        [Fact]
        public void Should_Set_Correct_ParentGroup_For_Cells_In_PrimaryDiagonalGroup_When_IsKillerSudoku_Set()
        {
            // ASSUMPTION: That another test checks the correct cells are in the group.
            //  ie that there is are links from the group to the correct cells.
            // This checks the opposite: The link from each cell back up to the group exists.

            // Arrange
            Sudoku.Grid grid = new();

            // Act
            grid.IsKillerSudoku = true;

            // Assert
            int groupIndex = 0;
            var group = grid.GetDiagonalGroup(groupIndex);

            var cellsThatShouldBeInGroup = grid.GetCellsThatShouldBeInDiagonal(groupIndex);
            foreach (var cell in cellsThatShouldBeInGroup)
            {
                var matchingParentGroups =
                    cell.ParentGroups.Where(pg => pg.GroupType == CellGroupType.Diagonal
                                            && pg.Index == groupIndex);
                matchingParentGroups
                    .Should().NotBeNull()
                    .And.ContainSingle();
                var matchingParentGroup = matchingParentGroups.First();
                matchingParentGroup.Should().BeSameAs(group);
            }
        }

        [Fact]
        public void Should_Add_Single_CellGroup_For_SecondaryDiagonal_When_IsKillerSudoku_Set()
        {
            // Arrange
            Sudoku.Grid grid = new();

            // Act
            grid.IsKillerSudoku = true;

            // Assert
            grid.Groups.Where(cg => cg.GroupType == CellGroupType.Diagonal && cg.Index == 1)
                .Should().ContainSingle();
        }

        [Fact]
        public void Should_Have_CorrectCells_In_CellGroup_For_SecondaryDiagonal_When_IsKillerSudoku_Set()
        {
            // Arrange
            Sudoku.Grid grid = new();

            // Act
            grid.IsKillerSudoku = true;


            // Assert
            grid.ShouldHaveCorrectCellsInDiagonalCellGroup(1);
        }

        [Fact]
        public void Should_Set_Correct_ParentGroup_For_Cells_In_SecondaryDiagonalGroup_When_IsKillerSudoku_Set()
        {
            // ASSUMPTION: That another test checks the correct cells are in the group.
            //  ie that there is are links from the group to the correct cells.
            // This checks the opposite: The link from each cell back up to the group exists.

            // Arrange
            Sudoku.Grid grid = new();

            // Act
            grid.IsKillerSudoku = true;


            // Assert
            int groupIndex = 1;
            var group = grid.GetDiagonalGroup(groupIndex);

            var cellsThatShouldBeInGroup = grid.GetCellsThatShouldBeInDiagonal(groupIndex);
            foreach (var cell in cellsThatShouldBeInGroup)
            {
                var matchingParentGroups =
                    cell.ParentGroups.Where(pg => pg.GroupType == CellGroupType.Diagonal
                                            && pg.Index == groupIndex);
                matchingParentGroups
                    .Should().NotBeNull()
                    .And.ContainSingle();
                var matchingParentGroup = matchingParentGroups.First();
                matchingParentGroup.Should().BeSameAs(group);
            }
        }

        [Fact]
        public void Should_NotAdd_NewCellGroups_For_Diagonals_When_IsKillerSudoku_Set_And_DiagonalGroups_AlreadyExist()
        {
            // Arrange
            Sudoku.Grid grid = new(isKillerSodoku: true);

            // Act
            grid.IsKillerSudoku = true;

            // Assert
            grid.Groups.Where(cg => cg.GroupType == CellGroupType.Diagonal)
                .Should().HaveCount(2);
        }

        [Fact]
        public void Should_NotAdd_NewCellGroups_For_Diagonals_When_IsKillerSudoku_Cleared()
        {
            // Arrange
            Sudoku.Grid grid = new();

            // Act
            grid.IsKillerSudoku = false;

            // Assert
            grid.Groups.Should().NotContain(cg => cg.GroupType == CellGroupType.Diagonal);
        }

        [Fact]
        public void Should_Remove_ExistingCellGroups_For_Diagonals_When_IsKillerSudoku_Cleared()
        {
            // Arrange
            Sudoku.Grid grid = new(isKillerSodoku: true);

            // Act
            grid.IsKillerSudoku = false;

            // Assert
            grid.Groups.Should().NotContain(cg => cg.GroupType == CellGroupType.Diagonal);
        }

        [Fact]
        public void Should_Remove_Existing_DiagonalParentGroups_For_Cells_On_Diagonals_When_IsKillerSudoku_Cleared()
        {
            // Arrange
            Sudoku.Grid grid = new(isKillerSodoku: true);

            // Act
            grid.IsKillerSudoku = false;

            // Assert
            var cellsOnDiagonals = grid.GetCellsThatShouldBeInDiagonal(0)
                                    .Concat(grid.GetCellsThatShouldBeInDiagonal(1));
            foreach (var cell in cellsOnDiagonals)
            {
                var matchingParentGroups =
                    cell.ParentGroups.Where(pg => pg.GroupType == CellGroupType.Diagonal
                    );
                matchingParentGroups.Should().BeEmpty();
            }
        }
    }
}
