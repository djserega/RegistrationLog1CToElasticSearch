using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationLog1CToElasticSearch
{
    public class FileLogger
    {
        private readonly Logger _logger;

        public FileLogger(MainConfig config)
        {
            _logger = new LoggerConfiguration()
                .WriteTo
                .File(Path.Combine(config.BasePath, config.LoggingPrefix + ".txt"),
                      Serilog.Events.LogEventLevel.Information,
                      "{Timestamp:" + config.LoggingFormat + "} [{Level:u3}] {Message:lj}{NewLine}",
                      rollingInterval: (RollingInterval)Enum.Parse(typeof(RollingInterval), "3"),
                      retainedFileCountLimit: config.LoggingCountFiles)
                .CreateLogger();

            LogInf("Starting app");
        }

        public void LogErr(string message) => _logger.Error(message);
        public void LogInf(string message) => _logger.Information(message);
        public void LogDebug(string message) => _logger.Debug(message);

    }
}
