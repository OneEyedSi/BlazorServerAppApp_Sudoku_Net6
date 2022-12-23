using Sudoku = SudokuClassLibrary;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SudokuClassLibrary.Tests.Cell
{
    public class Cell_RemoveInitialValue
    {

        [Fact]
        public void Should_NotThrow()
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
        public void Should_Clear_IsInitialValue()
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
        public void Should_Clear_Value()
        {
            // Arrange
            int? previousValue = 4;
            int? newValue = null;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);
            cell.SetValue(previousValue);

            // Act
            cell.SetInitialValue(newValue);

            // Assert
            cell.GetValue().HasValue.Should().BeFalse();
        }

        [Fact]
        public void Should_Set_All_PossibleValues()
        {
            // Arrange
            int? newValue = null;
            Sudoku.Cell cell = new Sudoku.Cell(1, 1);

            // Act
            cell.SetInitialValue(newValue);

            // Assert
            cell.GetPossibleValuesDictionary().ShouldHaveExpectedValuesSetToRange(1, 9);
        }
    }
}
