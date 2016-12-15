using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VkAPI.Cities;
using VkAPI.Universities;

// ReSharper disable CheckNamespace

namespace VkAPI
{
    public static partial class VkApi
    {
        public static async Task<List<University>> SearchForUniversitiesAsync(int countryId, int cityId, string searchQuery, int count = DefaultCount)
        {
            const string citiesMethod = "getUniversities";

            string countryParam = $"country_id={countryId}";
            string cityParam = $"city_id={cityId}";
            string queryParam = $"q={searchQuery}";
            string countParam = $"count={count}";

            try
            {
                using (var client = new HttpClient())
                {
                    var requestUri = BuildDatabaseUri(citiesMethod,
                        countryParam, cityParam, queryParam, countParam);

                    var response = await client.GetAsync(requestUri);
                    response.EnsureSuccessStatusCode();

                    string rawJson = await response.Content.ReadAsStringAsync();

                    return await Task.Run(() =>
                        JsonConvert.DeserializeObject<VkResponse<UniversitiesResponse>>(rawJson)
                            .Response
                            .Universities);
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
