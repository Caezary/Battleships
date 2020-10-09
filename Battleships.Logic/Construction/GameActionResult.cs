namespace Battleships.Logic.Construction
{
    public class GameActionResult
    {
        public GameActionOutcome Outcome { get; set; } = GameActionOutcome.Error;
        public string Description { get; set; } = "";
    }
}