using System.Collections.Generic;
using System.Linq;
using Battleships.Logic.Contracts;
using ConsoleGUI.Controls;

namespace Battleships.ConsoleApp
{
    public class BoardViewState : IUpdateBoardView
    {
        private const string MissText = " .";
        private const string HitText = "><";
        private const string SinkText = "##";
        private TextBlock[][] Board { get; }

        public TextBlock this[BoardCoordinates coords] => Board[coords.Column][coords.Row];

        public BoardViewState(BoardCoordinates dimensions)
        {
            Board = new TextBlock[dimensions.Column][];
            GenerateBoard(dimensions);
        }

        public void Missed(BoardCoordinates coords)
        {
            this[coords].Text = MissText;
        }

        public void GotHit(BoardCoordinates coords)
        {
            this[coords].Text = HitText;
        }

        public void Sunken(IEnumerable<BoardCoordinates> sunkenShipCoords)
        {
            sunkenShipCoords.ToList()
                .ForEach(coords => this[coords].Text = SinkText);
        }

        public void ResetGame()
        {
            foreach (var textBlockRow in Board)
            {
                foreach (var textBlock in textBlockRow)
                {
                    textBlock.Text = "";
                }
            }
        }

        private void GenerateBoard(BoardCoordinates dimensions)
        {
            Enumerable.Range(0, (int) dimensions.Column)
                .ToList()
                .ForEach(c => Board[c] = GenerateRow(dimensions));
        }

        private static TextBlock[] GenerateRow(BoardCoordinates dimensions)
        {
            return Enumerable.Range(0, (int) dimensions.Row)
                .Select(r => new TextBlock())
                .ToArray();
        }
    }
}