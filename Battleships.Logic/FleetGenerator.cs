using System.Collections.Generic;
using System.Linq;
using Battleships.Logic.Contracts;
using Battleships.Logic.Exceptions;

namespace Battleships.Logic
{
    public class FleetGenerator
    {
        private readonly IGenerateRandomness _randomGenerator;
        private readonly BoardCoordinates _boardBounds;

        public FleetGenerator(IGenerateRandomness randomGenerator, BoardCoordinates boardBounds)
        {
            _randomGenerator = randomGenerator;
            _boardBounds = boardBounds;
        }
        
        public IEnumerable<BoardCoordinates[]> Generate(IEnumerable<ShipGenerationDescriptor> shipsToGenerate)
        {
            var shipSizesToGenerate = shipsToGenerate
                .OrderByDescending(d => d.SquareSize)
                .SelectMany(desc => Enumerable.Range(0, (int) desc.Count), (desc, _) => desc.SquareSize)
                .ToList();
            
            if (!shipSizesToGenerate.Any())
            {
                throw new FleetGenerationError();
            }
            
            var takenSquares = new HashSet<BoardCoordinates>();

            return shipSizesToGenerate
                .Select(shipSize => GenerateNonCollidingShipCoords(shipSize, takenSquares));
        }

        private BoardCoordinates[] GenerateNonCollidingShipCoords(uint shipSize, ISet<BoardCoordinates> takenSquares)
        {
            BoardCoordinates[] shipCoords;
            do
            {
                shipCoords = GenerateShipCoords(shipSize);
            } while (takenSquares.Overlaps(shipCoords));
            
            AddToCollisionDetection(shipCoords, takenSquares);

            return shipCoords;
        }

        private BoardCoordinates[] GenerateShipCoords(uint shipSize)
        {
            var isHorizontal = _randomGenerator.GetBool();
            var columnBounds = _boardBounds.Column;
            var rowBounds = _boardBounds.Row;
            if (isHorizontal)
            {
                columnBounds -= shipSize - 1;
            }
            else
            {
                rowBounds -= shipSize - 1;
            }
            
            var column = _randomGenerator.GetInRange(0, columnBounds);
            var row = _randomGenerator.GetInRange(0, rowBounds);
            if (shipSize == 1)
            {
                return new[] {new BoardCoordinates(column, row)};
            }

            return isHorizontal
                ? CreateHorizontalShipCoords(column, row, shipSize)
                : CreateVerticalShipCoords(column, row, shipSize);
        }

        private static void AddToCollisionDetection(
            IEnumerable<BoardCoordinates> shipCoords, ISet<BoardCoordinates> takenSquares)
        {
            foreach (var coord in shipCoords)
            {
                takenSquares.Add(coord);
            }
        }

        private static BoardCoordinates[] CreateVerticalShipCoords(uint column, uint row, uint shipSize) =>
            GenerateDimensionValues(row, shipSize)
                .Select(r => new BoardCoordinates(column, r))
                .ToArray();

        private static BoardCoordinates[] CreateHorizontalShipCoords(uint column, uint row, uint shipSize) =>
            GenerateDimensionValues(column, shipSize)
                .Select(c => new BoardCoordinates(c, row))
                .ToArray();

        private static IEnumerable<uint> GenerateDimensionValues(uint dimensionStartValue, uint shipSize) =>
            Enumerable
                .Range((int) dimensionStartValue, (int) shipSize)
                .Select(x => (uint) x);
    }
}