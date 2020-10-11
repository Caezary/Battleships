using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Battleships.Logic.Construction;
using Battleships.Logic.Contracts;
using ConsoleGUI;
using ConsoleGUI.Api;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.Input;
using ConsoleGUI.Space;

namespace Battleships.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {   
            var boardSizeBounds = Defaults.BoardSizeBounds;
            var boardViewState = new BoardViewState(boardSizeBounds);

            var gameFlow = Generate.GameFlow()
                .WithBoardSize(boardSizeBounds)
                .WithBoardViewUpdater(boardViewState)
                .Build();
            
            var canvas = GenerateView(boardSizeBounds, boardViewState);
            var (textBox, userActionMessage) = GenerateInteractiveElements(canvas);
            
            SetupConsole(canvas);

            var isRunning = true;

            var inputReactionCallbacks = new InputReactionCallbacks(
                () => isRunning = false,
                result => HandleGameActionResult(userActionMessage, result),
                () => HandleReset(userActionMessage));
            var input = GenerateInputListeners(textBox, gameFlow, inputReactionCallbacks);
            
            while (isRunning)
            {
                Thread.Sleep(10);
                
                ConsoleManager.ReadInput(input);
            }
        }

        private static void HandleReset(TextBlock userActionMessage)
        {
            userActionMessage.Text = "Game reset";
        }

        private static void HandleGameActionResult(TextBlock userActionMessage, GameActionResult result)
        {
            if (!string.IsNullOrEmpty(result.Description))
            {
                userActionMessage.Text = result.Description;
                return;
            }

            userActionMessage.Text = result.Outcome switch
            {
                GameActionOutcome.Error => "Unspecified error occured",
                GameActionOutcome.Miss => "Shot missed",
                GameActionOutcome.Hit => "It's a hit!",
                GameActionOutcome.Sink => "Sunken ship!",
                GameActionOutcome.Win => "Game Won!",
                _ => ""
            };
        }

        private static void SetupConsole(Canvas canvas)
        {
            ConsoleManager.Console = new SimplifiedConsole();
            ConsoleManager.Setup();
            ConsoleManager.Content = canvas;
        }

        private static IInputListener[] GenerateInputListeners(
            TextBox textBox, GameFlowFacade gameFlow, InputReactionCallbacks inputReactionCallbacks)
        {
            return new IInputListener[]
            {
                new InputController(textBox, gameFlow, inputReactionCallbacks),
                textBox
            };
        }

        private static Canvas GenerateView(
            BoardCoordinates boardSizeBounds, BoardViewState boardViewState)
        {
            var canvas = new Canvas();

            var title = GenerateTitle();
            canvas.Add(title, new Rect(8, 1, 12, 3));

            var board = GenerateBoard(boardSizeBounds, boardViewState);
            canvas.Add(board, new Rect(3, 3, 24, 12));

            var helpText = GenerateHelpText();
            canvas.Add(helpText, new Rect(30, 2, 42, 7));

            return canvas;
        }

        private static (TextBox textBox, TextBlock userActionMessage) GenerateInteractiveElements(Canvas canvas)
        {
            var textBox = new TextBox();
            canvas.Add(
                new Background
                {
                    Color = SimplifiedColors.DarkGray,
                    Content = textBox
                },
                new Rect(30, 10, 6, 1));

            var userActionMessage = new TextBlock();
            canvas.Add(userActionMessage, new Rect(30, 12, 40, 1));
            return (textBox, userActionMessage);
        }

        private static BreakPanel GenerateHelpText()
        {
            return new BreakPanel
            {
                Content = new TextBlock
                {
                    Text = @"
Sink all ships to win.
Type coordinates to make a shot.
Try typing:
• coordinates (e.g. 'A1') to make a shot
• 'retry' to reset game
• 'exit' to exit game"
                }
            };
        }

        private static TextBlock GenerateTitle()
        {
            return new TextBlock
            {
                Color = Color.White,
                Text = "BATTLESHIPS"
            };
        }

        private static Grid GenerateBoard(BoardCoordinates boardSizeBounds, BoardViewState boardViewState)
        {
            var board = CreateBoard(boardSizeBounds);

            var columns = boardSizeBounds.GetColumnRange().ToList();
            var rows = boardSizeBounds.GetRowRange().ToList();

            CreateColumnHeaders(columns, board);
            CreateRowHeaders(rows, board);
            CreateBoardContent(columns, rows, board, boardViewState);
            return board;
        }

        private static void CreateBoardContent(List<int> columns, List<int> rows, Grid board, BoardViewState boardViewState)
        {
            columns.SelectMany(_ => rows, (c, r) => new BoardCoordinates((uint) c, (uint) r))
                .ToList()
                .ForEach(coords => board.AddChild(
                    (int) coords.Column + 1,
                    (int) coords.Row + 1,
                    new Background
                    {
                        Color = GenerateBoardFieldColor(coords),
                        Content = boardViewState[coords]
                    }));
        }

        private static void CreateRowHeaders(List<int> rows, Grid board)
        {
            rows.Select(r => new
                {
                    Control = new Background
                    {
                        Color = GenerateHeaderColor(r),
                        Content = new TextBlock
                        {
                            Color = SimplifiedColors.Magenta,
                            Text = GetRowLabel(r)
                        }
                    },
                    RowNo = r
                })
                .ToList()
                .ForEach(label => board.AddChild(0, label.RowNo + 1, label.Control));
        }

        private static void CreateColumnHeaders(List<int> columns, Grid board)
        {
            columns.Select(c => new
                {
                    Control = new Background
                    {
                        Color = GenerateHeaderColor(c),
                        Content = new TextBlock
                        {
                            Color = SimplifiedColors.Magenta,
                            Text = GetColumnLabel(c)
                        }
                    },
                    ColumnNo = c
                })
                .ToList()
                .ForEach(label => board.AddChild(label.ColumnNo + 1, 0, label.Control));
        }

        private static Grid CreateBoard(BoardCoordinates boardSizeBounds)
        {
            return new Grid
            {
                Columns = Enumerable.Range(0, (int) boardSizeBounds.Column + 1)
                    .Select(_ => new Grid.ColumnDefinition(2))
                    .ToArray(),
                Rows = Enumerable.Range(0, (int) boardSizeBounds.Row + 1)
                    .Select(_ => new Grid.RowDefinition(1))
                    .ToArray(),
            };
        }

        private static Color GenerateHeaderColor(int index)
        {
            return index % 2 == 0 ? SimplifiedColors.DarkGray : SimplifiedColors.Gray;
        }

        private static Color GenerateBoardFieldColor(BoardCoordinates coords)
        {
            var columnIsOdd = coords.Column % 2 == 0;
            var rowIsOdd = coords.Row % 2 == 0;
            return columnIsOdd == rowIsOdd ? SimplifiedColors.Cyan : SimplifiedColors.DarkCyan;
        }

        private static string GetRowLabel(int rowNo)
        {
            var number = rowNo + 1;
            return number < 10 ? $" {number}" : $"{number}";
        }

        private static string GetColumnLabel(int columnNo)
        {
            var label = (char) ('A' + columnNo);
            return $" {label}";
        }
    }
}