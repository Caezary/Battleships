using System.Collections.Generic;
using System.Linq;
using Battleships.Logic.Construction;
using FluentAssertions;
using Xunit;

namespace Battleships.Logic.Tests.Scenarios
{
    public class SimpleGameFlowScenario
    {
        [Fact]
        public void DefaultGameFlowGenerated_PlayerSpecifiesCoordsInSequence_GameIsWon()
        {
            var coordinates = GenerateAllBoardCoordinates();

            var flow = Generate.GameFlow().Build();

            var winningCoords = new List<string>();
            GameActionResult shotResult = new GameActionResult();
            
            foreach (var coords in coordinates)
            {
                shotResult = flow.MakeShot(coords);

                if (IsHit(shotResult))
                {
                    winningCoords.Add(coords);
                }
                
                if (IsWin(shotResult))
                {
                    break;
                }
            }

            shotResult.Outcome.Should().Be(GameActionOutcome.Win);
            winningCoords.Should().HaveCount(5 + 4 * 2);
        }

        [Fact]
        public void DefaultGameFlowGenerated_PlayerSpecifiesInvalidCoordinates_ErrorReturnedEveryTime()
        {
            var coordinates = new[]
            {
                "Z99",
                "A11",
                "K1",
                "K11",
                "test",
                "_____----_____",
            };
            
            var flow = Generate.GameFlow().Build();

            foreach (var coords in coordinates)
            {
                var shotResult = flow.MakeShot(coords);
                shotResult.Outcome.Should().Be(GameActionOutcome.Error);
                shotResult.Description.Should().NotBeEmpty();
            }
        }

        private static List<string> GenerateAllBoardCoordinates()
        {
            var columns = Enumerable.Range(0, 10).Select(c => (char) ('A' + c)).ToList();
            var rows = Enumerable.Range(1, 10).ToList();
            var coordinates = columns.SelectMany(_ => rows, (c, r) => $"{c}{r}").ToList();
            return coordinates;
        }

        private static bool IsHit(GameActionResult shotResult)
        {
            return shotResult.Outcome != GameActionOutcome.Error && shotResult.Outcome != GameActionOutcome.Miss;
        }

        private static bool IsWin(GameActionResult shotResult)
        {
            return shotResult.Outcome == GameActionOutcome.Win;
        }
    }
}