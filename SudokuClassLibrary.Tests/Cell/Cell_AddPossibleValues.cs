using System;
using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System.Collections.ObjectModel;

namespace SudokuClassLibrary.Tests.Cell
{
    public class Cell_AddPossibleValues
    {
        [Fact]
        public void Should_Throw_ArgumentException_When_ValuesSupplied_Include_Value_LessThan_1()
        {
            // Arrange
            var newPossibleValues = new[] { 0, 1, 2 };
            Sudoku.Cell cell = new(1, 1);

            // Act
            Action act = () => cell.AddPossibleValues(newPossibleValues);

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("Invalid value: Value must be between 1 and 9.*");
        }

        [Fact]
        public void Should_Throw_ArgumentException_When_ValuesSupplied_Include_Value_GreaterThan_9()
        {
            // Arrange
            var newPossibleValues = new[] { 1, 2, 10 };
            Sudoku.Cell cell = new(1, 1);

            // Act
            Action act = () => cell.AddPossibleValues(newPossibleValues);

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("Invalid value: Value must be between 1 and 9.*");
        }

        [Fact]
        public void Should_NotThrow_When_EmptyArray_Supplied()
        {
            // Arrange
            var newPossibleValues = new int[0];
            Sudoku.Cell cell = new(1, 1);

            // Act
            Action act = () => cell.AddPossibleValues(newPossibleValues);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_NotThrow_When_ValuesSupplied_All_Valid()
        {
            // Arrange
            var newPossibleValues = new[] { 1, 2, 3 };
            Sudoku.Cell cell = new(1, 1);

            // Act
            Action act = () => cell.AddPossibleValues(newPossibleValues);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_Add_ValidValues_Supplied_To_ExistingPossibleValues()
        {
            // Arrange
            var newPossibleValues = new[] { 4, 5, 6 };
            Sudoku.Cell cell = new(1, 1);
            var existingPossibleValues = new[] { 1, 2, 3 };
            cell.SetPossibleValues(existingPossibleValues);

            // Act
            cell.AddPossibleValues(newPossibleValues);

            // Assert
            cell.GetPossibleValues().Should().NotBeNullOrEmpty()
                .And.HaveCount(6)
                .And.OnlyContain(pv => (pv >= 1 && pv <= 6));
        }

        [Fact]
        public void Should_Remove_Duplicates_From_SuppliedValues()
        {
            // Arrange
            var newPossibleValues = new[] { 2, 3, 4, 5 };
            Sudoku.Cell cell = new(1, 1);
            var existingPossibleValues = new[] { 1, 2, 3 };
            cell.SetPossibleValues(existingPossibleValues);

            // Act
            cell.AddPossibleValues(newPossibleValues);

            // Assert
            cell.GetPossibleValues().Should().NotBeNullOrEmpty()
                .And.HaveCount(5)
                .And.OnlyContain(pv => (pv >= 1 && pv <= 5));
        }

        [Fact]
        public void Should_Return_ReadOnlyCollection_When_ValidValues_Supplied()
        {
            // Arrange
            var newPossibleValues = new[] { 4, 5, 6 };
            Sudoku.Cell cell = new(1, 1);
            var existingPossibleValues = new[] { 1, 2, 3 };
            cell.SetPossibleValues(existingPossibleValues);

            // Act
            var returnValues = cell.AddPossibleValues(newPossibleValues);

            // Assert
            Type expectedType = typeof(ReadOnlyCollection<int>);
            Type returnType = returnValues.GetType();
            returnType.Should().Be(expectedType);
        }

        [Fact]
        public void Should_Return_ExistingPossibleValues_With_SuppliedValues_Added_When_SuppliedValues_Valid_And_NonDuplicate()
        {
            // Arrange
            var newPossibleValues = new[] { 4, 5, 6 };
            Sudoku.Cell cell = new(1, 1);
            var existingPossibleValues = new[] { 1, 2, 3 };
            cell.SetPossibleValues(existingPossibleValues);

            // Act
            var returnValues = cell.AddPossibleValues(newPossibleValues);

            // Assert
            returnValues.Should().NotBeNullOrEmpty()
                .And.HaveCount(6)
                .And.OnlyContain(pv => (pv >= 1 && pv <= 6));
        }

        [Fact]
        public void Should_Return_Null_From_GetOnlyPossibleValue_When_SuppliedValues_Valid()
        {
            // Arrange
            var newPossibleValues = new[] { 4, 5, 6 };
            Sudoku.Cell cell = new(1, 1);
            var existingPossibleValues = new[] { 1, 2, 3 };
            cell.SetPossibleValues(existingPossibleValues);

            // Act
            cell.AddPossibleValues(newPossibleValues);

            // Assert
            int? onlyPossibleValue = cell.GetOnlyPossibleValue();
            onlyPossibleValue.HasValue.Should().BeFalse();
        }

        [Fact]
        public void Should_Clear_HasSinglePossibleValue_When_SuppliedValues_Valid()
        {
            // Arrange
            var newPossibleValues = new[] { 4, 5, 6 };
            Sudoku.Cell cell = new(1, 1);
            var existingPossibleValues = new[] { 1, 2, 3 };
            cell.SetPossibleValues(existingPossibleValues);

            // Act
            cell.AddPossibleValues(newPossibleValues);

            // Assert
            cell.HasSinglePossibleValue.Should().BeFalse();
        }
    }
}
