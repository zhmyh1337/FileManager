using CommandLine;
using CommandLine.Text;
using Terminal.Properties;
using System;
using System.Collections.Generic;
using Utilities;
using System.Linq;
using System.IO;

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
                var directory = BaseChangeDir.ChangePath(Dir);

                if (!HideFiles)
                {
                    var table = new Table(
                        4,
                        directory.EnumerateFiles("*", new EnumerationOptions()).Select(file => new object[] {
                            file.Name,
                            file.CreationTime,
                            file.LastWriteTime,
                            file.Length
                        }).ToArray(),
                        Localization.dirFiles,
                        new string[] {
                            Localization.dirName,
                            Localization.dirCreationTime,
                            Localization.dirChangeTime,
                            Localization.dirSizeBytes
                        },
                        new int?[] {
                            40,
                            null,
                            null,
                            null
                        }
                    );
                    table.Print();

                    if (!HideDirectories)
                        Logger.PrintLine();
                }

                if (!HideDirectories)
                {
                    var table = new Table(
                        3,
                        directory.EnumerateDirectories("*", new EnumerationOptions()).Select(dir => new object[] {
                            dir.Name,
                            dir.CreationTime,
                            dir.LastWriteTime
                        }).ToArray(),
                        Localization.dirDirectories,
                        new string[] {
                            Localization.dirName,
                            Localization.dirCreationTime,
                            Localization.dirChangeTime
                        },
                        new int?[] {
                            50,
                            null,
                            null
                        }
                    );
                    table.Print();
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
