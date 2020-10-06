using System.Collections.Generic;

namespace Battleships.Logic.Contracts.ShotOutcomes
{
    public class Sink
    {
        public IEnumerable<BoardCoordinates> SunkenShipCoords { get; }

        public Sink(IEnumerable<BoardCoordinates> sunkenShipCoords)
        {
            SunkenShipCoords = sunkenShipCoords;
        }
    }
}