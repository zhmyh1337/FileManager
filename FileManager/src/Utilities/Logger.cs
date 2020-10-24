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
            PrintLine($"{Localization.loggerSuccessPrefix} {message}", args);
        }

        public static void PrintError(string message, params object[] args)
        {
            PrintLine($"{Localization.loggerErrorPrefix} {message}", args);
        }

        public static void PrintLine(string message)
        {
            Writer(message);
        }

        public static void PrintLine(string message, params object[] args)
        {
            PrintLine(string.Format(message, args));
        }

        public static void PrintLine()
        {
            PrintLine("");
        }

        public static Action<string> Writer { get; set; } = Console.WriteLine;
    }
}
