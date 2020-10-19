using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager
{
    static class Logger
    {
        public static void PrintSuccess(string message, params object[] args)
        {
            Writer(string.Format("[Success] " + message, args));
        }

        public static void PrintError(string message, params object[] args)
        {
            Writer(string.Format("[Error] " + message, args));
        }

        public static Action<string> Writer { get; set; } = Console.WriteLine;
    }
}
