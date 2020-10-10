using System.Collections.Generic;
using Battleships.Logic.Contracts;

namespace Battleships.Logic.Construction
{
    internal class BoardViewUpdaterNullObject : IUpdateBoardView
    {
        public void Missed(BoardCoordinates coords)
        {
        }

        public void GotHit(BoardCoordinates coords)
        {
        }

        public void Sunken(IEnumerable<BoardCoordinates> sunkenShipCoords)
        {
        }

        public void ResetGame()
        {
        }
    }
}