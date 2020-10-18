using CommandLine;
using FileManager.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Command
{
    /// <summary>
    /// This command changes the disk if specified, otherwise displays the list of all disks.
    /// </summary>
    interface IDisk
    {
        [Option("disk", 
            HelpText = "diskDisk",
            Required = true,
            ResourceType = typeof(Localization))]
        string Disk { get; set; }
    }

    [Verb("disk", HelpText = "cmdDisk", ResourceType = typeof(Localization))]
    class NDisk : IDisk
    {
        public string Disk { get; set; }
    }

    [Verb("tmp")]
    class X
    {
    }
}
