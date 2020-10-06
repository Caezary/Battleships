using Battleships.Logic.Contracts.ShotOutcomes;
using OneOf;

namespace Battleships.Logic.Contracts
{
    public interface IManageBoard
    {
        bool IsLegalShot(BoardCoordinates coordinates);
        void MarkShot(BoardCoordinates coordinates, OneOf<Miss, Hit, Sink> shotOutcome);
    }
}