using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistrationLog1CToElasticSearch.Models.LogModels
{
    [Table("EventLog")]
    public class EventLog
    {
        [Key]
        public long RowID { get; set; }
        public long Severity { get; set; }
        public long Date { get; set; }
        public long ConnectID { get; set; }
        public long Session { get; set; }
        public long TransactionStatus { get; set; }
        public long TransactionDate { get; set; }
        public long TransactionID { get; set; }
        public long UserCode { get; set; }
        public long ComputerCode { get; set; }
        public long AppCode { get; set; }
        public long EventCode { get; set; }
        public string Comment { get; set; }
        public string MetadataCodes { get; set; }
        public long SessionDataSplitCode { get; set; }
        public long DataType { get; set; }
        public string Data { get; set; }
        public string DataPresentation { get; set; }
        public long WorkServerCode { get; set; }
        public long PrimaryPortCode { get; set; }
        public long SecondaryPortCode { get; set; }
    }
}
