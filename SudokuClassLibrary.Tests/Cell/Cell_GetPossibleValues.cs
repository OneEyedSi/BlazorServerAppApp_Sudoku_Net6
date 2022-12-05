using System;
using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;

namespace SudokuClassLibrary.Tests.Cell
{
    public class Cell_GetPossibleValues
    {
        [Fact]
        public void Should_Return_ReadOnlyCollection()
        {
            // Arrange
            Sudoku.Cell cell = new(1, 1);

            // Act
            var possibleValues = cell.GetPossibleValues();

            // Assert
            Type expectedType = typeof(ReadOnlyCollection<int>);
            Type returnType = possibleValues.GetType();
            returnType.Should().Be(expectedType);
        }
    }
}
