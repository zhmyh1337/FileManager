using FileManager.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager
{
    static class Debugger
    {
        public static void PrintLine(string message, params object[] args)
        {
            var wasClr = Console.ForegroundColor;
            Console.ForegroundColor = debuggerColor;
            Console.WriteLine(string.Format($"[{Localization.debuggerDebug}] {message}", args));
            Console.ForegroundColor = wasClr;
        }

        private const ConsoleColor debuggerColor = ConsoleColor.Red;
    }
}
