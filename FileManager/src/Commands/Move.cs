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
    /// This command moves file from one place to another (and overwrites the old file if specified).
    /// </summary>
    [Verb("move", HelpText = "cmdMove", ResourceType = typeof(Localization))]
    abstract class BaseMove : BaseCommand
    {
        [Value(0, MetaName = "from", HelpText = "moveFrom", Required = true, ResourceType = typeof(Localization))]
        public string From { get; set; }

        [Value(1, MetaName = "to", HelpText = "moveTo", Required = true, ResourceType = typeof(Localization))]
        public string To { get; set; }

        [Option('o', "overwrite", HelpText = "moveOverwrite", ResourceType = typeof(Localization))]
        public bool Overwrite { get; set; }

        public override void Execute()
        {
            base.Execute();

            try
            {
                if (!File.Exists(From))
                    throw new FileNotFoundException(string.Format(Localization.eFileNotExists, From));
                if (!Overwrite && File.Exists(To))
                    throw new ArgumentException(string.Format(Localization.eFileExists, To));

                File.Move(From, To, Overwrite);
            }
            catch (Exception e)
            {
                Logger.PrintError(e.Message);
                OnError();
            }
        }
    }

    class QMove : BaseMove, IQuiteable
    {
        public bool Quite { get; set; }
    }

    class NMove : BaseMove, INotQuiteable
    {
        public bool Quite { get => false; set => throw new NotImplementedException(); }
    }
}
