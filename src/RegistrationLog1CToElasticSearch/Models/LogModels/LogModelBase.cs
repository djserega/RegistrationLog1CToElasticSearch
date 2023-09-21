using System.ComponentModel.DataAnnotations;

namespace RegistrationLog1CToElasticSearch.Models.LogModels
{
    public interface ILogModelBase
    {
        [Key]
        public int Code { get; set; }
        public string? Name { get; set; }
    }

    public abstract class LogModelBase : ILogModelBase
    {
        [Key]
        public int Code { get; set; }
        public string? Name { get; set; }
    }
}
