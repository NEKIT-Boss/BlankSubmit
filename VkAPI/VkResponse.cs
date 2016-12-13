using Newtonsoft.Json;

namespace VkAPI
{
    public class VkResponse<T>
    {
        [JsonProperty("response")]
        public T Response { get; set; } 
    }
}