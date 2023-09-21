
namespace RegistrationLog1CToElasticSearch
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly MainConfig _mainConfig;
        private readonly EF.ReaderContext _dbContext;

        internal long _dateFrom;
        internal long _lastId;

        public Worker(ILogger<Worker> logger,
                      MainConfig mainConfig,
                      EF.ReaderContext dbContext)
        {
            _logger = logger;
            _mainConfig = mainConfig;
            _dbContext = dbContext;

            _dateFrom = mainConfig.SQLiteDateFrom.DateToSQLite();
        }

        private async Task GetSendLogs()
        {
            try
            {
                Processing.GetLogs getLogs = new(_logger, _mainConfig, _dbContext);

                List<Models.LogModels.EventLog> eventLogs = await getLogs.GetAsync(_dateFrom, _lastId);

                Processing.ElasticSearch elasticSearch = new(_logger, _mainConfig, _dbContext);

                int num = 0;
                foreach (Models.LogModels.EventLog item in eventLogs)
                {
                    num++;
                    await elasticSearch.SendItemLog(item);

                    _dateFrom = Math.Max(item.Date, _dateFrom);
                    _lastId = item.RowID;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error num: " + ex.ToString());
            }

            _logger.LogInformation($"Updating filter. Date {_dateFrom.DateFromSQLite()} & row ID {_lastId} --");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await GetSendLogs();

                await Task.Delay(TimeSpan.FromSeconds(_mainConfig.MainTimeoutSeconds), stoppingToken);
            }
        }
    }
}