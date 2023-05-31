using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Logging;

namespace Microsoft.Extensions.Logging;

public static class AbpLoggerExtensions
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
                logger.LogDebug(exception, message);
                break;
        }
    }

    public static void LogException(this ILogger logger, Exception ex, LogLevel? level = null)
    {
        var selectedLevel = level ?? ex.GetLogLevel();

        logger.LogWithLevel(selectedLevel, ex.Message, ex);
        LogKnownProperties(logger, ex, selectedLevel);
        LogSelfLogging(logger, ex);
        LogData(logger, ex, selectedLevel);
    }

    private static void LogKnownProperties(ILogger logger, Exception exception, LogLevel logLevel)
    {
        if (exception is IHasErrorCode exceptionWithErrorCode)
        {
            logger.LogWithLevel(logLevel, "Code:" + exceptionWithErrorCode.Code);
        }

        if (exception is IHasErrorDetails exceptionWithErrorDetails)
        {
            logger.LogWithLevel(logLevel, "Details:" + exceptionWithErrorDetails.Details);
        }
    }

    private static void LogData(ILogger logger, Exception exception, LogLevel logLevel)
    {
        if (exception.Data.Count <= 0)
        {
            return;
        }

        var exceptionData = new StringBuilder();
        exceptionData.AppendLine("---------- Exception Data ----------");
        foreach (var key in exception.Data.Keys)
        {
            exceptionData.AppendLine($"{key} = {exception.Data[key]}");
        }

        logger.LogWithLevel(logLevel, exceptionData.ToString());
    }

    private static void LogSelfLogging(ILogger logger, Exception exception)
    {
        var loggingExceptions = new List<IExceptionWithSelfLogging>();

        if (exception is IExceptionWithSelfLogging logging)
        {
            loggingExceptions.Add(logging);
        }
        else if (exception is AggregateException aggException && aggException.InnerException != null)
        {
            if (aggException.InnerException is IExceptionWithSelfLogging selfLogging)
            {
                loggingExceptions.Add(selfLogging);
            }

            foreach (var innerException in aggException.InnerExceptions)
            {
                if (innerException is IExceptionWithSelfLogging withSelfLogging)
                {
                    loggingExceptions.AddIfNotContains(withSelfLogging);
                }
            }
        }

        foreach (var ex in loggingExceptions)
        {
            ex.Log(logger);
        }
    }
}
