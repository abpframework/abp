using System;

namespace Volo.Abp.Cli.Commands.Services;

/// <summary>
/// Logger interface for MCP operations.
/// Writes detailed logs to file and critical messages (Warning/Error) to stderr.
/// Log level is controlled via ABP_MCP_LOG_LEVEL environment variable.
/// </summary>
public interface IMcpLogger
{
    /// <summary>
    /// Logs a debug message. Only written to file when log level is Debug.
    /// </summary>
    void Debug(string source, string message);

    /// <summary>
    /// Logs an informational message. Written to file when log level is Debug or Info.
    /// </summary>
    void Info(string source, string message);

    /// <summary>
    /// Logs a warning message. Written to file and stderr.
    /// </summary>
    void Warning(string source, string message);

    /// <summary>
    /// Logs an error message. Written to file and stderr.
    /// </summary>
    void Error(string source, string message);

    /// <summary>
    /// Logs an error message with exception details. Written to file and stderr.
    /// </summary>
    void Error(string source, string message, Exception exception);
}
