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
        public static async Task<IList<City>> SearchForCitiesAsync(int countryId, string searchQuery)
        {
            const string citiesMethod = "getCities";

            const string needAllParam = "need_all=1";
            string countryParam = $"country_id={countryId}";
            string queryParam = $"q={searchQuery}";

            try
            {
                using (var client = new HttpClient())
                {
                    var requestUri = new Uri(string.Format(VkDatabaseUrl,
                        citiesMethod,
                        string.Join(countryParam, needAllParam, queryParam)));

                    var response = await client.GetAsync(requestUri);
                    response.EnsureSuccessStatusCode();

                    string rawJson = await response.Content.ReadAsStringAsync();

                    return await Task.Run(() =>
                        JsonConvert.DeserializeObject<VkResponse<CitiesResponse>>(rawJson)
                            .Response
                            .Cities);
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
