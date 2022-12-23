using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System.Linq;
using System.Data.Common;
using System.Collections;

namespace SudokuClassLibrary.Tests.Grid
{
    public class Grid_ResetGame
    {
        [Fact]
        public void Should_ResultIn_9x9_CellArray()
        {
            // Arrange
            Sudoku.Grid grid = new();

            // Act
            grid.ResetGame();

            // Assert
            var cellsArray = grid.Cells;
            int numberRows = cellsArray.GetLength(0);
            numberRows.Should().Be(9);
            int numberColumns = cellsArray.GetLength(1);
            numberColumns.Should().Be(9);
        }

        [Fact]
        public void Should_ResultIn_No_Null_Cells_In_CellArray()
        {
            // Arrange
            Sudoku.Grid grid = new();

            // Act
            grid.ResetGame();

            // Assert
            grid.GetEnumerableCells().Should().NotContainNulls();
        }

        [Fact]
        public void Should_ResultIn_9_CellGroups_For_Rows()
        {
            // Arrange
            Sudoku.Grid grid = new();

            // Act
            grid.ResetGame();

            // Assert
            for (int row = 0; row<= 8; row++)
            {
                CellGroupType groupType = CellGroupType.Row;
                int index = row;

                var group = grid.Groups.FirstOrDefault(cg => cg.GroupType == groupType && cg.Index == index);
                group.Should().NotBeNull();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                var groupCells = group.Cells;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                groupCells.Should().NotBeNullOrEmpty()
                    .And.HaveCount(9)
                    .And.OnlyContain(c => c.Position.Row == row
                                        && c.Position.Column >= 0 && c.Position.Column<= 8);
            }
        }

        [Fact]
        public void Should_ResultIn_9_CellGroups_For_Columns()
        {
            // Arrange
            Sudoku.Grid grid = new();

            // Act
            grid.ResetGame();

            // Assert
            for (int column = 0; column<= 8; column++)
            {
                CellGroupType groupType = CellGroupType.Column;
                int index = column;

                var group = grid.Groups.FirstOrDefault(cg => cg.GroupType == groupType && cg.Index == index);
                group.Should().NotBeNull();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                var groupCells = group.Cells;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                groupCells.Should().NotBeNullOrEmpty()
                    .And.HaveCount(9)
                    .And.OnlyContain(c => c.Position.Row >= 0 && c.Position.Row<= 8
                                        && c.Position.Column == column);
            }
        }

        [Fact]
        public void Should_ResultIn_9_CellGroups_For_Squares()
        {
            // Arrange
            Sudoku.Grid grid = new();

            // Act
            grid.ResetGame();

            // Assert
            for (int row = 0; row<= 8; row += 3)
            {
                for (int column = 0; column<= 8; column += 3)
                {
                    CellGroupType groupType = CellGroupType.Square;
                    int index = Sudoku.CellGroup.GetSquareIndexForRowAndColumn(row, column);

                    var group = grid.Groups.FirstOrDefault(cg => cg.GroupType == groupType && cg.Index == index);
                    group.Should().NotBeNull();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    var groupCells = group.Cells;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                    int endRow = row + 2;
                    int endColumn = column + 2;
                    groupCells.Should().NotBeNullOrEmpty()
                        .And.HaveCount(9)
                        .And.OnlyContain((c => c.Position.Row >= row && c.Position.Row <= endRow
                                            && c.Position.Column >= column && c.Position.Column <= endColumn),
                           "because Square {0} should contain cells between positions ({1}, {2}) and ({3}, {4})",
                            index, row, column, endRow, endColumn);
                }
            }
        }

        [Fact]
        public void Should_Remove_CellGroups_For_Diagonals_When_IsKillerSudoku_False()
        {
            // Arrange
            Sudoku.Grid grid = new(isKillerSodoku: true);

            // Act
            grid.IsKillerSudoku = false;
            grid.ResetGame();

            // Assert
            var group = grid.Groups.FirstOrDefault(cg => cg.GroupType == CellGroupType.Diagonal);
            group.Should().BeNull();
        }

        [Fact]
        public void Should_ResultIn_2_CellGroups_For_Diagonals_When_IsKillerSudoku_Set()
        {
            // Arrange
            Sudoku.Grid grid = new(isKillerSodoku: false);

            // Act
            grid.IsKillerSudoku = true;
            grid.ResetGame();

            // Assert
            grid.Groups.Where(cg => cg.GroupType == CellGroupType.Diagonal)
                .Should().HaveCount(2);
        }

        [Fact]
        public void Should_ResultIn_Single_CellGroup_For_PrimaryDiagonal_When_IsKillerSudoku_Set()
        {
            // Arrange
            Sudoku.Grid grid = new(isKillerSodoku: false);

            // Act
            grid.IsKillerSudoku = true;
            grid.ResetGame();

            // Assert
            grid.Groups.Where(cg => cg.GroupType == CellGroupType.Diagonal && cg.Index == 0)
                .Should().ContainSingle();
        }

        [Fact]
        public void Should_ResultIn_CorrectCells_In_CellGroup_For_PrimaryDiagonal_When_IsKillerSudoku_Set()
        {
            // Arrange
            Sudoku.Grid grid = new(isKillerSodoku: false);

            // Act
            grid.IsKillerSudoku = true;
            grid.ResetGame();

            // Assert
            grid.ShouldHaveCorrectCellsInDiagonalCellGroup(0);
        }

