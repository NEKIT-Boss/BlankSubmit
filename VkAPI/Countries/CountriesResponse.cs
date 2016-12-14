using System.Collections.Generic;
using Newtonsoft.Json;

namespace VkAPI.Countries
{
    public class CountriesResponse
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("items")]
        public List<Country> Countries { get; set; }
    }
}