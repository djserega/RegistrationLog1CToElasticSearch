using System.Text.Json.Serialization;

namespace RegistrationLog1CToElasticSearch.Models
{
    public class MainConfig
    {
        [JsonPropertyName("main")]
        public Main Main { get; set; } = new();

        [JsonPropertyName("elasticsearch")]
        public Elasticsearch Elasticsearch { get; set; } = new();
        
        [JsonPropertyName("sqlite")]
        public SQLite SQLite { get; set; } = new();
       
        [JsonPropertyName("logging")]
        public Logging Logging { get; set; } = new();
    }

    public class Main
    {
        [JsonPropertyName("takeElements")]
        public int TakeElements { get; set; } = 1000;
        
        [JsonPropertyName("timeoutSeconds")]
        public int TimeoutSeconds { get; set; } = 5;
        
        [JsonPropertyName("enableBatchUnload")]
        public bool EnableBatchUnload { get; set; } = true;
        
        [JsonPropertyName("packetSendCount")]
        public int PacketSendCount { get; set; } = 6;
    }

    public class Elasticsearch
    {
        [JsonPropertyName("indexName")]
        public string IndexName { get; set; } = "log1c-";

        [JsonPropertyName("indexFormat")]
        public string IndexFormat { get; set; } = "yyyyMM";

        [JsonPropertyName("uri")]
        public string Uri { get; set; } = "https://ip:9200";

        [JsonPropertyName("login")]
        public string Login { get; set; } = "login";
        
        [JsonPropertyName("password")]
        public string Password { get; set; } = "password";
    }

    public class SQLite
    {
        [JsonPropertyName("logpath")]
        public string LogPath { get; set; } = "";
       
        [JsonPropertyName("dateFrom")]
        public DateTime DateFrom { get; set; } = DateTime.MinValue;
        
        [JsonPropertyName("rowIdFrom")]
        public long RowIdFrom { get; set; } = 0;
    }

    public class Logging
    {
        [JsonPropertyName("__interval")]
        public string IntervalComment { get; set; } = "Infinite - 0, Year - 1, Month - 2, Day - 3, Hour - 4, Minute - 5";
        
        [JsonPropertyName("interval")]
        public int Interval { get; set; } = 3;
        [JsonPropertyName("prefix")]
        public string Prefix { get; set; } = "log-";
        
        [JsonPropertyName("format")] 
        public string Format { get; set; } = "yyyy-MM-dd HH:mm:ss.fff zzz";
        
        [JsonPropertyName("countFiles")]
        public int CountFiles { get; set; } = 10;
    }
}