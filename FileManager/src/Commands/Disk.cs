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
    [Verb("disk", HelpText = "cmdDisk", ResourceType = typeof(Localization))]
    abstract class BaseDisk
    {
        [Option("disk", HelpText = "diskDisk", ResourceType = typeof(Localization))]
        public string Disk { get; set; }

        public void Execute()
        {
            Console.WriteLine((this as IQuite).Quite);
        }
    }
    
    class QDisk : BaseDisk, IQuiteable
    {
        public bool Quite { get; set; }
    }

    class NDisk : BaseDisk, INotQuiteable
    {
        public bool Quite { get => false; set => throw new NotImplementedException(); }
    }
}
