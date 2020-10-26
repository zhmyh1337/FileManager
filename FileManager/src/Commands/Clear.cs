using CommandLine;
using FileManager.Properties;
using System;
using Utilities;

namespace Command
{
    /// <summary>
    /// This command clears the console.
    /// </summary>
    [Verb("clear", HelpText = "cmdClear", ResourceType = typeof(Localization))]
    class BaseClear : BaseCommand
    {
        public override void Execute()
        {
            base.Execute();
            try
            {
                Console.Clear();
            }
            catch (Exception e)
            {
                Logger.PrintError(e.Message);
                OnError();
            }
        }
    }

    class QClear : BaseClear, IQuiteable
    {
        public bool Quite { get; set; }
    }

    class NClear : BaseClear, INotQuiteable
    {
        public bool Quite { get => false; set => throw new NotImplementedException(); }
    }
}
