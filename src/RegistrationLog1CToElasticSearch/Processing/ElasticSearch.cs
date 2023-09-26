using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace RegistrationLog1CToElasticSearch.Processing
{
    public class ElasticSearch
    {
        private readonly FileLogger _logger;
        private readonly MainConfig _mainConfig;
        private readonly ElasticsearchClient _client;

        public ElasticSearch(FileLogger logger,
                             MainConfig mainConfig)
        {
            _logger = logger;
            _mainConfig = mainConfig;

            ElasticsearchClientSettings settings = new ElasticsearchClientSettings(new Uri(_mainConfig.Uri))
                .Authentication(new BasicAuthentication(_mainConfig.ElasticLogin, _mainConfig.ElasticPassword))
                .ServerCertificateValidationCallback((a, b, c, d) => { return true; })
                .DisableDirectStreaming()
                .RequestTimeout(TimeSpan.FromMinutes(1));

            _client = new ElasticsearchClient(settings);

        }

        internal async Task SendLogAsync(Models.Item logItem, string index)
        {
#if DEBUG
            _logger.LogDebug($"{index} {System.Text.Json.JsonSerializer.Serialize(logItem)}");
#else
            try
            {
                IndexResponse response = await _client.IndexAsync(logItem, index);

                if (response.IsValidResponse)
                {
                    _logger.LogInf($"Index document with ID {response.Id} succeeded.");
                }
                else
                {
                    _logger.LogErr($"An error occurred while adding the document.\n{response.DebugInformation}");
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
            }
#endif
        }

        internal async Task SendLogsAsync(IEnumerable<Models.Item> logItems, string index, int numRunTask)
        {
#if DEBUG
            _logger.LogDebug($"{numRunTask} {index} {System.Text.Json.JsonSerializer.Serialize(logItems.First())}");
#else
            try
            {
                BulkResponse response = await _client.IndexManyAsync(logItems, index);

                if (response.IsValidResponse)
                {
                    _logger.LogInf($"Task: {numRunTask}. Documents added - {response.Items.Count}. With errors - {response.ItemsWithErrors.Count()}");
                }
                else
                {
                    _logger.LogErr($"Task: {numRunTask}. An error occurred while adding the documents.\n{response.DebugInformation}");
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
            }
#endif
        }
    }
}
