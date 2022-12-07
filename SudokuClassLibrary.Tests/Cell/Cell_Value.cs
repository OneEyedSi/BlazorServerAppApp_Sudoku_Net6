using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System;
using FluentAssertions.Events;
using System.Collections.Generic;
using System.Linq;

namespace SudokuClassLibrary.Tests.Cell
{
    public class Cell_Value
    {
        [Fact]
        public void Should_NotThrow_When_Value_SetNull()
        {
            // Arrange
            int? newValue = null;
            Sudoku.Cell cell = new(1, 1);

            // Act
            Action act = () => cell.Value = newValue;

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_Clear_HasValueSet_When_Value_SetNull()
        {
            // Arrange
            int? newValue = null;
            Sudoku.Cell cell = new(1, 1);

            // Act
            cell.Value = newValue;

            // Assert
            cell.HasValueSet.Should().BeFalse();
        }

        [Fact]
        public void Should_Set_All_PossibleValues_When_Value_SetNull()
        {
            // Arrange
            int? newValue = null;
            Sudoku.Cell cell = new(1, 1);

            // Act
            cell.Value = newValue;

            // Assert
            cell.GetPossibleValuesDictionary().ShouldHaveExpectedValuesSetToRange(1, 9);
        }

        [Fact]
        public void Should_Throw_ArgumentException_When_Value_Set_LessThan_1()
        {
            // Arrange
            int? newValue = 0;
            Sudoku.Cell cell = new(1, 1);

            // Act
            Action act = () => cell.Value = newValue;

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("Invalid value: Value must be between 1 and 9.*");
        }

        [Fact]
        public void Should_Throw_ArgumentException_When_Value_Set_GreaterThan_9()
        {
            // Arrange
            int? newValue = 10;
            Sudoku.Cell cell = new(1, 1);

            // Act
            Action act = () => cell.Value = newValue;

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("Invalid value: Value must be between 1 and 9.*");
        }

        [Fact]
        public void Should_Change_Value_When_Set_ValidValue()
        {
            // Arrange
            int? newValue = 4;
            Sudoku.Cell cell = new(1, 1);
            int expectedValue = newValue.Value;

            // Act
            cell.Value = newValue;

            // Assert
            cell.Value.ShouldHaveExpectedValue(expectedValue);
        }

        [Fact]
        public void Should_Set_HasValueSet_When_Set_ValidValue()
        {
            // Arrange
            int? newValue = 4;
            Sudoku.Cell cell = new(1, 1);

            // Act
            cell.Value = newValue;

            // Assert
            cell.HasValueSet.Should().BeTrue();
        }

        [Fact]
        public void Should_Set_PossibleValues_EqualTo_Value_When_Set_ValidValue()
        {
            // Arrange
            int? newValue = 4;
            Sudoku.Cell cell = new(1, 1);
            int expectedValue = newValue.Value;

            // Act
            cell.Value = newValue;

            // Assert
            cell.GetPossibleValuesDictionary().ShouldHaveExpectedValueSet(expectedValue);
        }

        [Fact]
        public void Should_RaiseEvent_CellValueChanged_When_NewValue_DifferentFrom_PreviousValue()
        {
            // Arrange
            int? newValue = 4;
            Sudoku.Cell cell = new(1, 1);
            using IMonitor<Sudoku.Cell> monitoredCell = cell.Monitor();

            // Act
            cell.Value = newValue;

            // Assert
            monitoredCell.Should()
                .Raise("CellValueChanged")
                .WithSender(cell);
        }

        [Fact]
        public void Should_Set_NewAndPreviousValues_When_RaiseEvent_CellValueChanged()
        {
            // Arrange
            int? newValue = 4;
            Sudoku.Cell cell = new(1, 1);
            using IMonitor<Sudoku.Cell> monitoredCell = cell.Monitor();

            // Act
            cell.Value = newValue;

            // Assert
            monitoredCell.Should()
                .Raise("CellValueChanged")
                .WithArgs<CellValueChangedEventArgs>(args => args.PreviousValue == null && args.NewValue == newValue.Value);
        }

        [Fact]
        public void Should_NotRaiseEvent_CellValueChanged_When_NewValue_SameAs_PreviousValue()
        {
            // Arrange
            int? previousValue = 4;
            int? newValue = 4;
            Sudoku.Cell cell = new(1, 1);
            cell.Value = previousValue;
            using IMonitor<Sudoku.Cell> monitoredCell = cell.Monitor();

            // Act
            cell.Value = newValue;

            // Assert
            monitoredCell.Should()
                .NotRaise("CellValueChanged");
        }

        [Fact]
        public void Should_Not_Change_Value_When_IsInitialValue_Set()
        {
            // Arrange
            int? initialValue = 4;
            int? newValue = 5;
            Sudoku.Cell cell = new(1, 1);
            cell.Value = initialValue;
            cell.IsInitialValue = true;
            int expectedValue = initialValue.Value;

            // Act
            cell.Value = newValue;

            // Assert
            cell.Value.ShouldHaveExpectedValue(expectedValue);
        }
    }
}
