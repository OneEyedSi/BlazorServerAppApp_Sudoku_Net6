using SudokuDataAccess.Models.Reference;
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
        public bool IsKillerSudoku { get; set; } = false;
        public string Title { get; set; }
        public DateTime TimeRecorded { get; set; } = DateTime.Now;
        public int Hash { get; set; }

        #region Parents ***************************************************************************

        public int LevelId { get; set; }
        public Level? Level { get; set; }

        #endregion

        #region Children **************************************************************************

        public List<UserGame> UserGames { get; set; }
        public List<GameInitialValue> Values { get; set; }

        #endregion
    }
}
