using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace RegistrationLog1CToElasticSearch
{
    public class MainConfig
    {
        private const string _configFileName = "config.json";

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

            try
            {
                _config = new ConfigurationBuilder()
                    .SetBasePath(_basePath)
                    .AddJsonFile(_configFileName, false, true)
                    .Build();
            }
            catch (Exception ex)
            {
                SaveConfig(new Models.MainConfig());

                _config = new ConfigurationBuilder()
                    .SetBasePath(_basePath)
                    .AddJsonFile(_configFileName, false, true)
                    .Build();
            }
        }

        #region Propetries

        internal string BasePath { get => _basePath; }

        #endregion

        #region Config data

        private readonly string _prefixMain = "main:";
        internal int MainTakeElements { get => _config.GetValue<int>(_prefixMain + "takeElements"); }
        internal int MainTimeoutSeconds { get => _config.GetValue<int>(_prefixMain + "timeoutSeconds"); }
        internal bool MainEnableBatchUnload { get => _config.GetValue<bool>(_prefixMain + "enableBatchUnload"); }
        internal int MainPacketSendCount { get => _config.GetValue<int>(_prefixMain + "packetSendCount"); }


        private readonly string _prefixElasticsearch = "elasticsearch:";
        internal string ElasticIndexName { get => _config.GetValue<string>(_prefixElasticsearch + "indexName")!; }
        internal string ElasticIndexFormat { get => _config.GetValue<string>(_prefixElasticsearch + "indexFormat")!; }
        internal string Uri { get => _config.GetValue<string>(_prefixElasticsearch + "uri")!; }
        internal string ElasticLogin { get => _config.GetValue<string>(_prefixElasticsearch + "login")!; }
        internal string ElasticPassword { get => _config.GetValue<string>(_prefixElasticsearch + "password")!; }


        private readonly string _prefixSQLite = "sqlite:";
        internal string SQLiteLogPath { get => _config.GetValue<string>(_prefixSQLite + "logpath")!; }
        internal DateTime SQLiteDateFrom { get => _config.GetValue<DateTime>(_prefixSQLite + "dateFrom"); }
        internal long SQLiteRowIdFrom { get => _config.GetValue<long>(_prefixSQLite + "rowIdFrom"); }


        private readonly string _prefixLogging = "logging:";
        internal string LoggingIntervalComment { get => _config.GetValue<string>(_prefixLogging + "__interval")!; } 
        internal int LoggingInterval { get => _config.GetValue<int>(_prefixLogging + "interval"); }
        internal string LoggingPrefix { get => _config.GetValue<string>(_prefixLogging + "prefix")!; }
        internal string LoggingFormat { get => _config.GetValue<string>(_prefixLogging + "format")!; }
        internal int LoggingCountFiles { get => _config.GetValue<int>(_prefixLogging + "countFiles"); }

        #endregion

        internal void UpdateFilterData(DateTime dateTime, long rowId)
        {
            // update
            Models.MainConfig config = GetMainConfig();

            if (config.SQLite.DateFrom == dateTime && config.SQLite.RowIdFrom == rowId)
                return;

            config.SQLite.DateFrom = dateTime;
            config.SQLite.RowIdFrom = rowId;

            SaveConfig(config);
        }

        private void SaveConfig(Models.MainConfig config)
        {
            // converted
            JsonSerializerOptions jsonWriteOptions = new()
            {
                WriteIndented = true
            };
            jsonWriteOptions.Converters.Add(new JsonStringEnumConverter());

            // update file
            File.WriteAllText(
                Path.Combine(_basePath, _configFileName),
                JsonSerializer.Serialize(config, jsonWriteOptions));
        }

        private Models.MainConfig GetMainConfig()
            => _config.Get<Models.MainConfig>()!;

    }
}
