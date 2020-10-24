﻿using CommandLine;
using FileManager;
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
    abstract class BaseDisk : BaseCommand
    {
        [Value(0, MetaName = "disk", HelpText = "diskDisk", ResourceType = typeof(Localization))]
        public string Disk { get; set; }

        public override void Execute()
        {
            base.Execute();
            // TODO table with ID, name, space, type etc.
            try
            {
                var allDrives = DriveInfo.GetDrives();

                if (Disk == null)
                {
                    int id = 1;
                    foreach (var drive in allDrives)
                    {
                        Logger.PrintLine($"{id++}) {drive.Name}");
                    }
                }
                else
                {
                    if (uint.TryParse(Disk, out var id))
                    {
                        if (id < 1 || id > allDrives.Length)
                        {
                            throw new ArgumentException(string.Format(Localization.errDiskInvalidNumber, id));
                        }
                        else
                        {
                            Terminal.WorkingDir = allDrives[id - 1].RootDirectory;
                        }
                    }
                    else
                    {
                        // Not case sensitive.
                        var found = Array.Find(allDrives, x => x.Name.ToLower() == Disk.ToLower());
                        if (found == null)
                        {
                            throw new ArgumentException(string.Format(Localization.errDiskInvalidName, Disk));
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
                Logger.PrintError(e.Message);
                OnError();
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
