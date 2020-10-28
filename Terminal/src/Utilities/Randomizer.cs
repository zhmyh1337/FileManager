using System;

namespace Utilities
{
    static class Randomizer
    {
        public static Random Get() => rng;

        private static readonly Random rng = new Random();
    }
}
