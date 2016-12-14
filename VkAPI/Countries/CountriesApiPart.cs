using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VkAPI.Countries;
// ReSharper disable CheckNamespace

namespace VkAPI
{
    public static partial class VkApi
    {
        private static IList<Country> _countriesCache;

        public static async Task<IList<Country>> GetAllCountriesAsync()
        {
            if (_countriesCache != null) return _countriesCache;

            const string countriesMethod = "getCountries";
            const string needAllParam = "need_all=1";

            try
            {
                using (var client = new HttpClient())
                {
                    var requestUri = new Uri(string.Format(VkDatabaseUrl, countriesMethod, needAllParam));
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
                return null;
            }
        }
    }
}