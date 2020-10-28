using CommandLine;
using CommandLine.Text;
using Terminal.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities;

namespace Command
{
    /// <summary>
    /// This command prints content of one (or more files) to console with default (or specified) encoding.
    /// </summary>
    [Verb("print", HelpText = "cmdPrint", ResourceType = typeof(Localization))]
    class BasePrint : BaseCommand
    {
        [Option('e', "encoding", Default = Encodings.SupportedEncodings.Default, 
            HelpText = "commonEncoding", ResourceType = typeof(Localization))]
        public Encodings.SupportedEncodings Encoding { get; set; }

        [Value(0, MetaName = "files", HelpText = "printFiles", Required = true, ResourceType = typeof(Localization))]
        public IEnumerable<string> Files { get; set; }

        [Usage(ApplicationAlias = "\b")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example(Localization.exampleCommon, 
                    new UnParserSettings { PreferShortName = true }, 
                    new BasePrint
                    {
                        Files = new string[] { "file.ext" },
                    });
                yield return new Example(Localization.exampleAdvanced,
                    new UnParserSettings { PreferShortName = true },
                    new BasePrint
                    {
                        Files = new string[] { "file1.ext", "file2.ext", "file3.ext" },
                        Encoding = Encodings.SupportedEncodings.ASCII,
                    });
            }
        }

        public override void Execute()
        {
            base.Execute();

            try
            {
                foreach (var filePath in Files)
                {
                    if (!File.Exists(filePath))
                    {
                        throw new FileNotFoundException(string.Format(Localization.errCommonFileNotExists, filePath));
                    }
                }

                var maxPathLength = Files.Select(s => s.Length).Max();
                const string pathHeaderTemplate = "===== {0} =====";

                foreach (var filePath in Files)
                {
                    Logger.PrintLine($"===== {filePath.PadRight(maxPathLength)} =====");
                    Logger.PrintLine(File.ReadAllText(filePath, Encoding.GetEncoding()));
                }

                // Not counting {0}.
                Logger.PrintLine(new string('=', pathHeaderTemplate.Length - 3 + maxPathLength));
            }
            catch (Exception e)
            {
                Logger.PrintError(e.Message);
                OnError();
            }
        }
    }

    class QPrint : BasePrint, IQuiteable
    {
        public bool Quite { get; set; }
    }

    class NPrint : BasePrint, INotQuiteable
    {
        public bool Quite { get => false; set => throw new NotImplementedException(); }
    }
}
