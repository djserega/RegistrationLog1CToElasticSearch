using Microsoft.EntityFrameworkCore;

namespace RegistrationLog1CToElasticSearch
{
    internal class ConverterLogs
    {
        private readonly ILogger<Worker> _logger;
        private readonly MainConfig _mainConfig;
        private readonly EF.ReaderContext _dbContext;

        public ConverterLogs(ILogger<Worker> logger,
                             MainConfig mainConfig,
                             EF.ReaderContext dbContext)
        {
            _logger = logger;
            _mainConfig = mainConfig;
            _dbContext = dbContext;
        }

        internal Models.Item ConvertEventLog(Models.LogModels.EventLog eventLog)
        {
            DateTime logItemDate = eventLog.Date.DateFromSQLite();
            
            _ = long.TryParse(eventLog.MetadataCodes, out long metadata);

            Models.Item logItem = new()
            {
                Index = _mainConfig.ElasticIndexName + logItemDate.ToString(_mainConfig.ElasticIndexFormat),

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
                MetadataUuid = GetUUID(_dbContext.MetadataCodes, metadata),
                PrimaryPortCode = GetName(_dbContext.PrimaryPortCodes, eventLog.PrimaryPortCode),
                SecondaryPortCode = eventLog.SecondaryPortCode,
                Session = eventLog.Session,
                SessionDataSplitCode = eventLog.SessionDataSplitCode,
                TransactionDate = eventLog.TransactionDate.DateFromSQLite(),
                TransactionID = eventLog.TransactionID,
                TransactionStatus = eventLog.TransactionStatus,
                User = GetName(_dbContext.UserCodes, eventLog.UserCode),
                UserUuid = GetUUID(_dbContext.UserCodes, eventLog.UserCode),
                WorkServerCode = eventLog.WorkServerCode
            };
            logItem.SetUrl();

            return logItem;
        }

        private string GetName<TEntity>(DbSet<TEntity> table, long code) where TEntity : Models.LogModels.LogModelBase
        {
            _logger.LogDebug($"GetName - {table.GetType()} - {code}");

            return table.FirstOrDefault(el => el.Code == code)?.Name ?? string.Empty;
        }
        private string GetUUID<TEntity>(DbSet<TEntity> table, long code) where TEntity : Models.LogModels.LogModelBaseUuid
        {
            _logger.LogDebug($"GetUUID - {table.GetType()} - {code}");

            return table.FirstOrDefault(el => el.Code == code)?.Uuid ?? string.Empty;
        }

        private static string ConvertEventName(string eventName)
        {
            return eventName.Replace("_$", "").Replace("$_", "");
        }
    }
}
