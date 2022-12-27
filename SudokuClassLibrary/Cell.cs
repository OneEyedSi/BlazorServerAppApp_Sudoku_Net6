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
        public CellValueChangedEventArgs(int? previousValue, int? newValue,
            bool isReplayingHistory = false, bool possibleValuesChanged = false)
        {
            PreviousValue = previousValue;
            NewValue = newValue;
            IsReplayingHistory = isReplayingHistory;
            PossibleValuesChanged = possibleValuesChanged;
        }
        public int? PreviousValue { get; set; }
        public int? NewValue { get; set; }
        public bool IsReplayingHistory { get; set; }
        public bool PossibleValuesChanged { get; set; }
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

        public bool HasValueSet { get { return (_value.HasValue); } }
        public bool HasSinglePossibleValue { get { return (GetOnlyPossibleValue() != null); } }

        public bool IsInitialValue { get; set; }

        public List<CellGroup> ParentGroups { get; } = new();

        public bool HasUniquePossibleValueInParentGroup => GetUniquePossibleValuesInParentGroups().Any();

        #endregion

        #region Methods ***************************************************************************

        public int? GetValue()
        {
            return _value;
        }

        public void ResetValue()
        {
            // Will also clear the IsInitialValue flag.
            SetInitialValue(null);
        }

        public void SetInitialValue(int? newValue, bool isReplayingHistory = false)
        {
            SetValue(newValue, setAsInitialValue: true, isReplayingHistory);
        }

        public void SetGameValue(int? newValue, bool isReplayingHistory = false)
        {
            SetValue(newValue, setAsInitialValue: false, isReplayingHistory);
        }

        public void SetValue(int? newValue, bool setAsInitialValue = false, bool isReplayingHistory = false)
        {
            // Initial values are read-only during game: Cannot set cell value during game if it was
            // previously set as an initial value.
            if (!setAsInitialValue && IsInitialValue)
            {
                return;
            }

            IsInitialValue = setAsInitialValue && newValue.HasValue;

            IsValidValue(newValue);
            int? previousValue = _value;
            _value = newValue;

            if (newValue != previousValue)
            {
                ResetPossibleValues();

                var eventArgs =
                    new CellValueChangedEventArgs(previousValue, newValue, isReplayingHistory);
                OnCellValueChanged(eventArgs);
            }
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

        private void IsValidValue(int? newValue)
        {
            if (!IsValidValue(newValue, out string? errorMessage))
            {
                throw new ArgumentException(errorMessage);
            }
        }

        public bool IsValidValue(int? newValue, out string? errorMessage)
        {
            errorMessage = null;
            if (!newValue.HasValue)
            {
                return true;
            }

            if (newValue.Value < 1 || newValue.Value > 9)
            {
                errorMessage = "Invalid value: Value must be between 1 and 9.";
                return false;
            }

            var validValues = GetPossibleValuesList();
            if (validValues.Contains(newValue.Value))
            {
                return true;
            }

            foreach (var parentGroup in ParentGroups)
            {
                if (!parentGroup.GetAvailableValues().Contains(newValue.Value))
                {
                    errorMessage = $"Invalid value: Value already set in the current {parentGroup.GroupType.ToString().ToLower()}.";
                    return false;
                }
            }

            errorMessage = "Invalid value: Unknown error.";
            return false;
        }

        public IEnumerable<int> GetUniquePossibleValuesInParentGroups()
        {
            // Should be maximum of one possible value that is unique in any parent 
            // group.  However, player may enter an incorrect value somewhere so it's 
            // possible there is more than one.
            List<int> uniquePossibleValuesInAGroup = new();
            foreach (var parentGroup in ParentGroups)
            {
                var possibleValuesUniqueInGroup =
                    GetThisCellPossibleValuesUniqueInGroup(parentGroup);
                if (possibleValuesUniqueInGroup?.Any() ?? false)
                {
                    uniquePossibleValuesInAGroup.AddRange(possibleValuesUniqueInGroup);
                }
            }

            return uniquePossibleValuesInAGroup.Distinct();
        }

        private IEnumerable<int>? GetThisCellPossibleValuesUniqueInGroup(CellGroup parentGroup)
        {
            if (!(parentGroup?.Cells?.Any() ?? false))
            {
                return new List<int>();
            }

            var uniquePossibleValuesInGroup = parentGroup.Cells
                .Where(c => !c.HasValueSet)
                .SelectMany(c => c.GetPossibleValuesList())
                .GroupBy(pv => pv)
                .Where(grp => grp.Count() == 1)
                .Select(grp => grp.Key);

            if (!uniquePossibleValuesInGroup.Any())
            {
                return new List<int>();
            }

            var thisCellPossibleValuesUniqueInGroup =
                this.GetPossibleValuesList().Intersect(uniquePossibleValuesInGroup);
            return thisCellPossibleValuesUniqueInGroup;
        }

        private void ResetPossibleValues()
        {
            if (_possibleValues == null)
            {
                _possibleValues = new();
            }

            bool setAllPossibleValues = !_value.HasValue;

            for (int i = 1; i <= 9; i++)
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

            bool refreshCellElement = false;

            for (int i = 1; i <= 9; i++)
            {
                // To start with all possible values are true.  So if not previously set, default
                // to true.
                if (!_possibleValues.TryGetValue(i, out bool previousValue))
                {
                    previousValue = true;
                }
                _possibleValues[i] = hashSet == null || hashSet.Contains(i);
                if (_possibleValues[i] != previousValue)
                {
                    refreshCellElement = true;
                }
            }

            if (refreshCellElement)
            {
                var cellChangedEventArgs =
                    new CellValueChangedEventArgs(previousValue: null, newValue: null,
                        possibleValuesChanged: true);
                OnCellValueChanged(cellChangedEventArgs);
            }
        }

        #endregion 
    }
}
