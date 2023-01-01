using SudokuDataAccess.Models.Reference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuDataAccess.Models
{
    public class GameRunOption
    {
        public int GameRunOptionId { get; set; }
        public bool Value { get; set; }

        #region Parents ***************************************************************************

        public int GameRunId { get; set; }
        public GameRun? GameRun { get; set; }

        public int OptionId { get; set; }
        public Option? Option { get; set; }

        #endregion
    }
}
