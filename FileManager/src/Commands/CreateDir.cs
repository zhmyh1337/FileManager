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
    [Verb("mkdir", HelpText = "cmdMkdir", ResourceType = typeof(Localization))]
    class BaseCreateDir : BaseCommand
    {
        [Value(0, MetaName = "dir", HelpText = "mkdirDir", ResourceType = typeof(Localization))]
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

    class QCreateDir : BaseCreateDir, IQuiteable
    {
        public bool Quite { get; set; }
    }

    class NCreateDir : BaseCreateDir, INotQuiteable
    {
        public bool Quite { get => false; set => throw new NotImplementedException(); }
    }
}
