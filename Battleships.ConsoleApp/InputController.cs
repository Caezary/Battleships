using System;
using Battleships.Logic.Construction;
using ConsoleGUI.Controls;
using ConsoleGUI.Input;

namespace Battleships.ConsoleApp
{
    public class InputController : IInputListener
    {
        private readonly TextBox _textBox;
        private readonly GameFlowFacade _gameFlow;
        private readonly InputReactionCallbacks _inputReactionCallbacks;

        public InputController(TextBox textBox, GameFlowFacade gameFlow, InputReactionCallbacks inputReactionCallbacks)
        {
            _textBox = textBox;
            _gameFlow = gameFlow;
            _inputReactionCallbacks = inputReactionCallbacks;
        }

        public void OnInput(InputEvent inputEvent)
        {
            if (inputEvent.Key.Key != ConsoleKey.Enter)
            {
                return;
            }

            switch (_textBox.Text.ToLower())
            {
                case "reset":
                case "retry":
                    _gameFlow.GenerateNewGame();
                    _inputReactionCallbacks.HandleReset();
                    break;
                case "exit":
                    _inputReactionCallbacks.Stop();
                    break;
                default:
                    HandleShotInput();
                    break;
            }

            _textBox.Text = string.Empty;
            inputEvent.Handled = true;
        }

        private void HandleShotInput()
        {
            var result = _gameFlow.MakeShot(_textBox.Text);
            _inputReactionCallbacks.HandleGameActionResult(result);
        }
    }
}