using System.Collections.Generic;

namespace Battleships.Logic.Contracts
{
    public interface IUpdateBoardView
    {
        void Missed(BoardCoordinates coords);
        void GotHit(BoardCoordinates coords);
        void Sunken(IEnumerable<BoardCoordinates> sunkenShipCoords);
    }
}