
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
            _lastId = mainConfig.SQLiteRowIdFrom;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await GetSendLogs();

                await Task.Delay(TimeSpan.FromSeconds(_mainConfig.MainTimeoutSeconds), stoppingToken);
            }
        }

        private async Task GetSendLogs()
        {
            try
            {
                Processing.GetLogs getLogs = new(_logger, _mainConfig, _dbContext);

                ConverterLogs converter = new(_logger, _mainConfig, _dbContext);

                if (_mainConfig.MainEnableBatchUnload)
                {
                    await BatchUnloadData(getLogs, converter);
                }
                else
                {
                    await UnloadDataWithOneRow(getLogs, converter);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error num: " + ex.ToString());
            }

            _logger.LogInformation($"Updating filter. Date {_dateFrom.DateFromSQLite()} & row ID {_lastId} --");
        }

        private async Task UnloadDataWithOneRow(Processing.GetLogs getLogs, ConverterLogs converter)
        {
            List<Models.LogModels.EventLog> eventLogs = await getLogs.GetEventLogsAsync(_dateFrom, _lastId);

            Processing.ElasticSearch elasticSearch = new(_logger, _mainConfig);

            foreach (Models.LogModels.EventLog item in eventLogs)
            {
                Models.Item logItem = converter.ConvertEventLog(item);

                await elasticSearch.SendLogAsync(logItem, logItem.Index);

                _dateFrom = Math.Max(item.Date, _dateFrom);
                _lastId = item.RowID;
            }

            SaveNewFilter();
        }

        private async Task BatchUnloadData(Processing.GetLogs getLogs, ConverterLogs converter)
        {
            int countKeys = _mainConfig.MainPacketSendCount;

            Task?[] updatingTask = new Task[countKeys];

            List<Models.LogModels.EventLog> eventLogs;

            int numRunTask = 0;
            do
            {
                numRunTask++;

                eventLogs = await getLogs.GetEventLogsAsync(_dateFrom, _lastId, numRunTask);

                if (eventLogs.Any())
                {
                    IEnumerable<Models.Item> itemsToUpload = ConvertListLogs(eventLogs, converter);

                    UpdateFilter(eventLogs);

                    InitButchUpdatingTask(updatingTask, new List<Models.Item>(itemsToUpload), numRunTask);

                    if (updatingTask.Count(el => el != null) == countKeys)
                    {
                        await Task.WhenAll(updatingTask!);

                        SaveNewFilter();

                        for (int idEndedTask = 0; idEndedTask < countKeys; idEndedTask++)
                        {
                            updatingTask[idEndedTask] = default;
                        }

                        // wait next timeout
                        break;
                    }
                }
                else
                {
                    if (updatingTask.Any(el => el != default))
                    {
                        await Task.WhenAll(updatingTask.Where(el => el != default)!);

                        SaveNewFilter();

                        // wait next timeout
                        break;
                    }

                }

            } while (eventLogs.Any());
        }

        private void InitButchUpdatingTask(Task?[] updatingTask, IEnumerable<Models.Item> itemsToUpload, int numRunTask)
        {
            int idCurrentTask = GetIdEmptyButchTask(updatingTask);

            updatingTask[idCurrentTask] = Task.Run(async () =>
            {
                Processing.ElasticSearch elasticSearch = new(_logger, _mainConfig);
                await elasticSearch.SendLogsAsync(itemsToUpload, itemsToUpload.First().Index, numRunTask);
            });
        }

        private void UpdateFilter(List<Models.LogModels.EventLog> eventLogs)
        {
            eventLogs.ForEach(log =>
            {
                _dateFrom = Math.Max(log.Date, _dateFrom);
                _lastId = log.RowID;
            });
        }

        private void SaveNewFilter()
        {
            _mainConfig.UpdateFilterData(_dateFrom.DateFromSQLite(), _lastId);
        }

        private static IEnumerable<Models.Item> ConvertListLogs(List<Models.LogModels.EventLog> logs, ConverterLogs converter)
        {
            IEnumerable<Models.Item> itemsToUpload = logs.Select(log => converter.ConvertEventLog(log));

            return itemsToUpload;
        }

        private static int GetIdEmptyButchTask(Task?[] tasks)
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                if (tasks[i] == default)
                    return i;
            }
            return default;
        }

    }
}