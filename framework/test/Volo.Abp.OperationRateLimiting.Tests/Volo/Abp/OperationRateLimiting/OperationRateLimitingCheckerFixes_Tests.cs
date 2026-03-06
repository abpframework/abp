using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.OperationRateLimiting;

/// <summary>
/// Tests for Fix #6: Phase 1 in CheckAsync now checks ALL rules before throwing,
/// so RetryAfter is the maximum across all blocking rules and RuleResults is complete.
/// </summary>
public class OperationRateLimitingCheckerPhase1_Tests : OperationRateLimitingTestBase
{
    private readonly IOperationRateLimitingChecker _checker;

    public OperationRateLimitingCheckerPhase1_Tests()
    {
        _checker = GetRequiredService<IOperationRateLimitingChecker>();
    }

    [Fact]
    public async Task Should_Report_Max_RetryAfter_When_Multiple_Rules_Block()
    {
        // TestCompositeMaxRetryAfter: Rule0 (5-min window, max=1), Rule1 (2-hr window, max=1)
        // Both rules use PartitionByParameter with the same key, so one request exhausts both.
        var param = $"max-retry-{Guid.NewGuid()}";
        var context = new OperationRateLimitingContext { Parameter = param };

        // First request: both rules go from 0 to 1 (exhausted, since maxCount=1)
        await _checker.CheckAsync("TestCompositeMaxRetryAfter", context);

        // Second request: both Rule0 and Rule1 are blocking.
        // Phase 1 checks all rules → RetryAfter must be the larger one (~2 hours).
        var exception = await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
        {
            await _checker.CheckAsync("TestCompositeMaxRetryAfter", context);
        });

        // RetryAfter should be at least 1 hour (i.e., from Rule1's 2-hour window, not Rule0's 5-min window)
        exception.Result.RetryAfter.ShouldNotBeNull();
        exception.Result.RetryAfter!.Value.ShouldBeGreaterThan(TimeSpan.FromHours(1));
    }

    [Fact]
    public async Task Should_Include_All_Rules_In_RuleResults_When_Multiple_Rules_Block()
    {
        var param = $"all-rules-{Guid.NewGuid()}";
        var context = new OperationRateLimitingContext { Parameter = param };

        // Exhaust both rules
        await _checker.CheckAsync("TestCompositeMaxRetryAfter", context);

        var exception = await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
        {
            await _checker.CheckAsync("TestCompositeMaxRetryAfter", context);
        });

        // Both rules must appear in RuleResults (not just the first blocking one)
        exception.Result.RuleResults.ShouldNotBeNull();
        exception.Result.RuleResults!.Count.ShouldBe(2);
        exception.Result.RuleResults[0].IsAllowed.ShouldBeFalse();
        exception.Result.RuleResults[1].IsAllowed.ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Include_Non_Blocking_Rules_In_RuleResults()
    {
        // TestCompositePartialBlock: Rule0 (max=1) blocks, Rule1 (max=100) is still within limit.
        // RuleResults must contain BOTH rules so callers get the full picture.
        var param = $"partial-block-{Guid.NewGuid()}";
        var context = new OperationRateLimitingContext { Parameter = param };

        // Exhaust only Rule0 (max=1)
        await _checker.CheckAsync("TestCompositePartialBlock", context);

        var exception = await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
        {
            await _checker.CheckAsync("TestCompositePartialBlock", context);
        });

        exception.Result.RuleResults.ShouldNotBeNull();
        exception.Result.RuleResults!.Count.ShouldBe(2);

        // Rule0 is blocking
        exception.Result.RuleResults[0].IsAllowed.ShouldBeFalse();
        exception.Result.RuleResults[0].MaxCount.ShouldBe(1);

        // Rule1 is still allowed (only 1/100 used), but is still present in results
        exception.Result.RuleResults[1].IsAllowed.ShouldBeTrue();
        exception.Result.RuleResults[1].MaxCount.ShouldBe(100);
        exception.Result.RuleResults[1].RemainingCount.ShouldBe(99);

        // The overall RetryAfter comes only from the blocking Rule0
        exception.Result.RetryAfter.ShouldNotBeNull();
        exception.Result.RetryAfter!.Value.TotalMinutes.ShouldBeLessThan(61); // ~1 hour from Rule0
    }
}

/// <summary>
/// Tests for Fix #1: Phase 2 in CheckAsync now checks the result of AcquireAsync.
/// Uses a mock store that simulates a concurrent race condition:
/// GetAsync (Phase 1) always reports quota available, but IncrementAsync (Phase 2) returns denied.
/// </summary>
public class OperationRateLimitingCheckerPhase2Race_Tests
    : AbpIntegratedTest<AbpOperationRateLimitingPhase2RaceTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    [Fact]
    public async Task Should_Throw_When_Phase2_Increment_Returns_Denied_Due_To_Race()
    {
        // The mock store always returns IsAllowed=true in GetAsync (Phase 1 passes)
        // but always returns IsAllowed=false in IncrementAsync (simulates concurrent exhaustion).
        // Before Fix #1, CheckAsync would silently succeed. After the fix it must throw.
        var checker = GetRequiredService<IOperationRateLimitingChecker>();
        var context = new OperationRateLimitingContext { Parameter = "race-test" };

        var exception = await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
        {
            await checker.CheckAsync("TestRacePolicy", context);
        });

        exception.PolicyName.ShouldBe("TestRacePolicy");
        exception.Result.IsAllowed.ShouldBeFalse();
        exception.HttpStatusCode.ShouldBe(429);
    }

    [Fact]
    public async Task IsAllowedAsync_Should_Not_Be_Affected_By_Phase2_Fix()
    {
        // IsAllowedAsync is read-only and does not call IncrementAsync,
        // so it should not be affected by the mock store's deny-on-increment behavior.
        var checker = GetRequiredService<IOperationRateLimitingChecker>();
        var context = new OperationRateLimitingContext { Parameter = "is-allowed-race" };

        // Should return true because GetAsync always returns allowed in the mock store
        var allowed = await checker.IsAllowedAsync("TestRacePolicy", context);
        allowed.ShouldBeTrue();
    }
}
