using CommandLine;
using System;
using System.IO;
using System.Linq;
using Terminal.Properties;
using Utilities;

namespace Command
{
    /// <summary>
    /// This command changes the disk if specified, otherwise displays the list of all disks.
    /// </summary>
    [Verb("disk", HelpText = "cmdDisk", ResourceType = typeof(Localization))]
    class BaseDisk : BaseCommand
    {
        [Value(0, MetaName = "disk", HelpText = "diskDisk", ResourceType = typeof(Localization))]
        public string Disk { get; set; }

        public override void Execute()
        {
            base.Execute();
            
            try
            {
                var allDrives = DriveInfo.GetDrives();

                if (Disk == null)
                {
                    var table = new Table(
                        6,
                        DriveInfo.GetDrives().Select((drive, id) => new object[] {
                            id + 1,
                            drive.Name,
                            drive.VolumeLabel == string.Empty ? Localization.diskNoLabel : drive.VolumeLabel,
                            drive.AvailableFreeSpace,
                            drive.TotalSize,
                            drive.DriveFormat
                        }).ToArray(),
                        Localization.diskDisks,
                        new string[] {
                            Localization.diskID,
                            Localization.diskName,
                            Localization.diskLabel,
                            Localization.diskFreeSpace,
                            Localization.diskTotalSize,
                            Localization.diskFormat
                        },
                        new int?[] {
                            null,
                            null,
                            20,
                            null,
                            null,
                            null
                        }
                    );
                    table.Print();
                }
                else
                {
                    if (uint.TryParse(Disk, out var id))
                    {
                        if (id < 1 || id > allDrives.Length)
                        {
                            throw new ArgumentException(string.Format(Localization.errDiskInvalidID, id));
                        }
                        else
                        {
                            Terminal.Terminal.WorkingDir = allDrives[id - 1].RootDirectory;
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
                            Terminal.Terminal.WorkingDir = found.RootDirectory;
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
