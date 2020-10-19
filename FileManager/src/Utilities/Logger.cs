using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager
{
    static class Logger
    {
        public static void PrintSuccess(string message, params object[] args)
        {
            Console.WriteLine("[Success] {0}", string.Format(message, args));
        }

        public static void PrintError(string message, params object[] args)
        {
            Console.WriteLine("[Error] {0}", string.Format(message, args));
        }
    }
}
