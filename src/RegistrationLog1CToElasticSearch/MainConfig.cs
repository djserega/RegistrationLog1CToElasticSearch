using System.Reflection;

namespace RegistrationLog1CToElasticSearch
{
    public class MainConfig
    {
        private readonly IConfigurationRoot _config;
        private readonly string _basePath;

        public MainConfig()
        {
            string location = (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).Location;

            if (string.IsNullOrEmpty(location))
                location = AppDomain.CurrentDomain.BaseDirectory;

            _basePath = new FileInfo(location).Directory!.FullName;

            if (string.IsNullOrEmpty(_basePath))
                throw new DirectoryNotFoundException("basePath");

            _config = new ConfigurationBuilder()
                    .SetBasePath(_basePath)
                    .AddJsonFile("config.json", false, true)
                    .Build();
        }

        internal string BasePath { get => _basePath; }

        
        private readonly string _prefixMain = "main:";
        internal int MainTakeElements { get => _config.GetValue<int>(_prefixMain + "takeElements"); }
        internal int MainTimeoutSeconds { get => _config.GetValue<int>(_prefixMain + "timeoutSeconds"); }


        private readonly string _prefixElasticsearch = "elasticsearch:";
        internal string ElasticIndexName { get => _config.GetValue<string>(_prefixElasticsearch + "indexName"); }
        internal string ElasticIndexFormat { get => _config.GetValue<string>(_prefixElasticsearch + "indexFormat"); }
        internal string Uri { get => _config.GetValue<string>(_prefixElasticsearch + "uri"); }
        internal string ElasticLogin { get => _config.GetValue<string>(_prefixElasticsearch + "login") ; }
        internal string ElasticPassword { get => _config.GetValue<string>(_prefixElasticsearch + "password") ; }


        private readonly string _prefixSQLite = "sqlite:";
        internal string SQLiteLogPath { get => _config.GetValue<string>(_prefixSQLite + "logpath") ; }
        internal DateTime SQLiteDateFrom { get => _config.GetValue<DateTime>(_prefixSQLite + "dateFrom"); }
    }
}
