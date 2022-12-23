using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;

namespace SudokuClassLibrary.Tests.CellGroup
{
    public class CellGroup_Cell_CellValueChanged
    {
        [Fact]
        public void Should_Remove_Value_From_AvailableValues_When_ChildCell_Value_Set()
        {
            // Arrange
            Sudoku.CellGroup cellGroup = new(CellGroupType.Row, 0);
            Sudoku.Cell cell = new(1, 1);
            cellGroup.AddCell(cell);

            // Act
            cell.SetValue(4);

            // Assert
            cellGroup.GetAvailableValues().Should().NotBeNullOrEmpty()
                .And.HaveCount(8)
                .And.OnlyContain(pv => (pv >= 1 && pv <= 3) || (pv >= 5 && pv <= 9));
        }
        [Fact]
        public void Should_Restore_Value_To_AvailableValues_When_ChildCell_Value_Cleared()
        {
            // Arrange
            Sudoku.CellGroup cellGroup = new(CellGroupType.Row, 0);
            Sudoku.Cell cell = new(1, 1);
            cellGroup.AddCell(cell);
            cell.SetValue(4);

            // Act
            cell.SetValue(null);

            // Assert
            cellGroup.GetAvailableValues().Should().NotBeNullOrEmpty()
                .And.HaveCount(9)
                .And.OnlyContain(pv => (pv >= 1 && pv <= 9));
        }
    }
}
