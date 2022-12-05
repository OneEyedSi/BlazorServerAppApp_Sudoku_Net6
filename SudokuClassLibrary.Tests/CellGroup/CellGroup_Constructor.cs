using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;

namespace SudokuClassLibrary.Tests.CellGroup
{
    public class CellGroup_Constructor
    {
        [Fact]
        public void Should_Set_GroupType()
        {
            // Arrange
            CellGroupType groupType = CellGroupType.Square;
            int index = 2;

            // Act
            Sudoku.CellGroup cellGroup = new(groupType, index);

            // Assert
            cellGroup.GroupType.Should().Be(groupType);
        }

        [Fact]
        public void Should_Set_Index()
        {
            // Arrange
            CellGroupType groupType = CellGroupType.Square;
            int index = 2;

            // Act
            Sudoku.CellGroup cellGroup = new(groupType, index);

            // Assert
            cellGroup.Index.Should().Be(index);
        }

        [Fact]
        public void Should_Initialize_AvailableValues()
        {
            // Arrange
            CellGroupType groupType = CellGroupType.Square;
            int index = 2;

            // Act
            Sudoku.CellGroup cellGroup = new(groupType, index);

            // Assert
            cellGroup.GetAvailableValues().Should().NotBeNullOrEmpty()
                .And.HaveCount(9)
                .And.OnlyContain(pv => (pv >= 1 && pv <= 9));
        }
    }
}
