using CommandLine;
using CommandLine.Text;
using FileManager.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using Utilities;

namespace Command
{
    /// <summary>
    /// This command creates a new file (or rewrites an existing one if the overwrite option was specified) 
    /// and fills it with content in some encoding if specified.
    /// </summary>
    [Verb("write", HelpText = "cmdWriteFile", ResourceType = typeof(Localization))]
    class BaseWriteFile : BaseCommand
    {
        [Option('e', "encoding", Default = Encodings.SupportedEncodings.Default, HelpText = "commonEncoding", ResourceType = typeof(Localization))]
        public Encodings.SupportedEncodings Encoding { get; set; }

        [Option('o', "overwrite", HelpText = "commonFileOverwrite", ResourceType = typeof(Localization))]
        public bool Overwrite { get; set; }

        [Value(0, MetaName = "file", HelpText = "writeFileFile", Required = true, ResourceType = typeof(Localization))]
        public string FilePath { get; set; }

        [Value(1, MetaName = "lines", HelpText = "writeFileLines", ResourceType = typeof(Localization))]
        public IEnumerable<string> Lines { get; set; }

        [Usage(ApplicationAlias = "\b")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example(Localization.exampleCommon,
                    new UnParserSettings { PreferShortName = true },
                    new BaseWriteFile
                    {
                        FilePath = "file.ext",
                    });
                yield return new Example(Localization.exampleAdvanced,
                    new UnParserSettings { PreferShortName = true },
                    new BaseWriteFile
                    {
                        FilePath = "C:/Directory name/file.ext",
                        Encoding = Encodings.SupportedEncodings.ASCII,
                        Overwrite = true,
                        Lines = new string[] { "file content" },
                    });
                yield return new Example(Localization.exampleMultiline,
                    new UnParserSettings { PreferShortName = true },
                    new BaseWriteFile
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
                Debugger.PrintLine("");

                if (!Overwrite && File.Exists(FilePath))
                    throw new ArgumentException(string.Format(Localization.errCommonFileExists, FilePath));

                using (StreamWriter writer = new StreamWriter(FilePath, false, Encoding.GetEncoding()))
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

    class QWriteFile : BaseWriteFile, IQuiteable
    {
        public bool Quite { get; set; }
    }

    class NWriteFile : BaseWriteFile, INotQuiteable
    {
        public bool Quite { get => false; set => throw new NotImplementedException(); }
    }
}
