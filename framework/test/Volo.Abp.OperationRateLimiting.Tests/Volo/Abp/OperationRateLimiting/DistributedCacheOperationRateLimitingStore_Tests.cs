using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.OperationRateLimiting;

public class DistributedCacheOperationRateLimitingStore_Tests : OperationRateLimitingTestBase
{
    private readonly IOperationRateLimitingStore _store;

    public DistributedCacheOperationRateLimitingStore_Tests()
    {
        _store = GetRequiredService<IOperationRateLimitingStore>();
    }

    [Fact]
    public async Task Should_Create_New_Window_On_First_Request()
    {
        var key = $"store-new-{Guid.NewGuid()}";
        var result = await _store.IncrementAsync(key, TimeSpan.FromHours(1), 5);

        result.IsAllowed.ShouldBeTrue();
        result.CurrentCount.ShouldBe(1);
        result.MaxCount.ShouldBe(5);
        result.RetryAfter.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Increment_Within_Window()
    {
        var key = $"store-incr-{Guid.NewGuid()}";

        await _store.IncrementAsync(key, TimeSpan.FromHours(1), 5);
        var result = await _store.IncrementAsync(key, TimeSpan.FromHours(1), 5);

        result.IsAllowed.ShouldBeTrue();
        result.CurrentCount.ShouldBe(2);
    }

    [Fact]
    public async Task Should_Reject_When_MaxCount_Reached()
    {
        var key = $"store-max-{Guid.NewGuid()}";

        await _store.IncrementAsync(key, TimeSpan.FromHours(1), 2);
        await _store.IncrementAsync(key, TimeSpan.FromHours(1), 2);
        var result = await _store.IncrementAsync(key, TimeSpan.FromHours(1), 2);

        result.IsAllowed.ShouldBeFalse();
        result.CurrentCount.ShouldBe(2);
        result.RetryAfter.ShouldNotBeNull();
        result.RetryAfter!.Value.TotalSeconds.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Reset_Counter()
    {
        var key = $"store-reset-{Guid.NewGuid()}";

        await _store.IncrementAsync(key, TimeSpan.FromHours(1), 2);
        await _store.IncrementAsync(key, TimeSpan.FromHours(1), 2);

        // At max now
        var result = await _store.IncrementAsync(key, TimeSpan.FromHours(1), 2);
        result.IsAllowed.ShouldBeFalse();

        // Reset
        await _store.ResetAsync(key);

        // Should be allowed again
        result = await _store.IncrementAsync(key, TimeSpan.FromHours(1), 2);
        result.IsAllowed.ShouldBeTrue();
        result.CurrentCount.ShouldBe(1);
    }

    [Fact]
    public async Task Should_Get_Status_Without_Incrementing()
    {
        var key = $"store-get-{Guid.NewGuid()}";

        await _store.IncrementAsync(key, TimeSpan.FromHours(1), 5);

        var result = await _store.GetAsync(key, TimeSpan.FromHours(1), 5);
        result.IsAllowed.ShouldBeTrue();
        result.CurrentCount.ShouldBe(1);

        // Get again should still be 1 (no increment)
        result = await _store.GetAsync(key, TimeSpan.FromHours(1), 5);
        result.CurrentCount.ShouldBe(1);
    }

    [Fact]
    public async Task Should_Not_Isolate_By_Tenant_At_Store_Level()
    {
        // Tenant isolation is now handled at the rule level (BuildStoreKey),
        // not at the store level. The store treats keys as opaque strings.
        var key = $"store-tenant-{Guid.NewGuid()}";

        await _store.IncrementAsync(key, TimeSpan.FromHours(1), 2);
        await _store.IncrementAsync(key, TimeSpan.FromHours(1), 2);

        var result = await _store.IncrementAsync(key, TimeSpan.FromHours(1), 2);
        result.IsAllowed.ShouldBeFalse();

        // Same key, same counter regardless of tenant context
        result = await _store.GetAsync(key, TimeSpan.FromHours(1), 2);
        result.IsAllowed.ShouldBeFalse();
        result.CurrentCount.ShouldBe(2);
    }

    [Fact]
    public async Task Should_Deny_Immediately_When_MaxCount_Is_Zero_Increment()
    {
        var key = $"store-zero-incr-{Guid.NewGuid()}";
        var result = await _store.IncrementAsync(key, TimeSpan.FromHours(1), 0);

        result.IsAllowed.ShouldBeFalse();
        result.CurrentCount.ShouldBe(0);
        result.MaxCount.ShouldBe(0);
        result.RetryAfter.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Deny_Immediately_When_MaxCount_Is_Zero_Get()
    {
        var key = $"store-zero-get-{Guid.NewGuid()}";
        var result = await _store.GetAsync(key, TimeSpan.FromHours(1), 0);

        result.IsAllowed.ShouldBeFalse();
        result.CurrentCount.ShouldBe(0);
        result.MaxCount.ShouldBe(0);
        result.RetryAfter.ShouldBeNull();
    }
}
