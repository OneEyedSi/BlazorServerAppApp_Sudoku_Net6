using System;
using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SudokuClassLibrary.Tests.Cell
{
    public class Cell_AddPossibleValue
    {
        [Fact]
        public void Should_Throw_ArgumentException_When_SuppliedValue_LessThan_1()
        {
            // Arrange
            int newPossibleValue = 0;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);

            // Act
            Action act = () => cell.AddPossibleValue(newPossibleValue);

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("Invalid value: Value must be between 1 and 9.*");
        }

        [Fact]
        public void Should_Throw_ArgumentException_When_SuppliedValue_GreaterThan_9()
        {
            // Arrange
            int newPossibleValue = 10;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);

            // Act
            Action act = () => cell.AddPossibleValue(newPossibleValue);

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("Invalid value: Value must be between 1 and 9.*");
        }

        [Fact]
        public void Should_NotThrow_When_SuppliedValue_Valid()
        {
            // Arrange
            int newPossibleValue = 4;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);

            // Act
            Action act = () => cell.AddPossibleValue(newPossibleValue);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_Add_ValidValue_Supplied_To_ExistingPossibleValues()
        {
            // Arrange
            int newPossibleValue = 4;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);
            var existingPossibleValues = new[] { 1, 2, 3 };
            cell.SetPossibleValues(existingPossibleValues);

            // Act
            cell.AddPossibleValue(newPossibleValue);

            // Assert
            cell.GetPossibleValues().Should().NotBeNullOrEmpty()
                .And.HaveCount(4)
                .And.OnlyContain(pv => (pv >= 1 && pv <= 4));
        }

        [Fact]
        public void Should_NotChange_ExistingPossibleValues_When_SuppliedValue_Duplicate_Of_ExistingValue()
        {
            // Arrange
            int newPossibleValue = 3;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);
            var existingPossibleValues = new[] { 1, 2, 3 };
            cell.SetPossibleValues(existingPossibleValues);

            // Act
            cell.AddPossibleValue(newPossibleValue);

            // Assert
            cell.GetPossibleValues().Should().NotBeNullOrEmpty()
                .And.HaveCount(3)
                .And.OnlyContain(pv => (pv >= 1 && pv <= 3));
        }

        [Fact]
        public void Should_Return_ReadOnlyCollection_When_SuppliedValue_Valid()
        {
            // Arrange
            int newPossibleValue = 4;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);
            var existingPossibleValues = new[] { 1, 2, 3 };
            cell.SetPossibleValues(existingPossibleValues);

            // Act
            var returnValues = cell.AddPossibleValue(newPossibleValue); 

            // Assert
            Type expectedType = typeof(ReadOnlyCollection<int>);
            Type returnType = returnValues.GetType();
            returnType.Should().Be(expectedType);
        }

        [Fact]
        public void Should_Return_ExistingPossibleValues_With_SuppliedValue_Added_When_SuppliedValue_Valid_And_NonDuplicate()
        {
            // Arrange
            int newPossibleValue = 4;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);
            var existingPossibleValues = new[] { 1, 2, 3 };
            cell.SetPossibleValues(existingPossibleValues);

            // Act
            var returnValues = cell.AddPossibleValue(newPossibleValue);

            // Assert
            returnValues.Should().NotBeNullOrEmpty()
                .And.HaveCount(4)
                .And.OnlyContain(pv => (pv >= 1 && pv <= 4));
        }

        [Fact]
        public void Should_Return_Null_From_GetOnlyPossibleValue_When_SuppliedValue_Valid()
        {
            // Arrange
            int newPossibleValue = 4;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);
            var existingPossibleValues = new[] { 1, 2, 3 };
            cell.SetPossibleValues(existingPossibleValues);

            // Act
            cell.AddPossibleValue(newPossibleValue);

            // Assert
            int? onlyPossibleValue = cell.GetOnlyPossibleValue();
            onlyPossibleValue.HasValue.Should().BeFalse();
        }

        [Fact]
        public void Should_Clear_HasSinglePossibleValue_When_SuppliedValue_Valid()
        {
            // Arrange
            int newPossibleValue = 4;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);
            var existingPossibleValues = new[] { 1, 2, 3 };
            cell.SetPossibleValues(existingPossibleValues);

            // Act
            cell.AddPossibleValue(newPossibleValue);

            // Assert
            cell.HasSinglePossibleValue.Should().BeFalse();
        }
    }
}
