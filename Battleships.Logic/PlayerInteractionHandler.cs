using Battleships.Logic.Contracts;
using Battleships.Logic.Exceptions;

namespace Battleships.Logic
{
    public class PlayerInteractionHandler : IHandlePlayerInteraction
    {
        private readonly IParseCoords _coordsParser;
        private readonly ISuperviseFleet _fleetSupervisor;
        private readonly IManageBoard _boardManager;

        public PlayerInteractionHandler(
            IParseCoords coordsParser, ISuperviseFleet fleetSupervisor, IManageBoard boardManager)
        {
            _coordsParser = coordsParser;
            _fleetSupervisor = fleetSupervisor;
            _boardManager = boardManager;
        }

        public ShotResult MakeShot(string coordinates)
        {
            var boardCoords = _coordsParser.Parse(coordinates);
            
            if(!_boardManager.IsLegalShot(boardCoords))
            {
                throw new CoordinatesOutOfBoundsError();
            }

            var outcome = _fleetSupervisor.CheckShot(boardCoords);

            var shotResult = outcome.Match(
                miss => ShotResult.Miss,
                hit => ShotResult.Hit,
                sink => _fleetSupervisor.FloatingShipsLeft > 0 ? ShotResult.Sink : ShotResult.Win);
            
            _boardManager.MarkShot(boardCoords, outcome);
            
            return shotResult;
        }
    }
}