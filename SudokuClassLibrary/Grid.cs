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
        public Grid(bool isKillerSodoku = false)
        {
            InitializeCells();
            AddGroups(isKillerSodoku);
        }

        public Cell[,] Cells { get; } = new Cell[9, 9];

        public List<CellGroup> Groups { get; } = new();

        private bool _isKillerSudoku = false;
        public bool IsKillerSudoku 
        {
            get
            {
                return _isKillerSudoku;
            }

            set
            {
                _isKillerSudoku = value;
                if (_isKillerSudoku)
                {
                    AddDiagonalGroups();
                }
                else
                {
                    RemoveDiagonalGroups();
                }
            }
        }

        public void ResetGame()
        {
            // Assume if the first cell is null they all will be.
            if (this.Cells[0, 0] == null)
            {
                InitializeCells();
                AddGroups(this.IsKillerSudoku);
            }
            else 
            { 
                foreach (var cell in this.Cells)
                {
                    cell.Value = null;
                }

                if (this.IsKillerSudoku)
                {
                    AddDiagonalGroups();
                }
                else
                {
                    RemoveDiagonalGroups();
                }
            }
        }

        private void InitializeCells()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    Cell cell = new Cell(row, column);
                    this.Cells[row, column] = cell;
                }
            }
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
                AddDiagonalGroups();
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
            // row and column of top left cell in each square.
            for (int row = 0; row < 9; row += 3)
            {
                for (int column = 0; column < 9; column += 3)
                {
                    // Squares will be numbered:
                    //  0,1,2
                    //  3,4,5
                    //  6,7,8
                    int squareIndex = CellGroup.GetSquareIndexForRowAndColumn(row, column);
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

        private void RemoveDiagonalGroups()
        {
            if (this.Groups == null || !this.Groups.Any())
            {
                return;
            }

            this.Groups.RemoveAll(g => g.GroupType == CellGroupType.Diagonal);
        }

        private void AddDiagonalGroups()
        {
            AddPrimaryDiagonalGroup();
            AddSecondaryDiagonalGroup();
        }

        private void AddPrimaryDiagonalGroup()
        {
            int groupIndex = 0;
            if (this.Groups?.Any(g => g.GroupType == CellGroupType.Diagonal && g.Index == groupIndex) ?? false)
            {
                return;
            }

            CellGroup group = new CellGroup(CellGroupType.Diagonal, groupIndex);

            for (int i = 0; i < 9; i++)
            {
                Cell cell = GetCell(i, i);
                group.AddCell(cell);
            }

            this.Groups?.Add(group);
        }

        private void AddSecondaryDiagonalGroup()
        {
            int groupIndex = 1;
            if (this.Groups?.Any(g => g.GroupType == CellGroupType.Diagonal && g.Index == groupIndex) ?? false)
            {
                return;
            }

            CellGroup group = new CellGroup(CellGroupType.Diagonal, groupIndex);

            for (int row = 0; row < 9; row++)
            {
                int column = 8 - row;

                Cell cell = GetCell(row, column);
                group.AddCell(cell);
            }

            this.Groups?.Add(group);
        }
    }
}
