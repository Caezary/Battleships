using System;
using Battleships.Logic.Construction;

namespace Battleships.ConsoleApp
{
    public class InputReactionCallbacks
    {
        public InputReactionCallbacks(Action stop, Action<GameActionResult> handleGameActionResult, Action handleReset)
        {
            Stop = stop;
            HandleGameActionResult = handleGameActionResult;
            HandleReset = handleReset;
        }

        public Action Stop { get; }
        public Action<GameActionResult> HandleGameActionResult { get; }
        public Action HandleReset { get; }
    }
}