using System.Text.Json.Serialization;

namespace RegistrationLog1CToElasticSearch.Models
{
    public class MainConfig
    {
        [JsonPropertyName("main")]
        public required Main Main { get; set; }
        [JsonPropertyName("elasticsearch")]
        public required Elasticsearch Elasticsearch { get; set; }
        [JsonPropertyName("sqlite")]
        public required SQLite SQLite { get; set; }
    }

    public class Main
    {
        [JsonPropertyName("takeElements")]
        public required int TakeElements { get; set; }
        [JsonPropertyName("timeoutSeconds")]
        public required int TimeoutSeconds { get; set; }
        [JsonPropertyName("enableBatchUnload")]
        public required bool EnableBatchUnload { get; set; }
	    [JsonPropertyName("packetSendCount")]
        public required int PacketSendCount { get; set; }
    }

    public class Elasticsearch
    {
        [JsonPropertyName("indexName")]
        public required string IndexName { get; set; }
        [JsonPropertyName("indexFormat")]
        public required string IndexFormat { get; set; }
        [JsonPropertyName("uri")]
        public required string Uri { get; set; }
        [JsonPropertyName("login")]
        public required string Login { get; set; }
        [JsonPropertyName("password")]
        public required string Password { get; set; }
    }

    public class SQLite
    {
        [JsonPropertyName("logpath")]
        public required string LogPath { get; set; }
        [JsonPropertyName("dateFrom")]
        public required DateTime DateFrom { get; set; }
        [JsonPropertyName("rowIdFrom")]
        public required long RowIdFrom { get; set; }
    }
}