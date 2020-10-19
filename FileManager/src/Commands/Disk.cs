using CommandLine;
using FileManager.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Command
{
    /// <summary>
    /// This command changes the disk if specified, otherwise displays the list of all disks.
    /// </summary>
    [Verb("disk", HelpText = "cmdDisk", ResourceType = typeof(Localization))]
    abstract class BaseDisk : ICommand
    {
        [Option("disk", HelpText = "diskDisk", ResourceType = typeof(Localization))]
        public string Disk { get; set; }

        public void Execute(Action afterError)
        {
            // TODO table with ID, name, space, type etc.

            Console.WriteLine(Disk ?? "(null)");
            foreach (var drive in DriveInfo.GetDrives())
            {
                Console.WriteLine(drive.DriveType.ToString());
            }
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
