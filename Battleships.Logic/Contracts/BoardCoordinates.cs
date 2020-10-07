using System;

namespace Battleships.Logic.Contracts
{
    public class BoardCoordinates : IEquatable<BoardCoordinates>
    {
        public uint Column { get; }
        public uint Row { get; }

        public BoardCoordinates(uint column, uint row)
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
                return ((int) Column * 397) ^ (int) Row;
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