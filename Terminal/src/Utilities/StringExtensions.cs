using System;

namespace Utilities
{
    static class StringExtensions
    {
        /// <summary>
        /// This method cuts the string with ... if length <paramref name="maxLength"/> is exceeded.
        /// </summary>
        /// <returns>Cutted string.</returns>
        /// <example>
        /// This code:
        /// <code>
        /// "abacaba".CutWithDots(6)
        /// </code>
        /// Returns "aba..."
        /// </example>
        public static string CutWithDots(this string s, int maxLength)
        {
            if (s.Length <= maxLength)
                return s;

            return s.Remove(Math.Max(0, maxLength - 3)) + 
                new string('.', Math.Min(3, maxLength));
        }
    }
}
