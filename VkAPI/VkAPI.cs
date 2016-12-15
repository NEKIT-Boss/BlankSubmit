using System;
using System.Collections.Generic;
using System.Linq;

namespace VkAPI
{
    /// <summary>
    /// Class for accesing VK Api, static because, why to separate to subservices in such a small app
    /// Static will do nice here
    /// </summary>
    public static partial class VkApi
    {
        private const string VkDatabaseUrl = @"https://api.vk.com/method/database.{0}?{1}";
        private const string VersionParam = "v=5.60";
        private const string LangParam = "lang=ru";

        /// <summary>
        /// Default async request count, open for later pagination support
        /// </summary>
        private const int DefaultCount = 10;

        /// <summary>
        /// Wrapper to always include language and version parameters
        /// </summary>
        /// <param name="queryParameters">Actual parameters</param>
        /// <returns>Built up query</returns>
        private static string BuildQuery(IEnumerable<string> queryParameters)
        {
            return string.Join("&", new[] {VersionParam, LangParam}.Concat(queryParameters));
        }

        /// <summary>
        /// Builds up the VKDatabase request
        /// </summary>
        /// <param name="method">Target method</param>
        /// <param name="parameters">Request parameters</param>
        /// <returns>Built up request <see cref="Uri"/></returns>
        private static Uri BuildDatabaseUri(string method, params string[] parameters)
        {
            return new Uri(string.Format(VkDatabaseUrl, 
                method, BuildQuery(parameters)));
        }
    }
}
