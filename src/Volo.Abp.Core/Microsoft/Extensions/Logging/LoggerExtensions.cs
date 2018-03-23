using System;
using System.Collections.Generic;
using Volo.Abp.Logging;

namespace Microsoft.Extensions.Logging
{
    public static class LoggerExtensions
    {
        public static void LogWithLevel(this ILogger logger, LogLevel logLevel, string message)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    logger.LogCritical(message);
                    break;
                case LogLevel.Error:
                    logger.LogError(message);
                    break;
                case LogLevel.Warning:
                    logger.LogWarning(message);
                    break;
                case LogLevel.Information:
                    logger.LogInformation(message);
                    break;
                case LogLevel.Trace:
                    logger.LogTrace(message);
                    break;
                default: // LogLevel.Debug || LogLevel.None
                    logger.LogDebug(message);
                    break;
            }
        }

        public static void LogWithLevel(this ILogger logger, LogLevel logLevel, string message, Exception exception)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    logger.LogCritical(exception, message);
                    break;
                case LogLevel.Error:
                    logger.LogError(exception, message);
                    break;
                case LogLevel.Warning:
                    logger.LogWarning(exception, message);
                    break;
                case LogLevel.Information:
                    logger.LogInformation(exception, message);
                    break;
                case LogLevel.Trace:
                    logger.LogTrace(exception, message);
                    break;
                default: // LogLevel.Debug || LogLevel.None
                    logger.LogDebug(message);
                    break;
            }
        }

        public static void LogException(this ILogger logger, Exception ex, LogLevel? level = null)
        {
            logger.LogWithLevel(
                level ?? (ex as IHasLogLevel)?.LogLevel ?? LogLevel.Error,
                ex.Message,
                ex
            );

            LogDetails(logger, ex);
        }

        private static void LogDetails(ILogger logger, Exception exception)
        {
            var loggingExceptions = new List<IExceptionCanLogDetails>();

            if (exception is IExceptionCanLogDetails)
            {
                loggingExceptions.Add(exception as IExceptionCanLogDetails);
            }
            else if (exception is AggregateException && exception.InnerException != null)
            {
                var aggException = exception as AggregateException;
                if (aggException.InnerException is IExceptionCanLogDetails)
                {
                    loggingExceptions.Add(aggException.InnerException as IExceptionCanLogDetails);
                }

                foreach (var innerException in aggException.InnerExceptions)
                {
                    if (innerException is IExceptionCanLogDetails)
                    {
                        loggingExceptions.AddIfNotContains(innerException as IExceptionCanLogDetails);
                    }
                }
            }

            foreach (var ex in loggingExceptions)
            {
                ex.LogDetails(logger);
            }
        }
    }
}
