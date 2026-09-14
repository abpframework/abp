using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Threading;

public class RetryHelper_Tests
{
    private static RetryOptions NoDelay(
        int maxRetryCount = 3,
        Func<Exception, bool>? shouldRetryOnException = null,
        Func<RetryAttempt, Task>? onRetry = null)
    {
        var options = new RetryOptions
        {
            MaxRetryCount = maxRetryCount,
            DelayFactory = _ => TimeSpan.Zero,
            OnRetry = onRetry
        };

        if (shouldRetryOnException != null)
        {
            options.ShouldRetryOnException = shouldRetryOnException;
        }

        return options;
    }

    private static RetryOptions<TResult> NoDelay<TResult>(
        int maxRetryCount = 3,
        Func<TResult, bool>? shouldRetryOnResult = null,
        Func<Exception, bool>? shouldRetryOnException = null,
        Func<RetryAttempt<TResult>, Task>? onRetry = null)
    {
        var options = new RetryOptions<TResult>
        {
            MaxRetryCount = maxRetryCount,
            DelayFactory = _ => TimeSpan.Zero,
            OnRetry = onRetry
        };

        if (shouldRetryOnResult != null)
        {
            options.ShouldRetryOnResult = shouldRetryOnResult;
        }

        if (shouldRetryOnException != null)
        {
            options.ShouldRetryOnException = shouldRetryOnException;
        }

        return options;
    }

    private static async Task ThrowFromHelperAsync()
    {
        await Task.Yield();
        throw new InvalidOperationException("thrown-by-helper");
    }

    #region Defaults

    [Fact]
    public void RetryOptions_Should_Have_Expected_Defaults()
    {
        var options = new RetryOptions();

        options.MaxRetryCount.ShouldBe(3);
        options.OnRetry.ShouldBeNull();
        options.ShouldRetryOnException(new Exception()).ShouldBeTrue();
        options.ShouldRetryOnException(new OperationCanceledException()).ShouldBeFalse();

        options.DelayFactory(1).ShouldBe(TimeSpan.FromSeconds(2));
        options.DelayFactory(2).ShouldBe(TimeSpan.FromSeconds(4));
        options.DelayFactory(3).ShouldBe(TimeSpan.FromSeconds(8));

        options.DelayFactory(21).ShouldBe(TimeSpan.FromSeconds(2097152));
        options.DelayFactory(22).ShouldBe(TimeSpan.FromMilliseconds(int.MaxValue));
        options.DelayFactory(int.MaxValue).ShouldBe(TimeSpan.FromMilliseconds(int.MaxValue));
    }

    [Fact]
    public void RetryOptions_Of_TResult_Should_Have_Expected_Defaults()
    {
        var options = new RetryOptions<int>();

        options.MaxRetryCount.ShouldBe(3);
        options.OnRetry.ShouldBeNull();
        options.ShouldRetryOnException(new Exception()).ShouldBeTrue();
        options.ShouldRetryOnException(new OperationCanceledException()).ShouldBeFalse();
        options.ShouldRetryOnResult(42).ShouldBeFalse();

        options.DelayFactory(1).ShouldBe(TimeSpan.FromSeconds(2));
        options.DelayFactory(2).ShouldBe(TimeSpan.FromSeconds(4));
        options.DelayFactory(3).ShouldBe(TimeSpan.FromSeconds(8));

        options.DelayFactory(21).ShouldBe(TimeSpan.FromSeconds(2097152));
        options.DelayFactory(22).ShouldBe(TimeSpan.FromMilliseconds(int.MaxValue));
        options.DelayFactory(int.MaxValue).ShouldBe(TimeSpan.FromMilliseconds(int.MaxValue));
    }

    #endregion

    #region ExecuteAsync

