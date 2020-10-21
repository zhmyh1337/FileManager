using FileManager.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager
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
            PrintLine(string.Format($"[{Localization.debuggerDebug}] {message}", args));
        }

        private const ConsoleColor debuggerColor = ConsoleColor.Red;
    }
}
