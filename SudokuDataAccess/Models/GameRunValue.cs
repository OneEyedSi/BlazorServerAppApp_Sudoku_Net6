using SudokuDataAccess.Models.Reference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuDataAccess.Models
{
    public class GameRunValue
    {
        public int GameRunValueId { get; set; }
        public int? Value { get; set; }

        #region Parents ***************************************************************************

        public int GameRunId { get; set; }
        public GameRun? GameRun { get; set; }

        public int PositionId { get; set; }
        public Position? Position { get; set; }

        #endregion
    }
}
