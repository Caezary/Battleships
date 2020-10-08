namespace Battleships.Logic.Contracts
{
    public interface IGenerateRandomness
    {
        uint GetInRange(uint minInclusive, uint maxExclusive);
        bool GetBool();
    }
}