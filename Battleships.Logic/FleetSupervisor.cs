using System.Collections.Generic;
using System.Linq;
using Battleships.Logic.Contracts;
using Battleships.Logic.Contracts.ShotOutcomes;
using OneOf;

namespace Battleships.Logic
{
    public class FleetSupervisor : ISuperviseFleet
    {
        private readonly List<Ship> _fleet;

        public FleetSupervisor(IEnumerable<BoardCoordinates[]> fleet)
        {
            _fleet = fleet.Select(coords => new Ship(coords)).ToList();
        }

        public int FloatingShipsLeft => _fleet.Count;
        
        public OneOf<Miss, Hit, Sink> CheckShot(BoardCoordinates coordinates)
        {
            var hitShip = _fleet.FirstOrDefault(ship => ship.IsPartOfShip(coordinates));
            if (hitShip == null)
            {
                return new Miss();
            }
            
            hitShip.SetHit(coordinates);
            
            if(hitShip.IsFloating())
            {
                return new Hit();
            }

            _fleet.Remove(hitShip);
            return new Sink(hitShip.Coordinates);
        }
        
        private class Ship
        {
            private readonly Dictionary<BoardCoordinates, ShipPartStatus> _status;

            public IEnumerable<BoardCoordinates> Coordinates => _status.Keys;

            public Ship(IEnumerable<BoardCoordinates> shipCoords)
            {
                _status = shipCoords.ToDictionary(coords => coords, _ => ShipPartStatus.Floating);
            }

            public bool IsPartOfShip(BoardCoordinates coords) => _status.ContainsKey(coords);
            public bool IsFloating() => _status.Any(kvp => kvp.Value == ShipPartStatus.Floating);

            public void SetHit(BoardCoordinates coords) => _status[coords] = ShipPartStatus.Hit;
        }

        private enum ShipPartStatus
        {
            Floating,
            Hit
        }
    }
}