    [Fact]
    public async Task ExecuteAsync_Should_Not_Retry_When_The_Action_Succeeds()
    {
        var attempts = 0;
        var retries = 0;

        await RetryHelper.ExecuteAsync(
            _ =>
            {
                attempts++;
                return Task.CompletedTask;
            },
            NoDelay(onRetry: _ => { retries++; return Task.CompletedTask; }));

        attempts.ShouldBe(1);
        retries.ShouldBe(0);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Retry_Until_The_Action_Succeeds()
    {
        var attempts = 0;

        await RetryHelper.ExecuteAsync(
            _ =>
            {
                attempts++;
                if (attempts < 3)
                {
                    throw new InvalidOperationException("not yet");
                }

                return Task.CompletedTask;
            },
            NoDelay());

        attempts.ShouldBe(3);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Rethrow_The_Last_Exception_When_The_Retries_Are_Exhausted()
    {
        var attempts = 0;

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await RetryHelper.ExecuteAsync(
                _ =>
                {
                    attempts++;
                    throw new InvalidOperationException($"attempt-{attempts}");
                },
                NoDelay(maxRetryCount: 3)));

        attempts.ShouldBe(4); // the initial attempt + 3 retries
        exception.Message.ShouldBe("attempt-4");
    }

    [Fact]
    public async Task ExecuteAsync_Should_Preserve_The_Stack_Trace_Of_The_Rethrown_Exception()
    {
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await RetryHelper.ExecuteAsync(
                _ => ThrowFromHelperAsync(),
                NoDelay(maxRetryCount: 1)));

        exception.Message.ShouldBe("thrown-by-helper");
        exception.StackTrace.ShouldNotBeNull();
        exception.StackTrace.ShouldContain(nameof(ThrowFromHelperAsync));
    }

    [Fact]
    public async Task ExecuteAsync_Should_Not_Retry_When_ShouldRetryOnException_Returns_False()
    {
        var attempts = 0;
        var retries = 0;

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await RetryHelper.ExecuteAsync(
                _ =>
                {
                    attempts++;
                    throw new InvalidOperationException();
                },
                NoDelay(shouldRetryOnException: _ => false, onRetry: _ => { retries++; return Task.CompletedTask; })));

        attempts.ShouldBe(1);
        retries.ShouldBe(0);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Only_Retry_The_Handled_Exception_Types()
    {
        var attempts = 0;

        await Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await RetryHelper.ExecuteAsync(
                _ =>
                {
                    attempts++;
                    if (attempts < 3)
                    {
                        throw new IOException();
                    }

                    throw new UnauthorizedAccessException();
                },
                NoDelay(maxRetryCount: 10, shouldRetryOnException: ex => ex is IOException)));

