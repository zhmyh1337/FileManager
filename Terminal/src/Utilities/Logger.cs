using Terminal.Properties;
using System;

namespace Utilities
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
            WriteLiner(message);
        }

        public static void PrintLine(string message, params object[] args)
        {
            PrintLine(string.Format(message, args));
        }

        public static void PrintLine()
        {
            PrintLine("");
        }

        public static void Print(string message)
        {
            Writer(message);
        }

        public static void Print(string message, params object[] args)
        {
            Print(string.Format(message, args));
        }

        public static Action<string> Writer { get; set; } = Console.Write;
        public static Action<string> WriteLiner { get; set; } = Console.WriteLine;
    }
}
