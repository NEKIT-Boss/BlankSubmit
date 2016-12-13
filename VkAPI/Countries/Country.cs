using Newtonsoft.Json;

namespace VkAPI.Countries
{
    public class Country
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Name { get; set; }
    }
}