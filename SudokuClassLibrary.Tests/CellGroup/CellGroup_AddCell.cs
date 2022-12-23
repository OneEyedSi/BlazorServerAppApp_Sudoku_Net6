using System;
using Sudoku = SudokuClassLibrary;
using Xunit;
using FluentAssertions;
using System.Collections.ObjectModel;
using FluentAssertions.Events;

namespace SudokuClassLibrary.Tests.CellGroup
{
    public class CellGroup_AddCell
    {
        [Fact]
        public void Should_Add_Cell_To_CellsList()
        {
            // Arrange
            Sudoku.CellGroup cellGroup = new(CellGroupType.Row, 0);
            Sudoku.Cell cell = new(1, 1);

            // Act
            cellGroup.AddCell(cell);

            // Assert
            cellGroup.Cells.Should().NotBeNullOrEmpty()
                 .And.HaveCount(1)
                 .And.Contain(cell);
        }

        [Fact]
        public void Should_Add_Group_To_Cell_ParentGroups()
        {
            // Arrange
            Sudoku.CellGroup cellGroup = new(CellGroupType.Row, 0);
            Sudoku.Cell cell = new(1, 1);

            // Act
            cellGroup.AddCell(cell);

            // Assert
            cell.ParentGroups.Should().NotBeNullOrEmpty()
                .And.HaveCount(1)
                .And.Contain(cellGroup);
        }

        [Fact]
        public void Should_Remove_Value_From_AvailableValues_When_Add_Cell_With_Value_Set_And_doRecalculateValues_Argument_True()
        {
            // Arrange
            Sudoku.CellGroup cellGroup = new(CellGroupType.Row, 0);
            Sudoku.Cell cell = new(1, 1);
            cell.SetValue(4);

            // Act
            cellGroup.AddCell(cell, doRecalculateValues: true);

            // Assert
            cellGroup.GetAvailableValues().Should().NotBeNullOrEmpty()
                .And.HaveCount(8)
                .And.OnlyContain(pv => (pv >= 1 && pv <= 3) || (pv >= 5 && pv <= 9));
        }

        [Fact]
        public void Should_NotChange_AvailableValues_When_Add_Cell_With_No_Value_Set_And_doRecalculateValues_Argument_True()
        {
            // Arrange
            Sudoku.CellGroup cellGroup = new(CellGroupType.Row, 0);
            Sudoku.Cell cell = new(1, 1);

            // Act
            cellGroup.AddCell(cell, doRecalculateValues: true);

            // Assert
            cellGroup.GetAvailableValues().Should().NotBeNullOrEmpty()
                .And.HaveCount(9)
                .And.OnlyContain(pv => (pv >= 1 && pv <= 9));
        }

        [Fact]
        public void Should_NotRemove_Value_From_AvailableValues_When_Add_Cell_With_Value_Set_And_doRecalculateValues_Argument_False()
        {
            // Arrange
            Sudoku.CellGroup cellGroup = new(CellGroupType.Row, 0);
            Sudoku.Cell cell = new(1, 1);
            cell.SetValue(4);

            // Act
            cellGroup.AddCell(cell, doRecalculateValues: false);

            // Assert
            cellGroup.GetAvailableValues().Should().NotBeNullOrEmpty()
                .And.HaveCount(9)
                .And.OnlyContain(pv => (pv >= 1 && pv <= 9));
        }

        [Fact]
        public void Should_RaiseEvent_RecalculateCellPossibleValues_When_doRecalculateValues_Argument_True()
        {
            // Arrange
            Sudoku.CellGroup cellGroup = new(CellGroupType.Row, 0);
            Sudoku.Cell cell = new(1, 1);
            using IMonitor<Sudoku.CellGroup> monitoredGroup = cellGroup.Monitor();

            // Act
            cellGroup.AddCell(cell, doRecalculateValues: true);

            // Assert
            monitoredGroup.Should()
                .Raise("RecalculateCellPossibleValues")
                .WithSender(cellGroup);
        }

        [Fact]
        public void Should_NotRaiseEvent_RecalculateCellPossibleValues_When_doRecalculateValues_Argument_False()
        {
            // Arrange
            Sudoku.CellGroup cellGroup = new(CellGroupType.Row, 0);
            Sudoku.Cell cell = new(1, 1);
            using IMonitor<Sudoku.CellGroup> monitoredGroup = cellGroup.Monitor();

            // Act
            cellGroup.AddCell(cell, doRecalculateValues: false);

            // Assert
            monitoredGroup.Should()
                .NotRaise("RecalculateCellPossibleValues");
        }
    }
}
