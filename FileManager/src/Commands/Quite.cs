using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager
{
    interface IQuite
    {
        bool Quite { get; set; }
    }

    abstract class NotQuitable : IQuite
    {
        public bool Quite { get => false; set => throw new NotImplementedException(); }
    }

    abstract class Quitable : IQuite
    {
        [Option('q', "quite", HelpText = "Suppresses summary message.")]
        public bool Quite { get; set; }
    }
}
