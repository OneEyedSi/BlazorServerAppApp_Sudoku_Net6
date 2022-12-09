using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System.Linq;
using System.Collections.Generic;

namespace SudokuClassLibrary.Tests.CellGroup
{
    public class CellGroup_GetSquareIndexForRowAndColumn
    {
        [Fact]
        public void Should_Return_Negative1_For_Row_LessThan_0()
        {
            // Arrange

            // Act
            var index = Sudoku.CellGroup.GetSquareIndexForRowAndColumn(-1, 0);

            // Assert
            index.Should().Be(-1);
        }

        [Fact]
        public void Should_Return_Negative1_For_Column_LessThan_0()
        {
            // Arrange

            // Act
            var index = Sudoku.CellGroup.GetSquareIndexForRowAndColumn(0, -1);

            // Assert
            index.Should().Be(-1);
        }

        [Fact]
        public void Should_Return_Negative1_For_Row_GreaterThan_8()
        {
            // Arrange

            // Act
            var index = Sudoku.CellGroup.GetSquareIndexForRowAndColumn(9, 0);

            // Assert
            index.Should().Be(-1);
        }

        [Fact]
        public void Should_Return_Negative1_For_Column_GreaterThan_8()
        {
            // Arrange

            // Act
            var index = Sudoku.CellGroup.GetSquareIndexForRowAndColumn(0, 9);

            // Assert
            index.Should().Be(-1);
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public void Should_Return_Correct_Index_For_Specified_Row_And_Column(PositionValue positionAndExpectedValue)
        {
            // Arrange
            var inputPosition = positionAndExpectedValue.Position;
            var inputRow = inputPosition.Row;
            var inputColumn = inputPosition.Column;
#pragma warning disable CS8629 // Nullable value type may be null.
            int expectedIndex = positionAndExpectedValue.Value.Value;
#pragma warning restore CS8629 // Nullable value type may be null.

            // Act
            var actualIndex = Sudoku.CellGroup.GetSquareIndexForRowAndColumn(inputRow, inputColumn);

            // Assert
            actualIndex.Should().Be(expectedIndex, 
                $"because row {inputRow} and column {inputColumn} should generate index of {expectedIndex}");
        }

        public static TheoryData<PositionValue> GetTestData =>
            new TheoryData<PositionValue>
            {
                new PositionValue(new Position(row: 0, column: 0), value: 0),
                new(new(0, 1), 0),
                new(new(0, 2), 0),
                new(new(0, 3), 1),
                new(new(0, 4), 1),
                new(new(0, 5), 1),
                new(new(0, 6), 2),
                new(new(0, 7), 2),
                new(new(0, 8), 2),

                new(new(1, 0), 0),
                new(new(1, 1), 0),
                new(new(1, 2), 0),
                new(new(1, 3), 1),
                new(new(1, 4), 1),
                new(new(1, 5), 1),
                new(new(1, 6), 2),
                new(new(1, 7), 2),
                new(new(1, 8), 2),

                new(new(2, 0), 0),
                new(new(2, 1), 0),
                new(new(2, 2), 0),
                new(new(2, 3), 1),
                new(new(2, 4), 1),
                new(new(2, 5), 1),
                new(new(2, 6), 2),
                new(new(2, 7), 2),
                new(new(2, 8), 2),

                new(new(3, 0), 3),
                new(new(3, 1), 3),
                new(new(3, 2), 3),
                new(new(3, 3), 4),
                new(new(3, 4), 4),
                new(new(3, 5), 4),
                new(new(3, 6), 5),
                new(new(3, 7), 5),
                new(new(3, 8), 5),

                new(new(4, 0), 3),
                new(new(4, 1), 3),
                new(new(4, 2), 3),
                new(new(4, 3), 4),
                new(new(4, 4), 4),
                new(new(4, 5), 4),
                new(new(4, 6), 5),
                new(new(4, 7), 5),
                new(new(4, 8), 5),

                new(new(5, 0), 3),
                new(new(5, 1), 3),
                new(new(5, 2), 3),
                new(new(5, 3), 4),
                new(new(5, 4), 4),
                new(new(5, 5), 4),
                new(new(5, 6), 5),
                new(new(5, 7), 5),
                new(new(5, 8), 5),

                new(new(6, 0), 6),
                new(new(6, 1), 6),
                new(new(6, 2), 6),
                new(new(6, 3), 7),
                new(new(6, 4), 7),
                new(new(6, 5), 7),
                new(new(6, 6), 8),
                new(new(6, 7), 8),
                new(new(6, 8), 8),

                new(new(7, 0), 6),
                new(new(7, 1), 6),
                new(new(7, 2), 6),
                new(new(7, 3), 7),
                new(new(7, 4), 7),
                new(new(7, 5), 7),
                new(new(7, 6), 8),
                new(new(7, 7), 8),
                new(new(7, 8), 8),

                new(new(8, 0), 6),
                new(new(8, 1), 6),
                new(new(8, 2), 6),
                new(new(8, 3), 7),
                new(new(8, 4), 7),
                new(new(8, 5), 7),
                new(new(8, 6), 8),
                new(new(8, 7), 8),
                new(new(8, 8), 8)
            };
    }
}
