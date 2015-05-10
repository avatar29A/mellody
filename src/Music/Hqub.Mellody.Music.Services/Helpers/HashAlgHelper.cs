using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Hqub.Mellody.Music.Helpers
{
    public static class HashAlgHelper
    {
        /// <summary>
        /// Cals sha256 hash from string.
        /// </summary>
        /// <param name="source">Source string for hashing.</param>
        /// <returns>String is hashsing is alg sha256</returns>
        public static string GetHash(string source)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(source);
            var hashstring = new SHA256Managed();
            var hash = hashstring.ComputeHash(bytes);
            var hashString = hash.Aggregate(string.Empty, (current, x) => current + String.Format("{0:x2}", x));

            return hashString;
        }
    }
}
