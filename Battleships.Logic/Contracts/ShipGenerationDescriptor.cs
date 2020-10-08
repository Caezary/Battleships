namespace Battleships.Logic.Contracts
{
    public class ShipGenerationDescriptor
    {
        private const uint DefaultSquareSize = 1;
        private const uint DefaultCount = 1;

        public uint SquareSize { get; set; } = DefaultSquareSize;
        public uint Count { get; set; } = DefaultCount;
    }
}