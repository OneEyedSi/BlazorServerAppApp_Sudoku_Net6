using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;

namespace SudokuClassLibrary.Tests.Cell
{
    public class Cell_IsOnKillerDiagonal
    {
        [Fact]
        public void Should_Return_False_When_ParentGroups_NotSet()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);

            // Act
            bool isOnKillerDiagonal = cell.IsOnKillerDiagonal();

            // Assert
            isOnKillerDiagonal.Should().BeFalse();
        }

        [Fact]
        public void Should_Return_False_When_ParentGroups_DoNotInclude_Diagonal()
        {
            // Arrange
            Sudoku.CellGroup parentRowGroup = new(CellGroupType.Row, 1);
            Sudoku.CellGroup parentColumnGroup = new(CellGroupType.Column, 1);
            Sudoku.Cell cell = new(1, 1);
            cell.ParentGroups.Add(parentRowGroup);
            cell.ParentGroups.Add(parentColumnGroup);

            // Act
            bool isOnKillerDiagonal = cell.IsOnKillerDiagonal();

            // Assert
            isOnKillerDiagonal.Should().BeFalse();
        }

        [Fact]
        public void Should_Return_True_When_ParentGroups_Include_Diagonal()
        {
            // Arrange
            Sudoku.CellGroup parentRowGroup = new(CellGroupType.Row, 1);
            Sudoku.CellGroup parentColumnGroup = new(CellGroupType.Column, 1);
            Sudoku.CellGroup parentDiagonalGroup = new(CellGroupType.Diagonal, 0);            
            Sudoku.Cell cell = new(1, 1);
            cell.ParentGroups.Add(parentRowGroup);
            cell.ParentGroups.Add(parentColumnGroup);
            cell.ParentGroups.Add(parentDiagonalGroup);

            // Act
            bool isOnKillerDiagonal = cell.IsOnKillerDiagonal();

            // Assert
            isOnKillerDiagonal.Should().BeTrue();
        }
    }
}
