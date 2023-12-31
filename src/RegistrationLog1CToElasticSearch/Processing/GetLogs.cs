﻿namespace RegistrationLog1CToElasticSearch.Processing
{
    internal class GetLogs
    {
        private readonly FileLogger _logger;
        private readonly MainConfig _mainConfig;
        private readonly EF.ReaderContext _dbContext;

        public GetLogs(FileLogger logger,
                       MainConfig mainConfig,
                       EF.ReaderContext dbContext)
        {
            _logger = logger;
            _mainConfig = mainConfig;
            _dbContext = dbContext;
        }

        internal async Task<List<Models.LogModels.EventLog>> GetEventLogsAsync(long dateFrom, long lastId, int numRunTask = 0)
        {
            _logger.LogInf($"Task: {numRunTask}. Selecting. Date {dateFrom.DateFromSQLite()} & row ID {lastId} --");

            List<Models.LogModels.EventLog> eventLogs = await _dbContext.GetEventLogsAsync(
                el => el.Date > dateFrom && el.RowID > lastId,
                default,
                _mainConfig.MainTakeElements);

            return eventLogs;
        }
    }
}
