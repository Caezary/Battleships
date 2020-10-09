using System;
using Battleships.Logic.Contracts;

namespace Battleships.Logic
{
    public class RandomGenerator : IGenerateRandomness
    {
        private readonly Random _random = new Random();
        
        public uint GetInRange(uint minInclusive, uint maxExclusive)
        {
            return (uint) _random.Next((int) minInclusive, (int) maxExclusive);
        }

        public bool GetBool()
        {
            return _random.Next() % 2 == 0;
        }
    }
}