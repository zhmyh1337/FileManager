using CommandLine;
using FileManager;
using FileManager.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Command
{
    /// <summary>
    /// This command changes console foreground color.
    /// </summary>
    [Verb("color", HelpText = "cmdColor", ResourceType = typeof(Localization))]
    abstract class BaseColor : BaseCommand
    {
        [Value(0, MetaName = "color", HelpText = "colorColor", ResourceType = typeof(Localization))]
        public ConsoleColor? Color { get; set; }

        public override void Execute()
        {
            base.Execute();
            try
            {
                if (Color == null)
                {
                    var wasClr = Console.ForegroundColor;
                    foreach (ConsoleColor color in Enum.GetValues(typeof(ConsoleColor)))
                    {
                        Console.ForegroundColor = color;
                        Logger.Print("{0}. {1}", (int)color, color);
                    }
                    Console.ForegroundColor = wasClr;
                }
                else
                {
                    if (Color < ConsoleColor.Black || Color > ConsoleColor.White)
                        throw new ArgumentException(string.Format(Localization.eColorBounds, Color));
                    Console.ForegroundColor = (ConsoleColor)Color;
                    Logger.PrintSuccess();
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
