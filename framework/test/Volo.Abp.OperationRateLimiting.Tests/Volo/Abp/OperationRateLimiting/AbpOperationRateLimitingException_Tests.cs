using System;
using Shouldly;
using Xunit;

namespace Volo.Abp.OperationRateLimiting;

public class AbpOperationRateLimitingException_Tests
{
    [Fact]
    public void Should_Set_HttpStatusCode_To_429()
    {
        var result = new OperationRateLimitingResult
        {
            IsAllowed = false,
            MaxCount = 3,
            CurrentCount = 3,
            RemainingCount = 0,
            RetryAfter = TimeSpan.FromMinutes(15)
        };

        var exception = new AbpOperationRateLimitingException("TestPolicy", result);

        exception.HttpStatusCode.ShouldBe(429);
    }

    [Fact]
    public void Should_Use_ExceedLimit_Code_When_RetryAfter_Is_Set()
    {
        var result = new OperationRateLimitingResult
        {
            IsAllowed = false,
            MaxCount = 3,
            CurrentCount = 3,
            RemainingCount = 0,
            RetryAfter = TimeSpan.FromMinutes(5)
        };

        var exception = new AbpOperationRateLimitingException("TestPolicy", result);

        exception.Code.ShouldBe(AbpOperationRateLimitingErrorCodes.ExceedLimit);
    }

    [Fact]
    public void Should_Use_ExceedLimitPermanently_Code_When_RetryAfter_Is_Null()
    {
        var result = new OperationRateLimitingResult
        {
            IsAllowed = false,
            MaxCount = 0,
            CurrentCount = 0,
            RemainingCount = 0,
            RetryAfter = null
        };

        var exception = new AbpOperationRateLimitingException("TestPolicy", result);

        exception.Code.ShouldBe(AbpOperationRateLimitingErrorCodes.ExceedLimitPermanently);
    }

    [Fact]
    public void Should_Set_Custom_ErrorCode()
    {
        var result = new OperationRateLimitingResult
        {
            IsAllowed = false,
            MaxCount = 3,
            CurrentCount = 3,
            RemainingCount = 0
        };

        var exception = new AbpOperationRateLimitingException("TestPolicy", result, "App:Custom:Error");

        exception.Code.ShouldBe("App:Custom:Error");
    }

    [Fact]
    public void Should_Include_Data_Properties()
    {
        var result = new OperationRateLimitingResult
        {
            IsAllowed = false,
            MaxCount = 3,
            CurrentCount = 3,
            RemainingCount = 0,
            RetryAfter = TimeSpan.FromMinutes(15),
            WindowDuration = TimeSpan.FromHours(1)
        };

        var exception = new AbpOperationRateLimitingException("TestPolicy", result);

        exception.Data["PolicyName"].ShouldBe("TestPolicy");
        exception.Data["MaxCount"].ShouldBe(3);
        exception.Data["CurrentCount"].ShouldBe(3);
        exception.Data["RemainingCount"].ShouldBe(0);
        exception.Data["RetryAfterSeconds"].ShouldBe(900);
        exception.Data["RetryAfterMinutes"].ShouldBe(15);
        exception.Data["WindowDurationSeconds"].ShouldBe(3600);
    }

    [Fact]
    public void Should_Store_PolicyName_And_Result()
    {
        var result = new OperationRateLimitingResult
        {
            IsAllowed = false,
            MaxCount = 5,
            CurrentCount = 5,
            RemainingCount = 0,
            RetryAfter = TimeSpan.FromHours(1)
        };

        var exception = new AbpOperationRateLimitingException("MyPolicy", result);

        exception.PolicyName.ShouldBe("MyPolicy");
        exception.Result.ShouldBeSameAs(result);
    }
}
