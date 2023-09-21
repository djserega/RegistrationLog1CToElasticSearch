using System.Text.Json.Serialization;

namespace RegistrationLog1CToElasticSearch.Models
{
    public class MainConfig
    {
        [JsonPropertyName("main")]
        public Main Main { get; set; }
        [JsonPropertyName("elasticsearch")]
        public Elasticsearch Elasticsearch { get; set; }
        [JsonPropertyName("sqlite")]
        public SQLite SQLite { get; set; }
    }

    public class Main
    {
        [JsonPropertyName("takeElements")]
        public int TakeElements { get; set; }
        [JsonPropertyName("timeoutSeconds")]
        public int TimeoutSeconds { get; set; }
    }

    public class Elasticsearch
    {
        [JsonPropertyName("indexName")]
        public string IndexName { get; set; }
        [JsonPropertyName("indexFormat")]
        public string IndexFormat { get; set; }
        [JsonPropertyName("uri")]
        public string Uri { get; set; }
        [JsonPropertyName("login")]
        public string Login { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }

    public class SQLite
    {
        [JsonPropertyName("logpath")]
        public string LogPath { get; set; }
        [JsonPropertyName("dateFrom")]
        public DateTime DateFrom { get; set; }
        [JsonPropertyName("rowIdFrom")]
        public long RowIdFrom { get; set; }
    }
}