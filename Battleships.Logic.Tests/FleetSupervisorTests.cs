using System.Collections.Generic;
using Battleships.Logic.Contracts;
using Battleships.Logic.Contracts.ShotOutcomes;
using FluentAssertions;
using Xunit;

namespace Battleships.Logic.Tests
{
    public class FleetSupervisorTests
    {
        [Fact]
        public void CheckShot_NothingHit_ReturnsMissAndFleetSizeNotReduced()
        {
            var fleet = new[] {CreateOneSquareShip(), CreateTwoSquareShip(), CreateThreeSquareShip()};
            var sut = CreateSupervisor(fleet);
            var coords = new BoardCoordinates(0, 1);

            var result = sut.CheckShot(coords);

            result.Value.Should().BeOfType<Miss>();
            sut.FloatingShipsLeft.Should().Be(3);
        }
        
        [Fact]
        public void CheckShot_ThreeSquareUntouchedShipHit_ReturnsHitAndFleetSizeNotReduced()
        {
            var fleet = new[] {CreateOneSquareShip(), CreateTwoSquareShip(), CreateThreeSquareShip()};
            var sut = CreateSupervisor(fleet);
            var coords = new BoardCoordinates(6, 2);

            var result = sut.CheckShot(coords);

            result.Value.Should().BeOfType<Hit>();
            sut.FloatingShipsLeft.Should().Be(3);
        }
        
        [Fact]
        public void CheckShot_SingleOneSquareShipHit_ReturnsSinkAndFleetSizeReducedToZero()
        {
            var fleet = new[] {CreateOneSquareShip()};
            var sut = CreateSupervisor(fleet);
            var coords = new BoardCoordinates(2, 2);

            var result = sut.CheckShot(coords);

            result.Value.Should().BeOfType<Sink>();
            result.AsT2.SunkenShipCoords.Should().Contain(CreateOneSquareShip());
            sut.FloatingShipsLeft.Should().Be(0);
        }
        
        [Fact]
        public void CheckShot_TwoSquareShipHitTwoTimes_ReturnsSinkAndFleetSizeReduced()
        {
            var fleet = new[] {CreateOneSquareShip(), CreateTwoSquareShip(), CreateThreeSquareShip()};
            var sut = CreateSupervisor(fleet);
            var coordsA = new BoardCoordinates(4, 2);
            var coordsB = new BoardCoordinates(4, 3);

            sut.CheckShot(coordsA);
            var result = sut.CheckShot(coordsB);

            result.Value.Should().BeOfType<Sink>();
            result.AsT2.SunkenShipCoords.Should().Contain(CreateTwoSquareShip());
            sut.FloatingShipsLeft.Should().Be(2);
        }

        private static FleetSupervisor CreateSupervisor(IEnumerable<BoardCoordinates[]> fleet)
        {
            return new FleetSupervisor(fleet);
        }

        private static BoardCoordinates[] CreateOneSquareShip()
            => new[] {new BoardCoordinates(2, 2)};

        private static BoardCoordinates[] CreateTwoSquareShip()
            => new[]
            {
                new BoardCoordinates(4, 2),
                new BoardCoordinates(4, 3)
            };

        private static BoardCoordinates[] CreateThreeSquareShip()
            => new[]
            {
                new BoardCoordinates(6, 2),
                new BoardCoordinates(6, 3),
                new BoardCoordinates(6, 4)
            };
    }
}