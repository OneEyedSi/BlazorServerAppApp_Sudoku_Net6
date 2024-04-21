using Microsoft.EntityFrameworkCore;
using SudokuClassLibrary;
using SudokuDataAccess;
using SudokuDataAccess.Models;
using refModels = SudokuDataAccess.Models.Reference;
using SudokuWebApp.Data.ModelMappers;
using System.Linq;

namespace SudokuWebApp.Data
{
    public class GameService
    {
        private readonly IDbContextFactory<DataContext> _dbContextFactory;
        public GameService(IDbContextFactory<DataContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<int?> SaveGameBoardAsync(Grid gameGrid)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            if (dbContext == null || gameGrid == null)
            {
                return null;
            }

            var initialCells = gameGrid.GetInitialCells();
            if (initialCells.IsNullOrEmpty())
            {
                return null;
            }

            var gameBoardId = GetExistingGameBoardId(gameGrid);
            if (gameBoardId.HasValue)
            {
                return gameBoardId;
            }

            var game = initialCells.ToGame(gameGrid.IsKillerSudoku);

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            return game.GameId;
        }

        public int? GetExistingGameBoardId(Grid gameGrid)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            if (dbContext == null)
            {
                return null;
            }

            // No need for null check of gameGrid or its initial cells.  These are checked in calling method.

            // Rough filter to reduce the number of records retrieved:
            // Same number of initial cells plus first initial cell in same location.

            var initialCells = gameGrid.GetInitialCells();
            var numberOfInitialCells = initialCells.Count();

            // If there are no initial cells then the game board is empty - no need to look for a saved copy.
            if (numberOfInitialCells == 0)
            {
                return null;
            }

            Cell firstCell = initialCells.First();
            int firstCellValue = firstCell.GetValue() ?? 0;

            // If the first cell marked as an initial value has no value set something has gone really wrong.
            if (firstCellValue == 0) 
            {
                return null;
            }

            int firstCellPositionId = refModels.Position.GetPositionIdFromRowAndColumn(firstCell.Row, firstCell.Column);

            // Rough first filter.
            var matchingGameBoards = dbContext.Games
                                        .Include(gb => gb.Values)
                                        .Where(gb => gb.Values.Count == numberOfInitialCells)
                                        .Where(gb => gb.Values.Any(v => v.Value == firstCellValue 
                                                                        && v.PositionId == firstCellPositionId));

            if (!matchingGameBoards.Any())
            {
                return null;
            }

            var orderedCellDetails = initialCells
                .Select(c => (Value: c.GetValue(), PositionId: refModels.Position.GetPositionIdFromRowAndColumn(c.Row, c.Column)))
                .OrderBy(t => t.PositionId)
                .ToList();
            foreach (var matchingBoard in matchingGameBoards)
            {
                // Ensure the values retrieved from the database are in the same order as the initial cell details.
                var boardValues = matchingBoard.Values.OrderBy(v => v.PositionId).ToList();
                bool isMatch = true;
                for(int i = 0; i < numberOfInitialCells; i++)
                {
                    var initialCellValue = orderedCellDetails[i].Value;
                    var initialCellPositionId = orderedCellDetails[i].PositionId;
                    var boardValue = matchingBoard.Values[i];
                    if (boardValue.Value != initialCellValue || boardValue.PositionId != initialCellPositionId)
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (isMatch)
                {
                    return matchingBoard.GameId;
                }
            }

            return null;
        }
    }
}
