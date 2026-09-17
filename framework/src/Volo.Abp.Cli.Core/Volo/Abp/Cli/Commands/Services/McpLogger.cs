using System;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands.Services;

/// <summary>
/// MCP logger implementation that writes to both file (via Serilog) and stderr.
/// - All logs at or above the configured level are written to file via ILogger
/// - Warning and Error logs are also written to stderr
/// - Log level is controlled via ABP_MCP_LOG_LEVEL environment variable
/// </summary>
public class McpLogger : IMcpLogger, ISingletonDependency
{
    private const string LogPrefix = "[MCP]";
    
    private readonly ILogger<McpLogger> _logger;
    private readonly McpLogLevel _configuredLogLevel;

    public McpLogger(ILogger<McpLogger> logger)
    {
        _logger = logger;
        _configuredLogLevel = GetConfiguredLogLevel();
    }

    public void Debug(string source, string message)
    {
        Log(McpLogLevel.Debug, source, message);
    }

    public void Info(string source, string message)
    {
        Log(McpLogLevel.Info, source, message);
    }

    public void Warning(string source, string message)
    {
        Log(McpLogLevel.Warning, source, message);
    }

    public void Error(string source, string message)
    {
        Log(McpLogLevel.Error, source, message);
    }

    public void Error(string source, string message, Exception exception)
    {
#if DEBUG
        var fullMessage = $"{message} | Exception: {exception.GetType().Name}: {exception.Message}";
#else
        var fullMessage = $"{message} | Exception: {exception.GetType().Name}";
#endif
        Log(McpLogLevel.Error, source, fullMessage);
    }

    private void Log(McpLogLevel level, string source, string message)
    {
        if (_configuredLogLevel == McpLogLevel.None || level < _configuredLogLevel)
        {
            return;
        }

        var mcpFormattedMessage = $"{LogPrefix}[{source}] {message}";

        // File logging via Serilog
        switch (level)
        {
            case McpLogLevel.Debug:
                _logger.LogDebug(mcpFormattedMessage);
                break;
            case McpLogLevel.Info:
                _logger.LogInformation(mcpFormattedMessage);
                break;
            case McpLogLevel.Warning:
                _logger.LogWarning(mcpFormattedMessage);
                break;
            case McpLogLevel.Error:
                _logger.LogError(mcpFormattedMessage);
                break;
        }

        // Stderr output for MCP protocol (Warning/Error only)
        if (level >= McpLogLevel.Warning)
        {
            WriteToStderr(level.ToString().ToUpperInvariant(), message);
        }
    }

    private void WriteToStderr(string level, string message)
    {
        try
        {
            // Use synchronous write to avoid async issues in MCP context
            Console.Error.WriteLine($"{LogPrefix}[{level}] {message}");
        }
        catch
        {
            // Silently ignore stderr write errors
        }
    }

    private static McpLogLevel GetConfiguredLogLevel()
    {
        var envValue = Environment.GetEnvironmentVariable(CliConsts.McpLogLevelEnvironmentVariable);
        var isEmpty = string.IsNullOrWhiteSpace(envValue);

#if DEBUG
        // In development builds, allow full control via environment variable
        if (isEmpty)
        {
            return McpLogLevel.Info; // Default level
        }
        
        return ParseLogLevel(envValue, allowDebug: true);
#else
        // In release builds, restrict to Warning or higher (ignore env variable for Debug/Info)
        if (isEmpty)
        {
            return McpLogLevel.Info; // Default level
        }

        return ParseLogLevel(envValue, allowDebug: false);
#endif
    }

    private static McpLogLevel ParseLogLevel(string value, bool allowDebug)
    {
        return value.ToLowerInvariant() switch
        {
            "debug" => allowDebug ? McpLogLevel.Debug : McpLogLevel.Info,
            "info" => McpLogLevel.Info,
            "warning" => McpLogLevel.Warning,
            "error" => McpLogLevel.Error,
            "none" => McpLogLevel.None,
            _ => McpLogLevel.Info
        };
    }
}

/// <summary>
/// Log levels for MCP logging.
/// </summary>
public enum McpLogLevel
{
    Debug = 0,
    Info = 1,
    Warning = 2,
    Error = 3,
    None = 4
}
