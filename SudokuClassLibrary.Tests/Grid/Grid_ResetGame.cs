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
            grid.Cells.Cast<Sudoku.Cell>().Should().NotContainNulls();
        }

        [Fact]
        public void Should_ResultIn_9_CellGroups_For_Rows()
        {
            // Arrange
            Sudoku.Grid grid = new();

            // Act
            grid.ResetGame();

            // Assert
            for (int row = 0; row < 9; row++)
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
                                        && c.Position.Column >= 0 && c.Position.Column < 9);
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
            for (int column = 0; column < 9; column++)
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
                    .And.OnlyContain(c => c.Position.Row >= 0 && c.Position.Row < 9
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
            for (int row = 0; row < 9; row += 3)
            {
                for (int column = 0; column < 9; column += 3)
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
        public void Should_ResultIn_CellGroups_For_Diagonals_When_IsKillerSudoku_Set()
        {
            // Arrange
            Sudoku.Grid grid = new(isKillerSodoku: false);

            // Act
            grid.IsKillerSudoku = true;
            grid.ResetGame();

            // Assert
            var groups = grid.Groups.Where(cg => cg.GroupType == CellGroupType.Diagonal);
            groups.Should().NotBeNullOrEmpty()
                .And.HaveCount(2);
        }

        [Fact]
        public void Should_ResultIn_CellGroup_For_PrimaryDiagonal_When_IsKillerSudoku_Set()
        {
            // Arrange
            Sudoku.Grid grid = new(isKillerSodoku: false);

            // Act
            grid.IsKillerSudoku = true;
            grid.ResetGame();

            // Assert
            var group = grid.Groups.FirstOrDefault(cg => cg.GroupType == CellGroupType.Diagonal && cg.Index == 0);
            group.Should().NotBeNull();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var groupCells = group.Cells;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            groupCells.Should().NotBeNullOrEmpty()
                .And.HaveCount(9)
                .And.OnlyContain(c => c.Position.Row >= 0 && c.Position.Row < 9
                                    && c.Position.Column == c.Position.Row);
        }

        [Fact]
        public void Should_ResultIn_CellGroup_For_SecondaryDiagonal_When_IsKillerSudoku_Set()
        {
            // Arrange
            Sudoku.Grid grid = new(isKillerSodoku: false);

            // Act
            grid.IsKillerSudoku = true;
            grid.ResetGame();

            // Assert
            var group = grid.Groups.FirstOrDefault(cg => cg.GroupType == CellGroupType.Diagonal && cg.Index == 1);
            group.Should().NotBeNull();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var groupCells = group.Cells;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            groupCells.Should().NotBeNullOrEmpty()
                .And.HaveCount(9)
                .And.OnlyContain(c => c.Position.Row >= 0 && c.Position.Row < 9
                                    && c.Position.Column == (8 - c.Position.Row));
        }

        [Fact]
        public void Should_Remove_CellValues_When_PreviouslySet()
        {
            // Arrange
            Sudoku.Grid grid = new();
            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++) 
                { 
                    var cell = grid.Cells[row, column];
                    cell.Value = ((column + row) % 9) + 1;
                }
            }

            int numberOfCellsWithValuesBefore = grid.Cells.Cast<Sudoku.Cell>().Count(c => c.Value.HasValue);

            // Act
            grid.ResetGame();

            // Assert
            int numberOfCellsWithValuesAfter = grid.Cells.Cast<Sudoku.Cell>().Count(c => c.Value.HasValue);
            numberOfCellsWithValuesBefore.Should().Be(81);
            numberOfCellsWithValuesAfter.Should().Be(0);
        }
    }
}
