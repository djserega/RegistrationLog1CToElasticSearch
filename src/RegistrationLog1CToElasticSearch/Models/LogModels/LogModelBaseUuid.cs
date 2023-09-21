using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationLog1CToElasticSearch.Models.LogModels
{
    public class LogModelBaseUuid : LogModelBase
    {
        public string Uuid { get; set; }
    }
}
