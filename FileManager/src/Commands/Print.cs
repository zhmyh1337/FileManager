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
    /// This command scans directory for files and/or directories.
    /// </summary>
    [Verb("print", HelpText = "cmdPrint", ResourceType = typeof(Localization))]
    abstract class BasePrint : BaseCommand
    {
        public enum EncodingTypes
        {
            Default = 0,
            UTF8 = 1,
            ASCII = 2,
            UNICODE = 3
        }

        [Option('e', "encoding", Default = EncodingTypes.Default, HelpText = "printEnc", ResourceType = typeof(Localization))]
        public EncodingTypes Encoding { get; set; }

        [Value(0, MetaName = "dirs", HelpText = "printDirs", Required = true, ResourceType = typeof(Localization))]
        public IEnumerable<string> Dirs { get; set; }

        public override void Execute()
        {
            base.Execute();

            try
            {
                
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