        [Fact]
        public void Should_ResultIn_Correct_ParentGroup_For_Cells_In_PrimaryDiagonalGroup_When_IsKillerSudoku_Set()
        {
            // ASSUMPTION: That another test checks the correct cells are in the group.
            //  ie that there is are links from the group to the correct cells.
            // This checks the opposite: The link from each cell back up to the group exists.

            // Arrange
            Sudoku.Grid grid = new(isKillerSodoku: false);

            // Act
            grid.IsKillerSudoku = true;
            grid.ResetGame();

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
        public void Should_ResultIn_Single_CellGroup_For_SecondaryDiagonal_When_IsKillerSudoku_Set()
        {
            // Arrange
            Sudoku.Grid grid = new(isKillerSodoku: false);

            // Act
            grid.IsKillerSudoku = true;
            grid.ResetGame();

            // Assert
            grid.Groups.Where(cg => cg.GroupType == CellGroupType.Diagonal && cg.Index == 1)
                .Should().ContainSingle();
        }

        [Fact]
        public void Should_ResultIn_CorrectCells_In_CellGroup_For_SecondaryDiagonal_When_IsKillerSudoku_Set()
        {
            // Arrange
            Sudoku.Grid grid = new(isKillerSodoku: false);

            // Act
            grid.IsKillerSudoku = true;
            grid.ResetGame();


            // Assert
            grid.ShouldHaveCorrectCellsInDiagonalCellGroup(1);
        }

        [Fact]
        public void Should_ResultIn_Correct_ParentGroup_For_Cells_In_SecondaryDiagonalGroup_When_IsKillerSudoku_Set()
        {
            // ASSUMPTION: That another test checks the correct cells are in the group.
            //  ie that there is are links from the group to the correct cells.
            // This checks the opposite: The link from each cell back up to the group exists.

            // Arrange
            Sudoku.Grid grid = new(isKillerSodoku: false);

            // Act
            grid.IsKillerSudoku = true;
            grid.ResetGame();


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
        public void Should_Remove_CellValues_When_PreviouslySet()
        {
            // Arrange
            Sudoku.Grid grid = new();
            // Set every cell value, ensuring that the same values do not appear in the same row 
            // or column.
            for (int row = 0; row<= 8; row++)
            {
                for (int column = 0; column<= 8; column++) 
                { 
                    var cell = grid.Cells[row, column];
                    cell.SetValue(((column + row) % 9) + 1);
                }
            }

            int numberOfCellsWithValuesBefore = grid.GetEnumerableCells().Count(c => c.GetValue().HasValue);

            // Act
            grid.ResetGame();

            // Assert
            int numberOfCellsWithValuesAfter = grid.GetEnumerableCells().Count(c => c.GetValue().HasValue);
            numberOfCellsWithValuesBefore.Should().Be(81);
            numberOfCellsWithValuesAfter.Should().Be(0);
        }

        [Fact]
        public void Should_NotAdd_NewCellGroups_For_Diagonals_When_IsKillerSudoku_Cleared()
        {
            // Arrange
            Sudoku.Grid grid = new(isKillerSodoku: false);

            // Act
            grid.IsKillerSudoku = false;
            grid.ResetGame();

            // Assert
            grid.Groups.Should().NotContain(cg => cg.GroupType == CellGroupType.Diagonal);
        }

        [Fact]
        public void Should_Remove_ExistingCellGroups_For_Diagonals_When_IsKillerSudoku_Cleared()
        {
            // Arrange
            Sudoku.Grid grid = new(isKillerSodoku: true);
            int numberOfCellGroupsBefore = grid.Groups.Count(cg => cg.GroupType == CellGroupType.Diagonal);

            // Act
            grid.IsKillerSudoku = false;
            grid.ResetGame();

            // Assert
            int numberOfCellGroupsAfter = grid.Groups.Count(cg => cg.GroupType == CellGroupType.Diagonal);
            numberOfCellGroupsBefore.Should().Be(2);
            numberOfCellGroupsAfter.Should().Be(0);
        }

        [Fact]
        public void Should_Remove_Existing_DiagonalParentGroups_For_Cells_On_Diagonals_When_IsKillerSudoku_Cleared()
        {
            // Arrange
            Sudoku.Grid grid = new(isKillerSodoku: true);
            var cellsOnDiagonals = grid.GetCellsThatShouldBeInDiagonal(0)
                                    .Concat(grid.GetCellsThatShouldBeInDiagonal(1));
            // The centre cell appears twice, since it's on both diagonals.  Use Distinct to only operate on it once.
            int numberOfCellsWithDiagonalGroupsBefore =
                cellsOnDiagonals.Distinct().Sum(c => c.ParentGroups.Count(pg => pg.GroupType == CellGroupType.Diagonal));

            // Act
            grid.IsKillerSudoku = false;
            grid.ResetGame();

            // Assert
            int numberOfCellsWithDiagonalGroupsAfter =
                cellsOnDiagonals.Sum(c => c.ParentGroups.Count(pg => pg.GroupType == CellGroupType.Diagonal));
            numberOfCellsWithDiagonalGroupsBefore.Should().Be(18);
            numberOfCellsWithDiagonalGroupsAfter.Should().Be(0);
        }
    }
}
