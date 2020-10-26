using FileManager.Properties;
using System;

namespace Utilities
{
    static class Debugger
    {
        public static void PrintLine(string message)
        {
            var wasClr = Console.ForegroundColor;
            Console.ForegroundColor = debuggerColor;
            Console.WriteLine(message);
            Console.ForegroundColor = wasClr;
        }

        public static void PrintLine(string message, params object[] args)
        {
            PrintLine(string.Format($"{Localization.debuggerPrefix} {message}", args));
        }

        private const ConsoleColor debuggerColor = ConsoleColor.Red;
    }
}
