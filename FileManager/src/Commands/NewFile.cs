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
    /// This command creates a new file (and fills it with content in some encoding if specified).
    /// </summary>
    [Verb("new", HelpText = "cmdNewFile", ResourceType = typeof(Localization))]
    class BaseNewFile : BaseCommand
    {
        public enum EncodingTypes
        {
            Default = 0,
            UTF8 = 1,
            ASCII = 2,
            Unicode = 3
        }

        [Option('e', "encoding", Default = EncodingTypes.Default, HelpText = "commonEncoding", ResourceType = typeof(Localization))]
        public EncodingTypes Encoding_ { get; set; }

        [Option('o', "overwrite", HelpText = "commonFileOverwrite", ResourceType = typeof(Localization))]
        public bool Overwrite { get; set; }

        [Value(0, MetaName = "file", HelpText = "newFileFile", Required = true, ResourceType = typeof(Localization))]
        public string FilePath { get; set; }

        [Value(1, MetaName = "lines", HelpText = "newFileLines", ResourceType = typeof(Localization))]
        public IEnumerable<string> Lines { get; set; }

        [Usage(ApplicationAlias = "\b")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example(Localization.exampleCommon,
                    new UnParserSettings { PreferShortName = true },
                    new BaseNewFile
                    {
                        FilePath = "file.ext",
                    });
                yield return new Example(Localization.exampleAdvanced,
                    new UnParserSettings { PreferShortName = true },
                    new BaseNewFile
                    {
                        FilePath = "C:/Directory name/file.ext",
                        Encoding_ = EncodingTypes.ASCII,
                        Overwrite = true,
                        Lines = new string[] { "file content" },
                    });
                yield return new Example(Localization.exampleMultiline,
                    new UnParserSettings { PreferShortName = true },
                    new BaseNewFile
                    {
                        FilePath = "file.ext",
                        Lines = new string[] { "line 1", "line 2", "line 3" },
                    });
            }
        }

        public override void Execute()
        {
            base.Execute();

            try
            {
                Encoding encoding = Encoding_ switch
                {
                    EncodingTypes.Default => Encoding.Default,
                    EncodingTypes.UTF8 => Encoding.UTF8,
                    EncodingTypes.ASCII => Encoding.ASCII,
                    EncodingTypes.Unicode => Encoding.Unicode,
                    _ => Encoding.Default,
                };

                if (!Overwrite && File.Exists(FilePath))
                    throw new ArgumentException(string.Format(Localization.errCommonFileExists, FilePath));

                using (StreamWriter writer = new StreamWriter(FilePath, false, encoding))
                {
                    foreach (var line in Lines)
                    {
                        writer.WriteLine(line);
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

    class QNewFile : BaseNewFile, IQuiteable
    {
        public bool Quite { get; set; }
    }

    class NNewFile : BaseNewFile, INotQuiteable
    {
        public bool Quite { get => false; set => throw new NotImplementedException(); }
    }
}
