using CommandLine;
using Terminal.Properties;
using System;
using System.IO;
using Utilities;

namespace Command
{
    /// <summary>
    /// This command creates a directory with the specified path.
    /// </summary>
    [Verb("mkdir", HelpText = "cmdMkdir", ResourceType = typeof(Localization))]
    class BaseMakeDir : BaseCommand
    {
        [Value(0, MetaName = "dir", HelpText = "mkdirDir", Required = true, ResourceType = typeof(Localization))]
        public string Dir { get; set; }

        public override void Execute()
        {
            base.Execute();
            try
            {
                if (Directory.Exists(Dir))
                    throw new ArgumentException(Localization.errCommonDirExists);

                Directory.CreateDirectory(Dir);
            }
            catch (Exception e)
            {
                Logger.PrintError(e.Message);
                OnError();
            }
        }
    }

    class QMakeDir : BaseMakeDir, IQuiteable
    {
        public bool Quite { get; set; }
    }

    class NMakeDir : BaseMakeDir, INotQuiteable
    {
        public bool Quite { get => false; set => throw new NotImplementedException(); }
    }
}
