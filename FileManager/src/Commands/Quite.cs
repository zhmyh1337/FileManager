using CommandLine;
using FileManager.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Command
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
    /// We need to implement this interface every time the user is working w/o args.
    /// I will use a name like "QCommand" for such command classes.
    /// </summary>
    /// <remarks>
    /// To my mind, the best implementation will be:
    /// <code>
    /// public bool Quite { get; set; }
    /// </code>
    /// </remarks>
    interface IQuiteable : IQuite
    {
        [Option('q', "quite", HelpText = "quiteQuite", ResourceType = typeof(Localization))]
        new bool Quite { get; set; }
    }

    /// <summary>
    /// We need to implement this interface every time the user is working w/o args.
    /// I will use a name like "NCommand" for such command classes.
    /// </summary>
    /// <remarks>
    /// To my mind, the best implementation will be:
    /// <code>
    /// public bool Quite { get => false; set => throw new NotImplementedException(); }
    /// </code>
    /// </remarks>
    interface INotQuiteable : IQuite
    {
        new bool Quite { get; set; }
    }
}
