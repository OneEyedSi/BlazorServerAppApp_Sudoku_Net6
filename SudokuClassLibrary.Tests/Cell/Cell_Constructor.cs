using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace SudokuClassLibrary.Tests.Cell
{
    public class Cell_Constructor
    {
        [Fact]
        public void Should_Set_Position_When_RowAndColumn_Specified()
        {
            // Arrange
            int row = 1;
            int column = 2;

            // Act
            Sudoku.Cell cell = new(row, column);

            // Assert
            cell.Position.Should().NotBeNull();
            cell.Position.Row.Should().Be(row);
            cell.Position.Column.Should().Be(column);
        }

        [Fact]
        public void Should_Set_Position_When_PositionObject_Specified()
        {
            // Arrange
            int row = 2;
            int column = 3;
            Position position = new(row, column);

            // Act
            Sudoku.Cell cell = new(position);

            // Assert
            cell.Position.Should().NotBeNull();
            cell.Position.Row.Should().Be(row);
            cell.Position.Column.Should().Be(column);
        }

        [Fact]
        public void Should_Set_Value_When_InitialValue_Specified_With_RowAndColumn()
        {
            // Arrange
            int row = 1;
            int column = 2;
            int initialValue = 4;

            // Act
            Sudoku.Cell cell = new Sudoku.Cell(row, column, initialValue);

            // Assert
            cell.GetValue().Value.Should().Be(initialValue);
        }

        [Fact]
        public void Should_Set_Value_When_InitialValue_Specified_With_Position()
        {
            // Arrange
            int row = 1;
            int column = 2;
            int initialValue = 4;
            Position position = new(row, column);

            // Act
            Sudoku.Cell cell = new Sudoku.Cell(position, initialValue);

            // Assert
            cell.GetValue().HasValue.Should().BeTrue();
            cell.GetValue().Value.Should().Be(initialValue);
        }

        [Fact]
        public void Should_Set_IsInitialValue_When_InitialValue_Specified_With_RowAndColumn()
        {
            // Arrange
            int row = 1;
            int column = 2;
            int initialValue = 4;

            // Act
            Sudoku.Cell cell = new Sudoku.Cell(row, column, initialValue);

            // Assert
            cell.IsInitialValue.Should().BeTrue();
        }

        [Fact]
        public void Should_Set_IsInitialValue_When_InitialValue_Specified_With_Position()
        {
            // Arrange
            int row = 1;
            int column = 2;
            int initialValue = 4;
            Position position = new(row, column);

            // Act
            Sudoku.Cell cell = new Sudoku.Cell(position, initialValue);

            // Assert
            cell.IsInitialValue.Should().BeTrue();
        }

        [Fact]
        public void Should_Set_All_PossibleValues_When_InitialValue_NotSpecified_With_RowAndColumn()
        {
            // Arrange
            int row = 1;
            int column = 2;

            // Act
            Sudoku.Cell cell = new Sudoku.Cell(row, column);

            // Assert
            cell.GetPossibleValuesDictionary().ShouldHaveExpectedValuesSetToRange(minValue: 1, maxValue: 9);
        }

        [Fact]
        public void Should_Set_All_PossibleValues_When_InitialValue_NotSpecified_With_Position()
        {
            // Arrange
            int row = 1;
            int column = 2;
            Position position = new(row, column);

            // Act
            Sudoku.Cell cell = new Sudoku.Cell(position);

            // Assert
            cell.GetPossibleValuesDictionary().ShouldHaveExpectedValuesSetToRange(minValue: 1, maxValue: 9);
        }

        [Fact]
        public void Should_Clear_IsInitialValue_When_InitialValue_NotSpecified_With_RowAndColumn()
        {
            // Arrange
            int row = 1;
            int column = 2;

            // Act
            Sudoku.Cell cell = new Sudoku.Cell(row, column);

            // Assert
            cell.IsInitialValue.Should().BeFalse();
        }

        [Fact]
        public void Should_Clear_IsInitialValue_When_InitialValue_NotSpecified_With_Position()
        {
            // Arrange
            int row = 1;
            int column = 2;
            Position position = new(row, column);

            // Act
            Sudoku.Cell cell = new Sudoku.Cell(position);

            // Assert
            cell.IsInitialValue.Should().BeFalse();
        }
    }
}
