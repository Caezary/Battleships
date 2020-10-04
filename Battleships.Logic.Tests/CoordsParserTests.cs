using System;
using Battleships.Logic.Exceptions;
using FluentAssertions;
using Xunit;

namespace Battleships.Logic.Tests
{
    public class CoordsParserTests
    {
        private readonly CoordsParser _sut = new CoordsParser();

        [Fact]
        public void Parse_EmptyString_ThrowsError()
        {
            var given = "";

            Action act = () => _sut.Parse(given);

            act.Should().Throw<MalformedCoordinateError>();
        }
        
        [Fact]
        public void Parse_NoRowCoord_ThrowsError()
        {
            var given = "A";

            Action act = () => _sut.Parse(given);

            act.Should().Throw<MalformedCoordinateError>();
        }
        
        [Fact]
        public void Parse_NoColumnCoord_ThrowsError()
        {
            var given = "10";

            Action act = () => _sut.Parse(given);

            act.Should().Throw<MalformedCoordinateError>();
        }
        
        [Theory]
        [InlineData("A2", 0, 1)]
        [InlineData("b9", 1, 8)]
        [InlineData("C7", 2, 6)]
        [InlineData("H10", 7, 9)]
        [InlineData("Z100", 25, 99)]
        public void Parse_CorrectCoords_ReturnsArrayCoords(string coords, int expectedColumn, int expectedRow)
        {
            var (column, row) = _sut.Parse(coords);

            column.Should().Be(expectedColumn);
            row.Should().Be(expectedRow);
        }
    }
}
