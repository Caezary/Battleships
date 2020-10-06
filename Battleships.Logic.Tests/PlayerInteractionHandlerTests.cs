using System;
using AutoFixture;
using Battleships.Logic.Contracts;
using Battleships.Logic.Contracts.ShotOutcomes;
using Battleships.Logic.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Battleships.Logic.Tests
{
    public class PlayerInteractionHandlerTests
    {
        private readonly Fixture _fixture = new Fixture(); 
        private readonly Mock<IParseCoords> _coordsParserMock = new Mock<IParseCoords>();
        private readonly Mock<ISuperviseFleet> _fleetSupervisorMock = new Mock<ISuperviseFleet>();
        private readonly Mock<IManageBoard> _boardManagerMock = new Mock<IManageBoard>();
        private readonly BoardCoordinates _boardCoordinates;
        private readonly PlayerInteractionHandler _sut;

        public PlayerInteractionHandlerTests()
        {
            _sut = new PlayerInteractionHandler(
                _coordsParserMock.Object, _fleetSupervisorMock.Object, _boardManagerMock.Object);
            
            _boardCoordinates = _fixture.Create<BoardCoordinates>();
            _coordsParserMock.Setup(p => p.Parse(It.IsAny<string>()))
                .Returns(_boardCoordinates);
            _fleetSupervisorMock.SetupGet(f => f.FloatingShipsLeft).Returns(3);
            _boardManagerMock.Setup(m => m.IsLegalShot(It.IsAny<BoardCoordinates>()))
                .Returns(true);
        }

        [Fact]
        public void MakeShot_CoordsOutOfBounds_Throws()
        {
            var coords = "K11";
            _boardManagerMock.Setup(m => m.IsLegalShot(It.IsAny<BoardCoordinates>()))
                .Returns(false);

            Action act = () => _sut.MakeShot(coords);

            act.Should().Throw<CoordinatesOutOfBoundsError>();
        }
        
        [Fact]
        public void MakeShot_CoordsPointToEmptySpace_MissReturnedAndViewUpdated()
        {
            var coords = "A1";
            var miss = _fixture.Create<Miss>();
            _fleetSupervisorMock.Setup(f => f.CheckShot(It.IsAny<BoardCoordinates>()))
                .Returns(miss);

            var result = _sut.MakeShot(coords);

            result.Should().Be(ShotResult.Miss);
            _boardManagerMock.Verify(b => b.MarkShot(_boardCoordinates, miss));
        }
        
        [Fact]
        public void MakeShot_CoordsPointToUntouchedShip_HitReturnedAndViewUpdated()
        {
            var coords = "A1";
            var hit = _fixture.Create<Hit>();
            _fleetSupervisorMock.Setup(f => f.CheckShot(It.IsAny<BoardCoordinates>()))
                .Returns(hit);

            var result = _sut.MakeShot(coords);

            result.Should().Be(ShotResult.Hit);
            _boardManagerMock.Verify(b => b.MarkShot(_boardCoordinates, hit));
        }
        
        [Fact]
        public void MakeShot_CoordsPointToAlmostSunkenShip_SinkReturnedAndViewUpdated()
        {
            var coords = "A1";
            var sink = _fixture.Create<Sink>();
            _fleetSupervisorMock.Setup(f => f.CheckShot(It.IsAny<BoardCoordinates>()))
                .Returns(sink);

            var result = _sut.MakeShot(coords);

            result.Should().Be(ShotResult.Sink);
            _boardManagerMock.Verify(b => b.MarkShot(_boardCoordinates, sink));
        }
        
        [Fact]
        public void MakeShot_CoordsPointToLastAlmostSunkenShip_WinReturnedAndViewUpdated()
        {
            var coords = "A1";
            var sink = _fixture.Create<Sink>();
            _fleetSupervisorMock.Setup(f => f.CheckShot(It.IsAny<BoardCoordinates>()))
                .Returns(sink);
            _fleetSupervisorMock.SetupGet(f => f.FloatingShipsLeft).Returns(0);

            var result = _sut.MakeShot(coords);

            result.Should().Be(ShotResult.Win);
            _boardManagerMock.Verify(b => b.MarkShot(_boardCoordinates, sink));
        }
    }
}