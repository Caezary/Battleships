using System.Collections.Generic;
using System.Linq;
using Battleships.Logic.Contracts;

namespace Battleships.ConsoleApp
{
    public static class BoardCoordinatesExtensions
    {
        public static IEnumerable<int> GetRowRange(this BoardCoordinates dimensions)
        {
            return Enumerable.Range(0, (int) dimensions.Row);
        }

        public static IEnumerable<int> GetColumnRange(this BoardCoordinates dimensions)
        {
            return Enumerable.Range(0, (int) dimensions.Column);
        }
    }
}