using System;
using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SudokuClassLibrary.Tests.CellGroup
{
    public class CellGroup_GetAvailableValues
    {
        [Fact]
        public void Should_Return_ReadOnlyCollection()
        {
            // Arrange
            Sudoku.CellGroup cellGroup = new(CellGroupType.Row, 0);

            // Act
            var availableValues = cellGroup.GetAvailableValues();

            // Assert
            Type expectedType = typeof(ReadOnlyCollection<int>);
            Type returnType = availableValues.GetType();
            returnType.Should().Be(expectedType);
        }
    }
}
