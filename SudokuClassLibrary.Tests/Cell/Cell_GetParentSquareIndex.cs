using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;

namespace SudokuClassLibrary.Tests.Cell
{
    public class Cell_GetParentSquareIndex
    {
        [Fact]
        public void Should_Return_Null_When_ParentGroups_NotSet()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);

            // Act
            int? parentSquareIndex = cell.GetParentSquareIndex();

            // Assert
            parentSquareIndex.HasValue.Should().BeFalse();
        }

        [Fact]
        public void Should_Return_Null_When_ParentGroups_DoNotInclude_Square()
        {
            // Arrange
            Sudoku.CellGroup parentRowGroup = new(CellGroupType.Row, 1);
            Sudoku.CellGroup parentColumnGroup = new(CellGroupType.Column, 1);
            Sudoku.Cell cell = new(1, 1);
            cell.ParentGroups.Add(parentRowGroup);
            cell.ParentGroups.Add(parentColumnGroup);

            // Act
            int? parentSquareIndex = cell.GetParentSquareIndex();

            // Assert
            parentSquareIndex.HasValue.Should().BeFalse();
        }

        [Fact]
        public void Should_Return_CurrentIndex_When_ParentGroups_Include_Square()
        {
            // Arrange
            int row = 4;
            int column = 4;
            int squareIndex = 4;
            Sudoku.CellGroup parentRowGroup = new(CellGroupType.Row, row);
            Sudoku.CellGroup parentColumnGroup = new(CellGroupType.Column, column);
            Sudoku.CellGroup parentSquareGroup = new(CellGroupType.Square, squareIndex);
            Sudoku.Cell cell = new(row, column);
            cell.ParentGroups.Add(parentRowGroup);
            cell.ParentGroups.Add(parentColumnGroup);
            cell.ParentGroups.Add(parentSquareGroup);

            // Act
            int? parentSquareIndex = cell.GetParentSquareIndex();

            // Assert
            parentSquareIndex.Value.Should().Be(squareIndex);
        }
    }
}
