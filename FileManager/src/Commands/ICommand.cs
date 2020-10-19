using System;
using System.Collections.Generic;
using System.Text;

namespace Command
{
    interface ICommand
    {
        /// <summary>
        /// Executes command.
        /// If an error occured, executes <paramref name="onError"/>.
        /// </summary>
        void Execute(Action onError);
    }
}
