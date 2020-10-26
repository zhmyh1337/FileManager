using CommandLine;
using FileManager.Properties;
using System;
using System.IO;
using Utilities;

namespace Command
{
    /// <summary>
    /// This command deletes a file.
    /// </summary>
    [Verb("del", HelpText = "cmdDelFile", ResourceType = typeof(Localization))]
    class BaseDeleteFile : BaseCommand
    {
        [Value(0, MetaName = "file", HelpText = "delFileFile", Required = true, ResourceType = typeof(Localization))]
        public string FilePath { get; set; }

        public override void Execute()
        {
            base.Execute();

            try
            {
                if (!File.Exists(FilePath))
                    throw new FileNotFoundException(string.Format(Localization.errCommonFileNotExists, FilePath));

                File.Delete(FilePath);
            }
            catch (Exception e)
            {
                Logger.PrintError(e.Message);
                OnError();
            }
        }
    }

    class QDeleteFile : BaseDeleteFile, IQuiteable
    {
        public bool Quite { get; set; }
    }

    class NDeleteFile : BaseDeleteFile, INotQuiteable
    {
        public bool Quite { get => false; set => throw new NotImplementedException(); }
    }
}
