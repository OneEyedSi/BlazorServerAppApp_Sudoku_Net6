﻿using System;
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

        public bool HasInitialValue => GetEnumerableCells().Any(c => c.IsInitialValue);
        public bool HasGameValue => GetEnumerableCells().Any(c => c.HasValueSet && !c.IsInitialValue);
        public bool GameIsComplete => GetEnumerableCells().All(c => c.HasValueSet);

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
                    cell.ResetValue();
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

        public void RestartGame()
        {
            var nonInitialCells = this.GetEnumerableCells().Where(c => !c.IsInitialValue);
            foreach(var cell in nonInitialCells)
            {
                cell.SetGameValue(null);
            }
        }

        public void LoadGame(IEnumerable<Cell> initialCells, bool isKillerSudoku)
        {
            this.Groups.Clear();
            InitializeCells();
            AddGroups(isKillerSudoku);
            if (!initialCells.IsNullOrEmpty())
            {
                foreach(var cell in initialCells)
                {
                    var gridCell = GetCellByPosition(cell.Position);
                    if (gridCell != null)
                    {
                        gridCell.SetInitialValue(cell.GetValue(), isReplayingHistory: true);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the cells with initial values.
        /// </summary>
        /// <returns>IEnumerable<Cell></returns>
        public IEnumerable<Cell> GetInitialCells()
        {
            return GetEnumerableCells().Where(c => c.IsInitialValue);
        }

        /// <summary>
        /// Converts the 2D array of Cells into an enumerable, making it easy to iterate over.
        /// </summary>
        /// <returns>IEnumerable<Cell></returns>
        public IEnumerable<Cell> GetEnumerableCells()
        {
            return this.Cells.Cast<Cell>();
        }

        public Cell? GetCellByPosition(Position position)
        {
            if (position == null)
            {
                return null;
            }
            if (position.Row < 0 || position.Row > 8 
                || position.Column < 0 || position.Column > 8)
            {
                return null;
            }

            return this.Cells[position.Row, position.Column];
        }

        private void InitializeCells()
        {
            for (int row = 0; row <= 8; row++)
            {
                for (int column = 0; column <= 8; column++)
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

        private void AddGroups(bool isKillerSudoku)
        {
            AddRowGroups();
            AddColumnGroups();
            AddSquareGroups();

            if (isKillerSudoku)
            {
                AddDiagonalGroups();
            }
        }

        private void AddRowGroups()
        {
            for (int row = 0; row <= 8; row++)
            {
                CellGroup group = new CellGroup(CellGroupType.Row, row);

                for (int column = 0; column <= 8; column++)
                {
                    Cell cell = GetCell(row, column);
                    group.AddCell(cell);
                }

                this.Groups.Add(group);
            }
        }

        private void AddColumnGroups()
        {
            for (int column = 0; column <= 8; column++)
            {
                CellGroup group = new CellGroup(CellGroupType.Column, column);

                for (int row = 0; row <= 8; row++)
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
            for (int row = 0; row <= 8; row += 3)
            {
                for (int column = 0; column <= 8; column += 3)
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

            for (int row = 0; row <= 8; row++)
            {
                int column = CellGroup.GetPrimaryDiagonalColumnForRow(row);
                Cell cell = GetCell(row, column);
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

            for (int row = 0; row <= 8; row++)
            {
                int column = CellGroup.GetSecondaryDiagonalColumnForRow(row);
                Cell cell = GetCell(row, column);
                group.AddCell(cell);
            }

            this.Groups?.Add(group);
        }

        private void RemoveDiagonalGroups()
        {
            if (this.Groups == null || !this.Groups.Any())
            {
                return;
            }

            foreach (var group in this.Groups.Where(g => g.GroupType == CellGroupType.Diagonal))
            {
                foreach (var cell in group.Cells.ToList())
                {
                    group.RemoveCell(cell);
                }
            }

            this.Groups.RemoveAll(g => g.GroupType == CellGroupType.Diagonal);
        }
    }
}
