#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using RabbitMQ.Client;
using Shouldly;
using Xunit;

namespace Volo.Abp.RabbitMQ;

public class ChannelPool_Tests
{
    private static readonly TimeSpan RaceTimeout = TimeSpan.FromSeconds(5);

    // ---- Regression tests for issue #25310 / PR #25311 --------------------

    /// <summary>
    /// After a RabbitMQ restart the cached channel becomes closed. Two callers
    /// that both observe the stale poolItem must not deadlock while the pool
    /// replaces it with a fresh channel.
    /// </summary>
    [Fact]
    public async Task AcquireAsync_Should_Not_Hang_When_Channel_Closed_With_Concurrent_Callers()
    {
        var fixture = BuildConnectionPool();
        var channelPool = new TestChannelPool(fixture.Pool);

        using (await channelPool.AcquireAsync("q"))
        {
        }

        fixture.Channel1.IsClosed.Returns(true);
        fixture.Channel1
            .DisposeAsync()
            .Returns(_ => new ValueTask(Task.Delay(300)));

        using var barrier = new Barrier(2);

        var firstCaller = Task.Run(async () =>
        {
            barrier.SignalAndWait(RaceTimeout);
            using var accessor = await channelPool.AcquireAsync("q");
        });

        var secondCaller = Task.Run(async () =>
        {
            barrier.SignalAndWait(RaceTimeout);
            await Task.Delay(50);
            using var accessor = await channelPool.AcquireAsync("q");
        });

        var completed = Task.WhenAll(firstCaller, secondCaller);
        var winner = await Task.WhenAny(completed, Task.Delay(RaceTimeout));

        winner.ShouldBe(
            completed,
            $"AcquireAsync is hanging on a stale poolItem. firstCaller={firstCaller.Status}, secondCaller={secondCaller.Status}");

        await completed;
    }

    /// <summary>
    /// When several callers race through the closed-channel recovery path, only one new
    /// channel must be created and the dictionary must keep exactly the channel that callers
    /// receive — otherwise the replacement leaks on the broker side.
    /// </summary>
    [Fact]
    public async Task AcquireAsync_Should_Create_Only_One_Replacement_Channel_When_Many_Callers_Race()
    {
        var fixture = BuildConnectionPool();
        var channelPool = new TestChannelPool(fixture.Pool);

        using (await channelPool.AcquireAsync("q"))
        {
        }

        fixture.Channel1.IsClosed.Returns(true);

        const int callerCount = 8;
        using var barrier = new Barrier(callerCount);
        var tasks = new Task<IChannel>[callerCount];
        for (var i = 0; i < callerCount; i++)
        {
            tasks[i] = Task.Run(async () =>
            {
                barrier.SignalAndWait(RaceTimeout);
                using var accessor = await channelPool.AcquireAsync("q");
                return accessor.Channel;
            });
        }

        var channels = await Task.WhenAll(tasks);

        foreach (var channel in channels)
        {
            channel.ShouldBe(fixture.Channel2, "every caller must receive the single replacement channel");
        }

        fixture.CreateChannelCalls.ShouldBe(
            2,
            $"exactly one initial + one replacement channel should be created, but got {fixture.CreateChannelCalls}");
    }

    // ---- Behavior guard-rails: make sure the refactor preserves core semantics ----

    [Fact]
    public async Task AcquireAsync_Should_Return_Channel_From_ConnectionPool()
    {
        var fixture = BuildConnectionPool();
        var channelPool = new TestChannelPool(fixture.Pool);

        using var accessor = await channelPool.AcquireAsync("q");

        accessor.Channel.ShouldBe(fixture.Channel1);
        accessor.Name.ShouldBe("q");
        fixture.CreateChannelCalls.ShouldBe(1);
    }

