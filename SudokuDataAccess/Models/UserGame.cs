using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuDataAccess.Models
{
    public class UserGame
    {
        public int UserGameId { get; set; }

        #region Parents ***************************************************************************

        public int UserProfileId { get; set; }
        public UserProfile? UserProfile {  get; set; }

        public int GameId { get; set; }
        public Game? Game { get; set; }

        #endregion

        #region Children **************************************************************************

        public List<GameRun> Runs { get; set; } = new List<GameRun>();

        #endregion
    }
}
