using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.AgentTools;

/// <summary>Collects formatted log messages, for "logged" assertions.</summary>
public class ListLogger<T> : ILogger<T>
{
    public ConcurrentQueue<string> Messages { get; } = new();

    /// <summary>The same messages with their level.</summary>
    public ConcurrentQueue<(LogLevel Level, string Message)> Entries { get; } = new();

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var message = formatter(state, exception);
        Messages.Enqueue(message);
        Entries.Enqueue((logLevel, message));
    }
}
