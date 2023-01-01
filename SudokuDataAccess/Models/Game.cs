using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuDataAccess.Models
{
    public class Game
    {
        public int GameId { get; set; }
        public string Title { get; set; }
        public DateTime TimeRecorded { get; set; }

        #region Parents ***************************************************************************

        public int UserProfileId { get; set; }
        public UserProfile? UserProfile {  get; set; }

        public int GameBoardId { get; set; }
        public GameBoard? GameBoard { get; set; }

        #endregion

        #region Children **************************************************************************

        public List<GameRun> Runs { get; set; }

        #endregion
    }
}
