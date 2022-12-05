// See https://aka.ms/new-console-template for more information
using SudokuClassLibrary;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SudokuClassLibrary
{
    internal class Program
    {
        private void Dummy()
        {
            string? errorMessage = null;

            Position position = new(4, 6);

            Cell cell = new(position);
            DisplayCellDetails(cell, "Cell initialized without value");

            cell.RemovePossibleValue(1);
            DisplayCellDetails(cell, "Removed possible value 1");

            cell.RemovePossibleValues(new[] { 2, 3 });
            DisplayCellDetails(cell, "Removed possible values 2 & 3");

            cell.SetPossibleValue(6);
            DisplayCellDetails(cell, "Set possible value to 6");

            cell.SetPossibleValues(new[] { 1, 2, 3 });
            DisplayCellDetails(cell, "Set possible values to 1, 2, 3");

            AlterCellAndDisplayDetails<IEnumerable<int>, ReadOnlyCollection<int>>(cell, cell.SetPossibleValues,
                new[] { 0, 1, 2, 3, 4 },
                "Set possible values including invalid value 0");

            AlterCellAndDisplayDetails<IEnumerable<int>, ReadOnlyCollection<int>>(cell, cell.SetPossibleValues,
                new[] { 1, 2, 3, 4, 10 },
                "Set possible values including invalid value 10");

            cell.AddPossibleValue(4);
            DisplayCellDetails(cell, "Added possible value 4");

            cell.AddPossibleValues(new[] { 5, 6 });
            DisplayCellDetails(cell, "Added possible values 5 & 6");

            AlterCellAndDisplayDetails<IEnumerable<int>, ReadOnlyCollection<int>>(cell, cell.AddPossibleValues,
                new[] { 0, 5, 6 },
                "Added possible values, including invalid value 0");

            AlterCellAndDisplayDetails<IEnumerable<int>, ReadOnlyCollection<int>>(cell, cell.AddPossibleValues,
                new[] { 5, 6, 10 },
                "Added possible values, including invalid value 10");

            cell.Value = 7;
            DisplayCellDetails(cell, "Set cell Value = 7");

            try
            {
                cell.Value = 0;
            }
            catch (Exception ex)
            {
                errorMessage = $"{ex.GetType().Name}: {ex.Message}";
            }
            DisplayCellDetails(cell, "Set cell Value to invalid value 0", errorMessage);

            try
            {
                cell.Value = 10;
            }
            catch (Exception ex)
            {
                errorMessage = $"{ex.GetType().Name}: {ex.Message}";
            }
            DisplayCellDetails(cell, "Set cell Value to invalid value 10", errorMessage);

            cell = new Cell(position, 8);
            DisplayCellDetails(cell, "Reinitialized cell with value 8");

            try
            {
                cell = new Cell(position, 0);
            }
            catch (Exception ex)
            {
                errorMessage = $"{ex.GetType().Name}: {ex.Message}";
            }
            DisplayCellDetails(cell, "Reinitialized cell with invalid value 0", errorMessage);

            try
            {
                cell = new Cell(position, 10);
            }
            catch (Exception ex)
            {
                errorMessage = $"{ex.GetType().Name}: {ex.Message}";
            }
            DisplayCellDetails(cell, "Reinitialized cell with invalid value 10", errorMessage);

            cell = new Cell(position);
            DisplayCellDetails(cell, "Reinitialized cell without value");

            cell.RemovePossibleValues(new int[] { 1, 2, 3, 4, 5, 6, 8, 9 });
            DisplayCellDetails(cell, "Removed all possible values apart from 7");

            AlterCellAndDisplayDetails<int, ReadOnlyCollection<int>>(cell, cell.RemovePossibleValue, 7,
                "Removed last possible value, 7");

            cell.SetPossibleValues(new[] { 1, 2, 3 });
            DisplayCellDetails(cell, "Set possible values to 1, 2, 3");

            AlterCellAndDisplayDetails<IEnumerable<int>, ReadOnlyCollection<int>>(cell, cell.RemovePossibleValues,
                new[] { 1, 2, 3 }, "Removed all possible values");
        }

        static void AlterCellAndDisplayDetails<TIn, TResult>(Cell cell, Func<TIn, TResult> function, TIn argument, string title)
        {
            string? errorMessage = null;
            try
            {
                function(argument);
            }
            catch (Exception ex)
            {
                errorMessage = $"{ex.GetType().Name}: {ex.Message}";
            }
            DisplayCellDetails(cell, title, errorMessage);

        }

        static void DisplayCellDetails(Cell cell, string title, string? errorMessage = null)
        {
            Console.WriteLine(title);
            Console.WriteLine(new string('-', title.Length));

            if (errorMessage != null)
            {
                Console.WriteLine(errorMessage);
            }

            Console.WriteLine($"Cell Position: {cell.Position}");
            Console.WriteLine($"PossibleValues: {string.Join(", ", cell.GetPossibleValues())}");
            Console.WriteLine($"HasSinglePossibleValue: {cell.HasSinglePossibleValue}");
            Console.WriteLine($"IsInitialValue: {cell.IsInitialValue}");
            Console.WriteLine();
        }

        static string GetDisplayText(object? objectToDisplay)
        {
            return objectToDisplay?.ToString() ?? "[NULL]";
        }
    }
}