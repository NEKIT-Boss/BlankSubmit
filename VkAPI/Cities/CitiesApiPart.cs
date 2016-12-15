using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VkAPI.Cities;
// ReSharper disable CheckNamespace

namespace VkAPI
{
    public static partial class VkApi
    {
        public static async Task<List<City>> SearchForCitiesAsync(int countryId, string searchQuery, int count = DefaultCount)
        {
            const string citiesMethod = "getCities";

            const string needAllParam = "need_all=1";
            string countryParam = $"country_id={countryId}";
            string queryParam = $"q={searchQuery}";
            string countParam = $"count={count}";

            try
            {
                using (var client = new HttpClient())
                {
                    var requestUri = BuildDatabaseUri(citiesMethod,
                        needAllParam, countryParam, queryParam, countParam);

                    var response = await client.GetAsync(requestUri);
                    response.EnsureSuccessStatusCode();

                    string rawJson = await response.Content.ReadAsStringAsync();

                    return await Task.Factory.StartNew(() =>
                        JsonConvert.DeserializeObject<VkResponse<CitiesResponse>>(rawJson)
                            .Response
                            .Cities).ConfigureAwait(false);
                }
            }

            catch (Exception)
            {
                // TODO
                // Request exception behavior was not specified
                return null;
            }
        }
    }
}
