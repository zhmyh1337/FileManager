using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;

namespace FileManager
{
    /// <summary>
    /// This static class parses string as args.
    /// See <see href="https://stackoverflow.com/questions/298830/split-string-containing-command-line-parameters-into-string-in-c-sharp">here</see>.
    /// If we weren't cross-platform, I'd use CommandLineToArgvW() API (see <see href="https://stackoverflow.com/a/749653">here</see>).
    /// </summary>
    static class ArgsParser
    {
        public static IEnumerable<string> SplitCommandLine(string commandLine)
        {
            bool inQuotes = false;
            bool isEscaping = false;

            return commandLine.Split(c => {
                if (c == '\\' && !isEscaping) { isEscaping = true; return false; }

                if (c == '\"' && !isEscaping)
                    inQuotes = !inQuotes;

                isEscaping = false;

                return !inQuotes && Char.IsWhiteSpace(c)/*c == ' '*/;
            })
            .Select(arg => arg.Trim().TrimMatchingQuotes('\"').Replace("\\\"", "\""))
            .Where(arg => !string.IsNullOrEmpty(arg));
        }

        private static IEnumerable<string> Split(this string str, Func<char, bool> controller)
        {
            int nextPiece = 0;

            for (int c = 0; c < str.Length; c++)
            {
                if (controller(str[c]))
                {
                    yield return str.Substring(nextPiece, c - nextPiece);
                    nextPiece = c + 1;
                }
            }

            yield return str.Substring(nextPiece);
        }

        private static string TrimMatchingQuotes(this string input, char quote)
        {
            if ((input.Length >= 2) && (input[0] == quote) && (input[input.Length - 1] == quote))
                return input.Substring(1, input.Length - 2);

            return input;
        }
    }
}
