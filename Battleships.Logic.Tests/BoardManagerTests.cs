using Battleships.Logic.Contracts;
using Battleships.Logic.Contracts.ShotOutcomes;
using FluentAssertions;
using Moq;
using Xunit;

namespace Battleships.Logic.Tests
{
    public class BoardManagerTests
    {
        private readonly Mock<IUpdateBoardView> _boardViewUpdaterMock = new Mock<IUpdateBoardView>();
        
        [Theory]
        [InlineData(3, 2)]
        [InlineData(1, 1)]
        [InlineData(0, 0)]
        public void IsLegalShot_CoordsWithinBoardBounds_ReturnsTrue(uint column, uint row)
        {
            var coords = new BoardCoordinates(column, row);
            var sut = CreateSut(4, 4);

            var result = sut.IsLegalShot(coords);

            result.Should().BeTrue();
        }
        
        [Theory]
        [InlineData(3, 8)]
        [InlineData(6, 2)]
        [InlineData(9, 7)]
        public void IsLegalShot_CoordsOutsideBoardBounds_ReturnsFalse(uint column, uint row)
        {
            var coords = new BoardCoordinates(column, row);
            var sut = CreateSut(4, 4);

            var result = sut.IsLegalShot(coords);

            result.Should().BeFalse();
        }
        
        [Theory]
        [InlineData(3, 4)]
        [InlineData(4, 2)]
        [InlineData(4, 4)]
        public void IsLegalShot_AtLeastOneCoordinateEqualToBoardBounds_ReturnsFalse(uint column, uint row)
        {
            var coords = new BoardCoordinates(column, row);
            var sut = CreateSut(4, 4);

            var result = sut.IsLegalShot(coords);

            result.Should().BeFalse();
        }

        [Fact]
        public void MarkShot_MissGiven_UpdatesViewWithGivenCoords()
        {
            var coords = new BoardCoordinates(5, 7);
            var sut = CreateSut();
            
            sut.MarkShot(coords, new Miss());
            
            _boardViewUpdaterMock.Verify(u  => u.Missed(coords));
        }

        [Fact]
        public void MarkShot_HitGiven_UpdatesViewWithGivenCoords()
        {
            var coords = new BoardCoordinates(4, 3);
            var sut = CreateSut();
            
            sut.MarkShot(coords, new Hit());
            
            _boardViewUpdaterMock.Verify(u  => u.GotHit(coords));
        }

        [Fact]
        public void MarkShot_SinkGiven_UpdatesViewWithSunkenCoords()
        {
            var sink = new Sink(new[]
            {
                new BoardCoordinates(1, 3),
                new BoardCoordinates(1, 4),
                new BoardCoordinates(1, 5)
            });
            var coords = new BoardCoordinates(1, 4);
            var sut = CreateSut();
            
            sut.MarkShot(coords, sink);
            
            _boardViewUpdaterMock.Verify(u  => u.Sunken(sink.SunkenShipCoords));
        }

        private BoardManager CreateSut(uint maxColumn = 10, uint maxRow = 10)
        {
            var boundingCoords = new BoardCoordinates(maxColumn, maxRow);
            return new BoardManager(boundingCoords, _boardViewUpdaterMock.Object);
        }
    }
}