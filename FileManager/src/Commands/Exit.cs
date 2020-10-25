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
    /// This command terminates the program.
    /// </summary>
    [Verb("exit", HelpText = "cmdExit", ResourceType = typeof(Localization))]
    class BaseExit : BaseCommand
    {
        public override void Execute()
        {
            base.Execute();
            try
            {
                Environment.Exit(0);
            }
            catch (Exception e)
            {
                Logger.PrintError(e.Message);
                OnError();
            }
        }
    }

    class QExit : BaseExit, IQuiteable
    {
        public bool Quite { get; set; }
    }

    class NExit : BaseExit, INotQuiteable
    {
        public bool Quite { get => false; set => throw new NotImplementedException(); }
    }
}
