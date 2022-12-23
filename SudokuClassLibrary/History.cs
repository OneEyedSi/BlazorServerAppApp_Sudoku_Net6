using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SudokuClassLibrary
{
    /// <summary>
    /// Records the history of changes to cell values (either initial values or game values).
    /// </summary>
    /// <remarks>Undo involves returning the HistoryValue at the current index.
    /// 
    /// Redo involves returning the HistoryValue at the next index.
    /// 
    /// Each HistoryValue includes PreviousValue and NextValue, so each HistoryValue can be used 
    /// for Undo and later for Redo.
    /// 
    /// History starts with CurrentIndex = -1 and an empty list.  Both CanUndo and CanRedo return 
    /// false.  
    /// 
    /// When a HistoryValue is added to the list the CurrentIndex is incremented.  If there 
    /// were existing HistoryValues at or after that new CurrentIndex they are removed before the 
    /// new HistoryValue is added to the list.  After adding the new HistoryValue, CanUndo returns 
    /// true, CanRedo returns false.
    /// 
    /// For a non-empty list, when CurrentIndex = -1 CanUndo is false, CanRedo is true.
    /// 
    /// For a non-empty list, when CurrentIndex = maximum index (ie 1 less than list Count) then 
    /// CanUndo is true and CanRedo is false.
    /// 
    /// For a non-empty list, when CurrentIndex is greater than -1 and less than the maximum 
    /// index then both CanUndo and CanRedo are true.
    /// </remarks>
    public class History
    {
        private List<HistoryValue> _history = new();
        private int _currentIndex = -1;

        public bool CanUndo => (_currentIndex >= 0);

        public bool CanRedo => (_currentIndex < _history.Count - 1);

        public void AddChange(HistoryValue value)
        {
            // We're changing history; remove any future history.
            if (CanRedo) 
            {
                RemoveChangesAfterIndex(_currentIndex);
            }

            AddNewChange(value);
        }

        public HistoryValue? GetPreviousChange()
        {
            if (!CanUndo)
            {
                return null;
            }
            // Yeah, this could be written return _history[_currentIndex--]; but 
            // I think the intent is less confusing this way.
            int indexToReturn = _currentIndex;
            _currentIndex--;
            return _history[indexToReturn];
        }

        public HistoryValue? GetNextChange()
        {
            if (!CanRedo)
            {
                return null;
            }
            // Yeah, this could be written return _history[++_currentIndex]; but 
            // I think the intent is less confusing this way.
            _currentIndex++;
            return _history[_currentIndex];
        }

        public void Clear()
        {
            _history.Clear();
            _currentIndex = -1;
        }

        private void RemoveChangesAfterIndex(int index)
        {
            if (index < 0)
            {
                static bool predicate(HistoryValue hv) => true;
                _history.RemoveAll(predicate);
                return;
            }
            int startIndex = index + 1;
            int numberToRemove = _history.Count - startIndex;
            _history.RemoveRange(startIndex, numberToRemove);
        }

        private void AddNewChange(HistoryValue value)
        {
            _history.Add(value);
            _currentIndex = _history.Count - 1;
        }
    }
}
