using System;
using System.Collections.Generic;
using System.Text;

namespace Command
{
    interface ICommand
    {
        /// <summary>
        /// Executes command.
        /// </summary>
        void Execute();
    }

    abstract class BaseCommand : ICommand
    {
        public virtual void Execute()
        {
            // Print nothing if -q (--quite).
            if (((Command.IQuite)this).Quite)
                FileManager.Logger.Writer = x => { };
        }

        /// <summary>
        /// We call this from derived classes if an exception was caught.
        /// </summary>
        public void OnError()
        {
            if (((Command.IQuite)this).Quite)
                Environment.Exit(1);
        }
    }
}
