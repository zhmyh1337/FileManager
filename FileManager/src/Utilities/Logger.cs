using FileManager.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager
{
    static class Logger
    {
        public static void PrintSuccess(string message, params object[] args)
        {
            Print("[Success] " + message, args);
        }

        public static void PrintSuccess()
        {
            Print(Localization.successfulOperation);
        }

        public static void PrintError(string message, params object[] args)
        {
            Print("[Error] " + message, args);
        }

        public static void Print(string message, params object[] args)
        {
            Writer(string.Format(message, args));
        }

        public static Action<string> Writer { get; set; } = Console.WriteLine;
    }
}
