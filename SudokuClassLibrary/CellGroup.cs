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

        private void RecalculateAvailableValues()
        {
            InitializeAvailableValues();
            var valuesAlreadyPlaced = Cells.Where(c => c.HasValueSet).Select(c => c.Value.Value);
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
