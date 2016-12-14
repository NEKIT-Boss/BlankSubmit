using System;
using System.Collections.Generic;
using System.Linq;

namespace VkAPI
{
    public static partial class VkApi
    {
        private const string VkDatabaseUrl = @"https://api.vk.com/method/database.{0}?{1}";
        private const string VersionParam = "v=5.60";
        private const string LangParam = "lang=ru";

        // Automatically adds version and language to request
        private static string BuildQuery(IEnumerable<string> queryParameters)
        {
            return string.Join("&", new[] {VersionParam, LangParam}.Concat(queryParameters));
        }

        // Builds up the Uri with request parameters, adding language and version parameters automatically
        private static Uri BuildDatabaseUri(string method, params string[] parameters)
        {
            return new Uri(string.Format(VkDatabaseUrl, 
                method, BuildQuery(parameters)));
        }
    }
}
