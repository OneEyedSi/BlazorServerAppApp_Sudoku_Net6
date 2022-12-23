using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuClassLibrary
{
    public class CellGroup
    {
        public CellGroup(CellGroupType groupType, int index)
        {
            GroupType = groupType;
            Index = index;
            InitializeAvailableValues();
        }

        public CellGroupType GroupType { get; }

        public int Index { get; }

        public List<Cell> Cells { get; } = new();

        private List<int> _availableValues = new();

        public ReadOnlyCollection<int> GetAvailableValues()
        {
            return _availableValues.AsReadOnly();
        }

        public void AddCell(Cell cellToAddToGroup, bool doRecalculateValues = true)
        {
            Cells.Add(cellToAddToGroup);
            RecalculateCellPossibleValues += cellToAddToGroup.ParentGroup_RecalculateCellPossibleValues;
            cellToAddToGroup.CellValueChanged += Cell_CellValueChanged;
            cellToAddToGroup.ParentGroups.Add(this);
            if (doRecalculateValues)
            {
                RecalculateAvailableValues();
            }
        }

        public void RemoveCell(Cell cellToAddToGroup, bool doRecalculateValues = true)
        {
            Cells.Remove(cellToAddToGroup);
            RecalculateCellPossibleValues -= cellToAddToGroup.ParentGroup_RecalculateCellPossibleValues;
            cellToAddToGroup.CellValueChanged -= Cell_CellValueChanged;
            cellToAddToGroup.ParentGroups.Remove(this);
            if (doRecalculateValues)
            {
                RecalculateAvailableValues();
            }
        }

        public static int GetSquareIndexForCell(Cell cell)
        {
            int squareIndexForCell = GetSquareIndexForRowAndColumn(cell.Row, cell.Column);
            return squareIndexForCell;
        }

        public static int GetSquareIndexForRowAndColumn(int row, int column)
        {
            if (row < 0 || column < 0 || row > 8 || column > 8)
            {
                return -1;
            }

            // (row / 3) and (column / 3) are integer division.
            int squareIndexForCell = (row / 3) * 3 + (column / 3);
            return squareIndexForCell;
        }

        public static bool IsCellInPrimaryDiagonal(Cell cell)
        {
            var cellPosition = cell?.Position;
            if (cellPosition == null)
            {
                return false;
            }

            return (cellPosition.Column == GetPrimaryDiagonalColumnForRow(cellPosition.Row));
        }

        public static int GetPrimaryDiagonalColumnForRow(int row)
        {
            if (row < 0 || row > 8)
            {
                return -1;
            }

            int column = row;
            return column;
        }

        public static bool IsCellInSecondaryDiagonal(Cell cell)
        {
            var cellPosition = cell?.Position;
            if (cellPosition == null)
            {
                return false;
            }

            return (cellPosition.Column == GetSecondaryDiagonalColumnForRow(cellPosition.Row));
        }

        public static int GetSecondaryDiagonalColumnForRow(int row)
        {
            int maxValue = 8;
            if (row < 0 || row > maxValue)
            {
                return -1;
            }

            int column = maxValue - row;
            return column;
        }

        private void RecalculateAvailableValues()
        {
            InitializeAvailableValues();
            var valuesAlreadyPlaced = Cells.Where(c => c.HasValueSet).Select(c => c.GetValue().Value);
            _availableValues = _availableValues.Except(valuesAlreadyPlaced).ToList();
            var eventArgs = new EventArgs();
            OnRecalculateCellPossibleValues(eventArgs);
        }

        private void InitializeAvailableValues()
        {
            _availableValues = Enumerable.Range(start: 1, count: 9).ToList();
        }

        #region Events and Event Handlers *********************************************************

        public event EventHandler? RecalculateCellPossibleValues;

        private void OnRecalculateCellPossibleValues(EventArgs eventArgs)
        {
            RecalculateCellPossibleValues?.Invoke(this, eventArgs);
        }

        private void Cell_CellValueChanged(object? sender, CellValueChangedEventArgs eventArgs)
        {
            RecalculateAvailableValues();
        }

        #endregion
    }
}
