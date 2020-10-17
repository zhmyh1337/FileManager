using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FileManager
{
    /// <summary>
    /// This static class parses string as args.
    /// See <see href="https://stackoverflow.com/questions/298830/split-string-containing-command-line-parameters-into-string-in-c-sharp">here</see>.
    /// </summary>
    static class ArgsParser
    {
        [DllImport("shell32.dll", SetLastError = true)]
        static extern IntPtr CommandLineToArgvW(
        [MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);

        public static string[] CommandLineToArgs(string commandLine)
        {
            int argc;
            var argv = CommandLineToArgvW(commandLine, out argc);
            if (argv == IntPtr.Zero)
                throw new System.ComponentModel.Win32Exception();
            try
            {
                var args = new string[argc];
                for (var i = 0; i < args.Length; i++)
                {
                    var p = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
                    args[i] = Marshal.PtrToStringUni(p);
                }

                return args;
            }
            finally
            {
                Marshal.FreeHGlobal(argv);
            }
        }
    }
}
