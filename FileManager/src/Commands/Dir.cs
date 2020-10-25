using CommandLine;
using CommandLine.Text;
using FileManager;
using FileManager.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Command
{
    /// <summary>
    /// This command scans directory for files and/or directories.
    /// </summary>
    [Verb("dir", HelpText = "cmdDir", ResourceType = typeof(Localization))]
    class BaseDir : BaseCommand
    {
        [Value(0, MetaName = "dir", Default = "", HelpText = "dirDir", ResourceType = typeof(Localization))]
        public string Dir { get; set; }

        [Option('f', HelpText = "dirHideFiles", ResourceType = typeof(Localization))]
        public bool HideFiles { get; set; }

        [Option('d', HelpText = "dirHideDirs", ResourceType = typeof(Localization))]
        public bool HideDirectories { get; set; }

        [Usage(ApplicationAlias = "\b")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example(Localization.exampleCommon,
                    new UnParserSettings { PreferShortName = true },
                    new BaseDir
                    {
                        Dir = "C:/Directory",
                    });
                yield return new Example(Localization.exampleAdvanced,
                    new UnParserSettings { PreferShortName = true },
                    new BaseDir
                    {
                        Dir = "C:/Directory",
                        HideFiles = true,
                    });
            }
        }

        public override void Execute()
        {
            base.Execute();

            try
            {
                var dir = BaseChangeDir.ChangePath(Dir);

                if (!HideFiles)
                {
                    Logger.PrintLine(Localization.dirFiles);
                    var files = dir.GetFiles();
                    foreach (var file in files)
                    {
                        Logger.PrintLine("{0}", file.Name);
                    }

                    if (!HideDirectories)
                        Logger.PrintLine();
                }

                if (!HideDirectories)
                {
                    Logger.PrintLine(Localization.dirDirectories);
                    var directories = dir.GetDirectories();
                    foreach (var _dir in directories)
                    {
                        Logger.PrintLine("{0}", _dir.Name);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.PrintError(e.Message);
                OnError();
            }
        }
    }

    class QDir : BaseDir, IQuiteable
    {
        public bool Quite { get; set; }
    }

    class NDir : BaseDir, INotQuiteable
    {
        public bool Quite { get => false; set => throw new NotImplementedException(); }
    }
}
