﻿using System;
using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System.Collections.ObjectModel;

namespace SudokuClassLibrary.Tests.Cell
{
    public class Cell_HasSinglePossibleValue
    {
        [Fact]
        public void Should_Clear_HasSinglePossibleValue_When_Multiple_PossibleValues()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);
            cell.SetPossibleValueRange(1, 3);

            // Act

            // Assert
            cell.HasSinglePossibleValue.Should().BeFalse();
        }

        [Fact]
        public void Should_Set_HasSinglePossibleValue_When_Single_PossibleValue()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);
            cell.SetPossibleValue(1);

            // Act

            // Assert
            cell.HasSinglePossibleValue.Should().BeTrue();
        }

        [Fact]
        public void Should_Set_HasSinglePossibleValue_When_Value_Set()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);
            cell.SetValue(4);

            // Act

            // Assert
            cell.HasSinglePossibleValue.Should().BeTrue();
        }
        [Fact]
        public void Should_Clear_HasSinglePossibleValue_When_Value_Cleared()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);
            cell.SetValue(4);

            // Act
            cell.SetValue(null);

            // Assert
            cell.HasSinglePossibleValue.Should().BeFalse();
        }
    }
}
