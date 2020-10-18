using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager
{
    /// <summary>
    /// This interface is responsible for -q (--quite) option.
    /// </summary>
    /// <remarks>
    /// I decided to leave this option only when we work w/o args.
    /// But it was really painful, because I couldn't understand
    /// how to make dynamic options work properly in this parser.
    /// </remarks>
    interface IQuite
    {
        bool Quite { get; set; }
    }

    /// <summary>
    /// We need to inherit from this class every time the user is working w/o args.
    /// </summary>
    abstract class NotQuitable : IQuite
    {
        public bool Quite { get => false; set => throw new NotImplementedException(); }
    }

    /// <summary>
    /// We need to inherit from this class every time the user is working w/ args.
    /// </summary>
    abstract class Quitable : IQuite
    {
        [Option('q', "quite", HelpText = "Suppresses summary message.")]
        public bool Quite { get; set; }
    }
}
