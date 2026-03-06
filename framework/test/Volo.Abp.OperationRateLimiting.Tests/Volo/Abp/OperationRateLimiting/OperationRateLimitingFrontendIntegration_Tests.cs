using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Localization;
using Xunit;

namespace Volo.Abp.OperationRateLimiting;

public class OperationRateLimitingFrontendIntegration_Tests : OperationRateLimitingTestBase
{
    private readonly IOperationRateLimitingChecker _checker;
    private readonly IExceptionToErrorInfoConverter _errorInfoConverter;
    private readonly IOperationRateLimitingFormatter _formatter;

    public OperationRateLimitingFrontendIntegration_Tests()
    {
        _checker = GetRequiredService<IOperationRateLimitingChecker>();
        _errorInfoConverter = GetRequiredService<IExceptionToErrorInfoConverter>();
        _formatter = GetRequiredService<IOperationRateLimitingFormatter>();
    }

    [Fact]
    public async Task ErrorInfo_Should_Contain_Localized_Message_En()
    {
        using (CultureHelper.Use("en"))
        {
            var param = $"frontend-en-{Guid.NewGuid()}";
            var context = new OperationRateLimitingContext { Parameter = param };

            await _checker.CheckAsync("TestSimple", context);
            await _checker.CheckAsync("TestSimple", context);
            await _checker.CheckAsync("TestSimple", context);

            var exception = await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
            {
                await _checker.CheckAsync("TestSimple", context);
            });

            var errorInfo = _errorInfoConverter.Convert(exception);

            // The localized message should contain "Operation rate limit exceeded"
            errorInfo.Message.ShouldContain("Operation rate limit exceeded");
            errorInfo.Message.ShouldContain("minute(s)");
        }
    }

    [Fact]
    public async Task ErrorInfo_Should_Contain_Localized_Message_ZhHans()
    {
        using (CultureHelper.Use("zh-Hans"))
        {
            var param = $"frontend-zh-{Guid.NewGuid()}";
            var context = new OperationRateLimitingContext { Parameter = param };

            await _checker.CheckAsync("TestSimple", context);
            await _checker.CheckAsync("TestSimple", context);
            await _checker.CheckAsync("TestSimple", context);

            var exception = await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
            {
                await _checker.CheckAsync("TestSimple", context);
            });

            var errorInfo = _errorInfoConverter.Convert(exception);

            // The localized message should be in Chinese
            errorInfo.Message.ShouldContain("操作频率超出限制");
            errorInfo.Message.ShouldContain("分钟");
        }
    }

    [Fact]
    public async Task ErrorInfo_Should_Include_Structured_Data_For_Frontend()
    {
        var param = $"frontend-data-{Guid.NewGuid()}";
        var context = new OperationRateLimitingContext
        {
            Parameter = param,
            ExtraProperties =
            {
                ["Email"] = "user@example.com"
            }
        };

        await _checker.CheckAsync("TestSimple", context);
        await _checker.CheckAsync("TestSimple", context);
        await _checker.CheckAsync("TestSimple", context);

        var exception = await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
        {
            await _checker.CheckAsync("TestSimple", context);
        });

        var errorInfo = _errorInfoConverter.Convert(exception);

        // Frontend receives error.code
        errorInfo.Code.ShouldBe(AbpOperationRateLimitingErrorCodes.ExceedLimit);

        // Frontend receives error.data for countdown timer and UI display
        exception.Data["PolicyName"].ShouldBe("TestSimple");
        exception.Data["MaxCount"].ShouldBe(3);
        exception.Data["CurrentCount"].ShouldBe(3);
        exception.Data["RemainingCount"].ShouldBe(0);

        // RetryAfterSeconds: frontend uses this for countdown
        var retryAfterSeconds = (int)exception.Data["RetryAfterSeconds"]!;
        retryAfterSeconds.ShouldBeGreaterThan(0);
        retryAfterSeconds.ShouldBeLessThanOrEqualTo(3600); // max 1 hour window

        // RetryAfterMinutes: frontend uses this for display
        var retryAfterMinutes = (int)exception.Data["RetryAfterMinutes"]!;
        retryAfterMinutes.ShouldBeGreaterThan(0);

        // RetryAfter: localized human-readable string
        exception.Data["RetryAfter"].ShouldNotBeNull();
        exception.Data["RetryAfter"].ShouldBeOfType<string>();

        // WindowDurationSeconds: the configured window duration
        var windowDurationSeconds = (int)exception.Data["WindowDurationSeconds"]!;
        windowDurationSeconds.ShouldBe(3600); // 1 hour window

        // WindowDescription: localized human-readable window description (e.g. "1 hour(s)")
        exception.Data["WindowDescription"].ShouldNotBeNull();
        exception.Data["WindowDescription"].ShouldBeOfType<string>();

        // RuleDetails: complete rule information for frontend
        var ruleDetails = exception.Data["RuleDetails"].ShouldBeOfType<List<Dictionary<string, object>>>();
        ruleDetails.Count.ShouldBe(1);
        ruleDetails[0]["RuleName"].ShouldBe("TestSimple:Rule[3600s,3,Parameter]");
        ruleDetails[0]["MaxCount"].ShouldBe(3);
        ruleDetails[0]["IsAllowed"].ShouldBe(false);
        ruleDetails[0]["WindowDurationSeconds"].ShouldBe(3600);
        ((string)ruleDetails[0]["WindowDescription"]).ShouldNotBeNullOrEmpty();
        ((int)ruleDetails[0]["RetryAfterSeconds"]).ShouldBeGreaterThan(0);
        ((string)ruleDetails[0]["RetryAfter"]).ShouldNotBeNullOrEmpty();

        // ExtraProperties passed through
        exception.Data["Email"].ShouldBe("user@example.com");
    }

    [Fact]
    public async Task GetStatusAsync_Should_Provide_Countdown_Data_For_Frontend()
    {
        var param = $"frontend-status-{Guid.NewGuid()}";
        var context = new OperationRateLimitingContext { Parameter = param };

        // Before any requests: frontend can show "3 remaining"
        var status = await _checker.GetStatusAsync("TestSimple", context);
        status.IsAllowed.ShouldBeTrue();
        status.RemainingCount.ShouldBe(3);
        status.MaxCount.ShouldBe(3);
        status.CurrentCount.ShouldBe(0);
        status.RetryAfter.ShouldBeNull();
        status.WindowDuration.ShouldBe(TimeSpan.FromHours(1));

        // After 2 requests: frontend shows "1 remaining"
        await _checker.CheckAsync("TestSimple", context);
        await _checker.CheckAsync("TestSimple", context);

        status = await _checker.GetStatusAsync("TestSimple", context);
        status.IsAllowed.ShouldBeTrue();
        status.RemainingCount.ShouldBe(1);
        status.MaxCount.ShouldBe(3);
        status.CurrentCount.ShouldBe(2);

        // After exhausting limit: frontend shows countdown
        await _checker.CheckAsync("TestSimple", context);

        status = await _checker.GetStatusAsync("TestSimple", context);
        status.IsAllowed.ShouldBeFalse();
        status.RemainingCount.ShouldBe(0);
        status.CurrentCount.ShouldBe(3);
        status.RetryAfter.ShouldNotBeNull();
        status.RetryAfter!.Value.TotalSeconds.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Custom_ErrorCode_Should_Appear_In_ErrorInfo()
    {
        var param = $"frontend-custom-code-{Guid.NewGuid()}";
        var context = new OperationRateLimitingContext { Parameter = param };

        await _checker.CheckAsync("TestCustomErrorCode", context);
        await _checker.CheckAsync("TestCustomErrorCode", context);

        var exception = await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
        {
            await _checker.CheckAsync("TestCustomErrorCode", context);
        });

        var errorInfo = _errorInfoConverter.Convert(exception);

        // Frontend can use error.code to decide which UI to show
        errorInfo.Code.ShouldBe("Test:CustomError");
    }

    [Fact]
    public void RetryAfterFormatter_Should_Format_Seconds()
    {
        using (CultureHelper.Use("en"))
        {
            _formatter.Format(TimeSpan.FromSeconds(30)).ShouldBe("30 second(s)");
        }

        using (CultureHelper.Use("zh-Hans"))
        {
            _formatter.Format(TimeSpan.FromSeconds(30)).ShouldBe("30 秒");
        }
    }

    [Fact]
    public void RetryAfterFormatter_Should_Format_Minutes()
    {
        using (CultureHelper.Use("en"))
        {
            _formatter.Format(TimeSpan.FromMinutes(15)).ShouldBe("15 minute(s)");
        }

        using (CultureHelper.Use("zh-Hans"))
        {
            _formatter.Format(TimeSpan.FromMinutes(15)).ShouldBe("15 分钟");
        }
    }

    [Fact]
    public void RetryAfterFormatter_Should_Format_MinutesAndSeconds()
    {
        using (CultureHelper.Use("en"))
        {
            // 70 seconds = 1 minute and 10 seconds
            _formatter.Format(TimeSpan.FromSeconds(70)).ShouldBe("1 minute(s) and 10 second(s)");
            _formatter.Format(TimeSpan.FromSeconds(90)).ShouldBe("1 minute(s) and 30 second(s)");
        }

        using (CultureHelper.Use("zh-Hans"))
        {
            _formatter.Format(TimeSpan.FromSeconds(70)).ShouldBe("1 分钟 10 秒");
            _formatter.Format(TimeSpan.FromSeconds(90)).ShouldBe("1 分钟 30 秒");
        }
    }

    [Fact]
    public void RetryAfterFormatter_Should_Format_Hours()
    {
        using (CultureHelper.Use("en"))
        {
            _formatter.Format(TimeSpan.FromHours(2)).ShouldBe("2 hour(s)");
        }

        using (CultureHelper.Use("zh-Hans"))
        {
            _formatter.Format(TimeSpan.FromHours(2)).ShouldBe("2 小时");
        }
    }

    [Fact]
    public void RetryAfterFormatter_Should_Format_HoursAndMinutes()
    {
        using (CultureHelper.Use("en"))
        {
            _formatter.Format(TimeSpan.FromMinutes(90)).ShouldBe("1 hour(s) and 30 minute(s)");
        }

        using (CultureHelper.Use("zh-Hans"))
        {
            _formatter.Format(TimeSpan.FromMinutes(90)).ShouldBe("1 小时 30 分钟");
        }
    }

    [Fact]
    public void RetryAfterFormatter_Should_Format_Days()
    {
        using (CultureHelper.Use("en"))
        {
            _formatter.Format(TimeSpan.FromDays(1)).ShouldBe("1 day(s)");
            _formatter.Format(TimeSpan.FromDays(3)).ShouldBe("3 day(s)");
        }

        using (CultureHelper.Use("zh-Hans"))
        {
            _formatter.Format(TimeSpan.FromDays(1)).ShouldBe("1 天");
            _formatter.Format(TimeSpan.FromDays(3)).ShouldBe("3 天");
        }
    }

    [Fact]
    public void RetryAfterFormatter_Should_Format_DaysAndHours()
    {
        using (CultureHelper.Use("en"))
        {
            _formatter.Format(TimeSpan.FromHours(30)).ShouldBe("1 day(s) and 6 hour(s)");
        }

        using (CultureHelper.Use("zh-Hans"))
        {
            _formatter.Format(TimeSpan.FromHours(30)).ShouldBe("1 天 6 小时");
        }
    }

    [Fact]
    public void RetryAfterFormatter_Should_Format_Months()
    {
        using (CultureHelper.Use("en"))
        {
            _formatter.Format(TimeSpan.FromDays(30)).ShouldBe("1 month(s)");
            _formatter.Format(TimeSpan.FromDays(90)).ShouldBe("3 month(s)");
        }

        using (CultureHelper.Use("zh-Hans"))
        {
            _formatter.Format(TimeSpan.FromDays(30)).ShouldBe("1 个月");
            _formatter.Format(TimeSpan.FromDays(90)).ShouldBe("3 个月");
        }
    }

    [Fact]
    public void RetryAfterFormatter_Should_Format_MonthsAndDays()
    {
        using (CultureHelper.Use("en"))
        {
            _formatter.Format(TimeSpan.FromDays(45)).ShouldBe("1 month(s) and 15 day(s)");
        }

        using (CultureHelper.Use("zh-Hans"))
        {
            _formatter.Format(TimeSpan.FromDays(45)).ShouldBe("1 个月 15 天");
        }
    }

    [Fact]
    public void RetryAfterFormatter_Should_Format_Years()
    {
        using (CultureHelper.Use("en"))
        {
            _formatter.Format(TimeSpan.FromDays(365)).ShouldBe("1 year(s)");
            _formatter.Format(TimeSpan.FromDays(730)).ShouldBe("2 year(s)");
        }

        using (CultureHelper.Use("zh-Hans"))
        {
            _formatter.Format(TimeSpan.FromDays(365)).ShouldBe("1 年");
            _formatter.Format(TimeSpan.FromDays(730)).ShouldBe("2 年");
        }
    }

    [Fact]
    public void RetryAfterFormatter_Should_Format_YearsAndMonths()
    {
        using (CultureHelper.Use("en"))
        {
            // 1 year + 60 days = 1 year and 2 months
            _formatter.Format(TimeSpan.FromDays(425)).ShouldBe("1 year(s) and 2 month(s)");
        }

        using (CultureHelper.Use("zh-Hans"))
        {
            _formatter.Format(TimeSpan.FromDays(425)).ShouldBe("1 年 2 个月");
        }
    }

    [Fact]
    public async Task Reset_Should_Allow_Frontend_To_Resume()
    {
        var param = $"frontend-reset-{Guid.NewGuid()}";
        var context = new OperationRateLimitingContext { Parameter = param };

        // Exhaust limit
        await _checker.CheckAsync("TestSimple", context);
        await _checker.CheckAsync("TestSimple", context);
        await _checker.CheckAsync("TestSimple", context);

        // Frontend shows "limit reached"
        var status = await _checker.GetStatusAsync("TestSimple", context);
        status.IsAllowed.ShouldBeFalse();

        // After reset (e.g. CAPTCHA verified), frontend can resume
        await _checker.ResetAsync("TestSimple", context);

        status = await _checker.GetStatusAsync("TestSimple", context);
        status.IsAllowed.ShouldBeTrue();
        status.RemainingCount.ShouldBe(3);
        status.CurrentCount.ShouldBe(0);
        status.RetryAfter.ShouldBeNull();
    }

    [Fact]
    public async Task IsAllowedAsync_Can_Be_Used_For_Frontend_PreCheck()
    {
        var param = $"frontend-precheck-{Guid.NewGuid()}";
        var context = new OperationRateLimitingContext { Parameter = param };

        // Frontend precheck: button should be enabled
        (await _checker.IsAllowedAsync("TestSimple", context)).ShouldBeTrue();

        // Consume all
        await _checker.CheckAsync("TestSimple", context);
        await _checker.CheckAsync("TestSimple", context);
        await _checker.CheckAsync("TestSimple", context);

        // Frontend precheck: button should be disabled
        (await _checker.IsAllowedAsync("TestSimple", context)).ShouldBeFalse();

        // IsAllowedAsync does NOT consume — calling again still returns false, not error
        (await _checker.IsAllowedAsync("TestSimple", context)).ShouldBeFalse();
    }
}
