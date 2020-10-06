using Battleships.Logic.Contracts;
using Battleships.Logic.Exceptions;

namespace Battleships.Logic
{
    public class CoordsParser : IParseCoords
    {
        public BoardCoordinates Parse(string coordinates)
        {
            if (coordinates.Length < 2)
            {
                throw new MalformedCoordinateError();
            }

            var column = char.ToUpperInvariant(coordinates[0]);
            var row = coordinates.Substring(1);

            if (!IsAsciiUppercaseLetter(column))
            {
                throw new MalformedCoordinateError();
            }

            if (!int.TryParse(row, out var rowValue))
            {
                throw new MalformedCoordinateError();
            }
            
            if(rowValue <= 0)
            {
                throw new MalformedCoordinateError();
            }

            var columnValue = (int) (column - 'A');
            rowValue -= 1;

            return new BoardCoordinates(columnValue, rowValue);
        }

        private static bool IsAsciiUppercaseLetter(char column)
        {
            return column >= 'A' && column <= 'Z';
        }
    }
}