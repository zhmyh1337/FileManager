using CommandLine;
using System;
using System.Linq;
using System.Threading;
using Utilities;

namespace Command
{
    [Verb("h@Ck3R", Hidden = true)]
    class BaseHacker : BaseCommand
    {
        [Option(Default = 100)]
        public int Columns { get; set; }

        [Option(Default = 10)]
        public int MinLength { get; set; }

        [Option(Default = 500)]
        public int MaxLength { get; set; }

        [Option(Default = 20)]
        public int Delay { get; set; }

        public override void Execute()
        {
            base.Execute();
            var wasColor = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                var lengths = new int[Columns].Select(_ => Randomizer.Get().Next(MinLength, MaxLength + 1)).ToArray();
                var localMax = lengths.Max();
                for (int i = 0; i < localMax; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        if (i >= lengths[j])
                        {
                            Logger.Print(" ");
                            continue;
                        }
                        Logger.Print(Randomizer.Get().Next(0, 2).ToString());
                    }
                    Logger.PrintLine();
                    Thread.Sleep(Delay);
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = wasColor;
                Logger.PrintError(e.Message);
                OnError();
            }
            finally
            {
                Console.ForegroundColor = wasColor;
            }
        }
    }

    class QHacker : BaseHacker, IQuiteable
    {
        public bool Quite { get; set; }
    }

    class NHacker : BaseHacker, INotQuiteable
    {
        public bool Quite { get => false; set => throw new NotImplementedException(); }
    }
}
