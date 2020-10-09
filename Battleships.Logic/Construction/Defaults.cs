using Battleships.Logic.Contracts;

namespace Battleships.Logic.Construction
{
    public static class Defaults
    {
        public static readonly BoardCoordinates BoardSizeBounds = new BoardCoordinates(10, 10);

        public static readonly ShipGenerationDescriptor[] InitialFleet = {
            new ShipGenerationDescriptor {SquareSize = 5, Count = 1},
            new ShipGenerationDescriptor {SquareSize = 4, Count = 2}
        };
    }
}