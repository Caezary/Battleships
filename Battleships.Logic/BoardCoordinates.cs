namespace Battleships.Logic
{
    public class BoardCoordinates
    {
        public int Column { get; }
        public int Row { get; }

        public BoardCoordinates(int column, int row)
        {
            Column = column;
            Row = row;
        }
    }
}