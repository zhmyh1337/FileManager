using CommandLine;
using FileManager;
using FileManager.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Command
{
    [Verb("tmp", HelpText = "tmp")]
    class Tmp
    {

    }

    /// <summary>
    /// This command changes the disk if specified, otherwise displays the list of all disks.
    /// </summary>
    [Verb("disk", HelpText = "cmdDisk", ResourceType = typeof(Localization))]
    abstract class BaseDisk : ICommand
    {
        [Value(0, MetaName = "diskDisk", HelpText = "diskDisk", ResourceType = typeof(Localization))]
        public string Disk { get; set; }

        public void Execute(Action onError)
        {
            // TODO table with ID, name, space, type etc.
            try
            {
                var allDrives = DriveInfo.GetDrives();

                if (Disk == null)
                {
                    int id = 1;
                    foreach (var drive in allDrives)
                    {
                        Console.WriteLine($"{id++}) {drive.Name}");
                    }
                }
                else
                {
                    if (uint.TryParse(Disk, out var id))
                    {
                        if (id < 1 || id > allDrives.Length)
                        {
                            throw new ArgumentException(string.Format(Localization.eDiskInvalidNumber, id));
                        }
                        else
                        {
                            Terminal.WorkingDir = allDrives[id - 1].RootDirectory;
                        }
                    }
                    else
                    {
                        var found = Array.Find(allDrives, x => x.Name == Disk);
                        if (found == null)
                        {
                            throw new ArgumentException(string.Format(Localization.eDiskInvalidName, Disk));
                        }
                        else
                        {
                            Terminal.WorkingDir = found.RootDirectory;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
