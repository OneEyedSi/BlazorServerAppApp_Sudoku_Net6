using System;
using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System.Collections.ObjectModel;

namespace SudokuClassLibrary.Tests.Cell
{
    public class Cell_RemovePossibleValues
    {
        [Fact]
        public void Should_NotThrow_When_ValuesSupplied_Include_Value_LessThan_1()
        {
            // Arrange
            var possibleValuesToRemove = new[] { 0, 1, 2 };
            Sudoku.Cell cell = new(1, 1);

            // Act
            Action act = () => cell.RemovePossibleValues(possibleValuesToRemove);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_NotThrow_When_ValuesSupplied_Include_Value_GreaterThan_9()
        {
            // Arrange
            var possibleValuesToRemove = new[] { 1, 2, 10 };
            Sudoku.Cell cell = new(1, 1);

            // Act
            Action act = () => cell.RemovePossibleValues(possibleValuesToRemove);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_NotThrow_When_EmptyArray_Supplied()
        {
            // Arrange
            var possibleValuesToRemove = new int[0];
            Sudoku.Cell cell = new(1, 1);

            // Act
            Action act = () => cell.RemovePossibleValues(possibleValuesToRemove);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_NotThrow_When_ValuesSupplied_All_Valid()
        {
            // Arrange
            var possibleValuesToRemove = new[] { 1, 2, 3 };
            Sudoku.Cell cell = new(1, 1);

            // Act
            Action act = () => cell.RemovePossibleValues(possibleValuesToRemove);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_Throw_InvalidOperationException_When_All_ExistingPossibleValues_Removed()
        {
            // Arrange
            var possibleValuesToRemove = new[] { 1, 2, 3 };
            Sudoku.Cell cell = new(1, 1);
            var existingPossibleValues = new[] { 1, 2, 3 };
            cell.SetPossibleValues(existingPossibleValues);

            // Act
            Action act = () => cell.RemovePossibleValues(possibleValuesToRemove);

            // Assert
            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Invalid operation: Cell (1, 1) has no possible values left.");
        }

        [Fact]
        public void Should_Remove_ValidValues_Supplied_From_ExistingPossibleValues()
        {
            // Arrange
            var possibleValuesToRemove = new[] { 4, 5, 6 };
            Sudoku.Cell cell = new(1, 1);

            // Act
            cell.RemovePossibleValues(possibleValuesToRemove);

            // Assert
            cell.GetPossibleValues().Should().NotBeNullOrEmpty()
                .And.HaveCount(6)
                .And.OnlyContain(pv => (pv >= 1 && pv <= 3) || (pv >= 7 && pv <= 9));
        }

        [Fact]
        public void Should_NotChange_ExistingPossibleValues_When_SuppliedValues_NotIn_ExistingValues()
        {
            // Arrange
            var possibleValuesToRemove = new[] { 4, 5, 6 };
            Sudoku.Cell cell = new(1, 1);
            var existingPossibleValues = new[] { 1, 2, 3 };
            cell.SetPossibleValues(existingPossibleValues);

            // Act
            cell.RemovePossibleValues(possibleValuesToRemove);

            // Assert
            cell.GetPossibleValues().Should().NotBeNullOrEmpty()
                .And.HaveCount(3)
                .And.OnlyContain(pv => (pv >= 1 && pv <= 3));
        }

        [Fact]
        public void Should_NotThrow_When_ValuesSupplied_Include_Duplicates()
        {
            // Arrange
            var possibleValuesToRemove = new[] { 1, 1, 1, 2, 3, 3 };
            Sudoku.Cell cell = new(1, 1);

            // Act
            Action act = () => cell.RemovePossibleValues(possibleValuesToRemove);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_Remove_Only_SuppliedValues_When_SuppliedValues_Include_Duplicates()
        {
            // Arrange
            var possibleValuesToRemove = new[] { 1, 1, 1, 2, 3, 3 };
            Sudoku.Cell cell = new(1, 1);

            // Act
            cell.RemovePossibleValues(possibleValuesToRemove);

            // Assert
            cell.GetPossibleValues().Should().NotBeNullOrEmpty()
                .And.HaveCount(6)
                .And.OnlyContain(pv => (pv >= 4 && pv <= 9));
        }

        [Fact]
        public void Should_Return_ReadOnlyCollection_When_ValidValues_Supplied()
        {
            // Arrange
            var possibleValuesToRemove = new[] { 4, 5, 6 };
            Sudoku.Cell cell = new(1, 1);

            // Act
            var returnValues = cell.RemovePossibleValues(possibleValuesToRemove);

            // Assert
            Type expectedType = typeof(ReadOnlyCollection<int>);
            Type returnType = returnValues.GetType();
            returnType.Should().Be(expectedType);
        }

        [Fact]
        public void Should_Return_ExistingPossibleValues_With_SuppliedValues_Removed_When_SuppliedValues_Valid_And_Equal_ExistingValues()
        {
            // Arrange
            var possibleValuesToRemove = new[] { 4, 5, 6 };
            Sudoku.Cell cell = new(1, 1);

            // Act
            var returnValues = cell.RemovePossibleValues(possibleValuesToRemove);

            // Assert
            returnValues.Should().NotBeNullOrEmpty()
                .And.HaveCount(6)
                .And.OnlyContain(pv => (pv >= 1 && pv <= 3) || (pv >= 7 && pv <= 9));
        }

        [Fact]
        public void Should_Return_Null_From_GetOnlyPossibleValue_When_Multiple_PossibleValues_Remain()
        {
            // Arrange
            var possibleValuesToRemove = new[] { 4, 5, 6 };
            Sudoku.Cell cell = new(1, 1);

            // Act
            cell.RemovePossibleValues(possibleValuesToRemove);

            // Assert
            int? onlyPossibleValue = cell.GetOnlyPossibleValue();
            onlyPossibleValue.HasValue.Should().BeFalse();
        }

        [Fact]
        public void Should_Clear_HasSinglePossibleValue_When_Multiple_PossibleValues_Remain()
        {
            // Arrange
            var possibleValuesToRemove = new[] { 4, 5, 6 };
            Sudoku.Cell cell = new(1, 1);

            // Act
            cell.RemovePossibleValues(possibleValuesToRemove);

            // Assert
            cell.HasSinglePossibleValue.Should().BeFalse();
        }

        [Fact]
        public void Should_Return_Remaining_PossibleValue_From_GetOnlyPossibleValue_When_Single_PossibleValue_Remains()
        {
            // Arrange
            var possibleValuesToRemove = new[] { 1, 2 };
            Sudoku.Cell cell = new(1, 1);
            var existingPossibleValues = new[] { 1, 2, 3 };
            cell.SetPossibleValues(existingPossibleValues);

            // Act
            cell.RemovePossibleValues(possibleValuesToRemove);

            // Assert
            int? onlyPossibleValue = cell.GetOnlyPossibleValue();
            onlyPossibleValue.HasValue.Should().BeTrue();
            onlyPossibleValue.Should().Be(3);
        }

        [Fact]
        public void Should_Set_HasSinglePossibleValue_When_Single_PossibleValue_Remains()
        {
            // Arrange
            var possibleValuesToRemove = new[] { 1, 2 };
            Sudoku.Cell cell = new(1, 1);
            var existingPossibleValues = new[] { 1, 2, 3 };
            cell.SetPossibleValues(existingPossibleValues);

            // Act
            cell.RemovePossibleValues(possibleValuesToRemove);

            // Assert
            cell.HasSinglePossibleValue.Should().BeTrue();
        }
    }
}
