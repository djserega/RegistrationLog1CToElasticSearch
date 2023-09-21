namespace RegistrationLog1CToElasticSearch.Processing
{
    internal class GetLogs
    {
        private readonly ILogger<Worker> _logger;
        private readonly MainConfig _mainConfig;
        private readonly EF.ReaderContext _dbContext;

        public GetLogs(ILogger<Worker> logger,
                       MainConfig mainConfig,
                       EF.ReaderContext dbContext)
        {
            _logger = logger;
            _mainConfig = mainConfig;
            _dbContext = dbContext;
        }

        internal async Task<List<Models.LogModels.EventLog>> GetAsync(long dateFrom, long lastId)
        {
            _logger.LogInformation($"Selecting. Date {dateFrom.DateFromSQLite()} & row ID {lastId} --");

            List<Models.LogModels.EventLog> eventLogs = await _dbContext.GetEventLogListAsync(
                el => el.Date > dateFrom && el.RowID > lastId,
                default,
                _mainConfig.MainTakeElements);

            return eventLogs;
        }
    }
}
