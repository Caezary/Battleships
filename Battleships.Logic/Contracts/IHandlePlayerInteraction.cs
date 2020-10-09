namespace Battleships.Logic.Contracts
{
    public interface IHandlePlayerInteraction
    {
        ShotResult MakeShot(string coordinates);
    }
}