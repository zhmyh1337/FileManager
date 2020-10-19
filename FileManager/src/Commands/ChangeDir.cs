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
                Terminal.WorkingDir = ChangePath(Dir);
            }
            catch (Exception e)
            {
                Logger.PrintError(e.Message);
                OnError();
            }
        }

        /// <summary>
        /// Combines <see cref="Terminal.WorkingDir"/> with <paramref name="path"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>The combined full path.</returns>
        /// <exception cref="DirectoryNotFoundException">When the combined directory doesn't exist.</exception>
        static public DirectoryInfo ChangePath(string path)
        {
            var testDirectoryStr = Path.GetFullPath(Path.Combine(Terminal.WorkingDir.FullName, path.Trim()));
            var testDirectory = new DirectoryInfo(testDirectoryStr);

            if (!testDirectory.Exists)
            {
                throw new DirectoryNotFoundException(Localization.eDirNotExists);
            }

            return testDirectory;
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
