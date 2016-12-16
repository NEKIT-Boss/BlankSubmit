using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VkAPI.Countries;
// ReSharper disable CheckNamespace

namespace VkAPI
{
    public static partial class VkApi
    {
        private static List<Country> _countriesCache;

        public static async Task<List<Country>> GetAllCountriesAsync()
        {
            if (_countriesCache != null) return _countriesCache;

            const string countriesMethod = "getCountries";
            const string needAllParam = "need_all=1";
            
            // Requesting max, I doubt that coutries count will change.
            // It is for the sake of simplicity
            const string countParam = "count=1000";

            try
            {
                using (var client = new HttpClient())
                {
                    var requestUri = new Uri(string.Format(VkDatabaseUrl, 
                        countriesMethod, 
                        string.Join("&", LangParam, VersionParam, needAllParam, countParam)));
                    var response = await client.GetAsync(requestUri);
                    response.EnsureSuccessStatusCode();

                    string rawJson = await response.Content.ReadAsStringAsync();

                    _countriesCache = await Task.Run(() =>
                        JsonConvert.DeserializeObject<VkResponse<CountriesResponse>>(rawJson)
                            .Response
                            .Countries).ConfigureAwait(false);

                    return _countriesCache;
                }
            }

            catch (Exception)
            {
                // TODO
                //Debugger.Break();
                return null;
            }
        }
    }
}