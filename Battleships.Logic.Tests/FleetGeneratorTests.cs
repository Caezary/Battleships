using System;
using System.Collections.Generic;
using System.Linq;
using Battleships.Logic.Contracts;
using Battleships.Logic.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Battleships.Logic.Tests
{
    public class FleetGeneratorTests
    {
        private readonly Mock<IGenerateRandomness> _randomGeneratorMock = new Mock<IGenerateRandomness>();
        private readonly BoardCoordinates _boardBounds = new BoardCoordinates(10, 10);
        private readonly FleetGenerator _sut;

        public FleetGeneratorTests()
        {
            _sut = new FleetGenerator(_randomGeneratorMock.Object, _boardBounds);
        }

        [Fact]
        public void Generate_NothingToGenerate_Throws()
        {
            var shipsToGenerate = Enumerable.Empty<ShipGenerationDescriptor>();
            
            Action act = () => _sut.Generate(shipsToGenerate).ToList();

            act.Should().Throw<FleetGenerationError>();
        }
        
        [Fact]
        public void Generate_SingleOneSquareShipToGenerate_ReturnsOneShipFleet()
        {
            var shipsToGenerate = new[] {new ShipGenerationDescriptor()};
            _randomGeneratorMock.SetupSequence(r => r.GetInRange(It.IsAny<uint>(), It.IsAny<uint>()))
                .Returns(2).Returns(2);
            
            var result = _sut.Generate(shipsToGenerate).ToList();

            result.Should().ContainSingle()
                .Which.Should().Equal(new BoardCoordinates(2, 2));
        }
        
        [Fact]
        public void Generate_TwoOneSquareShipsToGenerate_ReturnsTwoShipFleet()
        {
            var shipsToGenerate = new[] {new ShipGenerationDescriptor {Count = 2}};
            _randomGeneratorMock.SetupSequence(r => r.GetInRange(It.IsAny<uint>(), It.IsAny<uint>()))
                .Returns(2).Returns(2)
                .Returns(3).Returns(3);
            
            var result = _sut.Generate(shipsToGenerate).ToList();

            result.Should().HaveCount(2);
            result[0].Should().Contain(new BoardCoordinates(2, 2));
            result[1].Should().Contain(new BoardCoordinates(3, 3));
        }
        
        [Fact]
        public void Generate_TwoOneSquareShipsToGenerate_ReturnsTwoShipFleetAtDifferentPositions()
        {
            var shipsToGenerate = new[] {new ShipGenerationDescriptor {Count = 2}};
            _randomGeneratorMock.SetupSequence(r => r.GetInRange(It.IsAny<uint>(), It.IsAny<uint>()))
                .Returns(2).Returns(2)
                .Returns(2).Returns(2)
                .Returns(9).Returns(4);
            
            var result = _sut.Generate(shipsToGenerate).ToList();

            result.Should().HaveCount(2);
            result[0].Should().Contain(new BoardCoordinates(2, 2));
            result[1].Should().Contain(new BoardCoordinates(9, 4));
        }
        
        [Fact]
        public void Generate_SingleTwoSquareHorizontalShipToGenerate_ReturnsOneShipFleet()
        {
            var shipsToGenerate = new[] {new ShipGenerationDescriptor {SquareSize = 2}};
            _randomGeneratorMock.SetupSequence(r => r.GetInRange(It.IsAny<uint>(), It.IsAny<uint>()))
                .Returns(2).Returns(2);
            _randomGeneratorMock.Setup(r => r.GetBool()).Returns(true);
            
            var result = _sut.Generate(shipsToGenerate).ToList();

            result.Should().ContainSingle()
                .Which.Should().Equal(
                    new BoardCoordinates(2, 2), new BoardCoordinates(3, 2));
        }
        
        [Fact]
        public void Generate_SingleTwoSquareVerticalShipToGenerate_ReturnsOneShipFleet()
        {
            var shipsToGenerate = new[] {new ShipGenerationDescriptor {SquareSize = 2}};
            _randomGeneratorMock.SetupSequence(r => r.GetInRange(It.IsAny<uint>(), It.IsAny<uint>()))
                .Returns(2).Returns(2);
            _randomGeneratorMock.Setup(r => r.GetBool()).Returns(false);
            
            var result = _sut.Generate(shipsToGenerate).ToList();

            result.Should().ContainSingle()
                .Which.Should().Equal(
                    new BoardCoordinates(2, 2), new BoardCoordinates(2, 3));
        }
        
        [Fact]
        public void Generate_ManyShipsToGenerate_ReturnsGeneratedFleet()
        {
            var shipsToGenerate = new[]
            {
                new ShipGenerationDescriptor {SquareSize = 3, Count = 3},
                new ShipGenerationDescriptor {SquareSize = 4, Count = 2},
                new ShipGenerationDescriptor {SquareSize = 5, Count = 1}
            };
            MockRandomPositions();
            MockRandomHorizontality();
            
            var result = _sut.Generate(shipsToGenerate).ToList();

            result.Should().HaveCount(6);
            VerifyFiveSquareShipPosition(result);
            VerifyFourSquareShipsPositions(result);
            VerifyThreeSquareShipsPositions(result);
        }

        private void MockRandomPositions()
        {
            _randomGeneratorMock.SetupSequence(r => r.GetInRange(It.IsAny<uint>(), It.IsAny<uint>()))
                .Returns(4).Returns(1)
                .Returns(3).Returns(2)
                .Returns(8).Returns(3)
                .Returns(6).Returns(6)
                .Returns(0).Returns(3)
                .Returns(2).Returns(3)
                .Returns(1).Returns(7)
                .Returns(7).Returns(7)
                .Returns(2).Returns(5)
                .Returns(6).Returns(1);
        }

        private void MockRandomHorizontality()
        {
            _randomGeneratorMock.SetupSequence(r => r.GetBool())
                .Returns(false)
                .Returns(true)
                .Returns(false)
                .Returns(true)
                .Returns(true)
                .Returns(true)
                .Returns(true)
                .Returns(false)
                .Returns(true)
                .Returns(true);
        }

        private static void VerifyFiveSquareShipPosition(List<BoardCoordinates[]> result)
        {
            result[0].Should().Equal(
                new BoardCoordinates(4, 1),
                new BoardCoordinates(4, 2),
                new BoardCoordinates(4, 3),
                new BoardCoordinates(4, 4),
                new BoardCoordinates(4, 5));
        }

        private static void VerifyFourSquareShipsPositions(List<BoardCoordinates[]> result)
        {
            result[1].Should().Equal(
                new BoardCoordinates(8, 3),
                new BoardCoordinates(8, 4),
                new BoardCoordinates(8, 5),
                new BoardCoordinates(8, 6));
            result[2].Should().Equal(
                new BoardCoordinates(0, 3),
                new BoardCoordinates(1, 3),
                new BoardCoordinates(2, 3),
                new BoardCoordinates(3, 3));
        }

        private static void VerifyThreeSquareShipsPositions(List<BoardCoordinates[]> result)
        {
            result[3].Should().Equal(
                new BoardCoordinates(1, 7),
                new BoardCoordinates(2, 7),
                new BoardCoordinates(3, 7));
            result[4].Should().Equal(
                new BoardCoordinates(7, 7),
                new BoardCoordinates(7, 8),
                new BoardCoordinates(7, 9));
            result[5].Should().Equal(
                new BoardCoordinates(6, 1),
                new BoardCoordinates(7, 1),
                new BoardCoordinates(8, 1));
        }
    }
}