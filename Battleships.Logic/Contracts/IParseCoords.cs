namespace Battleships.Logic.Contracts
{
    public interface IParseCoords
    {
        BoardCoordinates Parse(string coordinates);
    }
}