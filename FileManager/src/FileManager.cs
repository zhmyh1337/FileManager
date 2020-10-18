using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace FileManager
{
    class FileManager
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Command.NDisk, Command.X>(args);
//             Parser.Default.ParseArguments<A>(args)
//                 .WithParsed<A>(x => { x.Quite = true; Console.WriteLine(x); });
        }
    }
}
