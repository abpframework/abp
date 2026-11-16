using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.DynamicProxy;

public abstract class AbpAsyncEnumerableInterceptionTestBase<TStartupModule> : AbpAsyncIntegratedTest<TStartupModule>, IAsyncLifetime
    where TStartupModule : IAbpModule
{
    protected override Task BeforeAddApplicationAsync(IServiceCollection services)
    {
        services.AddTransient<SimpleAsyncInterceptor>();
        services.AddTransient<SimpleAsyncInterceptor2>();
        services.AddTransient<SimpleResultCacheTestInterceptor>();
        services.AddTransient<AlwaysExceptionAsyncInterceptor>();
        services.AddSingleton<GatedTestInterceptor>();
        services.AddTransient<MultiplyingTestInterceptor>();
        services.AddTransient<RetryOnceTestInterceptor>();
        services.AddTransient<AsyncEnumerableInterceptionTargetClass>();
        services.AddTransient<AsyncEnumerableGatedTestObject>();
        services.AddTransient<AsyncEnumerableGatedScopedTestObject>();
        services.AddTransient<AsyncEnumerableConcurrentTestObject>();
        services.AddTransient<ScopeTestInterceptor>();
        services.AddSingleton<ScopeEventLog>();
        services.AddTransient<EventLoggingScopeInterceptor>();
        services.AddTransient<AsyncEnumerableScopeInterceptorTestObject>();
        services.AddTransient<AsyncEnumerableScopedTestObject>();
        services.AddTransient<AsyncEnumerableValueTaskSourceTestObject>();
        services.AddTransient<AsyncEnumerableThrowingInterceptorTestObject>();
        services.AddTransient<WrongReturnValueTestInterceptor>();
        services.AddTransient<AsyncEnumerableWrongReturnValueTestObject>();
        services.AddTransient<NullReturnValueTestInterceptor>();
        services.AddTransient<AsyncEnumerableNullReturnValueTestObject>();
        services.AddTransient<AsyncEnumerableDecoratedTestObject>();
        services.AddTransient<CachedTestObject>();

        services.OnRegistered(registration =>
        {
            if (typeof(AsyncEnumerableInterceptionTargetClass) == registration.ImplementationType)
            {
                registration.Interceptors.Add<SimpleAsyncInterceptor>();
                registration.Interceptors.Add<SimpleAsyncInterceptor2>();
                registration.Interceptors.Add<RetryOnceTestInterceptor>();
            }

            if (typeof(AsyncEnumerableScopeInterceptorTestObject) == registration.ImplementationType)
            {
                registration.Interceptors.Add<EventLoggingScopeInterceptor>();
            }

            if (typeof(AsyncEnumerableScopedTestObject) == registration.ImplementationType)
            {
                registration.Interceptors.Add<SimpleAsyncInterceptor>();
                registration.Interceptors.Add<ScopeTestInterceptor>();
            }

            if (typeof(AsyncEnumerableValueTaskSourceTestObject) == registration.ImplementationType)
            {
                registration.Interceptors.Add<ScopeTestInterceptor>();
            }

            if (typeof(AsyncEnumerableConcurrentTestObject) == registration.ImplementationType)
            {
                registration.Interceptors.Add<SimpleAsyncInterceptor>();
                registration.Interceptors.Add<SimpleAsyncInterceptor2>();
            }

            if (typeof(AsyncEnumerableGatedTestObject) == registration.ImplementationType)
            {
                registration.Interceptors.Add<GatedTestInterceptor>();
            }

            if (typeof(AsyncEnumerableGatedScopedTestObject) == registration.ImplementationType)
            {
                registration.Interceptors.Add<GatedTestInterceptor>();
                registration.Interceptors.Add<ScopeTestInterceptor>();
            }

            if (typeof(AsyncEnumerableWrongReturnValueTestObject) == registration.ImplementationType)
            {
                registration.Interceptors.Add<WrongReturnValueTestInterceptor>();
            }

            if (typeof(AsyncEnumerableNullReturnValueTestObject) == registration.ImplementationType)
            {
                registration.Interceptors.Add<NullReturnValueTestInterceptor>();
            }

            if (typeof(AsyncEnumerableThrowingInterceptorTestObject) == registration.ImplementationType)
            {
                registration.Interceptors.Add<AlwaysExceptionAsyncInterceptor>();
            }

            if (typeof(AsyncEnumerableDecoratedTestObject) == registration.ImplementationType)
            {
                registration.Interceptors.Add<MultiplyingTestInterceptor>();
            }

            if (typeof(CachedTestObject) == registration.ImplementationType)
            {
                registration.Interceptors.Add<SimpleResultCacheTestInterceptor>();
            }
        });

        return Task.CompletedTask;
    }

    [Fact]
    public async Task Should_Intercept_The_Call_And_Enumerate_Lazily()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableInterceptionTargetClass>();

        var items = await ToListAsync(target.GetItemsAsync(2));

        items.ShouldBe(new[] { 1, 2 });
        target.Logs.ShouldBe(new[]
        {
            "SimpleAsyncInterceptor_InterceptAsync_BeforeInvocation",
            "SimpleAsyncInterceptor2_InterceptAsync_BeforeInvocation",
            "SimpleAsyncInterceptor2_InterceptAsync_AfterInvocation",
            "SimpleAsyncInterceptor_InterceptAsync_AfterInvocation",
            "Produce1",
            "Produce2"
        });
    }

    [Fact]
    public async Task Should_Not_Block_The_Call_While_Interceptor_Is_Awaiting()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableGatedTestObject>();
        var interceptor = ServiceProvider.GetRequiredService<GatedTestInterceptor>();

        try
        {
            // Called on another thread, so a call blocking on the interceptor fails the test instead of hanging it.
            var sequence = await Task.Run(() => target.GetItemsAsync()).WaitAsync(TimeSpan.FromSeconds(10));
            var enumeration = ToListAsync(sequence);
            enumeration.IsCompleted.ShouldBeFalse();

            interceptor.Gate.SetResult(true);
            (await enumeration.WaitAsync(TimeSpan.FromSeconds(10))).ShouldBe(new[] { 1 });
        }
        finally
        {
            interceptor.Gate.TrySetResult(true);
        }
    }

    [Fact]
    public async Task Should_Throw_Synchronous_Interceptor_Exception_When_Enumerating()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableThrowingInterceptorTestObject>();

        var sequence = target.GetItemsAsync();

        await Should.ThrowAsync<AbpException>(ToListAsync(sequence));
        target.Invoked.ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Throw_If_Interceptor_Sets_Wrong_Return_Value_Type()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableWrongReturnValueTestObject>();

        await Should.ThrowAsync<InvalidCastException>(ToListAsync(target.GetItemsAsync()));
    }

    [Fact]
    public async Task Should_Enumerate_Return_Value_Replaced_By_Interceptor()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableDecoratedTestObject>();

        (await ToListAsync(target.GetItemsAsync())).ShouldBe(new[] { 10, 20 });
    }

    [Fact]
    public async Task Should_Enumerate_Return_Value_Of_Short_Circuiting_Interceptor()
    {
        var target = ServiceProvider.GetRequiredService<CachedTestObject>();

        var first = await ToListAsync(target.GetValuesAsync(42));

        // The test cache interceptor caches by method, so a different argument only returns 42 if it short-circuits.
        var second = await ToListAsync(target.GetValuesAsync(1));

        first.ShouldBe(new[] { 42, 43 });
        second.ShouldBe(new[] { 42, 43 });
    }

    [Fact]
    public async Task Should_Allow_Interceptor_To_Proceed_Again()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableInterceptionTargetClass>();

        (await ToListAsync(target.GetItemsCreatedOnSecondAttempt())).ShouldBe(new[] { 1 });
    }

    [Fact]
    public async Task Should_Propagate_Exception_Thrown_After_Items_Were_Delivered()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableInterceptionTargetClass>();
        var items = new List<int>();

        var exception = await Should.ThrowAsync<InvalidOperationException>(async () =>
        {
            await foreach (var item in target.GetOneThenThrowAsync())
            {
                items.Add(item);
            }
        });

        exception.Message.ShouldBe("Failed after first item");
        items.ShouldBe(new[] { 1 });
    }

    [Fact]
    public async Task Should_Pass_Cancellation_To_The_Source()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableInterceptionTargetClass>();
        var items = new List<int>();
        using var cancellationTokenSource = new CancellationTokenSource();

        await Should.ThrowAsync<OperationCanceledException>(async () =>
        {
            await foreach (var item in target.GetOneThenWaitAsync().WithCancellation(cancellationTokenSource.Token))
            {
                items.Add(item);
                cancellationTokenSource.CancelAfter(50);
            }
        }).WaitAsync(TimeSpan.FromSeconds(10));

        items.ShouldBe(new[] { 1 });
    }

    [Fact]
    public async Task Should_Set_Out_Parameters_When_Called()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableInterceptionTargetClass>();

        var sequence = target.GetItemsWithOutParameter(out var value);

        value.ShouldBe(42);
        (await ToListAsync(sequence)).ShouldBe(new[] { 1 });
    }

    [Fact]
    public async Task Should_Support_Concurrent_Enumerations_Of_The_Same_Sequence()
    {
        // A source without shared mutable state, so the test only checks the interception wrapper.
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableConcurrentTestObject>();
        var sequence = target.GetItemsAsync(3);

        var results = await Task.WhenAll(
            Task.Run(() => ToListAsync(sequence)),
            Task.Run(() => ToListAsync(sequence)));

        results[0].ShouldBe(new[] { 1, 2, 3 });
        results[1].ShouldBe(new[] { 1, 2, 3 });
    }

    [Fact]
    public async Task Should_Keep_The_Scope_Of_The_Interceptor_For_Every_Step_Of_The_Enumeration()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableScopedTestObject>();

        var values = new List<string?>();
        await foreach (var value in target.GetScopeValuesAsync(fail: false))
        {
            // The caller is not affected by the scope.
            ScopeTestInterceptor.ScopeValue.Value.ShouldBeNull();
            values.Add(value);
        }

        values.ShouldBe(new[] { "in-scope", "in-scope" });
        target.Logs.ShouldBe(new[] { "Completed:in-scope", "Disposed" });
    }

    [Fact]
    public async Task Should_Keep_The_Scope_When_The_Enumeration_Is_Started_With_A_Suppressed_Flow()
    {
        // No asynchronous interceptor, so the scope begins synchronously while the flow is suppressed.
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableValueTaskSourceTestObject>();
        var enumerator = target.GetScopeValuesAsync(fail: false).GetAsyncEnumerator();

        ValueTask<bool> moveNext;
        using (ExecutionContext.SuppressFlow())
        {
            moveNext = enumerator.MoveNextAsync();

            ExecutionContext.IsFlowSuppressed().ShouldBeTrue();
            ScopeTestInterceptor.ScopeValue.Value.ShouldBeNull();
        }

        var values = new List<string?>();
        while (await moveNext)
        {
            values.Add(enumerator.Current);
            moveNext = enumerator.MoveNextAsync();
        }

        await enumerator.DisposeAsync();

        ExecutionContext.IsFlowSuppressed().ShouldBeFalse();
        ScopeTestInterceptor.ScopeValue.Value.ShouldBeNull();
        values.ShouldBe(new[] { "in-scope", "in-scope" });
        target.Logs.ShouldBe(new[] { "Completed:in-scope", "Disposed" });
    }

    [Fact]
    public async Task Should_Keep_The_Context_Of_The_Caller_When_The_Enumeration_Is_Started_With_A_Suppressed_Flow_Before_The_Interceptors_Complete()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableGatedTestObject>();
        var interceptor = ServiceProvider.GetRequiredService<GatedTestInterceptor>();
        var enumerator = target.GetCallerValueAsync().GetAsyncEnumerator();

        AsyncEnumerableCallerContext.Value.Value = "enumeration";
        ValueTask<bool> moveNext;
        using (ExecutionContext.SuppressFlow())
        {
            moveNext = enumerator.MoveNextAsync();

            ExecutionContext.IsFlowSuppressed().ShouldBeTrue();
        }

        AsyncEnumerableCallerContext.Value.Value = null;
        interceptor.Gate.SetResult(true);

        (await moveNext).ShouldBeTrue();
        enumerator.Current.ShouldBe("enumeration");
        await enumerator.DisposeAsync();
    }

    [Fact]
    public async Task Should_Keep_The_Context_Of_The_Caller_And_The_Scope_When_The_Enumeration_Is_Started_With_A_Suppressed_Flow_Before_The_Interceptors_Complete()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableGatedScopedTestObject>();
        var interceptor = ServiceProvider.GetRequiredService<GatedTestInterceptor>();
        var enumerator = target.GetCallerAndScopeValuesAsync().GetAsyncEnumerator();

        AsyncEnumerableCallerContext.Value.Value = "enumeration";
        ValueTask<bool> moveNext;
        using (ExecutionContext.SuppressFlow())
        {
            moveNext = enumerator.MoveNextAsync();

            ExecutionContext.IsFlowSuppressed().ShouldBeTrue();
        }

        AsyncEnumerableCallerContext.Value.Value = "changed";
        interceptor.Gate.SetResult(true);

        var values = new List<string>();
        while (await moveNext)
        {
            values.Add(enumerator.Current);
            moveNext = enumerator.MoveNextAsync();
        }

        await enumerator.DisposeAsync();

        values.ShouldBe(new[] { "enumeration:in-scope", "enumeration:in-scope" });
        target.Logs.ShouldBe(new[] { "Completed:in-scope", "Disposed" });
        AsyncEnumerableCallerContext.Value.Value.ShouldBe("changed");
        ScopeTestInterceptor.ScopeValue.Value.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Keep_The_Context_Of_The_First_Step_For_Every_Step_Of_The_Enumeration()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableScopedTestObject>();
        var enumerator = target.GetCallerValuesAsync().GetAsyncEnumerator();

        AsyncEnumerableCallerContext.Value.Value = "first";
        (await enumerator.MoveNextAsync()).ShouldBeTrue();
        enumerator.Current.ShouldBe("first");

        AsyncEnumerableCallerContext.Value.Value = "second";
        (await enumerator.MoveNextAsync()).ShouldBeTrue();
        enumerator.Current.ShouldBe("first");

        await enumerator.DisposeAsync();
        AsyncEnumerableCallerContext.Value.Value.ShouldBe("second");
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Should_Keep_The_Scope_When_The_Value_Tasks_Of_The_Source_Complete(bool completeAsynchronously)
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableValueTaskSourceTestObject>();

        var items = new List<int>();
        await foreach (var item in target.GetItemsAsync(completeAsynchronously))
        {
            ScopeTestInterceptor.ScopeValue.Value.ShouldBeNull();
            items.Add(item);
        }

        items.ShouldBe(new[] { 1, 2 });
        target.Logs.ShouldBe(new[]
        {
            "MoveNext:in-scope", "MoveNext:in-scope", "MoveNext:in-scope", "Dispose:in-scope", "Completed:in-scope", "Disposed"
        });
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Should_Keep_The_Scope_When_The_Value_Task_Of_Disposing_The_Source_Completes(bool completeAsynchronously)
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableValueTaskSourceTestObject>();

        await foreach (var _ in target.GetItemsAsync(completeAsynchronously))
        {
            break;
        }

        ScopeTestInterceptor.ScopeValue.Value.ShouldBeNull();
        target.Logs.ShouldBe(new[] { "MoveNext:in-scope", "Dispose:in-scope", "Completed:in-scope", "Disposed" });
    }

    [Fact]
    public async Task Should_Keep_The_Scope_When_The_Value_Tasks_Of_The_Source_Are_Completed_Without_The_Execution_Context()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableValueTaskSourceTestObject>();

        var items = new List<int>();
        await foreach (var item in target.GetItemsCompletedWithoutExecutionContextAsync())
        {
            ScopeTestInterceptor.ScopeValue.Value.ShouldBeNull();
            items.Add(item);
        }

        items.ShouldBe(new[] { 1, 2 });
        target.Logs.ShouldBe(new[]
        {
            "OnCompleted:in-scope", "SetResult:", "MoveNext:in-scope",
            "OnCompleted:in-scope", "SetResult:", "MoveNext:in-scope",
            "OnCompleted:in-scope", "SetResult:", "MoveNext:in-scope",
            "OnCompleted:in-scope", "SetResult:", "Dispose:in-scope",
            "Completed:in-scope", "Disposed"
        });
    }

    [Fact]
    public async Task Should_Fail_And_Dispose_The_Scope_When_The_Sequence_Throws()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableScopedTestObject>();

        await Should.ThrowAsync<InvalidOperationException>(ToListAsync(target.GetScopeValuesAsync(fail: true)));

        target.Logs.ShouldBe(new[] { "Failed:Failed in scope", "Disposed" });
    }

    [Fact]
    public async Task Should_Complete_And_Dispose_The_Scope_When_The_Caller_Stops_Early()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableScopedTestObject>();

        await foreach (var _ in target.GetScopeValuesAsync(fail: true))
        {
            break;
        }

        target.Logs.ShouldBe(new[] { "Completed:in-scope", "Disposed" });
    }

    [Fact]
    public async Task Should_Enumerate_A_Null_Return_Value_As_An_Empty_Sequence()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableNullReturnValueTestObject>();

        (await ToListAsync(target.GetItemsAsync())).ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Not_Run_A_Scope_For_A_Method_The_Scope_Interceptor_Does_Not_Intercept()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableScopeInterceptorTestObject>();
        var log = ServiceProvider.GetRequiredService<ScopeEventLog>();

        (await ToListAsync(target.GetItemsWithoutScopeAsync())).ShouldBe(new[] { 1 });

        log.Events.ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Run_One_Scope_Around_A_Task_Method()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableScopeInterceptorTestObject>();
        var log = ServiceProvider.GetRequiredService<ScopeEventLog>();

        (await target.GetValueAsync()).ShouldBe(42);

        log.Events.ShouldBe(new[] { "Scope1.Begin", "Scope1.Complete", "Scope1.Dispose" });
    }

    [Fact]
    public async Task Should_Run_A_Scope_Around_The_Call_And_A_New_One_Around_Each_Enumeration()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableScopeInterceptorTestObject>();
        var log = ServiceProvider.GetRequiredService<ScopeEventLog>();

        var sequence = target.GetItemsAsync();
        (await ToListAsync(sequence)).ShouldBe(new[] { 1 });
        (await ToListAsync(sequence)).ShouldBe(new[] { 1 });

        log.TargetInvocations.ShouldBe(1);
        log.Events.ShouldBe(new[]
        {
            "Scope1.Begin", "Scope1.Complete", "Scope1.Dispose",
            "Scope2.Begin", "Scope2.Complete", "Scope2.Dispose",
            "Scope3.Begin", "Scope3.Complete", "Scope3.Dispose"
        });
    }

    [Fact]
    public async Task Should_Not_Begin_An_Enumeration_Scope_Until_The_Sequence_Is_Enumerated()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableScopeInterceptorTestObject>();
        var log = ServiceProvider.GetRequiredService<ScopeEventLog>();

        var sequence = target.GetItemsAsync();
        await using (sequence.GetAsyncEnumerator())
        {
        }

        // Only the scope of the call: getting and disposing the enumerator does not start an enumeration.
        log.Events.ShouldBe(new[] { "Scope1.Begin", "Scope1.Complete", "Scope1.Dispose" });
    }

    [Fact]
    public async Task Should_Fail_The_Scope_Of_The_Call_And_Not_Wrap_The_Sequence()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableScopeInterceptorTestObject>();
        var log = ServiceProvider.GetRequiredService<ScopeEventLog>();

        await Should.ThrowAsync<InvalidOperationException>(ToListAsync(target.GetItemsAsync(throwOnCall: true)));

        log.Events.ShouldBe(new[] { "Scope1.Begin", "Scope1.Fail:Failed on call", "Scope1.Dispose" });
    }

    [Fact]
    public async Task Should_Fail_The_Enumeration_Scope_When_The_Source_Throws_On_GetAsyncEnumerator()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableScopeInterceptorTestObject>();
        var log = ServiceProvider.GetRequiredService<ScopeEventLog>();

        await Should.ThrowAsync<InvalidOperationException>(ToListAsync(target.GetItemsThrowingOnGetAsyncEnumerator()));

        log.Events.ShouldBe(new[]
        {
            "Scope1.Begin", "Scope1.Complete", "Scope1.Dispose",
            "Scope2.Begin", "Scope2.Fail:Failed on GetAsyncEnumerator", "Scope2.Dispose"
        });
    }

    [Fact]
    public async Task Should_Fail_The_Enumeration_Scope_After_The_Failed_Source_Has_Been_Disposed()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableScopeInterceptorTestObject>();
        var log = ServiceProvider.GetRequiredService<ScopeEventLog>();

        await Should.ThrowAsync<InvalidOperationException>(ToListAsync(target.GetItemsThrowingOnCurrent()));

        log.Events.ShouldBe(new[]
        {
            "Scope1.Begin", "Scope1.Complete", "Scope1.Dispose",
            "Scope2.Begin", "Source.Dispose", "Scope2.Fail:Failed on Current", "Scope2.Dispose"
        });
    }

    [Fact]
    public async Task Should_Fail_The_Enumeration_Scope_With_The_Exception_Thrown_When_Disposing_The_Failed_Source_Throws()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableScopeInterceptorTestObject>();
        var log = ServiceProvider.GetRequiredService<ScopeEventLog>();

        var exception = await Should.ThrowAsync<InvalidOperationException>(ToListAsync(target.GetItemsThrowingOnCurrent(throwOnDispose: true)));

        exception.Message.ShouldBe("Failed on dispose");
        log.Events.ShouldBe(new[]
        {
            "Scope1.Begin", "Scope1.Complete", "Scope1.Dispose",
            "Scope2.Begin", "Source.Dispose", "Scope2.Fail:Failed on dispose", "Scope2.Dispose"
        });
    }

    [Fact]
    public async Task Should_Dispose_But_Not_Fail_The_Enumeration_Scope_When_Completing_It_Throws()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableScopeInterceptorTestObject>();
        var log = ServiceProvider.GetRequiredService<ScopeEventLog>();

        var exception = await Should.ThrowAsync<InvalidOperationException>(ToListAsync(target.GetItemsWithFailingCompleteAsync()));

        exception.Message.ShouldBe("Complete failed");
        log.Events.ShouldBe(new[]
        {
            "Scope1.Begin", "Scope1.Complete", "Scope1.Dispose",
            "Scope2.Begin", "Scope2.Complete", "Scope2.Dispose"
        });
    }

    [Fact]
    public async Task Should_Dispose_The_Enumeration_Scope_When_Failing_It_Throws()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableScopeInterceptorTestObject>();
        var log = ServiceProvider.GetRequiredService<ScopeEventLog>();

        var exception = await Should.ThrowAsync<InvalidOperationException>(ToListAsync(target.GetItemsWithFailingFailAsync()));

        exception.Message.ShouldBe("Fail failed");
        log.Events.ShouldBe(new[]
        {
            "Scope1.Begin", "Scope1.Complete", "Scope1.Dispose",
            "Scope2.Begin", "Scope2.Fail:Source failed", "Scope2.Dispose"
        });
    }

    [Fact]
    public async Task Should_Fail_The_Enumeration_Scope_When_The_Source_Throws_On_Dispose()
    {
        var target = ServiceProvider.GetRequiredService<AsyncEnumerableScopeInterceptorTestObject>();
        var log = ServiceProvider.GetRequiredService<ScopeEventLog>();

        // Stopping early runs the finally block of the source in DisposeAsync, not in MoveNextAsync.
        await Should.ThrowAsync<InvalidOperationException>(async () =>
        {
            await foreach (var _ in target.GetItemsAsync(throwOnDispose: true))
            {
                break;
            }
        });

        log.Events.ShouldBe(new[]
        {
            "Scope1.Begin", "Scope1.Complete", "Scope1.Dispose",
            "Scope2.Begin", "Scope2.Fail:Failed on dispose", "Scope2.Dispose"
        });
    }

    private static async Task<List<T>> ToListAsync<T>(IAsyncEnumerable<T> source)
    {
        var list = new List<T>();
        await foreach (var item in source)
        {
            list.Add(item);
        }

        return list;
    }
}
