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

            // This parser instance shows auto generated error
            // and version messages when parsing error occurs.
            // Btw I can disable autoversion, though I don't want to.
            var defaultParser = Parser.Default;
            // This instance doesn't show this stuff for some reason.
            // To show help/version when --help/--version is specified,
            // we will generate HelpText manually but only when the error
            // is caused by --help/--version options.
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
            //             CommandLine.Text.HelpText.AutoBuild
            cmd.Execute(() =>
            {
                // Exiting as -q (--quite) option is set.
                if (((Command.IQuite)cmd).Quite)
                    Environment.Exit(1);
            });

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

        private static void HandleErrors(IEnumerable<Error> errors)
        {
            // Checking whether a verb was specified.
            // We need this because "version" and "cmd --version"
            // throw the same error and we want to differentiate it.
            bool verbSpecified = lastParserResult.TypeInfo.Current != typeof(NullInstance);
            
            switch (errors?.FirstOrDefault())
            {
                case VersionRequestedError e:
                    if (verbSpecified)
                    {
                        Console.WriteLine(Localization.eVersionAsOption);
                    }
                    else
                    {
                        Console.WriteLine(HeadingInfo.Default);
                    }
                    break;
                // That's ok. Just continuing.
                case NoVerbSelectedError e:
                    break;
                case BadVerbSelectedError e:
                    Console.WriteLine(string.Format(Localization.eBadCommand, e.Token));
                    break;
                default:
                    if (verbSpecified)
                    {
                        Console.WriteLine(GenerateVerbHelpText());
                    }
                    else
                    {
                        Console.WriteLine(GenerateHelpHelpText());
                    }
                    break;
            }
        }

        private static void HandleErrorsArgsLaunching(IEnumerable<Error> errors)
        {
            HandleErrors(errors);
            return;
            Console.WriteLine($"{lastParserResult.TypeInfo.Current.Name}");
//             Console.WriteLine(lastParserResult.TypeInfo.C);
            Console.WriteLine(HelpText.AutoBuild(lastParserResult, e =>
            {
                e.AutoHelp = true;
                e.AutoVersion = false;
                return HelpText.DefaultParsingErrorsHandler(lastParserResult, e);
            }, ex => ex));
            return;
            Console.WriteLine(HelpText.AutoBuild(lastParserResult, e => e, ex => ex));
//             ErrorType.
            if (!errors.All(e => e.Tag == ErrorType.HelpRequestedError ||
                e.Tag == ErrorType.VersionRequestedError))
                Environment.Exit(1);
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
        /// <see cref="ParserResult{}"/> of last parsing to work with in handlers.
        /// </summary>
        private static ParserResult<object> lastParserResult;

        /// <summary>
        /// Working directory.
        /// </summary>
        public static DirectoryInfo WorkingDir { get; set; } = new DirectoryInfo(Directory.GetCurrentDirectory());
    }
}