    [Fact]
    public async Task AcquireAsync_Should_Reuse_Cached_Channel_On_Subsequent_Calls()
    {
        var fixture = BuildConnectionPool();
        var channelPool = new TestChannelPool(fixture.Pool);

        using (var first = await channelPool.AcquireAsync("q"))
        {
            first.Channel.ShouldBe(fixture.Channel1);
        }
        using (var second = await channelPool.AcquireAsync("q"))
        {
            second.Channel.ShouldBe(fixture.Channel1);
        }
        using (var third = await channelPool.AcquireAsync("q"))
        {
            third.Channel.ShouldBe(fixture.Channel1);
        }

        fixture.CreateChannelCalls.ShouldBe(1, "a healthy cached channel must be reused");
    }

    [Fact]
    public async Task AcquireAsync_Should_Keep_Separate_PoolItems_For_Different_ChannelNames()
    {
        var fixture = BuildConnectionPool();
        var channelPool = new TestChannelPool(fixture.Pool);

        using var a = await channelPool.AcquireAsync("queue-a");
        using var b = await channelPool.AcquireAsync("queue-b");

        a.Channel.ShouldBe(fixture.Channel1);
        b.Channel.ShouldBe(fixture.Channel2);
        a.Name.ShouldBe("queue-a");
        b.Name.ShouldBe("queue-b");
        fixture.CreateChannelCalls.ShouldBe(2);
    }

    [Fact]
    public async Task AcquireAsync_Should_Serialize_Concurrent_Callers_On_Same_Channel()
    {
        var fixture = BuildConnectionPool();
        var channelPool = new TestChannelPool(fixture.Pool);

        var first = await channelPool.AcquireAsync("q");

        var secondTask = Task.Run(() => channelPool.AcquireAsync("q"));

        await Task.Delay(100);
        secondTask.IsCompleted.ShouldBeFalse("second caller must block while the channel is held");

        first.Dispose();

        var completed = await Task.WhenAny(secondTask, Task.Delay(RaceTimeout));
        completed.ShouldBe(secondTask, "second caller must be unblocked after release");

        var secondAccessor = await secondTask;
        secondAccessor.Channel.ShouldBe(fixture.Channel1, "the cached channel is reused");

        secondAccessor.Dispose();
        fixture.CreateChannelCalls.ShouldBe(1, "channel is reused, never recreated");
    }

    [Fact]
    public async Task AcquireAsync_Should_Not_Rebuild_When_Cached_Channel_Is_Healthy()
    {
        var fixture = BuildConnectionPool();
        var channelPool = new TestChannelPool(fixture.Pool);

        using (await channelPool.AcquireAsync("q"))
        {
        }
        // Channel stays open, so subsequent calls must not go through the rebuild branch.
        using (await channelPool.AcquireAsync("q"))
        {
        }

        await fixture.Channel1.DidNotReceive().DisposeAsync();
        fixture.CreateChannelCalls.ShouldBe(1);
    }

    [Fact]
    public async Task AcquireAsync_Should_Propagate_Exception_When_CreateChannel_Fails()
    {
        var channel = Substitute.For<IChannel>();
        channel.IsClosed.Returns(false);

        var connection = Substitute.For<IConnection>();
        var attempts = 0;
        connection
            .CreateChannelAsync(Arg.Any<CreateChannelOptions?>(), Arg.Any<CancellationToken>())
            .Returns(_ =>
            {
                var n = Interlocked.Increment(ref attempts);
                if (n == 1)
                {
                    throw new InvalidOperationException("broker down");
                }
                return Task.FromResult(channel);
            });

        var connectionPool = Substitute.For<IConnectionPool>();
        connectionPool.GetAsync(Arg.Any<string?>()).Returns(Task.FromResult(connection));

        var channelPool = new TestChannelPool(connectionPool);

        await Should.ThrowAsync<InvalidOperationException>(() => channelPool.AcquireAsync("q"));

        // Subsequent call should be able to succeed (broker came back).
        using var accessor = await channelPool.AcquireAsync("q");
        accessor.Channel.ShouldBe(channel);
        attempts.ShouldBe(2);
    }

