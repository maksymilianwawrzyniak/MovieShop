using System;
using Microsoft.Extensions.Logging;
using ILogger = Neo4j.Driver.ILogger;

namespace WebApplication.Database
{
    public class DatabaseLogger : ILogger
    {
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        public DatabaseLogger(Microsoft.Extensions.Logging.ILogger logger)
        {
            _logger = logger;
        }
        public void Error(Exception cause, string format, params object[] args)
        {
            _logger.LogError(default, cause, format, args);
        }

        public void Warn(Exception cause, string message, params object[] args)
        {
            _logger.LogWarning(default, cause, message, args);
        }

        public void Info(string message, params object[] args)
        {
            _logger.LogInformation(default, default, message, args);
        }

        public void Debug(string message, params object[] args)
        {
            _logger.LogDebug(default, default, message, args);
        }

        public void Trace(string message, params object[] args)
        {
            _logger.LogTrace(default, default, message, args);
        }

        public bool IsTraceEnabled()
        {
            return _logger.IsEnabled(LogLevel.Trace);
        }

        public bool IsDebugEnabled()
        {
            return _logger.IsEnabled(LogLevel.Debug);
        }
    }
}