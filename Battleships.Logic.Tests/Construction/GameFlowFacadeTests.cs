using Battleships.Logic.Construction;
using Battleships.Logic.Contracts;
using Battleships.Logic.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Battleships.Logic.Tests.Construction
{
    public class GameFlowFacadeTests
    {
        private readonly Mock<IHandlePlayerInteraction> _interactionHandlerMock = new Mock<IHandlePlayerInteraction>();
        private readonly GameFlowFacade _sut;

        public GameFlowFacadeTests()
        {
            _sut = new GameFlowFacade(() => _interactionHandlerMock.Object); 
        }

        [Fact]
        public void GenerateNewGame_HasFactoryMethod_FactoryMethodUsed()
        {
            var factoryMethodCalled = false;
            IHandlePlayerInteraction FactoryMethodMock()
            {
                factoryMethodCalled = true;
                return _interactionHandlerMock.Object;
            }
            var sut = new GameFlowFacade(FactoryMethodMock);
            
            sut.GenerateNewGame();

            factoryMethodCalled.Should().BeTrue();
        }

        [Fact]
        public void MakeShot_MalformedCoordinatesGiven_ReturnsErrorResult()
        {
            _interactionHandlerMock.Setup(handler => handler.MakeShot(It.IsAny<string>()))
                .Throws<MalformedCoordinateError>();

            var result = _sut.MakeShot("test");

            result.Outcome.Should().Be(GameActionOutcome.Error);
            result.Description.Should().Be("The coordinates are malformed");
        }

        [Fact]
        public void MakeShot_CoordinatesOutOfBoundsGiven_ReturnsErrorResult()
        {
            _interactionHandlerMock.Setup(handler => handler.MakeShot(It.IsAny<string>()))
                .Throws<CoordinatesOutOfBoundsError>();

            var result = _sut.MakeShot("Z999");

            result.Outcome.Should().Be(GameActionOutcome.Error);
            result.Description.Should().Be("The coordinates are outside the board");
        }

        [Fact]
        public void MakeShot_ShotMissed_ReturnsMiss()
        {
            _interactionHandlerMock.Setup(handler => handler.MakeShot(It.IsAny<string>()))
                .Returns(ShotResult.Miss);

            var result = _sut.MakeShot("A1");

            result.Outcome.Should().Be(GameActionOutcome.Miss);
            result.Description.Should().Be("");
        }

        [Fact]
        public void MakeShot_ShotHit_ReturnsHit()
        {
            _interactionHandlerMock.Setup(handler => handler.MakeShot(It.IsAny<string>()))
                .Returns(ShotResult.Hit);

            var result = _sut.MakeShot("A1");

            result.Outcome.Should().Be(GameActionOutcome.Hit);
            result.Description.Should().Be("");
        }

        [Fact]
        public void MakeShot_ShotHasSunkenShip_ReturnsSink()
        {
            _interactionHandlerMock.Setup(handler => handler.MakeShot(It.IsAny<string>()))
                .Returns(ShotResult.Sink);

            var result = _sut.MakeShot("A1");

            result.Outcome.Should().Be(GameActionOutcome.Sink);
            result.Description.Should().Be("");
        }

        [Fact]
        public void MakeShot_ShotWon_ReturnsWin()
        {
            _interactionHandlerMock.Setup(handler => handler.MakeShot(It.IsAny<string>()))
                .Returns(ShotResult.Win);

            var result = _sut.MakeShot("A1");

            result.Outcome.Should().Be(GameActionOutcome.Win);
            result.Description.Should().Be("");
        }

        [Fact]
        public void MakeShot_AfterWinningShotWasMade_ReturnsError()
        {
            _interactionHandlerMock.SetupSequence(handler => handler.MakeShot(It.IsAny<string>()))
                .Returns(ShotResult.Win)
                .Returns(ShotResult.Miss);

            _sut.MakeShot("A1");
            var result = _sut.MakeShot("A2");

            result.Outcome.Should().Be(GameActionOutcome.Error);
            result.Description.Should().Be("Game already won");
        }
    }
}