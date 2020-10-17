using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileManager
{
    class FileManager
    {
        static void Main(string[] args)
        {
            
//             CommandLine.Parser.Default.ParseArguments<Options>(ArgsParser.CommandLineToArgs(@"-r ""a b"" c d"))
            CommandLine.Parser.Default.ParseArguments<Options>(args)
              .WithParsed(RunOptions)
              .WithNotParsed(HandleParseError);
        }
    }
}
