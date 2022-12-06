using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Data.Common;
using System.Text.RegularExpressions;
using System;

namespace SudokuClassLibrary.Tests.Grid
{
    public class Grid_Constructor
    {
        [Fact]
        public void Should_Create_9x9_CellArray()
        {
            // Arrange

            // Act
            Sudoku.Grid grid = new();

            // Assert
            var cellsArray = grid.Cells;
            int numberRows = cellsArray.GetLength(0);
            numberRows.Should().Be(9);
            int numberColumns = cellsArray.GetLength(1);
            numberColumns.Should().Be(9);
        }

        [Fact]
        public void Should_Populate_CellArray()
        {
            // Arrange

            // Act
            Sudoku.Grid grid = new();

            // Assert
            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    var cell = grid.Cells[row, column];
                    cell.Should().NotBeNull();
                }
            }
        }

        [Fact]
        public void Should_Add_9_CellGroups_For_Rows()
        {
            // Arrange

            // Act
            Sudoku.Grid grid = new();

            // Assert
            for (int row = 0; row < 9; row++)
            {
                CellGroupType groupType = CellGroupType.Row;
                int index = row;

                var group = grid.Groups.FirstOrDefault(cg => cg.GroupType == groupType && cg.Index == index);
                group.Should().NotBeNull();

                var groupCells = group.Cells;
                groupCells.Should().NotBeNullOrEmpty()
                    .And.HaveCount(9)
                    .And.OnlyContain(c => c.Position.Row == row
                                        && c.Position.Column >= 0 && c.Position.Column < 9);
            }
        }

        [Fact]
        public void Should_Add_9_CellGroups_For_Columns()
        {
            // Arrange

            // Act
            Sudoku.Grid grid = new();

            // Assert
            for (int column = 0; column < 9; column++)
            {
                CellGroupType groupType = CellGroupType.Column;
                int index = column;

                var group = grid.Groups.FirstOrDefault(cg => cg.GroupType == groupType && cg.Index == index);
                group.Should().NotBeNull();

                var groupCells = group.Cells;
                groupCells.Should().NotBeNullOrEmpty()
                    .And.HaveCount(9)
                    .And.OnlyContain(c => c.Position.Row >= 0 && c.Position.Row < 9 
                                        && c.Position.Column == column);
            }
        }

        [Fact]
        public void Should_Add_9_CellGroups_For_Squares()
        {
            // Arrange

            // Act
            Sudoku.Grid grid = new();

            // Assert
            for (int row = 0; row < 9; row += 3)
            {
                for (int column = 0; column < 9; column += 3)
                {
                    CellGroupType groupType = CellGroupType.Square;
                    int index = (row * 9 + column) / 3; 

                    var group = grid.Groups.FirstOrDefault(cg => cg.GroupType == groupType && cg.Index == index);
                    group.Should().NotBeNull();

                    var groupCells = group.Cells;
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
        public void Should_NotAdd_CellGroups_For_Diagonals_ByDefault()
        {
            // Arrange

            // Act
            Sudoku.Grid grid = new();

            // Assert
            var group = grid.Groups.FirstOrDefault(cg => cg.GroupType == CellGroupType.Diagonal);
            group.Should().BeNull();
        }

        [Fact]
        public void Should_Add_CellGroups_For_Diagonals_When_isKillerSudoku_Set()
        {
            // Arrange

            // Act
            Sudoku.Grid grid = new(isKillerSodoku: true);

            // Assert
            var groups = grid.Groups.Where(cg => cg.GroupType == CellGroupType.Diagonal);
            groups.Should().NotBeNullOrEmpty()
                .And.HaveCount(2);
        }

        [Fact]
        public void Should_Add_CellGroup_For_PrimaryDiagonal_When_isKillerSudoku_Set()
        {
            // Arrange

            // Act
            Sudoku.Grid grid = new(isKillerSodoku: true);

            // Assert
            var group = grid.Groups.FirstOrDefault(cg => cg.GroupType == CellGroupType.Diagonal && cg.Index == 0);
            group.Should().NotBeNull();

            var groupCells = group.Cells;
            groupCells.Should().NotBeNullOrEmpty()
                .And.HaveCount(9)
                .And.OnlyContain(c => c.Position.Row >= 0 && c.Position.Row < 9
                                    && c.Position.Column == c.Position.Row);
        }

        [Fact]
        public void Should_Add_CellGroup_For_SecondaryDiagonal_When_isKillerSudoku_Set()
        {
            // Arrange

            // Act
            Sudoku.Grid grid = new(isKillerSodoku: true);

            // Assert
            var group = grid.Groups.FirstOrDefault(cg => cg.GroupType == CellGroupType.Diagonal && cg.Index == 1);
            group.Should().NotBeNull();

            var groupCells = group.Cells;
            groupCells.Should().NotBeNullOrEmpty()
                .And.HaveCount(9)
                .And.OnlyContain(c => c.Position.Row >= 0 && c.Position.Row < 9
                                    && c.Position.Column == (8 - c.Position.Row));
        }
    }
}
