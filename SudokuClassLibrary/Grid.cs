using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuClassLibrary
{
    public class Grid
    {
        public Grid(bool isKillerSodoku = false) : this(null, isKillerSodoku)
        { }

        public Grid(List<PositionValue>? positionValues, bool isKillerSodoku = false)
        {
            positionValues = positionValues ?? new List<PositionValue>();
            InitializeCells(positionValues);
            AddGroups(isKillerSodoku);
        }

        public Cell[,] Cells { get; } = new Cell[9, 9];

        public List<CellGroup> Groups { get; } = new();

        private void InitializeCells(List<PositionValue> positionValues)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    Cell cell = new Cell(row, column);
                    this.Cells[row, column] = cell;
                }
            }

            if (positionValues == null)
            {
                return;
            }

            foreach (var positionValue in positionValues)
            {
                SetCellValue(positionValue, isInitialValue: true);
            }
        }

        private void SetCellValue(PositionValue positionValue, bool isInitialValue = false)
        {
            Cell cell = this.Cells[positionValue.Row, positionValue.Column];
            cell.Value = positionValue.Value;
            cell.IsInitialValue = isInitialValue;
        }

        private Cell GetCell(int row, int column)
        {
            return this.Cells[row, column];
        }

        private void AddGroups(bool isKillerSodoku)
        {
            AddRowGroups();
            AddColumnGroups();
            AddSquareGroups();

            if (isKillerSodoku)
            {
                AddPrimaryDiagonalGroup();
                AddSecondaryDiagonalGroup();
            }
        }

        private void AddRowGroups()
        {
            for (int row = 0; row < 9; row++)
            {
                CellGroup group = new CellGroup(CellGroupType.Row, row);

                for (int column = 0; column < 9; column++)
                {
                    Cell cell = GetCell(row, column);
                    group.AddCell(cell);
                }

                this.Groups.Add(group);
            }
        }

        private void AddColumnGroups()
        {
            for (int column = 0; column < 9; column++)
            {
                CellGroup group = new CellGroup(CellGroupType.Column, column);

                for (int row = 0; row < 9; row++)
                {
                    Cell cell = GetCell(row, column);
                    group.AddCell(cell);
                }

                this.Groups.Add(group);
            }
        }

        private void AddSquareGroups()
        {
            for (int row = 0; row < 9; row += 3)
            {
                for (int column = 0; column < 9; column += 3)
                {
                    // Squares will be numbered:
                    //  0,1,2
                    //  3,4,5
                    //  6,7,8
                    int squareIndex = (row * 9 + column) / 3;
                    CellGroup group = new CellGroup(CellGroupType.Square, squareIndex);

                    for (int rowOffset = 0; rowOffset < 3; rowOffset++)
                    {
                        for (int columnOffset = 0; columnOffset < 3; columnOffset++)
                        {
                            Cell cell = GetCell(row + rowOffset, column + columnOffset);
                            group.AddCell(cell);
                        }
                    }

                    this.Groups.Add(group);
                }
            }
        }

        private void AddPrimaryDiagonalGroup()
        {
            CellGroup group = new CellGroup(CellGroupType.Diagonal, 0);

            for (int i = 0; i < 9; i++)
            {
                Cell cell = GetCell(i, i);
                group.AddCell(cell);
            }

            this.Groups.Add(group);
        }

        private void AddSecondaryDiagonalGroup()
        {
            CellGroup group = new CellGroup(CellGroupType.Diagonal, 1);

            for (int row = 0; row < 9; row++)
            {
                int column = 8 - row;

                Cell cell = GetCell(row, column);
                group.AddCell(cell);
            }

            this.Groups.Add(group);
        }
    }
}
