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
    /// This command changes the working directory.
    /// </summary>
    [Verb("cd", HelpText = "cmdCd", ResourceType = typeof(Localization))]
    abstract class BaseChangeDir : BaseCommand
    {
        [Value(0, MetaName = "dir", HelpText = "cdDir", Required = true, ResourceType = typeof(Localization))]
        public string Dir { get; set; }

        public override void Execute()
        {
            base.Execute();
            
            try
            {
                var testDirectoryStr = Path.GetFullPath(Path.Combine(Terminal.WorkingDir.FullName, Dir.Trim()));
                var testDirectory = new DirectoryInfo(testDirectoryStr);
                Debugger.Print(testDirectoryStr);
                if (!testDirectory.Exists)
                {
                    throw new ArgumentException(Localization.eCdNotExists);
                }

                Terminal.WorkingDir = testDirectory;
            }
            catch (Exception e)
            {
                Logger.PrintError(e.Message);
                OnError();
            }
        }
    }

    class QChangeDir : BaseChangeDir, IQuiteable
    {
        public bool Quite { get; set; }
    }

    class NChangeDir : BaseChangeDir, INotQuiteable
    {
        public bool Quite { get => false; set => throw new NotImplementedException(); }
    }
}
