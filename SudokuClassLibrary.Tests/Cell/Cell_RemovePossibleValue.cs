using System;
using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System.Collections.ObjectModel;

namespace SudokuClassLibrary.Tests.Cell
{
    public class Cell_RemovePossibleValue
    {
        [Fact]
        public void Should_NotThrow_When_SuppliedValue_LessThan_1()
        {
            // Arrange
            int possibleValueToRemove = 0;
            Sudoku.Cell cell = new(1, 1);

            // Act
            Action act = () => cell.RemovePossibleValue(possibleValueToRemove);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_NotThrow_When_SuppliedValue_GreaterThan_9()
        {
            // Arrange
            int possibleValueToRemove = 10;
            Sudoku.Cell cell = new(1, 1);

            // Act
            Action act = () => cell.RemovePossibleValue(possibleValueToRemove);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_NotThrow_When_SuppliedValue_Valid()
        {
            // Arrange
            int possibleValueToRemove = 4;
            Sudoku.Cell cell = new(1, 1);

            // Act
            Action act = () => cell.RemovePossibleValue(possibleValueToRemove);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_Throw_InvalidOperationException_When_SuppliedValue_Equals_Single_ExistingPossibleValue()
        {
            // Arrange
            int possibleValueToRemove = 4;
            Sudoku.Cell cell = new(1, 1);
            var existingPossibleValues = new[] { 4 };
            cell.SetPossibleValues(existingPossibleValues);

            // Act
            Action act = () => cell.RemovePossibleValue(possibleValueToRemove);

            // Assert
            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Invalid operation: Cell (1, 1) has no possible values left.");
        }

        [Fact]
        public void Should_Remove_ValidValue_Supplied_From_ExistingPossibleValues()
        {
            // Arrange
            int possibleValueToRemove = 4;
            Sudoku.Cell cell = new(1, 1);

            // Act
            cell.RemovePossibleValue(possibleValueToRemove);

            // Assert
            cell.GetPossibleValues().Should().NotBeNullOrEmpty()
                .And.HaveCount(8)
                .And.OnlyContain(pv => (pv >= 1 && pv <= 3) || (pv >= 5 && pv <= 9));
        }

        [Fact]
        public void Should_NotChange_ExistingPossibleValues_When_SuppliedValue_Not_ExistingValue()
        {
            // Arrange
            int possibleValueToRemove = 4;
            Sudoku.Cell cell = new(1, 1);
            var existingPossibleValues = new[] { 1, 2, 3 };
            cell.SetPossibleValues(existingPossibleValues);

            // Act
            cell.RemovePossibleValue(possibleValueToRemove);

            // Assert
            cell.GetPossibleValues().Should().NotBeNullOrEmpty()
                .And.HaveCount(3)
                .And.OnlyContain(pv => (pv >= 1 && pv <= 3));
        }

        [Fact]
        public void Should_Return_ReadOnlyCollection_When_SuppliedValue_Valid()
        {
            // Arrange
            int possibleValueToRemove = 4;
            Sudoku.Cell cell = new(1, 1);

            // Act
            var returnValues = cell.RemovePossibleValue(possibleValueToRemove);

            // Assert
            Type expectedType = typeof(ReadOnlyCollection<int>);
            Type returnType = returnValues.GetType();
            returnType.Should().Be(expectedType);
        }

        [Fact]
        public void Should_Return_ExistingPossibleValues_With_SuppliedValue_Removed_When_SuppliedValue_Valid_And_Equals_ExistingValue()
        {
            // Arrange
            int possibleValueToRemove = 4;
            Sudoku.Cell cell = new(1, 1);

            // Act
            var returnValues = cell.RemovePossibleValue(possibleValueToRemove);

            // Assert
            returnValues.Should().NotBeNullOrEmpty()
                .And.HaveCount(8)
                .And.OnlyContain(pv => (pv >= 1 && pv <= 3) || (pv >= 5 && pv <= 9));
        }

        [Fact]
        public void Should_Return_Null_From_GetOnlyPossibleValue_When_Multiple_PossibleValues_Remain()
        {
            // Arrange
            int possibleValueToRemove = 4;
            Sudoku.Cell cell = new(1, 1);

            // Act
            cell.RemovePossibleValue(possibleValueToRemove);

            // Assert
            int? onlyPossibleValue = cell.GetOnlyPossibleValue();
            onlyPossibleValue.HasValue.Should().BeFalse();
        }

        [Fact]
        public void Should_Clear_HasSinglePossibleValue_When_Multiple_PossibleValues_Remain()
        {
            // Arrange
            int possibleValueToRemove = 4;
            Sudoku.Cell cell = new(1, 1);

            // Act
            cell.RemovePossibleValue(possibleValueToRemove);

            // Assert
            cell.HasSinglePossibleValue.Should().BeFalse();
        }

        [Fact]
        public void Should_Return_Remaining_PossibleValue_From_GetOnlyPossibleValue_When_Single_PossibleValue_Remains()
        {
            // Arrange
            int possibleValueToRemove = 2;
            Sudoku.Cell cell = new(1, 1);
            var existingPossibleValues = new[] { 2, 3 };
            cell.SetPossibleValues(existingPossibleValues);

            // Act
            cell.RemovePossibleValue(possibleValueToRemove);

            // Assert
            int? onlyPossibleValue = cell.GetOnlyPossibleValue();
            onlyPossibleValue.HasValue.Should().BeTrue();
            onlyPossibleValue.Should().Be(3);
        }

        [Fact]
        public void Should_Set_HasSinglePossibleValue_When_Single_PossibleValue_Remains()
        {
            // Arrange
            int possibleValueToRemove = 2;
            Sudoku.Cell cell = new(1, 1);
            var existingPossibleValues = new[] { 2, 3 };
            cell.SetPossibleValues(existingPossibleValues);

            // Act
            cell.RemovePossibleValue(possibleValueToRemove);

            // Assert
            cell.HasSinglePossibleValue.Should().BeTrue();
        }
    }
}
