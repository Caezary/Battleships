using System;
using Battleships.Logic.Contracts;
using Battleships.Logic.Contracts.ShotOutcomes;
using OneOf;

namespace Battleships.Logic
{
    public class BoardManager : IManageBoard
    {
        private readonly BoardCoordinates _boundingCoordinates;
        private readonly IUpdateBoardView _boardViewUpdater;

        public BoardManager(BoardCoordinates boundingCoordinates, IUpdateBoardView boardViewUpdater)
        {
            _boundingCoordinates = boundingCoordinates;
            _boardViewUpdater = boardViewUpdater;
        }

        public bool IsLegalShot(BoardCoordinates boardCoords)
        {
            return boardCoords.Column < _boundingCoordinates.Column &&
                   boardCoords.Row < _boundingCoordinates.Row;
        }

        public void MarkShot(BoardCoordinates coordinates, OneOf<Miss, Hit, Sink> shotOutcome)
        {
            shotOutcome.Switch(
                miss => _boardViewUpdater.Missed(coordinates),
                hit => _boardViewUpdater.GotHit(coordinates),
                sink => _boardViewUpdater.Sunken(sink.SunkenShipCoords));
        }
    }
}