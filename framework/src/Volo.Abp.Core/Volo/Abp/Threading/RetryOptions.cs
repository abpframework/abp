using System;
using System.Threading.Tasks;

namespace Volo.Abp.Threading;

/// <summary>
/// Options to control how <see cref="RetryHelper"/> retries a failing action.
/// </summary>
public class RetryOptions
{
    /// <summary>
    /// How many times to retry after the initial attempt fails. Default: 3.
    /// Set to <see cref="int.MaxValue"/> to retry forever.
    /// </summary>
    public int MaxRetryCount { get; set; } = 3;

    /// <summary>
    /// Returns how long to wait before the given retry.
    /// Default: 2^retryCount seconds, up to the longest delay that can be waited for.
    /// </summary>
    public Func<int, TimeSpan> DelayFactory { get; set; } = RetryHelper.GetExponentialDelay;

    /// <summary>
    /// Decides if the action should be retried after the given exception.
    /// Default: retries any exception except <see cref="OperationCanceledException"/>.
    /// </summary>
    public Func<Exception, bool> ShouldRetryOnException { get; set; } = exception => exception is not OperationCanceledException;

    /// <summary>
    /// Called once per retry, before waiting.
    /// </summary>
    public Func<RetryAttempt, Task>? OnRetry { get; set; }
}

/// <summary>
/// Options to control how <see cref="RetryHelper"/> retries a failing action that returns a value.
/// The action can also be retried because of the value it returned.
/// </summary>
/// <typeparam name="TResult">Type of the value returned by the retried action.</typeparam>
public class RetryOptions<TResult>
{
    /// <summary>
    /// How many times to retry after the initial attempt fails. Default: 3.
    /// Set to <see cref="int.MaxValue"/> to retry forever.
    /// </summary>
    public int MaxRetryCount { get; set; } = 3;

    /// <summary>
    /// Returns how long to wait before the given retry.
    /// Default: 2^retryCount seconds, up to the longest delay that can be waited for.
    /// </summary>
    public Func<int, TimeSpan> DelayFactory { get; set; } = RetryHelper.GetExponentialDelay;

    /// <summary>
    /// Decides if the action should be retried after the given exception.
    /// Default: retries any exception except <see cref="OperationCanceledException"/>.
    /// </summary>
    public Func<Exception, bool> ShouldRetryOnException { get; set; } = exception => exception is not OperationCanceledException;

    /// <summary>
    /// Decides if the action should be retried because of the value it returned. Default: no result is retried.
    /// </summary>
    public Func<TResult, bool> ShouldRetryOnResult { get; set; } = _ => false;

    /// <summary>
    /// Called once per retry, before the retried result is disposed and before waiting.
    /// </summary>
    public Func<RetryAttempt<TResult>, Task>? OnRetry { get; set; }
}
