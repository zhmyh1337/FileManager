using CommandLine;
using Terminal.Properties;
using System;
using Utilities;

namespace Command
{
    /// <summary>
    /// This command changes console foreground color.
    /// </summary>
    [Verb("color", HelpText = "cmdColor", ResourceType = typeof(Localization))]
    class BaseColor : BaseCommand
    {
        [Value(0, MetaName = "color", HelpText = "colorColor", ResourceType = typeof(Localization))]
        public ConsoleColor? Color { get; set; }

        [Option('r', "reset", HelpText = "colorReset", ResourceType = typeof(Localization))]
        public bool Reset { get; set; }

        public override void Execute()
        {
            base.Execute();
            try
            {
                if (Reset)
                {
                    Console.ResetColor();
                    return;
                }

                if (Color == null)
                {
                    var wasClr = Console.ForegroundColor;
                    foreach (ConsoleColor color in Enum.GetValues(typeof(ConsoleColor)))
                    {
                        Console.ForegroundColor = color;
                        Logger.PrintLine($"{(int)color}. {color}");
                    }
                    Console.ForegroundColor = wasClr;
                }
                else
                {
                    if (Color < ConsoleColor.Black || Color > ConsoleColor.White)
                        throw new ArgumentException(string.Format(Localization.errColorBounds, Color));
                    Console.ForegroundColor = (ConsoleColor)Color;
                }
            }
            catch (Exception e)
            {
                Logger.PrintError(e.Message);
                OnError();
            }
        }
    }

    class QColor : BaseColor, IQuiteable
    {
        public bool Quite { get; set; }
    }

    class NColor : BaseColor, INotQuiteable
    {
        public bool Quite { get => false; set => throw new NotImplementedException(); }
    }
}
