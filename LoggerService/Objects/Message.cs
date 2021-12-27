using Newtonsoft.Json;

namespace LoggerService
{
    public class Message
    {
        [JsonProperty(PropertyName = "message")]
        public string Value { get; set; }
    }
}