        attempts.ShouldBe(3); // 2 retried IOExceptions, then the unhandled one stops it
    }

    [Fact]
    public async Task ExecuteAsync_Should_Not_Retry_When_MaxRetryCount_Is_Zero()
    {
        var attempts = 0;

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await RetryHelper.ExecuteAsync(
                _ =>
                {
                    attempts++;
                    throw new InvalidOperationException();
                },
                NoDelay(maxRetryCount: 0)));

        attempts.ShouldBe(1);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Ask_The_DelayFactory_With_An_Increasing_One_Based_RetryCount()
    {
        var requestedRetryCounts = new List<int>();

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await RetryHelper.ExecuteAsync(
                _ => throw new InvalidOperationException(),
                new RetryOptions
                {
                    MaxRetryCount = 3,
                    DelayFactory = retryCount =>
                    {
                        requestedRetryCounts.Add(retryCount);
                        return TimeSpan.Zero;
                    }
                }));

        requestedRetryCounts.ShouldBe(new[] { 1, 2, 3 });
    }

    [Fact]
    public async Task ExecuteAsync_Should_Report_Every_Retry_To_OnRetry()
    {
        var attempts = 0;
        var reported = new List<RetryAttempt>();

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await RetryHelper.ExecuteAsync(
                _ =>
                {
                    attempts++;
                    throw new InvalidOperationException($"attempt-{attempts}");
                },
                new RetryOptions
                {
                    MaxRetryCount = 2,
                    DelayFactory = retryCount => TimeSpan.FromMilliseconds(retryCount),
                    OnRetry = attempt => { reported.Add(attempt); return Task.CompletedTask; }
                }));

        reported.Count.ShouldBe(2);

        reported[0].RetryCount.ShouldBe(1);
        reported[0].RetryDelay.ShouldBe(TimeSpan.FromMilliseconds(1));
        reported[0].Exception.ShouldNotBeNull();
        reported[0].Exception!.Message.ShouldBe("attempt-1");

        reported[1].RetryCount.ShouldBe(2);
        reported[1].RetryDelay.ShouldBe(TimeSpan.FromMilliseconds(2));
        reported[1].Exception!.Message.ShouldBe("attempt-2");
    }

    [Fact]
    public async Task ExecuteAsync_Should_Pass_The_CancellationToken_To_The_Action()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var received = CancellationToken.None;

        await RetryHelper.ExecuteAsync(
            cancellationToken =>
            {
                received = cancellationToken;
                return Task.CompletedTask;
            },
            NoDelay(),
            cancellationTokenSource.Token);

        received.ShouldBe(cancellationTokenSource.Token);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Stop_Waiting_When_The_CancellationToken_Is_Cancelled()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var attempts = 0;

        // Cancelled while Task.Delay is running, not before it starts.
        cancellationTokenSource.CancelAfter(TimeSpan.FromMilliseconds(50));

        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await RetryHelper.ExecuteAsync(
                _ =>
                {
                    attempts++;
                    throw new InvalidOperationException();
                },
                new RetryOptions
                {
                    MaxRetryCount = 5,
                    DelayFactory = _ => TimeSpan.FromMinutes(1)
                },
                cancellationTokenSource.Token));

        attempts.ShouldBe(1);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Not_Report_A_Retry_When_The_Action_Was_Cancelled()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var attempts = 0;
        var retries = 0;

        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await RetryHelper.ExecuteAsync(
                cancellationToken =>
                {
                    attempts++;
                    cancellationTokenSource.Cancel();
                    cancellationToken.ThrowIfCancellationRequested();
                    return Task.CompletedTask;
                },
                NoDelay(maxRetryCount: 5, onRetry: _ => { retries++; return Task.CompletedTask; }),
                cancellationTokenSource.Token));

        attempts.ShouldBe(1);
        retries.ShouldBe(0);
    }

    [Fact]
    public async Task ExecuteAsync_Of_TResult_Should_Not_Report_A_Retry_When_The_Action_Was_Cancelled()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var attempts = 0;
        var retries = 0;

        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await RetryHelper.ExecuteAsync<int>(
                cancellationToken =>
                {
                    attempts++;
                    cancellationTokenSource.Cancel();
                    cancellationToken.ThrowIfCancellationRequested();
                    return Task.FromResult(1);
                },
                NoDelay<int>(maxRetryCount: 5, onRetry: _ => { retries++; return Task.CompletedTask; }),
                cancellationTokenSource.Token));

        attempts.ShouldBe(1);
        retries.ShouldBe(0);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Retry_A_Cancellation_Exception_When_The_Predicate_Allows_It()
    {
        var attempts = 0;

        await RetryHelper.ExecuteAsync(
            _ =>
            {
                attempts++;
                if (attempts < 3)
                {
                    throw new OperationCanceledException();
                }

                return Task.CompletedTask;
            },
            NoDelay(maxRetryCount: 5, shouldRetryOnException: _ => true));

        attempts.ShouldBe(3);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Await_OnRetry_Before_The_Next_Attempt()
    {
        var attempts = 0;
        var onRetryFinished = 0;
        var onRetryFinishedBeforeNextAttempt = true;

        await RetryHelper.ExecuteAsync(
            async _ =>
            {
                attempts++;
                if (attempts > 1 && onRetryFinished != attempts - 1)
                {
                    onRetryFinishedBeforeNextAttempt = false;
                }

                if (attempts < 3)
                {
                    await Task.Yield();
                    throw new InvalidOperationException();
                }
            },
            NoDelay(
                maxRetryCount: 5,
                onRetry: async _ =>
                {
                    await Task.Delay(10);
                    onRetryFinished++;
                }));

        attempts.ShouldBe(3);
        onRetryFinished.ShouldBe(2);
        onRetryFinishedBeforeNextAttempt.ShouldBeTrue();
    }

    [Fact]
    public async Task ExecuteAsync_Should_Not_Run_The_Action_When_The_CancellationToken_Is_Already_Cancelled()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        var attempts = 0;

        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await RetryHelper.ExecuteAsync(
                _ =>
                {
                    attempts++;
                    return Task.CompletedTask;
                },
                NoDelay(),
                cancellationTokenSource.Token));

        attempts.ShouldBe(0);
    }

    [Fact]
    public async Task ExecuteAsync_Of_TResult_Should_Not_Run_The_Action_When_The_CancellationToken_Is_Already_Cancelled()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        var attempts = 0;

        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await RetryHelper.ExecuteAsync<int>(
                _ =>
                {
                    attempts++;
                    return Task.FromResult(1);
                },
                NoDelay<int>(),
                cancellationTokenSource.Token));

        attempts.ShouldBe(0);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Throw_When_The_DelayFactory_Throws()
    {
        await Assert.ThrowsAsync<OverflowException>(async () =>
            await RetryHelper.ExecuteAsync(
                _ => throw new InvalidOperationException(),
                new RetryOptions
                {
                    MaxRetryCount = 5,
                    DelayFactory = _ => TimeSpan.FromSeconds(double.MaxValue)
                }));

        await Assert.ThrowsAsync<FormatException>(async () =>
            await RetryHelper.ExecuteAsync(
                _ => throw new InvalidOperationException(),
                new RetryOptions
                {
                    MaxRetryCount = 5,
                    DelayFactory = _ => throw new FormatException()
                }));
    }

    [Fact]
    public async Task ExecuteAsync_Should_Report_The_Delay_It_Waits_For()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var reported = TimeSpan.Zero;

        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await RetryHelper.ExecuteAsync(
                _ => throw new InvalidOperationException(),
                new RetryOptions
                {
                    MaxRetryCount = 5,
                    DelayFactory = _ => TimeSpan.FromDays(100),
                    OnRetry = attempt =>
                    {
                        reported = attempt.RetryDelay;
                        cancellationTokenSource.Cancel();
                        return Task.CompletedTask;
                    }
                },
                cancellationTokenSource.Token));

        reported.ShouldBe(TimeSpan.FromMilliseconds(int.MaxValue));
    }

    [Fact]
    public async Task ExecuteAsync_Should_Use_The_Options_As_They_Were_When_It_Started()
    {
        var attempts = 0;
        var options = new RetryOptions
        {
            MaxRetryCount = 2,
            DelayFactory = _ => TimeSpan.Zero
        };

        options.OnRetry = _ =>
        {
            options.MaxRetryCount = 0;
            options.DelayFactory = null!;
            options.ShouldRetryOnException = null!;
            return Task.CompletedTask;
        };

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await RetryHelper.ExecuteAsync(
                _ =>
                {
                    attempts++;
                    throw new InvalidOperationException();
                },
                options));

        attempts.ShouldBe(3);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Not_Wait_For_A_Negative_Delay()
    {
        var attempts = 0;

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await RetryHelper.ExecuteAsync(
                _ =>
                {
                    attempts++;
                    throw new InvalidOperationException();
                },
                new RetryOptions
                {
                    MaxRetryCount = 2,
                    DelayFactory = _ => TimeSpan.FromSeconds(-1)
                }));

        attempts.ShouldBe(3);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Ask_The_Exception_Predicate_On_Every_Attempt()
    {
        var asked = 0;

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await RetryHelper.ExecuteAsync(
                _ => throw new InvalidOperationException(),
                NoDelay(maxRetryCount: 2, shouldRetryOnException: _ => { asked++; return true; })));

        asked.ShouldBe(3); // including the last attempt, which is not retried
    }

    [Fact]
    public async Task ExecuteAsync_Should_Throw_When_MaxRetryCount_Is_Negative()
    {
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            await RetryHelper.ExecuteAsync(_ => Task.CompletedTask, new RetryOptions { MaxRetryCount = -1 }));
    }

    [Fact]
    public async Task ExecuteAsync_Should_Throw_When_A_Delegate_Option_Is_Null()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await RetryHelper.ExecuteAsync(_ => Task.CompletedTask, new RetryOptions { DelayFactory = null! }));

        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await RetryHelper.ExecuteAsync(_ => Task.CompletedTask, new RetryOptions { ShouldRetryOnException = null! }));

        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await RetryHelper.ExecuteAsync<int>(_ => Task.FromResult(1), new RetryOptions<int> { ShouldRetryOnResult = null! }));
    }

    [Fact]
    public async Task ExecuteAsync_Should_Throw_When_The_Action_Is_Null()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await RetryHelper.ExecuteAsync(null!, NoDelay()));
    }

    [Fact]
    public async Task ExecuteAsync_Should_Work_Without_Options()
    {
        var attempts = 0;

        await RetryHelper.ExecuteAsync(_ =>
        {
            attempts++;
            return Task.CompletedTask;
        });

        attempts.ShouldBe(1);
    }

    #endregion

    #region ExecuteAsync<TResult>

    [Fact]
    public async Task ExecuteAsync_Of_TResult_Should_Return_The_Result_Without_Retrying_By_Default()
    {
        var attempts = 0;

        var result = await RetryHelper.ExecuteAsync(
            _ =>
            {
                attempts++;
                return Task.FromResult("ok");
            },
            NoDelay<string>());

        result.ShouldBe("ok");
        attempts.ShouldBe(1);
    }

    [Fact]
    public async Task ExecuteAsync_Of_TResult_Should_Retry_While_The_Result_Is_Not_Accepted()
    {
        var attempts = 0;

        var result = await RetryHelper.ExecuteAsync(
            _ =>
            {
                attempts++;
                return Task.FromResult(attempts);
            },
            NoDelay<int>(maxRetryCount: 5, shouldRetryOnResult: value => value < 3));

        result.ShouldBe(3);
        attempts.ShouldBe(3);
    }

    [Fact]
    public async Task ExecuteAsync_Of_TResult_Should_Return_The_Last_Result_When_The_Retries_Are_Exhausted()
    {
        var attempts = 0;

        var result = await RetryHelper.ExecuteAsync(
            _ =>
            {
                attempts++;
                return Task.FromResult(attempts);
            },
            NoDelay<int>(maxRetryCount: 2, shouldRetryOnResult: _ => true));

        result.ShouldBe(3); // the initial attempt + 2 retries, returned instead of throwing
        attempts.ShouldBe(3);
    }

    [Fact]
    public async Task ExecuteAsync_Of_TResult_Should_Retry_On_Exception_And_Return_The_Result()
    {
        var attempts = 0;

        var result = await RetryHelper.ExecuteAsync(
            _ =>
            {
                attempts++;
                if (attempts < 3)
                {
                    throw new IOException();
                }

                return Task.FromResult("ok");
            },
            NoDelay<string>());

        result.ShouldBe("ok");
        attempts.ShouldBe(3);
    }

    [Fact]
    public async Task ExecuteAsync_Of_TResult_Should_Rethrow_The_Last_Exception_When_The_Retries_Are_Exhausted()
    {
        var attempts = 0;

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await RetryHelper.ExecuteAsync<int>(
                _ =>
                {
                    attempts++;
                    throw new InvalidOperationException($"attempt-{attempts}");
                },
                NoDelay<int>(maxRetryCount: 2)));

        attempts.ShouldBe(3);
        exception.Message.ShouldBe("attempt-3");
    }

    [Fact]
    public async Task ExecuteAsync_Of_TResult_Should_Not_Retry_When_ShouldRetryOnException_Returns_False()
    {
        var attempts = 0;

        await Assert.ThrowsAsync<IOException>(async () =>
            await RetryHelper.ExecuteAsync<int>(
                _ =>
                {
                    attempts++;
                    throw new IOException();
                },
                NoDelay<int>(maxRetryCount: 5, shouldRetryOnException: _ => false)));

        attempts.ShouldBe(1);
    }

    [Fact]
    public async Task ExecuteAsync_Of_TResult_Should_Stop_On_An_Unhandled_Exception_After_Retrying_The_Result()
    {
        var attempts = 0;

        await Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await RetryHelper.ExecuteAsync<int>(
                _ =>
                {
                    attempts++;
                    if (attempts == 1)
                    {
                        return Task.FromResult(0);
                    }

                    throw new UnauthorizedAccessException();
                },
                NoDelay<int>(
                    maxRetryCount: 5,
                    shouldRetryOnResult: value => value == 0,
                    shouldRetryOnException: _ => false)));

        attempts.ShouldBe(2);
    }

    [Fact]
    public async Task ExecuteAsync_Of_TResult_Should_Report_The_Result_To_OnRetry_When_Retried_Because_Of_The_Result()
    {
        var reported = new List<RetryAttempt<string>>();

        var result = await RetryHelper.ExecuteAsync(
            _ => Task.FromResult("bad"),
            NoDelay<string>(maxRetryCount: 1, shouldRetryOnResult: value => value == "bad", onRetry: attempt => { reported.Add(attempt); return Task.CompletedTask; }));

        result.ShouldBe("bad");
        reported.Count.ShouldBe(1);
        reported[0].RetryCount.ShouldBe(1);
        reported[0].Result.ShouldBe("bad");
        reported[0].Exception.ShouldBeNull();
    }

    [Fact]
    public async Task ExecuteAsync_Of_TResult_Should_Report_The_Exception_To_OnRetry_When_Retried_Because_Of_An_Exception()
    {
        var reported = new List<RetryAttempt<string>>();
        var attempts = 0;

        var result = await RetryHelper.ExecuteAsync(
            _ =>
            {
                attempts++;
                if (attempts == 1)
                {
                    throw new IOException("io-failed");
                }

                return Task.FromResult("ok");
            },
            NoDelay<string>(maxRetryCount: 1, onRetry: attempt => { reported.Add(attempt); return Task.CompletedTask; }));

        result.ShouldBe("ok");
        reported.Count.ShouldBe(1);
        reported[0].RetryCount.ShouldBe(1);
        reported[0].Exception.ShouldBeOfType<IOException>();
        reported[0].Result.ShouldBeNull();
    }

    [Fact]
    public async Task ExecuteAsync_Of_TResult_Should_Pass_The_CancellationToken_To_The_Action()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var received = CancellationToken.None;

        await RetryHelper.ExecuteAsync(
            cancellationToken =>
            {
                received = cancellationToken;
                return Task.FromResult(1);
            },
            NoDelay<int>(),
            cancellationTokenSource.Token);

        received.ShouldBe(cancellationTokenSource.Token);
    }

    [Fact]
    public async Task ExecuteAsync_Of_TResult_Should_Throw_When_The_Action_Is_Null()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await RetryHelper.ExecuteAsync<int>(null!, NoDelay<int>()));
    }

    [Fact]
    public async Task ExecuteAsync_Of_TResult_Should_Work_Without_Options()
    {
        var result = await RetryHelper.ExecuteAsync(_ => Task.FromResult("ok"));

        result.ShouldBe("ok");
    }

    #endregion

    #region Disposing and unlimited retries

    [Fact]
    public async Task ExecuteAsync_Of_TResult_Should_Dispose_A_Retried_Result()
    {
        var disposed = new List<int>();
        var attempts = 0;

        var result = await RetryHelper.ExecuteAsync(
            _ =>
            {
                attempts++;
                return Task.FromResult(new DisposableResult(attempts, disposed));
            },
            NoDelay<DisposableResult>(maxRetryCount: 5, shouldRetryOnResult: value => value.Id < 3));

        result.Id.ShouldBe(3);
        disposed.ShouldBe(new[] { 1, 2 }); // the returned one is not disposed
    }

    [Fact]
    public async Task ExecuteAsync_Of_TResult_Should_Not_Dispose_A_Result_That_Is_Not_Retried()
    {
        var disposed = new List<int>();

        var result = await RetryHelper.ExecuteAsync(
            _ => Task.FromResult(new DisposableResult(1, disposed)),
            NoDelay<DisposableResult>());

        result.Id.ShouldBe(1);
        disposed.ShouldBeEmpty();
    }

    [Fact]
    public async Task ExecuteAsync_Of_TResult_Should_Not_Fail_When_Disposing_The_Retried_Result_Throws()
    {
        var attempts = 0;

        var result = await RetryHelper.ExecuteAsync(
            _ =>
            {
                attempts++;
                return Task.FromResult(new ThrowingDisposableResult(attempts));
            },
            NoDelay<ThrowingDisposableResult>(maxRetryCount: 5, shouldRetryOnResult: value => value.Id < 3));

        result.Id.ShouldBe(3);
        attempts.ShouldBe(3);
    }

    [Fact]
    public async Task ExecuteAsync_Of_TResult_Should_Dispose_The_Result_When_ShouldRetryOnResult_Throws()
    {
        var disposed = new List<int>();

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await RetryHelper.ExecuteAsync(
                _ => Task.FromResult(new DisposableResult(1, disposed)),
                NoDelay<DisposableResult>(
                    maxRetryCount: 1,
                    shouldRetryOnResult: _ => throw new InvalidOperationException("predicate failed"),
                    shouldRetryOnException: _ => false)));

        disposed.ShouldBe(new[] { 1 });
    }

    [Fact]
    public async Task ExecuteAsync_Of_TResult_Should_Retry_The_Action_When_ShouldRetryOnResult_Throws()
    {
        var attempts = 0;

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await RetryHelper.ExecuteAsync(
                _ =>
                {
                    attempts++;
                    return Task.FromResult(1);
                },
                NoDelay<int>(
                    maxRetryCount: 2,
                    shouldRetryOnResult: _ => throw new InvalidOperationException("predicate failed"))));

        // A throwing result predicate counts as a failed attempt.
        attempts.ShouldBe(3);
    }

    [Fact]
    public async Task ExecuteAsync_Of_TResult_Should_Prefer_IAsyncDisposable_Over_IDisposable()
    {
        var log = new List<string>();
        var attempts = 0;

        await RetryHelper.ExecuteAsync(
            _ =>
            {
                attempts++;
                return Task.FromResult(new BothDisposableResult(attempts, log));
            },
            NoDelay<BothDisposableResult>(maxRetryCount: 3, shouldRetryOnResult: value => value.Id < 2));

        log.ShouldBe(new[] { "async:1" });
    }

    [Fact]
    public async Task ExecuteAsync_Should_Keep_Retrying_When_MaxRetryCount_Is_The_Maximum()
    {
        var attempts = 0;

        await RetryHelper.ExecuteAsync(
            _ =>
            {
                attempts++;
                if (attempts < 5)
                {
                    throw new InvalidOperationException();
                }

                return Task.CompletedTask;
            },
            NoDelay(maxRetryCount: int.MaxValue));

        attempts.ShouldBe(5);
    }

    private class DisposableResult : IDisposable
    {
        public int Id { get; }

        private readonly List<int> _disposed;

        public DisposableResult(int id, List<int> disposed)
        {
            Id = id;
            _disposed = disposed;
        }

        public void Dispose()
        {
            _disposed.Add(Id);
        }
    }

    private class BothDisposableResult : IDisposable, IAsyncDisposable
    {
        public int Id { get; }

        private readonly List<string> _log;

        public BothDisposableResult(int id, List<string> log)
        {
            Id = id;
            _log = log;
        }

        public void Dispose()
        {
            _log.Add($"sync:{Id}");
        }

        public ValueTask DisposeAsync()
        {
            _log.Add($"async:{Id}");
            return default;
        }
    }

    private class ThrowingDisposableResult : IDisposable
    {
        public int Id { get; }

        public ThrowingDisposableResult(int id)
        {
            Id = id;
        }

        public void Dispose()
        {
            throw new InvalidOperationException("dispose failed");
        }
    }

    #endregion
}
