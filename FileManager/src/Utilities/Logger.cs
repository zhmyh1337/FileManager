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
            PrintLine($"[{Localization.loggerSuccess}] {message}", args);
        }

        public static void PrintSuccess()
        {
            PrintLine($"{Localization.loggerSuccess}.");
        }

        public static void PrintError(string message, params object[] args)
        {
            PrintLine($"[{Localization.loggerError}] {message}", args);
        }

        public static void PrintError()
        {
            PrintLine($"{Localization.loggerError}.");
        }

        public static void PrintLine(string message, params object[] args)
        {
            Writer(string.Format(message, args));
        }

        public static void PrintLine()
        {
            PrintLine("");
        }

        public static Action<string> Writer { get; set; } = Console.WriteLine;
    }
}
