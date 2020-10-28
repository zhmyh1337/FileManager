using System.Security.Cryptography;

namespace Utilities
{
    static class HashAlgorithms
    {
        public enum SupportedHashAlgorithms
        {
            MD5,
            SHA1,
            SHA256,
            SHA384,
            SHA512,
        }

        public static HashAlgorithm GetHashAlgorithm(this SupportedHashAlgorithms hashAlgorithm) =>
            hashAlgorithm switch
            {
                SupportedHashAlgorithms.MD5 => md5,
                SupportedHashAlgorithms.SHA1 => sha1,
                SupportedHashAlgorithms.SHA256 => sha256,
                SupportedHashAlgorithms.SHA384 => sha384,
                SupportedHashAlgorithms.SHA512 => sha512,
                _ => null,
            };

        private static readonly MD5 md5 = MD5.Create();
        private static readonly SHA1 sha1 = SHA1.Create();
        private static readonly SHA256 sha256 = SHA256.Create();
        private static readonly SHA384 sha384 = SHA384.Create();
        private static readonly SHA512 sha512 = SHA512.Create();
    }
}
