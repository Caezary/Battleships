using System.Collections.Generic;
using System.Linq;
using Battleships.Logic.Contracts;

namespace Battleships.Logic.Construction
{
    public class GameFlowBuilder
    {
        private readonly IParseCoords _coordsParser = new CoordsParser();
        private readonly RandomGenerator _randomGenerator = new RandomGenerator();
        
        private BoardCoordinates _boardSizeBounds = Defaults.BoardSizeBounds;
        private ShipGenerationDescriptor[] _initialFleet = Defaults.InitialFleet;
        private IUpdateBoardView _boardViewUpdater = new BoardViewUpdaterNullObject();

        public GameFlowBuilder WithBoardSize(uint columns, uint rows) =>
            WithBoardSize(new BoardCoordinates(columns, rows));

        public GameFlowBuilder WithBoardSize(BoardCoordinates boardSizeBounds)
        {
            _boardSizeBounds = boardSizeBounds;
            return this;
        }

        public GameFlowBuilder WithInitialFleet(IEnumerable<ShipGenerationDescriptor> descriptors)
        {
            _initialFleet = descriptors.ToArray();
            return this;
        }

        public GameFlowBuilder WithBoardViewUpdater(IUpdateBoardView updater)
        {
            _boardViewUpdater = updater;
            return this;
        }

        public GameFlowFacade Build()
        {
            return new GameFlowFacade(_boardViewUpdater, GameStateFactoryMethod);
        }

        private IHandlePlayerInteraction GameStateFactoryMethod()
        {
            var fleetGenerator = new FleetGenerator(_randomGenerator, _boardSizeBounds);
            var fleet = fleetGenerator.Generate(_initialFleet);
            var fleetSupervisor = new FleetSupervisor(fleet);

            var boardManager = new BoardManager(_boardSizeBounds, _boardViewUpdater);
            
            return new PlayerInteractionHandler(_coordsParser, fleetSupervisor, boardManager);
        }
    }
}