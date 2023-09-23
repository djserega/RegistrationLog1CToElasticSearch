
using System.Text.Json.Serialization;

namespace RegistrationLog1CToElasticSearch.Models
{
    internal class Item
    {

        [JsonIgnore()]
        internal required string Index { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Date { get; set; }
        public required string User { get; set; }
        public required string UserUuid { get; set; }
        public required string Computer { get; set; }
        public required string Event { get; set; }
        public required string Comment { get; set; }
        public required string DataPresentation { get; set; }
        public required string Data { get; set; }
        public required string Metadata { get; set; }
        public required string MetadataUuid { get; set; }
        public long DataType { get; set; }
        public required string App { get; set; }
        public required string PrimaryPortCode { get; set; }
        public long SecondaryPortCode { get; set; }
        public long WorkServerCode { get; set; }
        public long TransactionID { get; set; }
        public long ConnectID { get; set; }
        public long Session { get; set; }
        public long SessionDataSplitCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public long TransactionStatus { get; set; }
        public string? Url { get; set; }


        internal void SetUrl()
        {
            string newUrl = string.Empty;

            if (!string.IsNullOrEmpty(Metadata)
                && !string.IsNullOrEmpty(Data))
            {
                int positionColon = Data.IndexOf(':');

                if (positionColon > 0)
                {
                    string dataStartsWith = Data[..positionColon];

                    if (int.TryParse(dataStartsWith, out _))
                    {
                        string dataToUrl = Data[(positionColon + 1)..];

                        newUrl = $"e1cib/data/{Metadata}?ref={dataToUrl}";
                    }
                };
            };

            Url = newUrl;
        }
    }
}
