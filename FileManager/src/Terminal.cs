using CommandLine;
using CommandLine.Text;
using FileManager.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FileManager
{
    class Terminal
    {
        static void Main(string[] args)
        {
            bool launchingWithArgs = args.Length != 0;

            // Printing program name, version and copyright info.
            if (!launchingWithArgs)
            {
                Console.WriteLine(HeadingInfo.Default);
                Console.WriteLine(CopyrightInfo.Default);
                Console.WriteLine();
            }

            var parser = new Parser();

            WorkingCycle(args, launchingWithArgs, parser);

//             foreach (ConsoleColor color in Enum.GetValues(typeof(ConsoleColor)))
//             {
//                 Console.ForegroundColor = color;
//                 Console.WriteLine($"Foreground color set to {color}");
//             }
        }

        private static void WorkingCycle(string[] args, bool launchingWithArgs, Parser parser)
        {
            bool firstCommand = true;

            while (true)
            {
                if (launchingWithArgs && firstCommand)
                {
                    lastParserResult = parser.ParseArguments(args, quiteableCommands);
                    lastParserResult
                        .WithParsed<Command.ICommand>(Execute)
                        .WithNotParsed(HandleErrorsArgsLaunching);
                }
                else
                {
                    Console.Write("{0}> ", WorkingDir);
                    string readParameters = Console.ReadLine();

                    lastParserResult = parser.ParseArguments(ArgsParser.SplitCommandLine(readParameters), notQuiteableCommands);
                    lastParserResult
                        .WithParsed<Command.ICommand>(Execute)
                        .WithNotParsed(HandleErrors);
                }
                firstCommand = false;
            }
        }

        private static void Execute(Command.ICommand cmd)
        {
            cmd.Execute();

            // Adding empty line separator.
            Logger.Print("");

            // Exiting as -q (--quite) option is set.
            if (((Command.IQuite)cmd).Quite)
                Environment.Exit(0);
        }

        /// <summary>
        /// This method generates help text when a verb was specified,
        /// but an error occured (e. g. when parsing options).
        /// It shows our options + "--help" option.
        /// </summary>
        private static HelpText GenerateVerbHelpText() => HelpText.AutoBuild(
            lastParserResult,
            e =>
            {
                e.AutoHelp = true;
                e.AutoVersion = false;
                e.Heading = string.Empty;
                e.Copyright = string.Empty;
                return HelpText.DefaultParsingErrorsHandler(lastParserResult, e);
            },
            ex => ex
        );

        /// <summary>
        /// This method generates help text when "help" or "--help" command was specified.
        /// It shows our commands + "version" + "help".
        /// </summary>
        private static HelpText GenerateHelpHelpText() => HelpText.AutoBuild(
            lastParserResult,
            e =>
            {
                e.AutoHelp = true;
                e.AutoVersion = true;
                e.Heading = string.Empty;
                e.Copyright = string.Empty;
                return HelpText.DefaultParsingErrorsHandler(lastParserResult, e);
            }
        );

        /// <summary>
        /// This parser is when launching w/o args or it's not the first command.
        /// </summary>
        /// <param name="errors"></param>
        private static void HandleErrors(IEnumerable<Error> errors)
        {
            // Checking whether a verb was specified.
            // We need this because "version" and "cmd --version"
            // throw the same error and we want to differentiate it.
            bool verbSpecified = lastParserResult.TypeInfo.Current != typeof(NullInstance);

            switch (errors?.FirstOrDefault())
            {
                case VersionRequestedError _:
                    if (verbSpecified)
                    {
                        Logger.PrintError(Localization.eVersionAsOption);
                    }
                    else
                    {
                        Console.WriteLine(HeadingInfo.Default);
                    }
                    return;
                // That's ok. Just continuing.
                case NoVerbSelectedError _:
                    return;
                case BadVerbSelectedError e:
                    Logger.PrintError(Localization.eBadCommand, e.Token);
                    return;
                case HelpVerbRequestedError _:
                    Console.WriteLine(GenerateHelpHelpText());
                    return;
                case Error e:
                    if (verbSpecified)
                    {
                        Console.WriteLine(GenerateVerbHelpText());
                    }
                    else
                    {
                        Logger.PrintError(e.Tag.ToString());
                    }
                    return;
            }
        }

        /// <summary>
        /// This parser is when launching w/ args and it's the first command.
        /// </summary>
        /// <param name="errors"></param>
        private static void HandleErrorsArgsLaunching(IEnumerable<Error> errors)
        {
            // Checking whether a verb was specified.
            // We need this because "version" and "cmd --version"
            // throw the same error and we want to differentiate it.
            bool verbSpecified = lastParserResult.TypeInfo.Current != typeof(NullInstance);

            switch (errors?.FirstOrDefault())
            {
                case VersionRequestedError _:
                    if (!verbSpecified)
                    {
                        Console.WriteLine(HeadingInfo.Default);
                        return;
                    }
                    break;
                // That's ok. Just continuing.
                case NoVerbSelectedError _:
                    return;
                case BadVerbSelectedError _:
                    break;
                case HelpVerbRequestedError _:
                    Console.WriteLine(GenerateHelpHelpText());
                    return;
            }

            Environment.Exit(1);
        }

        /// <summary>
        /// Launch w/ args and it's the first command.
        /// </summary>
        private static readonly Type[] quiteableCommands = new Type[] {
            typeof(Command.QDisk),
            typeof(Command.QChangeDir),
            typeof(Command.QDir),
            typeof(Command.QPrint),
            typeof(Command.QCopy),
            typeof(Command.QMove),
        };

        /// <summary>
        /// Launch w/o args or it's not the first command.
        /// </summary>
        private static readonly Type[] notQuiteableCommands = new Type[] {
            typeof(Command.NDisk),
            typeof(Command.NChangeDir),
            typeof(Command.NDir),
            typeof(Command.NPrint),
            typeof(Command.NCopy),
            typeof(Command.NMove),
        };

        /// <summary>
        /// <see cref="ParserResult{}"/> of last parsing to work with in handlers.
        /// </summary>
        private static ParserResult<object> lastParserResult;

        /// <summary>
        /// Working directory.
        /// </summary>
        public static DirectoryInfo WorkingDir
        {
            get { return workingDir; }
            set { Directory.SetCurrentDirectory((workingDir = value).FullName); }
        }
        private static DirectoryInfo workingDir = new DirectoryInfo(Directory.GetCurrentDirectory());
    }
}
