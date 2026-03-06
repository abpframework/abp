using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.OperationRateLimiting;

namespace MyCompanyName.MyProjectName.Web.Controllers;

[Route("api/rate-limit-demo")]
public class RateLimitDemoController : AbpController
{
    private readonly IOperationRateLimitingChecker _checker;

    public RateLimitDemoController(IOperationRateLimitingChecker checker)
    {
        _checker = checker;
    }

    /// <summary>
    /// Demo 1: Public - PartitionByParameter (phone number)
    /// </summary>
    [HttpPost("send-sms-code")]
    [AllowAnonymous]
    public async Task<IActionResult> SendSmsCode([FromBody] SendSmsCodeInput input)
    {
        await _checker.CheckAsync("Demo_SendSmsCode", new OperationRateLimitingContext
        {
            Parameter = input.PhoneNumber,
            ExtraProperties =
            {
                ["PhoneNumber"] = input.PhoneNumber
            }
        });

        return Ok(new { success = true, message = $"SMS code sent to {input.PhoneNumber}" });
    }

    /// <summary>
    /// Demo 2: Public - PartitionByClientIp
    /// </summary>
    [HttpPost("login-attempt")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAttempt([FromBody] LoginAttemptInput input)
    {
        await _checker.CheckAsync("Demo_LoginAttempt", new OperationRateLimitingContext
        {
            ExtraProperties =
            {
                ["Username"] = input.Username
            }
        });

        return Ok(new { success = true, message = $"Login attempt for {input.Username}" });
    }

    /// <summary>
    /// Demo 3: Authenticated - PartitionByCurrentUser
    /// </summary>
    [HttpPost("generate-api-key")]
    [Authorize]
    public async Task<IActionResult> GenerateApiKey()
    {
        await _checker.CheckAsync("Demo_GenerateApiKey");

        return Ok(new { success = true, message = "API key generated", key = "demo-key-" + Guid.NewGuid().ToString("N")[..8] });
    }

    /// <summary>
    /// Demo 4: Authenticated - PartitionByEmail (auto from current user)
    /// </summary>
    [HttpPost("send-email-code")]
    [Authorize]
    public async Task<IActionResult> SendEmailCode()
    {
        await _checker.CheckAsync("Demo_SendEmailCode");

        return Ok(new { success = true, message = "Email code sent" });
    }

    /// <summary>
    /// Demo 5a: Authenticated - Composite (ByUser + ByClientIp)
    /// </summary>
    [HttpPost("composite-user-ip")]
    [Authorize]
    public async Task<IActionResult> CompositeUserIp()
    {
        await _checker.CheckAsync("Demo_Composite_UserIp");

        return Ok(new { success = true, message = "Order created", orderId = "ORD-" + Guid.NewGuid().ToString("N")[..6] });
    }

    /// <summary>
    /// Demo 5b: Authenticated - Composite (ByParameter + ByUser)
    /// </summary>
    [HttpPost("composite-param-user")]
    [Authorize]
    public async Task<IActionResult> CompositeParamUser([FromBody] CompositeParamUserInput input)
    {
        await _checker.CheckAsync("Demo_Composite_ParamUser", new OperationRateLimitingContext
        {
            Parameter = input.Key
        });

        return Ok(new { success = true, message = $"Operation completed for key: {input.Key}" });
    }

    /// <summary>
    /// Demo 5c: Authenticated - Composite (ByParameter + ByUser + ByClientIp)
    /// </summary>
    [HttpPost("composite-triple")]
    [Authorize]
    public async Task<IActionResult> CompositeTriple([FromBody] CompositeTripleInput input)
    {
        await _checker.CheckAsync("Demo_Composite_Triple", new OperationRateLimitingContext
        {
            Parameter = input.Key
        });

        return Ok(new { success = true, message = $"Triple composite OK for key: {input.Key}" });
    }

    /// <summary>
    /// Demo 6: Public - Custom error code
    /// </summary>
    [HttpPost("submit-feedback")]
    [AllowAnonymous]
    public async Task<IActionResult> SubmitFeedback([FromBody] SubmitFeedbackInput input)
    {
        await _checker.CheckAsync("Demo_SubmitFeedback", new OperationRateLimitingContext
        {
            Parameter = input.Email,
            ExtraProperties =
            {
                ["Email"] = input.Email,
                ["Category"] = input.Category
            }
        });

        return Ok(new { success = true, message = "Feedback submitted" });
    }

    /// <summary>
    /// Demo 7: Public - Long duration hours
    /// </summary>
    [HttpPost("long-hours")]
    [AllowAnonymous]
    public async Task<IActionResult> LongHours([FromBody] LongHoursInput input)
    {
        await _checker.CheckAsync("Demo_LongHours", new OperationRateLimitingContext
        {
            Parameter = input.Key
        });

        return Ok(new { success = true, message = $"Operation completed for key: {input.Key}" });
    }

    /// <summary>
    /// Demo 8: Public - Long duration days
    /// </summary>
    [HttpPost("long-days")]
    [AllowAnonymous]
    public async Task<IActionResult> LongDays()
    {
        await _checker.CheckAsync("Demo_LongDays");

        return Ok(new { success = true, message = "Daily operation completed" });
    }

    /// <summary>
    /// Demo 9: Authenticated - Custom multi-key resolver (Parameter + UserId combined)
    /// </summary>
    [HttpPost("custom-multi-key")]
    [Authorize]
    public async Task<IActionResult> CustomMultiKey([FromBody] CustomMultiKeyInput input)
    {
        await _checker.CheckAsync("Demo_CustomMultiKey", new OperationRateLimitingContext
        {
            Parameter = input.ResourceId
        });

        return Ok(new { success = true, message = $"Resource '{input.ResourceId}' processed" });
    }

    /// <summary>
    /// Demo 10: Public - PartitionByParameter with WithMultiTenancy()
    /// Same parameter value has independent counters per tenant.
    /// </summary>
    [HttpPost("demo-tenant-isolated")]
    [AllowAnonymous]
    public async Task<IActionResult> DemoTenantIsolated([FromBody] DemoTenantIsolatedInput input)
    {
        await _checker.CheckAsync("Demo_TenantIsolated", new OperationRateLimitingContext
        {
            Parameter = input.Key
        });

        return Ok(new { success = true, message = $"Tenant-isolated operation completed for key: {input.Key}" });
    }

    /// <summary>
    /// Get status without consuming quota
    /// </summary>
    [HttpGet("status/{policyName}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetStatus(string policyName, [FromQuery] string? parameter = null)
    {
        var context = new OperationRateLimitingContext { Parameter = parameter };
        var status = await _checker.GetStatusAsync(policyName, context);

        return Ok(new
        {
            status.IsAllowed,
            status.RemainingCount,
            status.MaxCount,
            status.CurrentCount,
            RetryAfterSeconds = (int)(status.RetryAfter?.TotalSeconds ?? 0)
        });
    }

    /// <summary>
    /// Reset a policy counter
    /// </summary>
    [HttpPost("reset/{policyName}")]
    [AllowAnonymous]
    public async Task<IActionResult> Reset(string policyName, [FromQuery] string? parameter = null)
    {
        var context = new OperationRateLimitingContext { Parameter = parameter };
        await _checker.ResetAsync(policyName, context);

        return Ok(new { success = true, message = $"Policy '{policyName}' reset" });
    }

    /// <summary>
    /// Reset all demo policies
    /// </summary>
    [HttpPost("reset-all")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetAll(
        [FromQuery] string? smsPhone = null,
        [FromQuery] string? feedbackEmail = null,
        [FromQuery] string? longHoursKey = null,
        [FromQuery] string? compositeParamKey = null,
        [FromQuery] string? compositeTripleKey = null,
        [FromQuery] string? customMultiKeyResourceId = null,
        [FromQuery] string? tenantIsolatedKey = null)
    {
        var policies = new[]
        {
            ("Demo_SendSmsCode", smsPhone),
            ("Demo_LoginAttempt", (string?)null),
            ("Demo_GenerateApiKey", (string?)null),
            ("Demo_SendEmailCode", (string?)null),
            ("Demo_Composite_UserIp", (string?)null),
            ("Demo_Composite_ParamUser", compositeParamKey),
            ("Demo_Composite_Triple", compositeTripleKey),
            ("Demo_SubmitFeedback", feedbackEmail),
            ("Demo_LongHours", longHoursKey),
            ("Demo_LongDays", (string?)null),
            ("Demo_CustomMultiKey", customMultiKeyResourceId),
            ("Demo_TenantIsolated", tenantIsolatedKey),
        };

        foreach (var (policyName, parameter) in policies)
        {
            try
            {
                await _checker.ResetAsync(policyName, new OperationRateLimitingContext { Parameter = parameter });
            }
            catch
            {
                // Ignore errors for individual resets (e.g. not logged in for auth policies)
            }
        }

        return Ok(new { success = true, message = "All policies reset" });
    }
}

public class SendSmsCodeInput
{
    public string PhoneNumber { get; set; } = default!;
}

public class LoginAttemptInput
{
    public string Username { get; set; } = default!;
}

public class SubmitFeedbackInput
{
    public string Email { get; set; } = default!;
    public string Category { get; set; } = default!;
}

public class LongHoursInput
{
    public string Key { get; set; } = default!;
}

public class CompositeParamUserInput
{
    public string Key { get; set; } = default!;
}

public class CompositeTripleInput
{
    public string Key { get; set; } = default!;
}

public class CustomMultiKeyInput
{
    public string ResourceId { get; set; } = default!;
}

public class DemoTenantIsolatedInput
{
    public string Key { get; set; } = default!;
}
