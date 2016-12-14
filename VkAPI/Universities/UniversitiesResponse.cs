using System.Collections.Generic;
using Newtonsoft.Json;

namespace VkAPI.Universities
{
    public class UniversitiesResponse
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("items")]
        public List<University> Universities { get; set; }

    }
}