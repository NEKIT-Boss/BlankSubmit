using System.Collections.Generic;
using Newtonsoft.Json;

namespace VkAPI.Cities
{
    public class CitiesResponse
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("items")]
        public IList<City> Cities { get; set; }

    }
}