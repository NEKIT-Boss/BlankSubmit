using Newtonsoft.Json;

namespace VkAPI
{
    /// <summary>
    /// Generic responseof 5.6 VK Api
    /// </summary>
    /// <typeparam name="T">Response Type</typeparam>
    public class VkResponse<T>
    {
        [JsonProperty("response")]
        public T Response { get; set; } 
    }
}