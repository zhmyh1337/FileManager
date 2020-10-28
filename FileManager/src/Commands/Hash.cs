using CommandLine;
using CommandLine.Text;
using FileManager.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities;

namespace Command
{
    /// <summary>
    /// This command prints the hash sums of files.
    /// </summary>
    [Verb("hash", HelpText = "cmdHash", ResourceType = typeof(Localization))]
    class BaseHash : BaseCommand
    {
        [Option('a', "algorithm", Default = HashAlgorithms.SupportedHashAlgorithms.MD5, 
            HelpText = "hashAlgo", ResourceType = typeof(Localization))]
        public HashAlgorithms.SupportedHashAlgorithms HashAlgo { get; set; }

        [Value(0, MetaName = "files", HelpText = "hashFiles", Required = true, ResourceType = typeof(Localization))]
        public IEnumerable<string> Files { get; set; }

        [Usage(ApplicationAlias = "\b")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example(Localization.exampleCommon,
                    new UnParserSettings { PreferShortName = true },
                    new BaseHash
                    {
                        Files = new string[] { "file.ext" },
                    });
                yield return new Example(Localization.exampleAdvanced,
                    new UnParserSettings { PreferShortName = true },
                    new BaseHash
                    {
                        Files = new string[] { "file1.ext", "file2.ext", "file3.ext" },
                        HashAlgo = HashAlgorithms.SupportedHashAlgorithms.SHA512,
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

                Logger.PrintLine($"[{HashAlgo} hashing algorithm]");
                foreach (var filePath in Files)
                {
                    Logger.PrintLine($"===== {filePath.PadRight(maxPathLength)} =====");

                    var hashingAlgorithm = HashAlgorithms.GetHashAlgorithm(HashAlgo);
                    var inputBytes = File.ReadAllBytes(filePath);
                    var hashBytes = hashingAlgorithm.ComputeHash(inputBytes);
                    
                    Logger.PrintLine(string.Join("", hashBytes.Select(x => x.ToString("x2"))));
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

    class QHash : BaseHash, IQuiteable
    {
        public bool Quite { get; set; }
    }

    class NHash : BaseHash, INotQuiteable
    {
        public bool Quite { get => false; set => throw new NotImplementedException(); }
    }
}
