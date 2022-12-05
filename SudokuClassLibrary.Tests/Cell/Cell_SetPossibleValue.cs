using System;
using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SudokuClassLibrary.Tests.Cell
{
    public class Cell_SetPossibleValue
    {
        [Fact]
        public void Should_Throw_ArgumentException_When_SuppliedValue_LessThan_1()
        {
            // Arrange
            int newPossibleValue = 0;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);

            // Act
            Action act = () => cell.SetPossibleValue(newPossibleValue);

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
            Action act = () => cell.SetPossibleValue(newPossibleValue);

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
            Action act = () => cell.SetPossibleValue(newPossibleValue);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_Set_PossibleValues_EqualTo_ValidValue_Supplied()
        {
            // Arrange
            int newPossibleValue = 4;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);

            // Act
            cell.SetPossibleValue(newPossibleValue);

            // Assert
            cell.GetPossibleValues().Should().NotBeNullOrEmpty()
                .And.HaveCount(1)
                .And.Contain(newPossibleValue);
        }

        [Fact]
        public void Should_Return_ReadOnlyCollection_When_SuppliedValue_Valid()
        {
            // Arrange
            int newPossibleValue = 4;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);

            // Act
            var returnValues = cell.SetPossibleValue(newPossibleValue); 

            // Assert
            Type expectedType = typeof(ReadOnlyCollection<int>);
            Type returnType = returnValues.GetType();
            returnType.Should().Be(expectedType);
        }

        [Fact]
        public void Should_Return_Value_EqualTo_ValidValue_Supplied()
        {
            // Arrange
            int newPossibleValue = 4;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);

            // Act
            var returnValues = cell.SetPossibleValue(newPossibleValue);

            // Assert
            returnValues.Should().NotBeNullOrEmpty()
                .And.HaveCount(1)
                .And.Contain(newPossibleValue);
        }

        [Fact]
        public void Should_Return_Value_From_GetOnlyPossibleValue_EqualTo_ValidValue_Supplied()
        {
            // Arrange
            int newPossibleValue = 4;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);

            // Act
            cell.SetPossibleValue(newPossibleValue);
            int? onlyPossibleValue = cell.GetOnlyPossibleValue();

            // Assert
            onlyPossibleValue.HasValue.Should().BeTrue();
            onlyPossibleValue.Should().Be(newPossibleValue);
        }

        [Fact]
        public void Should_Set_HasSinglePossibleValue_When_SuppliedValue_Valid()
        {
            // Arrange
            int newPossibleValue = 4;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);

            // Act
            cell.SetPossibleValue(newPossibleValue);

            // Assert
            cell.HasSinglePossibleValue.Should().BeTrue();
        }
    }
}