    [Fact]
    public async Task AcquireAsync_Should_Dispose_Stale_Channel_Even_When_Recreate_Fails()
    {
        var staleChannel = Substitute.For<IChannel>();
        staleChannel.IsClosed.Returns(false);

        var connection = Substitute.For<IConnection>();
        var attempts = 0;
        connection
            .CreateChannelAsync(Arg.Any<CreateChannelOptions?>(), Arg.Any<CancellationToken>())
            .Returns(_ =>
            {
                var n = Interlocked.Increment(ref attempts);
                if (n == 1)
                {
                    return Task.FromResult(staleChannel);
                }
                throw new InvalidOperationException("broker still down");
            });

        var connectionPool = Substitute.For<IConnectionPool>();
        connectionPool.GetAsync(Arg.Any<string?>()).Returns(Task.FromResult(connection));

        var channelPool = new TestChannelPool(connectionPool);

        using (await channelPool.AcquireAsync("q"))
        {
        }

        staleChannel.IsClosed.Returns(true);

        await Should.ThrowAsync<InvalidOperationException>(() => channelPool.AcquireAsync("q"));

        await staleChannel.Received(1).DisposeAsync();
    }

    [Fact]
    public async Task AcquireAsync_Should_Throw_After_Pool_Disposed()
    {
        var fixture = BuildConnectionPool();
        var channelPool = new TestChannelPool(fixture.Pool);

        await channelPool.DisposeAsync();

        await Should.ThrowAsync<ObjectDisposedException>(() => channelPool.AcquireAsync("q"));
    }

    [Fact]
    public async Task AcquireAsync_Should_Only_Create_One_Channel_Even_When_First_Callers_Race()
    {
        var fixture = BuildConnectionPool();
        var channelPool = new TestChannelPool(fixture.Pool);

        const int callerCount = 8;
        using var barrier = new Barrier(callerCount);
        var tasks = new Task<IChannel>[callerCount];
        for (var i = 0; i < callerCount; i++)
        {
            tasks[i] = Task.Run(async () =>
            {
                barrier.SignalAndWait(RaceTimeout);
                using var accessor = await channelPool.AcquireAsync("q");
                return accessor.Channel;
            });
        }

        var channels = await Task.WhenAll(tasks);

        foreach (var channel in channels)
        {
            channel.ShouldBe(fixture.Channel1, "the first-creation semaphore must serialize initial creation");
        }
        fixture.CreateChannelCalls.ShouldBe(1);
    }

    // ---- Fixture ---------------------------------------------------------

    private static ConnectionPoolFixture BuildConnectionPool()
    {
        var channel1 = Substitute.For<IChannel>();
        channel1.IsClosed.Returns(false);

        var channel2 = Substitute.For<IChannel>();
        channel2.IsClosed.Returns(false);

        var fixture = new ConnectionPoolFixture(channel1, channel2);

        var connection = Substitute.For<IConnection>();
        connection
            .CreateChannelAsync(Arg.Any<CreateChannelOptions?>(), Arg.Any<CancellationToken>())
            .Returns(_ =>
            {
                var n = Interlocked.Increment(ref fixture.CreateChannelCallsField);
                return n == 1 ? Task.FromResult(channel1) : Task.FromResult(channel2);
            });

        var connectionPool = Substitute.For<IConnectionPool>();
        connectionPool.GetAsync(Arg.Any<string?>()).Returns(Task.FromResult(connection));

        fixture.Pool = connectionPool;
        return fixture;
    }

    private sealed class ConnectionPoolFixture
    {
        public IConnectionPool Pool { get; set; } = default!;
        public IChannel Channel1 { get; }
        public IChannel Channel2 { get; }
        public int CreateChannelCallsField;
        public int CreateChannelCalls => CreateChannelCallsField;

        public ConnectionPoolFixture(IChannel channel1, IChannel channel2)
        {
            Channel1 = channel1;
            Channel2 = channel2;
        }
    }

    private sealed class TestChannelPool : ChannelPool
    {
        public TestChannelPool(IConnectionPool connectionPool) : base(connectionPool)
        {
        }
    }
}
