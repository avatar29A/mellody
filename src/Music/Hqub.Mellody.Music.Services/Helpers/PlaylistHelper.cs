using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hqub.Mellody.Poco;
using System.Text.RegularExpressions;

namespace Hqub.Mellody.Music.Helpers
{
    public static class PlaylistHelper
    {
        /// <summary>
        /// Extract the name of the artist from query.
        /// </summary>
        /// <param name="query">Query come from user.</param>
        /// <returns>Artist name</returns>
        public static string GetArtistName(QueryEntity query)
        {
            if (query.TypeQuery != TypeQuery.Album)
            {
                return query.Name;
            }

            var complexName = query.Name.Split('-');
            return complexName.Length < 2 ? query.Name : complexName[0];
        }

        /// <summary>
        /// Split track name
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string[] SplitTitle(string query)
        {
            var complexName = query.Split('-');

            return complexName.Select(n => n.Trim()).ToArray();
        }

        /// <summary>
        /// Remove all specific symbols.
        /// </summary>
        /// <param name="name">Input text</param>
        /// <param name="type">Type request (Album, Artist or Query)</param>
        /// <returns>Cleaned text</returns>
        public static string ClearName(string name, TypeQuery type)
        {
            if (type == TypeQuery.Artist || type == TypeQuery.Query)
                return Normalize(name);

            //if Album:
            var split = name.Split('-');

            return string.Join("", split.Select(Normalize).ToArray());
        }

        /// <summary>
        /// Generate query name for hash algorithm.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string ConvertQueryToString(QueryEntity query)
        {
            var joinQuery = string.Format("{0}{1}",
                Enum.GetName(typeof(TypeQuery), query.TypeQuery), ClearName(query.Name, query.TypeQuery));

            return joinQuery;
        }

        private static string Normalize(string name)
        {
            return Regex.Replace(name, "[^\\w\\d]", string.Empty).Trim().ToLower();
        }

       
    }
}
