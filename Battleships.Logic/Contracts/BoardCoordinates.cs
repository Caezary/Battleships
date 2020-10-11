using System;

namespace Battleships.Logic.Contracts
{
    public readonly struct BoardCoordinates : IEquatable<BoardCoordinates>
    {
        public uint Column { get; }
        public uint Row { get; }

        public BoardCoordinates(uint column, uint row)
        {
            Column = column;
            Row = row;
        }
        
        public bool Equals(BoardCoordinates other)
        {
            return Column == other.Column && Row == other.Row;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
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