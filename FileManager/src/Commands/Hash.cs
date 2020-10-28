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
                var algorithmHeader = $"{HashAlgo} hashing algorithm";
                var hashingAlgorithm = HashAlgorithms.GetHashAlgorithm(HashAlgo);
                // We encode each byte with two symbols.
                var hashSumSymbolLength = hashingAlgorithm.HashSize / 8 * 2;
                var consoleMinWidthToDoTable = Math.Max(algorithmHeader.Length + 2, hashSumSymbolLength + maxPathLength + 3) + 1;

                Func<string, string> filePathToHashSumString = filePath =>
                {
                    var fileBytes = File.ReadAllBytes(filePath);
                    var hashBytes = hashingAlgorithm.ComputeHash(fileBytes);
                    return string.Join("", hashBytes.Select(x => x.ToString("x2")));
                };

                // No space for table.
                if (Console.WindowWidth < consoleMinWidthToDoTable)
                {
                    const string pathHeaderTemplate = "===== {0} =====";
                    Logger.PrintLine($"[{algorithmHeader}]");

                    foreach (var filePath in Files)
                    {
                        Logger.PrintLine($"===== {filePath.PadRight(maxPathLength)} =====");
                        Logger.PrintLine(filePathToHashSumString(filePath));
                    }

                    // Not counting {0}.
                    Logger.PrintLine(new string('=', pathHeaderTemplate.Length - 3 + maxPathLength));
                }
                else
                {
                    var data = new string[Files.Count(), 2];
                    for (int i = 0; i < Files.Count(); i++)
                    {
                        var filePath = Files.ElementAt(i);
                        data[i, 0] = filePath;
                        data[i, 1] = filePathToHashSumString(filePath);
                    }

                    // Keep in mind that (tableHeader[0].Length + tableHeader[1].Length)
                    // is gotta be < (algorithmHeader.Length).
                    var tableHeader = new string[2] { "filePath", "hashSum" };
                    var table = new Table(2, data, algorithmHeader, tableHeader);
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

    class QHash : BaseHash, IQuiteable
    {
        public bool Quite { get; set; }
    }

    class NHash : BaseHash, INotQuiteable
    {
        public bool Quite { get => false; set => throw new NotImplementedException(); }
    }
}
