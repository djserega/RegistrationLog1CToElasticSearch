using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
