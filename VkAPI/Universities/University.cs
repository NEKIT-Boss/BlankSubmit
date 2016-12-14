using Newtonsoft.Json;

namespace VkAPI.Universities
{
    public class University
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Name { get; set; }
    }
}