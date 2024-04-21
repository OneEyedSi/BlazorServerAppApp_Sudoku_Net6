using SudokuClassLibrary;
using SudokuDataAccess.Models;
using referenceModel = SudokuDataAccess.Models.Reference;

namespace SudokuWebApp.Data.ModelMappers
{
    public static class GameMapperExtensions
    {
        public static Game? ToGame(this IEnumerable<Cell>? initialCells, bool isKillerSudoku)
        {
#pragma warning disable CS8604 // Possible null reference argument.  Suppress because IsNullOrEmpty() can handle nulls.
            if (initialCells.IsNullOrEmpty())
            {
                return null;
            }
#pragma warning restore CS8604 // Possible null reference argument.

            var game = new Game
            {
                IsKillerSudoku = isKillerSudoku,
                Title = $"Game_{DateTime.Now.ToString("yyyyMMdd_HHmmss_fff")}"
            };

            foreach (var cell in initialCells)
            {
                if (cell.HasValueSet)
                {
#pragma warning disable CS8629 // Nullable value type may be null.
                    game.Values.Add(new GameInitialValue(cell.GetValue().Value, cell.Row, cell.Column));
#pragma warning restore CS8629 // Nullable value type may be null.
                }
            }

            return game;
        }

        public static IEnumerable<Cell> ToIEnumerableCells(Game? game)
        {
            var initialCells = new List<Cell>();

            if (game == null || game.Values.IsNullOrEmpty())
            {
                return initialCells;
            }

            foreach (var value in game.Values)
            {
                referenceModel.Position? position = value.Position;
                if (position != null)
                {
                    var cell = new Cell(position.Row, position.Column, value.Value);
                    initialCells.Add(cell);
                }
            }

            return initialCells;
        }
    }
}
