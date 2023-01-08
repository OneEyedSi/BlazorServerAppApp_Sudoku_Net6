using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SudokuDataAccess.Models
{
    public class GameRun
    {
        public int GameRunId { get; set; }
        public int RunNumber { get; set; }
        public DateTime TimeRecorded { get; set; }
        public long? MillisecondsTaken { get; set; }
        public bool WasCompleted { get; set; }

        #region Parents ***************************************************************************

        public int UserGameId { get; set; }
        public UserGame? Game { get; set; }

        #endregion

        #region Children **************************************************************************

        public List<GameRunOption> Options { get; set; }
        public List<GameRunValue> Values { get; set; }
        public List<GameRunHistoryEntry> History { get; set; }

        #endregion
    }
}
