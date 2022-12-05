using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SudokuClassLibrary
{
    public class CellValueChangedEventArgs : EventArgs
    {
        public CellValueChangedEventArgs(int? previousValue, int? newValue)
        {
            PreviousValue = previousValue;
            NewValue = newValue;
        }
        public int? PreviousValue { get; set; }
        public int? NewValue { get; set; }
    }

    public class Cell
    {
        #region Constructors **********************************************************************

        public Cell(int row, int column) : this(row, column, null)
        { }

        public Cell(int row, int column, int? initialValue) : this(new Position(row, column), initialValue)
        { }

        public Cell(Position position) : this(position, null)
        { }

        public Cell(Position position, int? initialValue)
        {
            Position = position;
            SetInitialValue(initialValue);
        }

        #endregion

        #region Properties ************************************************************************

        public Position Position { get; }

        private int? _value;
        public int? Value
        {
            get
            {
                return _value;
            }

            set
            {
                CheckNewValue(value, this.GetCallerName());
                int? previousValue = _value;
                _value = value;
                ResetPossibleValues();
                int? newValue = _value;

                if (newValue != previousValue)
                {
                    var eventArgs = new CellValueChangedEventArgs(previousValue, newValue);
                    OnCellValueChanged(eventArgs);
                }
            }
        }

        public bool HasValueSet { get { return (_value.HasValue); } }
        public bool HasSinglePossibleValue { get { return (GetOnlyPossibleValue() != null); } }

        public bool IsInitialValue { get; set; }

        public List<CellGroup> ParentGroups { get; } = new();

        #endregion

        #region Methods ***************************************************************************

        public void SetInitialValue(int? value)
        {
            Value = value;
            IsInitialValue = value.HasValue;
            ResetPossibleValues();
        }

        private List<int> _possibleValues = new();

        public ReadOnlyCollection<int> GetPossibleValues()
        {
            return _possibleValues.AsReadOnly();
        }

        public ReadOnlyCollection<int> SetPossibleValue(int possibleValue)
        {
            var possibleValues = new List<int> { possibleValue };
            return SetPossibleValues(possibleValues);
        }

        public ReadOnlyCollection<int> SetPossibleValues(IEnumerable<int> possibleValues)
        {
            CheckNewPossibleValues(possibleValues);
            _possibleValues = DeDuplicateAndSortEnumerable(possibleValues);
            return _possibleValues.AsReadOnly();
        }

        public ReadOnlyCollection<int> AddPossibleValue(int valueToAdd)
        {
            var valuesToAdd = new List<int> { valueToAdd };
            return AddPossibleValues(valuesToAdd);
        }

        public ReadOnlyCollection<int> AddPossibleValues(IEnumerable<int> valuesToAdd)
        {
            CheckNewPossibleValues(valuesToAdd, throwOnNoNewValues: false);
            _possibleValues.AddRange(valuesToAdd);
            _possibleValues = DeDuplicateAndSortEnumerable(_possibleValues);

            return _possibleValues.AsReadOnly();
        }

        public ReadOnlyCollection<int> RemovePossibleValue(int valueToRemove)
        {
            var valuesToRemove = new List<int> { valueToRemove };
            return RemovePossibleValues(valuesToRemove);
        }

        public ReadOnlyCollection<int> RemovePossibleValues(IEnumerable<int> valuesToRemove)
        {
            _possibleValues.RemoveAll(x => valuesToRemove.Contains(x));

            if (!_possibleValues.Any())
            {
                // Restore the previous values before throwing.
                _possibleValues = new List<int>(valuesToRemove);

                throw new InvalidOperationException($"Invalid operation: Cell {Position} has no possible values left.");
            }

            return _possibleValues.AsReadOnly();
        }

        /// <summary>
        /// Returns the possible value if there is only one, otherwise returns null.
        /// </summary>
        public int? GetOnlyPossibleValue()
        {
            if (_possibleValues.Count > 1)
            {
                return null;
            }

            return _possibleValues.First();
        }

        private void CheckNewPossibleValues(IEnumerable<int> newValues, bool throwOnNoNewValues = true)
        {
            string errorMessage =
                "Invalid value: Cannot set possible values as empty; possible values must always have at least one value.";
            if (throwOnNoNewValues)
            {
                if (newValues == null)
                {
                    throw new ArgumentNullException(errorMessage);
                }

                if (!newValues.Any())
                {
                    throw new ArgumentException(errorMessage);
                }
            }

            foreach (var value in newValues)
            {
                CheckNewValue(value, this.GetCallerName());
            }
        }

        private static void CheckNewValue(int? newValue, string propertyName)
        {
            if (newValue.HasValue && (newValue.Value < 1 || newValue.Value > 9))
            {
                throw new ArgumentException(
                    $"Invalid value: Value must be between 1 and 9.  Attempted to set it to {newValue.Value}");
            }
        }

        private List<int> DeDuplicateAndSortEnumerable(IEnumerable<int> originalEnumerable)
        {
            if (originalEnumerable == null || !originalEnumerable.Any())
            {
                return new List<int>();
            }

            return originalEnumerable.Distinct().OrderBy(x => x).ToList();
        }

        private void ResetPossibleValues()
        {
            if (_value.HasValue)
            {
                _possibleValues = new List<int> { _value.Value };
                return;
            }
            _possibleValues = Enumerable.Range(1, 9).ToList();
        }

        #endregion

        #region Events and Event Handlers *********************************************************

        public event EventHandler<CellValueChangedEventArgs>? CellValueChanged;

        private void OnCellValueChanged(CellValueChangedEventArgs eventArgs)
        {
            CellValueChanged?.Invoke(this, eventArgs);
        }

        public void ParentGroup_RecalculateCellPossibleValues(object? sender, EventArgs eventArgs)
        {
            // Get intersection of available values from each parent group.
            // Apparently intersections with HashSets, rather than Lists, are 
            // faster.
            // See https://stackoverflow.com/questions/1674742/intersection-of-multiple-lists-with-ienumerable-intersect
            HashSet<int>? hashSet = null;
            foreach (var parentGroup in ParentGroups)
            {
                if (hashSet == null)
                {
                    hashSet = new HashSet<int>(parentGroup.GetAvailableValues());
                }
                else
                {
                    hashSet.IntersectWith(parentGroup.GetAvailableValues());
                }
            }

            _possibleValues = hashSet == null ? new List<int>() : hashSet.ToList();
        }

        #endregion 
    }
}
