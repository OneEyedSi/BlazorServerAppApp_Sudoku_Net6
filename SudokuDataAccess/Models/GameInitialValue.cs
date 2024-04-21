using SudokuDataAccess.Models.Reference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuDataAccess.Models
{
    public class GameInitialValue
    {
        public GameInitialValue()
        {

        }

        public GameInitialValue(int? value, int row, int column)
        {
            Value = value;
            PositionId = Position.GetPositionIdFromRowAndColumn(row, column);
        }

        public int GameInitialValueId { get; set; }
        public int? Value { get; set; }


        #region Parents ***************************************************************************

        public int GameId { get; set; }
        public Game? Game {  get; set; }

        public int PositionId { get; set; }
        public Position? Position {  get; set; }

        #endregion
    }
}
