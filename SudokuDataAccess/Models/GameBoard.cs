using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuDataAccess.Models
{
    public class GameBoard
    {
        public int GameBoardId { get; set; }
        public bool IsKillerSudoku { get; set; } = false;
        public string Title { get; set; }

        #region Children **************************************************************************

        public List<UserGame> UserGames { get; set; }
        public List<GameBoardValue> Values { get; set; }

        #endregion
    }
}
