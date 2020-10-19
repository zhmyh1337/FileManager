using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace FileManager
{
    class Terminal
    {
        static void Main(string[] args)
        {
            bool launchingWithArgs = args.Length != 0;

            // This parser instance shows auto generated error and version message on error.
            // Btw I can disable autoversion, though I don't want to.
            var defaultParser = Parser.Default;
            // This instance doesn't show this stuff for some reason.
            var noThrowParser = new Parser();

            bool firstCommand = true;

            while (true)
            {
                if (launchingWithArgs && firstCommand)
                {
                    noThrowParser.ParseArguments(args, quiteableCommands)
                        .WithParsed(x => Execute((Command.ICommand)x))
                        .WithNotParsed(x => Environment.Exit(1));
                }
                else
                {
                    Console.Write("{0}> ", WorkingDir);
                    string readParameters = Console.ReadLine();

                    // Skip if no not white characters in string.
                    if (!string.IsNullOrWhiteSpace(readParameters))
                    {
                        defaultParser.ParseArguments(ArgsParser.SplitCommandLine(readParameters), notQuiteableCommands)
                            .WithParsed(x => Execute((Command.ICommand)x))
                            .WithNotParsed(x => HandleErrors(x));
                    }
                }
                firstCommand = false;
            }
            
//             foreach (ConsoleColor color in Enum.GetValues(typeof(ConsoleColor)))
//             {
//                 Console.ForegroundColor = color;
//                 Console.WriteLine($"Foreground color set to {color}");
//             }
        }

        private static void Execute(Command.ICommand cmd)
        {
            cmd.Execute(() => {
                // Exiting as -q (--quite) option is set.
                if (((Command.IQuite)cmd).Quite)
                    Environment.Exit(1);
            });

            // Exiting as -q (--quite) option is set.
            if (((Command.IQuite)cmd).Quite)
                Environment.Exit(0);
        }

        private static void HandleErrors(IEnumerable<Error> errors)
        {

        }

        /// <summary>
        /// Launch w/ args and it's the first command.
        /// </summary>
        private static readonly Type[] quiteableCommands = new Type[] {
            typeof(Command.QDisk),
        };

        /// <summary>
        /// Launch w/o args or it's not the first command.
        /// </summary>
        private static readonly Type[] notQuiteableCommands = new Type[] {
            typeof(Command.NDisk),
        };

        /// <summary>
        /// Working directory.
        /// </summary>
        public static DirectoryInfo WorkingDir { get; set; } = new DirectoryInfo(Directory.GetCurrentDirectory());
    }
}
