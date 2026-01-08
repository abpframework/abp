using System;
using System.IO;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands.Services;

/// <summary>
/// MCP logger implementation that writes to both file and stderr.
/// - All logs at or above the configured level are written to file
/// - Warning and Error logs are also written to stderr
/// - Log level is controlled via ABP_MCP_LOG_LEVEL environment variable
/// </summary>
public class McpLogger : IMcpLogger, ISingletonDependency
{
    private const long MaxLogFileSizeBytes = 5 * 1024 * 1024; // 5MB
    private const string LogPrefix = "[MCP]";
    
    private readonly object _fileLock = new();
    private readonly McpLogLevel _configuredLogLevel;

    public McpLogger()
    {
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
        if (_configuredLogLevel == McpLogLevel.None)
        {
            return;
        }

        if (level < _configuredLogLevel)
        {
            return;
        }

        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var levelStr = level.ToString().ToUpperInvariant();
        var formattedMessage = $"[{timestamp}][{levelStr}][{source}] {message}";

        // Write to file (all levels at or above configured level)
        WriteToFile(formattedMessage);

        // Write to stderr for Warning and Error levels
        if (level >= McpLogLevel.Warning)
        {
            WriteToStderr(levelStr, message);
        }
    }

    private void WriteToFile(string formattedMessage)
    {
        try
        {
            lock (_fileLock)
            {
                EnsureLogDirectoryExists();
                RotateLogFileIfNeeded();

                File.AppendAllText(
                    CliPaths.McpLog,
                    formattedMessage + Environment.NewLine,
                    Encoding.UTF8);
            }
        }
        catch
        {
            // Silently ignore file write errors to not disrupt MCP operations
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

    private void EnsureLogDirectoryExists()
    {
        var directory = Path.GetDirectoryName(CliPaths.McpLog);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    private void RotateLogFileIfNeeded()
    {
        try
        {
            if (!File.Exists(CliPaths.McpLog))
            {
                return;
            }

            var fileInfo = new FileInfo(CliPaths.McpLog);
            if (fileInfo.Length < MaxLogFileSizeBytes)
            {
                return;
            }

            var backupPath = CliPaths.McpLog + ".1";
            
            // Delete old backup if exists
            if (File.Exists(backupPath))
            {
                File.Delete(backupPath);
            }

            // Rename current log to backup
            File.Move(CliPaths.McpLog, backupPath);
        }
        catch
        {
            // Silently ignore rotation errors
        }
    }

    private static McpLogLevel GetConfiguredLogLevel()
    {
#if DEBUG
        // In development builds, allow full control via environment variable
        var envValue = Environment.GetEnvironmentVariable(CliConsts.McpLogLevelEnvironmentVariable);
        
        if (string.IsNullOrWhiteSpace(envValue))
        {
            return McpLogLevel.Info; // Default level
        }

        return envValue.ToLowerInvariant() switch
        {
            "debug" => McpLogLevel.Debug,
            "info" => McpLogLevel.Info,
            "warning" => McpLogLevel.Warning,
            "error" => McpLogLevel.Error,
            "none" => McpLogLevel.None,
            _ => McpLogLevel.Info
        };
#else
        // In release builds, restrict to Warning or higher (ignore env variable for Debug/Info)
        var envValue = Environment.GetEnvironmentVariable(CliConsts.McpLogLevelEnvironmentVariable);
        
        if (string.IsNullOrWhiteSpace(envValue))
        {
            return McpLogLevel.Info; // Default level
        }

        return envValue.ToLowerInvariant() switch
        {
            "debug" => McpLogLevel.Info,     // Cap Debug to Info
            "info" => McpLogLevel.Info,
            "warning" => McpLogLevel.Warning,
            "error" => McpLogLevel.Error,
            "none" => McpLogLevel.None,
            _ => McpLogLevel.Info
        };
#endif
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
