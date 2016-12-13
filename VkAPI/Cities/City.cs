using Newtonsoft.Json;

namespace VkAPI.Cities
{
    public class City
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Name { get; set; }
    }
}