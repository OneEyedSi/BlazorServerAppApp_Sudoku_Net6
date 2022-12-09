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

        public int Row => Position.Row;
        public int Column => Position.Column;

        private int? _value;
        public int? Value
        {
            get
            {
                return _value;
            }

            set
            {
                // Cannot change cell value if it was one of the initial values in the game.
                if (IsInitialValue)
                {
                    return;
                }

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
            // Must clear IsInitialValue because, if it were previously set, it would prevent Value 
            // from being set.
            IsInitialValue = false;
            Value = value;
            IsInitialValue = value.HasValue;
        }

        public void RemoveInitialValue()
        {
            SetInitialValue(null);
        }

        private Dictionary<int, bool> _possibleValues = new();

        /// <summary>
        /// Returns a read-only dictionary with nine elements, representing the nine possible 
        /// values the cell could have.  Any element set to true indicates that corresponding 
        /// key value is a possible value for the cell.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyDictionary<int, bool> GetPossibleValuesDictionary()
        {
            // This should never happen as we only expose _possibleValues as read-only.
            // But let's be safe.
            if (!(_possibleValues?.Any() ?? false))
            {
                ResetPossibleValues();
            }

            return new ReadOnlyDictionary<int, bool>(_possibleValues);
        }

        public IReadOnlyCollection<int> GetPossibleValuesList()
        {
            IReadOnlyDictionary<int, bool> possibleValuesDictionary = GetPossibleValuesDictionary();
            var possibleValuesSet = possibleValuesDictionary
                                        .Where(kvp => kvp.Value == true)
                                        .Select(kvp => kvp.Key)
                                        .ToList();
            return possibleValuesSet;
        }

        public int GetNumberOfPossibleValues()
        {
            return GetPossibleValuesList().Count;
        }

        /// <summary>
        /// Returns the possible value if there is only one, otherwise returns null.
        /// </summary>
        public int? GetOnlyPossibleValue()
        {
            if (GetNumberOfPossibleValues() > 1)
            {
                return null;
            }

            return _possibleValues.FirstOrDefault(kvp => kvp.Value == true).Key;
        }

        /// <summary>
        /// Returns the Index of the parent Square this cell is a member of, if the parent square 
        /// is set.
        /// </summary>
        /// <returns>Returns the Index of the parent Square this cell is a member of, if the parent 
        /// square is set.  Otherwise returns null.</returns>
        public int? GetParentSquareIndex()
        {
            return ParentGroups?.FirstOrDefault(pg => pg.GroupType == CellGroupType.Square)?.Index;
        }

        /// <summary>
        /// Indicates whether the cell is on a diagonal in a Killer Sudoku game.
        /// </summary>
        /// <returns>true if the game is a Killer Sudoku, as opposed to a regular Sudoku, and the 
        /// cell is on either the primary or secondary diagonal.  Otherwise returns false.</returns>
        /// <remarks>In Killer Sudoku no value may be repeated on either the primary or secondary 
        /// diagonal.  For regular Sudoku the diagonals can have repeated values.  So the diagonal 
        /// is only important in Killer Sudoku and can be ignored in regular Sudoku.</remarks>
        public bool IsOnKillerDiagonal()
        {
            return ParentGroups?.Any(pg => pg.GroupType == CellGroupType.Diagonal) ?? false;
        }

        private static void CheckNewValue(int? newValue, string propertyName)
        {
            if (newValue.HasValue && (newValue.Value < 1 || newValue.Value > 9))
            {
                throw new ArgumentException(
                    $"Invalid value: Value must be between 1 and 9.  Attempted to set it to {newValue.Value}");
            }
        }

        private void ResetPossibleValues()
        {
            if (_possibleValues == null)
            {
                _possibleValues = new();
            }

            bool setAllPossibleValues = !_value.HasValue;

            for(int i = 1; i <= 9; i++)
            {
                _possibleValues[i] = setAllPossibleValues || i == _value.Value;
            }
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
            
            // Should be illegal: No parent group.  But then what raised the event this method handled?
            if (hashSet == null)
            {
                _possibleValues = new();
            }

            for (int i = 1; i <= 9; i++)
            {
                _possibleValues[i] = hashSet == null || hashSet.Contains(i);
            }
        }

        #endregion 
    }
}
