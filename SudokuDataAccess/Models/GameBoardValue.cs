﻿using SudokuDataAccess.Models.Reference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuDataAccess.Models
{
    public class GameBoardValue
    {
        public GameBoardValue()
        {

        }

        public GameBoardValue(int? value, int row, int column)
        {
            Value = value;
            PositionId = Position.GetPositionIdFromRowAndColumn(row, column);
        }

        public int GameBoardValueId { get; set; }
        public int? Value { get; set; }


        #region Parents ***************************************************************************

        public int GameBoardId { get; set; }
        public GameBoard? GameBoard {  get; set; }

        public int PositionId { get; set; }
        public Position? Position {  get; set; }

        #endregion
    }
}
