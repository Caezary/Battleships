using Battleships.Logic.Contracts.ShotOutcomes;
using OneOf;

namespace Battleships.Logic.Contracts
{
    public interface ISuperviseFleet
    {
        int FloatingShipsLeft { get; }
        OneOf<Miss, Hit, Sink> CheckShot(BoardCoordinates coordinates);
    }
}