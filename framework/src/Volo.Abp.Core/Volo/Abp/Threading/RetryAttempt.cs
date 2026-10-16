using System;

namespace Volo.Abp.Threading;

/// <summary>
/// A failed attempt that <see cref="RetryHelper"/> is about to retry.
/// </summary>
public class RetryAttempt
{
    /// <summary>
    /// Number of the retry that is about to be performed, starting from 1.
    /// </summary>
    public int RetryCount { get; }

    /// <summary>
    /// How long to wait before the retry.
    /// </summary>
    public TimeSpan RetryDelay { get; }

    /// <summary>
    /// The exception that caused the retry. Null if the attempt is retried because of its result.
    /// </summary>
    public Exception? Exception { get; }

    public RetryAttempt(int retryCount, TimeSpan retryDelay, Exception? exception)
    {
        RetryCount = retryCount;
        RetryDelay = retryDelay;
        Exception = exception;
    }
}

/// <summary>
/// A failed attempt that <see cref="RetryHelper"/> is about to retry.
/// </summary>
/// <typeparam name="TResult">Type of the value returned by the retried action.</typeparam>
public class RetryAttempt<TResult> : RetryAttempt
{
    /// <summary>
    /// The result that caused the retry. Default if the attempt is retried because of an exception.
    /// </summary>
    public TResult? Result { get; }

    public RetryAttempt(int retryCount, TimeSpan retryDelay, Exception? exception, TResult? result)
        : base(retryCount, retryDelay, exception)
    {
        Result = result;
    }
}
