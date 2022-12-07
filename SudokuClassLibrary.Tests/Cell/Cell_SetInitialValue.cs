using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuClassLibrary.Tests.Cell
{
    public class Cell_SetInitialValue
    {
        [Fact]
        public void Should_NotThrow_When_SetInitialValue_Null()
        {
            // Arrange
            int? newValue = null;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);

            // Act
            Action act = () => cell.SetInitialValue(newValue);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_Clear_IsInitialValue_When_SetInitialValue_Null()
        {
            // Arrange
            int? newValue = null;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);

            // Act
            cell.SetInitialValue(newValue);

            // Assert
            cell.IsInitialValue.Should().BeFalse();
        }

        [Fact]
        public void Should_Clear_Value_When_SetInitialValue_Null()
        {
            // Arrange
            int? previousValue = 4;
            int? newValue = null;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);
            cell.Value = previousValue;

            // Act
            cell.SetInitialValue(newValue);

            // Assert
            cell.Value.HasValue.Should().BeFalse();
        }

        [Fact]
        public void Should_Set_All_PossibleValues_When_SetInitialValue_Null()
        {
            // Arrange
            int? newValue = null;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);

            // Act
            cell.SetInitialValue(newValue);

            // Assert
            cell.GetPossibleValuesDictionary().ShouldHaveExpectedValuesSetToRange(1, 9);
        }

        [Fact]
        public void Should_Throw_ArgumentException_When_SetInitialValue_LessThan_1()
        {
            // Arrange
            int? newValue = 0;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);

            // Act
            Action act = () => cell.SetInitialValue(newValue);

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("Invalid value: Value must be between 1 and 9.*");
        }

        [Fact]
        public void Should_Throw_ArgumentException_When_SetInitialValue_GreaterThan_9()
        {
            // Arrange
            int? newValue = 10;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);

            // Act
            Action act = () => cell.SetInitialValue(newValue);

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("Invalid value: Value must be between 1 and 9.*");
        }

        [Fact]
        public void Should_NotThrow_When_SetInitialValue_ValidValue()
        {
            // Arrange
            int? newValue = 4;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);

            // Act
            Action act = () => cell.SetInitialValue(newValue);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_Set_IsInitialValue_When_SetInitialValue_ValidValue()
        {
            // Arrange
            int? newValue = 4;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);

            // Act
            cell.SetInitialValue(newValue);

            // Assert
            cell.IsInitialValue.Should().BeTrue();
        }

        [Fact]
        public void Should_Set_Value_When_SetInitialValue_ValidValue()
        {
            // Arrange
            int? newValue = 4;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);
            int expectedValue = newValue.Value;

            // Act
            cell.SetInitialValue(newValue);

            // Assert
            cell.Value.ShouldHaveExpectedValue(expectedValue);
        }

        [Fact]
        public void Should_Set_HasValueSet_When_SetInitialValue_ValidValue()
        {
            // Arrange
            int? newValue = 4;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);

            // Act
            cell.SetInitialValue(newValue);

            // Assert
            cell.HasValueSet.Should().BeTrue();
        }

        [Fact]
        public void Should_Set_PossibleValues_EqualTo_Value_When_SetInitialValue_ValidValue()
        {
            // Arrange
            int? newValue = 4;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);
            int expectedValue = newValue.Value;

            // Act
            cell.SetInitialValue(newValue);

            // Assert
            cell.GetPossibleValuesDictionary().ShouldHaveExpectedValueSet(expectedValue);
        }
    }
}
