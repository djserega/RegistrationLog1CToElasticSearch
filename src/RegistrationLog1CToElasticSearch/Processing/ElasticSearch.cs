using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.EntityFrameworkCore;

namespace RegistrationLog1CToElasticSearch.Processing
{
    public class ElasticSearch
    {
        private readonly ILogger<Worker> _logger;
        private readonly MainConfig _mainConfig;
        private readonly ElasticsearchClient _client;

        private readonly EF.ReaderContext _dbContext;


        public ElasticSearch(ILogger<Worker> logger,
                             MainConfig mainConfig,
                             EF.ReaderContext dbContext)
        {
            _logger = logger;
            _mainConfig = mainConfig;
            _dbContext = dbContext;

            ElasticsearchClientSettings settings = new ElasticsearchClientSettings(new Uri(_mainConfig.Uri))
                .Authentication(new BasicAuthentication(_mainConfig.ElasticLogin, _mainConfig.ElasticPassword))
                .ServerCertificateValidationCallback((a, b, c, d) => { return true; })
                .DisableDirectStreaming()
                .RequestTimeout(TimeSpan.FromMinutes(1));

            _client = new ElasticsearchClient(settings);

        }

        internal async Task SendItemLog(Models.LogModels.EventLog eventLog)
        {
            DateTime logItemDate = eventLog.Date.DateFromSQLite();

            Models.Item logItem = ConvertEventLog(eventLog, logItemDate);

            string currentIndex = _mainConfig.ElasticIndexName + logItemDate.ToString(_mainConfig.ElasticIndexFormat);

#if DEBUG
            _logger.LogDebug(currentIndex);
            _logger.LogDebug(System.Text.Json.JsonSerializer.Serialize(logItem));
#else
            try
            {
                IndexResponse response = await _client.IndexAsync(logItem, currentIndex);

                if (response.IsValidResponse)
                {
                    _logger.LogInformation($"Index document with ID {response.Id} succeeded.");
                }
                else
                {
                    _logger.LogError($"An error occurred while adding the document.\n{response.DebugInformation}");
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
            }
#endif
        }

        private Models.Item ConvertEventLog(Models.LogModels.EventLog eventLog, DateTime logItemDate)
        {
            _ = long.TryParse(eventLog.MetadataCodes, out long metadata);

            Models.Item logItem = new()
            {
                Date = logItemDate,
                App = GetName(_dbContext.AppCodes, eventLog.AppCode),
                Comment = eventLog.Comment,
                Computer = GetName(_dbContext.ComputerCodes, eventLog.ComputerCode),
                ConnectID = eventLog.ConnectID,
                Data = eventLog.Data,
                DataPresentation = eventLog.DataPresentation,
                DataType = eventLog.DataType,
                Event = ConvertEventName(GetName(_dbContext.EventCodes, eventLog.EventCode)),
                Metadata = GetName(_dbContext.MetadataCodes, metadata),
                MetadataUuid = GetUuid(_dbContext.MetadataCodes, metadata),
                PrimaryPortCode = GetName(_dbContext.PrimaryPortCodes, eventLog.PrimaryPortCode),
                SecondaryPortCode = eventLog.SecondaryPortCode,
                Session = eventLog.Session,
                SessionDataSplitCode = eventLog.SessionDataSplitCode,
                TransactionDate = eventLog.TransactionDate.DateFromSQLite(),
                TransactionID = eventLog.TransactionID,
                TransactionStatus = eventLog.TransactionStatus,
                User = GetName(_dbContext.UserCodes, eventLog.UserCode),
                UserUuid = GetUuid(_dbContext.UserCodes, eventLog.UserCode),
                WorkServerCode = eventLog.WorkServerCode
            };

            return logItem;
        }

        private string GetName<TEntity>(DbSet<TEntity> table, long code) where TEntity : Models.LogModels.LogModelBase
        {
            _logger.LogDebug($"GetName - {table.GetType()} - {code}");

            return table.FirstOrDefault(el => el.Code == code)?.Name ?? string.Empty;
        }
        private string GetUuid<TEntity>(DbSet<TEntity> table, long code) where TEntity : Models.LogModels.LogModelBaseUuid
        {
            _logger.LogDebug($"GetUuid - {table.GetType()} - {code}");
       
            return table.FirstOrDefault(el => el.Code == code)?.Uuid ?? string.Empty;
        }

        private static string ConvertEventName(string eventName)
        {
            return eventName.Replace("_$", "").Replace("$_", "");
        }

    }
}
