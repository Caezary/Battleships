using System;
using Battleships.Logic.Contracts;
using Battleships.Logic.Exceptions;

namespace Battleships.Logic.Construction
{
    public class GameFlowFacade
    {
        private readonly IUpdateBoardView _boardViewUpdater;
        private readonly Func<IHandlePlayerInteraction> _gameStateFactoryMethod;
        private IHandlePlayerInteraction _interactionHandler;
        private GameActionOutcome _currentOutcome = GameActionOutcome.Error;

        public GameFlowFacade(
            IUpdateBoardView boardViewUpdater, Func<IHandlePlayerInteraction> gameStateFactoryMethod)
        {
            _boardViewUpdater = boardViewUpdater;
            _gameStateFactoryMethod = gameStateFactoryMethod;
            _interactionHandler = _gameStateFactoryMethod();
        }

        public void GenerateNewGame()
        {
            _interactionHandler = _gameStateFactoryMethod();
            _boardViewUpdater.ResetGame();
            _currentOutcome = GameActionOutcome.Error;
        }

        public GameActionResult MakeShot(string coordinates)
        {
            if (_currentOutcome == GameActionOutcome.Win)
            {
                return ErrorResult("Game already won");
            }
            
            var result = GetShotResult(coordinates);
            _currentOutcome = result.Outcome;
            
            return result;
        }

        private GameActionResult GetShotResult(string coordinates)
        {
            try
            {
                var result = _interactionHandler.MakeShot(coordinates);
                return GameResult(result);
            }
            catch (MalformedCoordinateError)
            {
                return ErrorResult("The coordinates are malformed");
            }
            catch (CoordinatesOutOfBoundsError)
            {
                return ErrorResult("The coordinates are outside the board");
            }
        }

        private static GameActionResult GameResult(ShotResult result) =>
            new GameActionResult
            {
                Outcome = result switch
                {
                    ShotResult.Miss => GameActionOutcome.Miss,
                    ShotResult.Hit => GameActionOutcome.Hit,
                    ShotResult.Sink => GameActionOutcome.Sink,
                    ShotResult.Win => GameActionOutcome.Win,
                    _ => GameActionOutcome.Error
                }
            };

        private static GameActionResult ErrorResult(string description) =>
            new GameActionResult
            {
                Outcome = GameActionOutcome.Error,
                Description = description
            };
    }
}