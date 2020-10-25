using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager
{
    class Randomizer
    {
        public static Random Get() => rng;

        private static readonly Random rng = new Random();
    }
}
