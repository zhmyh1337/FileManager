using CommandLine;
using FileManager.Properties;
using System;
using System.IO;
using Utilities;

namespace Command
{
    /// <summary>
    /// This command removes the directory with the specified path.
    /// </summary>
    [Verb("rmdir", HelpText = "cmdRmdir", ResourceType = typeof(Localization))]
    class BaseRemoveDir : BaseCommand
    {
        [Value(0, MetaName = "dir", HelpText = "rmdirDir", Required = true, ResourceType = typeof(Localization))]
        public string Dir { get; set; }

        public override void Execute()
        {
            base.Execute();
            try
            {
                if (!Directory.Exists(Dir))
                    throw new ArgumentException(Localization.errCommonDirNotExists);

                Directory.Delete(Dir, true);
            }
            catch (Exception e)
            {
                Logger.PrintError(e.Message);
                OnError();
            }
        }
    }

    class QRemoveDir : BaseRemoveDir, IQuiteable
    {
        public bool Quite { get; set; }
    }

    class NRemoveDir : BaseRemoveDir, INotQuiteable
    {
        public bool Quite { get => false; set => throw new NotImplementedException(); }
    }
}
