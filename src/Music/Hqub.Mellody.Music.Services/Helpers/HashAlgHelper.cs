using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Hqub.Mellody.Music.Helpers
{
    public static class HashAlgHelper
    {
        public static string GetHash(string query)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(query);
            var hashstring = new SHA256Managed();
            var hash = hashstring.ComputeHash(bytes);
            var hashString = hash.Aggregate(string.Empty, (current, x) => current + String.Format("{0:x2}", x));

            return hashString;
        }
    }
}
