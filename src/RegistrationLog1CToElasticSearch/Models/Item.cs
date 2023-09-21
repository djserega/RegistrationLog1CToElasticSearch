
using System.Text.Json.Serialization;

namespace RegistrationLog1CToElasticSearch.Models
{
    internal class Item
    {
        [JsonPropertyName("timestamp")]
        public DateTime Date { get; set; }
        public string User { get; set; }
        public string UserUuid { get; set; }
        public string Computer { get; set; }
        public string Event { get; set; }
        public string Comment { get; set; }
        public string DataPresentation { get; set; }
        public string Data { get; set; }
        public string Metadata { get; set; }
        public string MetadataUuid { get; set; }
        public long DataType { get; set; }
        public string App { get; set; }
        public string PrimaryPortCode { get; set; }
        public long SecondaryPortCode { get; set; }
        public long WorkServerCode { get; set; }
        public long TransactionID { get; set; }
        public long ConnectID { get; set; }
        public long Session { get; set; }
        public long SessionDataSplitCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public long TransactionStatus { get; set; }
    }
}
