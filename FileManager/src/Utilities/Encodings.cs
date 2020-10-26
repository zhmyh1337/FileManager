using System.Text;

namespace Utilities
{
    static class Encodings
    {
        public enum SupportedEncodings
        {
            Default = 0,
            UTF8 = 1,
            ASCII = 2,
            Unicode = 3,
        }

        public static Encoding GetEncoding(this SupportedEncodings encoding) =>
            encoding switch
            {
                SupportedEncodings.Default => Encoding.Default,
                SupportedEncodings.UTF8 => Encoding.UTF8,
                SupportedEncodings.ASCII => Encoding.ASCII,
                SupportedEncodings.Unicode => Encoding.Unicode,
                _ => Encoding.Default,
            };
    }
}
