using FileManager.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager
{
    static class Debugger
    {
        public static void Print(string message, params object[] args)
        {
            var wasClr = Console.ForegroundColor;
            Console.ForegroundColor = debuggerColor;
            Console.WriteLine(string.Format("[DEBUG] " + message, args));
            Console.ForegroundColor = wasClr;
        }

        static Debugger()
        {
            Print(Localization.debuggerWarning);
        }

        private const ConsoleColor debuggerColor = ConsoleColor.Red;
    }
}
