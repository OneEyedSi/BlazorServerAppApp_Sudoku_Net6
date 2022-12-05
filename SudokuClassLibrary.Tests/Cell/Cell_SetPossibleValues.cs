using System;
using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SudokuClassLibrary.Tests.Cell
{
    public class Cell_SetPossibleValues
    {
        [Fact]
        public void Should_Throw_ArgumentException_When_ValuesSupplied_Include_Value_LessThan_1()
        {
            // Arrange
            var newPossibleValues = new[] { 0, 1, 2 };
            Sudoku.Cell cell = new(1, 1);

            // Act
            Action act = () => cell.SetPossibleValues(newPossibleValues);

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
            Action act = () => cell.SetPossibleValues(newPossibleValues);

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("Invalid value: Value must be between 1 and 9.*");
        }

        [Fact]
        public void Should_Throw_ArgumentException_When_EmptyArray_Supplied()
        {
            // Arrange
            var newPossibleValues = new int[0];
            Sudoku.Cell cell = new(1, 1);

            // Act
            Action act = () => cell.SetPossibleValues(newPossibleValues);

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("Invalid value: Cannot set possible values as empty*");
        }

        [Fact]
        public void Should_NotThrow_When_ValuesSupplied_All_Valid()
        {
            // Arrange
            var newPossibleValues = new[] { 1, 2, 3 };
            Sudoku.Cell cell = new(1, 1);

            // Act
            Action act = () => cell.SetPossibleValues(newPossibleValues);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_Set_PossibleValues_EqualTo_ValidValues_Supplied()
        {
            // Arrange
            var newPossibleValues = new[] { 1, 2, 3 };
            Sudoku.Cell cell = new(1, 1);

            // Act
            cell.SetPossibleValues(newPossibleValues);

            // Assert
            cell.GetPossibleValues().Should().NotBeNullOrEmpty()
                .And.HaveCount(newPossibleValues.Length)
                .And.OnlyContain(pv => (pv >= 1 && pv <= 3));
        }

        [Fact]
        public void Should_Remove_Duplicates_From_SuppliedValues()
        {
            // Arrange
            var newPossibleValues = new[] { 2, 2, 2, 1, 3, 3 };
            Sudoku.Cell cell = new(1, 1);

            // Act
            cell.SetPossibleValues(newPossibleValues);

            // Assert
            cell.GetPossibleValues().Should().NotBeNullOrEmpty()
                .And.HaveCount(3)
                .And.OnlyContain(pv => (pv >= 1 && pv <= 3));
        }

        [Fact]
        public void Should_Return_ReadOnlyCollection_When_ValidValues_Supplied()
        {
            // Arrange
            var newPossibleValues = new[] { 1, 2, 3 };
            Sudoku.Cell cell = new(1, 1);

            // Act
            var returnValues = cell.SetPossibleValues(newPossibleValues);

            // Assert
            Type expectedType = typeof(ReadOnlyCollection<int>);
            Type returnType = returnValues.GetType();
            returnType.Should().Be(expectedType);
        }

        [Fact]
        public void Should_Return_SameValues_When_ValidValues_Supplied()
        {
            // Arrange
            var newPossibleValues = new[] { 1, 2, 3 };
            Sudoku.Cell cell = new(1, 1);

            // Act
            var returnValues = cell.SetPossibleValues(newPossibleValues);

            // Assert
            returnValues.Should().NotBeNullOrEmpty()
                .And.HaveCount(newPossibleValues.Length)
                .And.OnlyContain(pv => (pv >= 1 && pv <= 3));
        }

        [Fact]
        public void Should_Return_Null_From_GetOnlyPossibleValue_When_Multiple_ValidValues_Supplied()
        {
            // Arrange
            var newPossibleValues = new[] { 1, 2, 3 };
            Sudoku.Cell cell = new(1, 1);

            // Act
            cell.SetPossibleValues(newPossibleValues);
            int? onlyPossibleValue = cell.GetOnlyPossibleValue();

            // Assert
            onlyPossibleValue.HasValue.Should().BeFalse();
        }

        [Fact]
        public void Should_Clear_HasSinglePossibleValue_When_Multiple_ValidValues_Supplied()
        {
            // Arrange
            var newPossibleValues = new[] { 1, 2, 3 };
            Sudoku.Cell cell = new(1, 1);

            // Act
            cell.SetPossibleValues(newPossibleValues);

            // Assert
            cell.HasSinglePossibleValue.Should().BeFalse();
        }

        [Fact]
        public void Should_Return_SameValue_From_GetOnlyPossibleValue_When_Single_ValidValue_Supplied()
        {
            // Arrange
            var newPossibleValues = new[] { 1 };
            Sudoku.Cell cell = new(1, 1);

            // Act
            cell.SetPossibleValues(newPossibleValues);
            int? onlyPossibleValue = cell.GetOnlyPossibleValue();

            // Assert
            onlyPossibleValue.HasValue.Should().BeTrue();
            onlyPossibleValue.Should().Be(newPossibleValues.FirstOrDefault());
        }

        [Fact]
        public void Should_Set_HasSinglePossibleValue_When_Single_ValidValue_Supplied()
        {
            // Arrange
            var newPossibleValues = new[] { 1 };
            Sudoku.Cell cell = new(1, 1);

            // Act
            cell.SetPossibleValues(newPossibleValues);

            // Assert
            cell.HasSinglePossibleValue.Should().BeTrue();
        }
    }
}
