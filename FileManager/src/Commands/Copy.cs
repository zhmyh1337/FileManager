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
    /// This command copies file from one place to another (and overwrites the old file if specified).
    /// </summary>
    [Verb("copy", HelpText = "cmdCopy", ResourceType = typeof(Localization))]
    class BaseCopy : BaseCommand
    {
        [Value(0, MetaName = "from", HelpText = "copyFrom", Required = true, ResourceType = typeof(Localization))]
        public string From { get; set; }

        [Value(1, MetaName = "to", HelpText = "copyTo", Required = true, ResourceType = typeof(Localization))]
        public string To { get; set; }

        [Option('o', "overwrite", HelpText = "commonFileOverwrite", ResourceType = typeof(Localization))]
        public bool Overwrite { get; set; }

        [Usage(ApplicationAlias = "\b")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example(Localization.exampleCommon,
                    new UnParserSettings { PreferShortName = true },
                    new BaseCopy
                    {
                        From = "C:/From/file.ext",
                        To = "C:/To/file.ext",
                    });
                yield return new Example(Localization.exampleAdvanced,
                    new UnParserSettings { PreferShortName = true },
                    new BaseCopy
                    {
                        From = "C:/From/file.ext",
                        To = "C:/To/file.ext",
                        Overwrite = true,
                    });
            }
        }

        public override void Execute()
        {
            base.Execute();

            try
            {
                if (!File.Exists(From))
                    throw new FileNotFoundException(string.Format(Localization.errCommonFileNotExists, From));
                if (!Overwrite && File.Exists(To))
                    throw new ArgumentException(string.Format(Localization.errCommonFileExists, To));

                File.Copy(From, To, Overwrite);
            }
            catch (Exception e)
            {
                Logger.PrintError(e.Message);
                OnError();
            }
        }
    }

    class QCopy : BaseCopy, IQuiteable
    {
        public bool Quite { get; set; }
    }

    class NCopy : BaseCopy, INotQuiteable
    {
        public bool Quite { get => false; set => throw new NotImplementedException(); }
    }
}
