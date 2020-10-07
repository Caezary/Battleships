using System;

namespace Battleships.Logic.Contracts
{
    public class BoardCoordinates : IEquatable<BoardCoordinates>
    {
        public int Column { get; }
        public int Row { get; }

        public BoardCoordinates(int column, int row)
        {
            Column = column;
            Row = row;
        }
        
        public bool Equals(BoardCoordinates? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Column == other.Column && Row == other.Row;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BoardCoordinates) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Column * 397) ^ Row;
            }
        }

        public static bool operator ==(BoardCoordinates? left, BoardCoordinates? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BoardCoordinates? left, BoardCoordinates? right)
        {
            return !Equals(left, right);
        }
    }
}