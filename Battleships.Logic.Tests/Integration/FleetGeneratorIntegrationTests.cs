using System.Collections.Generic;
using System.Linq;
using Battleships.Logic.Contracts;
using FluentAssertions;
using Xunit;

namespace Battleships.Logic.Tests.Integration
{
    public class FleetGeneratorIntegrationTests
    {
        private readonly IGenerateRandomness _randomGenerator = new RandomGenerator();

        [Theory]
        [InlineData(18, 18)]
        [InlineData(16, 16)]
        [InlineData(14, 14)]
        [InlineData(12, 12)]
        [InlineData(10, 10)]
        public void Generate_DifferentBoardSizesAndStandardFleetSize_WillGenerateWithinBounds(
            uint columns, uint rows)
        {
            var boardBounds = new BoardCoordinates(columns, rows);
            var sut = new FleetGenerator(_randomGenerator, boardBounds);
            var shipsToGenerate = GetShipsToGenerate();

            var result = sut.Generate(shipsToGenerate);

            VerifyGeneratedWithinBounds(result, columns, rows);
        }

        [Fact]
        public void Generate_RepeatedHundredTimes_WillAlwaysGenerateWithinBounds()
        {
            var columns = 10U;
            var rows = 10U;
            var boardBounds = new BoardCoordinates(columns, rows);
            var sut = new FleetGenerator(_randomGenerator, boardBounds);
            var shipsToGenerate = GetShipsToGenerate();

            foreach (var _ in Enumerable.Range(0, 100))
            {
                var result = sut.Generate(shipsToGenerate);

                VerifyGeneratedWithinBounds(result, columns, rows);
            }
        }

        private static void VerifyGeneratedWithinBounds(IEnumerable<BoardCoordinates[]> result, uint columns, uint rows)
        {
            result.SelectMany(coords => coords).ToList().Should()
                .OnlyHaveUniqueItems()
                .And.HaveCount(35)
                .And.OnlyContain(coords => coords.Column < columns && coords.Row < rows);
        }

        private static ShipGenerationDescriptor[] GetShipsToGenerate()
        {
            return new[]
            {
                new ShipGenerationDescriptor {SquareSize = 1, Count = 5},
                new ShipGenerationDescriptor {SquareSize = 2, Count = 4},
                new ShipGenerationDescriptor {SquareSize = 3, Count = 3},
                new ShipGenerationDescriptor {SquareSize = 4, Count = 2},
                new ShipGenerationDescriptor {SquareSize = 5, Count = 1}
            };
        }
    }
}