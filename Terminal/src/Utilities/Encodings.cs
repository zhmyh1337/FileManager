using System;
using System.Text;

namespace Utilities
{
    static class Encodings
    {
        public enum SupportedEncodings
        {
            Default,
            UTF8,
            ASCII,
            Unicode,
        }

        public static Encoding GetEncoding(this SupportedEncodings encoding) =>
            encoding switch
            {
                SupportedEncodings.Default => Encoding.Default,
                SupportedEncodings.UTF8 => Encoding.UTF8,
                SupportedEncodings.ASCII => Encoding.ASCII,
                SupportedEncodings.Unicode => Encoding.Unicode,
                _ => null,
            };
    }
}
