using System.Text.Json.Serialization;

namespace ElasticSearchWebLogs.API
{
    public class WebLog
    {
        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("response")]
        public int Response { get; set; }
    }
}
