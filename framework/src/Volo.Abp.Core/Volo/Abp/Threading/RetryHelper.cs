using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Threading;

/// <summary>
/// Runs an action and retries it while it keeps failing, waiting between the attempts.
/// See <see cref="RetryOptions"/> to control the retry count, the delay and which failures are retried.
/// </summary>
public static class RetryHelper
{
    // Task.Delay rejects a longer delay on the netstandard targets.
    private static readonly TimeSpan MaxDelay = TimeSpan.FromMilliseconds(int.MaxValue);

    /// <summary>
    /// Runs the given action and retries it while it throws a retryable exception.
    /// Rethrows the last exception when the retries are exhausted.
    /// </summary>
    public static async Task ExecuteAsync(
        Func<CancellationToken, Task> action,
        RetryOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(action, nameof(action));

        options ??= new RetryOptions();

        // Read once, so that changing a shared instance cannot affect a running execution.
        var maxRetryCount = options.MaxRetryCount;
        var delayFactory = options.DelayFactory;
        var shouldRetryOnException = options.ShouldRetryOnException;
        var onRetry = options.OnRetry;

        Validate(maxRetryCount, delayFactory, shouldRetryOnException);

        var retryCount = 0;

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Exception? caught = null;

            try
            {
                await action(cancellationToken);
                return;
            }
            catch (Exception ex)
            {
                caught = ex;
            }

            var exception = caught!;

            // Asked on every attempt, including the last one, which is not retried.
            var isHandled = shouldRetryOnException(exception);

            if (!isHandled || IsLastRetry(retryCount, maxRetryCount))
            {
                ExceptionDispatchInfo.Capture(exception).Throw();
            }

            retryCount = NextRetryCount(retryCount);

            var delay = GetDelay(delayFactory, retryCount);
            if (onRetry != null)
            {
                await onRetry(new RetryAttempt(retryCount, delay, exception));
            }

            await WaitAsync(delay, cancellationToken);
        }
    }

    /// <summary>
    /// Runs the given action and retries it while it throws a retryable exception or returns a retryable result.
    /// Rethrows the last exception when the retries are exhausted, or returns the result of the last attempt
    /// if it failed because of its result. A retried result is disposed if it is disposable.
    /// </summary>
    public static async Task<TResult> ExecuteAsync<TResult>(
        Func<CancellationToken, Task<TResult>> action,
        RetryOptions<TResult>? options = null,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(action, nameof(action));

        options ??= new RetryOptions<TResult>();

        // Read once, so that changing a shared instance cannot affect a running execution.
        var maxRetryCount = options.MaxRetryCount;
        var delayFactory = options.DelayFactory;
        var shouldRetryOnException = options.ShouldRetryOnException;
        var shouldRetryOnResult = options.ShouldRetryOnResult;
        var onRetry = options.OnRetry;

        Validate(maxRetryCount, delayFactory, shouldRetryOnException);
        Check.NotNull(shouldRetryOnResult, nameof(RetryOptions<TResult>.ShouldRetryOnResult));

        var retryCount = 0;

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Exception? exception = null;
            var result = default(TResult)!;

            // A result the caller never sees must not leak, whatever throws before it is disposed.
            var discardResult = false;
            TimeSpan delay;

            try
            {
                try
                {
                    result = await action(cancellationToken);
                    discardResult = true;

                    if (!shouldRetryOnResult(result))
                    {
                        discardResult = false;
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                // Asked on every attempt, including the last one, which is not retried.
                var isUnhandledException = exception != null && !shouldRetryOnException(exception);

                if (isUnhandledException || IsLastRetry(retryCount, maxRetryCount))
                {
                    if (exception != null)
                    {
                        ExceptionDispatchInfo.Capture(exception).Throw();
                    }

                    discardResult = false;
                    return result;
                }

                retryCount = NextRetryCount(retryCount);

                delay = GetDelay(delayFactory, retryCount);
                if (onRetry != null)
                {
                    await onRetry(new RetryAttempt<TResult>(retryCount, delay, exception, exception == null ? result : default));
                }
            }
            finally
            {
                // After OnRetry and before the wait, as the retried result is reported but not kept.
                if (discardResult)
                {
                    await TryDisposeAsync(result);
                }
            }

            await WaitAsync(delay, cancellationToken);
        }
    }

    private static void Validate(
        int maxRetryCount,
        Func<int, TimeSpan> delayFactory,
        Func<Exception, bool> shouldRetryOnException)
    {
        if (maxRetryCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(RetryOptions.MaxRetryCount), maxRetryCount, "Must not be negative.");
        }

        Check.NotNull(delayFactory, nameof(RetryOptions.DelayFactory));
        Check.NotNull(shouldRetryOnException, nameof(RetryOptions.ShouldRetryOnException));
    }

    // int.MaxValue means "retry forever": the counter stops there instead of overflowing.
    private static bool IsLastRetry(int retryCount, int maxRetryCount)
    {
        return maxRetryCount != int.MaxValue && retryCount >= maxRetryCount;
    }

    private static TimeSpan GetDelay(Func<int, TimeSpan> delayFactory, int retryCount)
    {
        var delay = delayFactory(retryCount);

        // Clamped before it is reported, so that the reported delay is the one that is waited for.
        return delay > MaxDelay ? MaxDelay : delay;
    }

    internal static TimeSpan GetExponentialDelay(int retryCount)
    {
        var seconds = Math.Pow(2, retryCount);

        // Stops growing instead of overflowing when the retry count gets high.
        return seconds >= MaxDelay.TotalSeconds ? MaxDelay : TimeSpan.FromSeconds(seconds);
    }

    private static int NextRetryCount(int retryCount)
    {
        return retryCount == int.MaxValue ? retryCount : retryCount + 1;
    }

    private static async Task TryDisposeAsync<TResult>(TResult result)
    {
        try
        {
            if (result is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync();
            }
            else if (result is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
        catch
        {
            // A result that is being thrown away should not fail the retry.
        }
    }

    private static async Task WaitAsync(TimeSpan delay, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (delay > TimeSpan.Zero)
        {
            await Task.Delay(delay, cancellationToken);
        }
    }
}
