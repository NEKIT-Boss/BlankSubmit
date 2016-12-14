using System.Collections.Generic;
using Newtonsoft.Json;

namespace VkAPI.Universities
{
    public class UniversitiesResponse
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("items")]
        public IList<University> Universities { get; set; }

    }
}