using System;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Threading;

public class KeyedLock_Tests
{
    [Fact]
    public async Task TryLock_Should_Acquire_Immediately_When_Free()
    {
        var key = "key-try-1";
        var handle = await KeyedLock.TryLockAsync(key);
        handle.ShouldNotBeNull();
        handle!.Dispose();

        var handle2 = await KeyedLock.TryLockAsync(key);
        handle2.ShouldNotBeNull();
        handle2!.Dispose();
    }

    [Fact]
    public async Task TryLock_Should_Return_Null_When_Already_Locked()
    {
        var key = "key-try-2";
        using (await KeyedLock.LockAsync(key))
        {
            var handle2 = await KeyedLock.TryLockAsync(key);
            handle2.ShouldBeNull();
        }

        var handle3 = await KeyedLock.TryLockAsync(key);
        handle3.ShouldNotBeNull();
        handle3!.Dispose();
    }

    [Fact]
    public async Task LockAsync_Should_Block_Until_Released()
    {
        var key = "key-block-1";
        var sw = Stopwatch.StartNew();

        Task inner;
        using (await KeyedLock.LockAsync(key))
        {
            inner = Task.Run(async () =>
            {
                using (await KeyedLock.LockAsync(key))
                {
                    // Acquired only after outer lock is released
                }
            });

            // While holding the outer lock, inner waiter should not complete
            await Task.Delay(200);
            inner.IsCompleted.ShouldBeFalse();
        }

        // After releasing, inner should complete; elapsed >= hold time
        await inner;
        sw.ElapsedMilliseconds.ShouldBeGreaterThanOrEqualTo(180);
    }

    [Fact]
    public async Task TryLock_With_Timeout_Should_Return_Null_When_Busy()
    {
        var key = "key-timeout-1";
        using (await KeyedLock.LockAsync(key))
        {
            var handle = await KeyedLock.TryLockAsync(key, TimeSpan.FromMilliseconds(50));
            handle.ShouldBeNull();
        }
    }

    [Fact]
    public async Task TryLock_With_Timeout_Should_Succeed_If_Released_In_Time()
    {
        var key = "key-timeout-2";
        // Hold the lock manually
        var outer = await KeyedLock.LockAsync(key);
        var tryTask = KeyedLock.TryLockAsync(key, TimeSpan.FromMilliseconds(200));
        await Task.Delay(50);
        // Release within the timeout window
        outer.Dispose();
        var handle2 = await tryTask;
        handle2.ShouldNotBeNull();
        handle2!.Dispose();
    }

    [Fact]
    public async Task LockAsync_With_Cancellation_Should_Rollback_RefCount()
    {
        var key = "key-cancel-1";
        var cts = new CancellationTokenSource();
        await cts.CancelAsync();
        await Should.ThrowAsync<OperationCanceledException>(async () =>
        {
            await KeyedLock.LockAsync(key, cts.Token);
        });

        // After cancellation, we should still be able to acquire the key
        var handle = await KeyedLock.TryLockAsync(key);
        handle.ShouldNotBeNull();
        handle!.Dispose();
    }

    [Fact]
    public async Task TryLock_With_Cancellation_Should_Rollback()
    {
        var key = "key-cancel-2";
        // Ensure it's initially free
        var h0 = await KeyedLock.TryLockAsync(key);
        h0?.Dispose();

        var cts = new CancellationTokenSource();
        await cts.CancelAsync();
        await Should.ThrowAsync<OperationCanceledException>(async () =>
        {
            await KeyedLock.TryLockAsync(key, TimeSpan.FromMilliseconds(200), cts.Token);
        });

        // After cancellation, the key should be acquirable
        var handle = await KeyedLock.TryLockAsync(key);
        handle.ShouldNotBeNull();
        handle!.Dispose();
    }

    [Fact]
    public async Task Serializes_Access_For_Same_Key()
    {
        var key = "key-serial-1";
        int counter = 0;
        var tasks = Enumerable.Range(0, 10).Select(async _ =>
        {
            using (await KeyedLock.LockAsync(key))
            {
                var current = counter;
                await Task.Delay(10);
                counter = current + 1;
            }
        });

        await Task.WhenAll(tasks);
        counter.ShouldBe(10);
    }

    [Fact]
    public async Task Multiple_Keys_Should_Not_Block_Each_Other()
    {
        var key1 = "key-multi-1";
        var key2 = "key-multi-2";

        using (await KeyedLock.LockAsync(key1))
        {
            var handle2 = await KeyedLock.TryLockAsync(key2);
            handle2.ShouldNotBeNull();
            handle2!.Dispose();
        }
    }

    [Fact]
    public async Task TryLock_Default_Overload_Delegates_To_Full_Overload()
    {
        var key = "key-default-1";
        using (await KeyedLock.LockAsync(key))
        {
            var h1 = await KeyedLock.TryLockAsync(key);
            h1.ShouldBeNull();
        }

        var h2 = await KeyedLock.TryLockAsync(key);
        h2.ShouldNotBeNull();
        h2!.Dispose();
    }
}
