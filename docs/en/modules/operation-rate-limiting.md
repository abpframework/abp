````json
//[doc-seo]
{
    "Description": "Learn how to use the Operation Rate Limiting module (Pro) in ABP to control the frequency of specific operations like SMS sending, login attempts, and resource-intensive tasks."
}
````

# Operation Rate Limiting Module (Pro)

> You must have an [ABP Team or a higher license](https://abp.io/pricing) to use this module.

ABP provides an operation rate limiting system that allows you to control the frequency of specific operations in your application. You may need operation rate limiting for several reasons:

* Do not allow sending an SMS verification code to the same phone number more than 3 times in an hour.
* Do not allow generating a "monthly sales report" more than 2 times per day for each user (if generating the report is resource-intensive).
* Restrict login attempts per IP address to prevent brute-force attacks.

> This is not for [ASP.NET Core's built-in rate limiting middleware](https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit) which works at the HTTP request pipeline level. This module works at the **application/domain code level** and is called explicitly from your services. See the [Combining with ASP.NET Core Rate Limiting](#combining-with-aspnet-core-rate-limiting) section for a comparison.

## How to Install

This module is used by the [Account (Pro)](account-pro.md) module internally and comes pre-installed in the latest [startup templates](../solution-templates). So, no need to manually install it.

## Quick Start

This section shows the basic usage of the operation rate limiting system with a simple example.

### Defining a Policy

First, define a rate limiting policy in the `ConfigureServices` method of your [module class](../framework/architecture/modularity/basics.md):

````csharp
Configure<AbpOperationRateLimitingOptions>(options =>
{
    options.AddPolicy("SendSmsCode", policy =>
    {
        policy.WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 1)
              .PartitionByParameter();
    });
});
````

* `"SendSmsCode"` is a unique name for this policy.
* `WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 1)` means at most **1 request per minute**.
* `PartitionByParameter()` means the counter is keyed by the parameter you pass at check time (e.g., a phone number), so different phone numbers have independent counters.

### Checking the Limit

Then inject `IOperationRateLimitingChecker` and call `CheckAsync` in your service:

````csharp
public class SmsAppService : ApplicationService
{
    private readonly IOperationRateLimitingChecker _rateLimitChecker;

    public SmsAppService(IOperationRateLimitingChecker rateLimitChecker)
    {
        _rateLimitChecker = rateLimitChecker;
    }

    public virtual async Task SendCodeAsync(string phoneNumber)
    {
        await _rateLimitChecker.CheckAsync("SendSmsCode", phoneNumber);

        // If we reach here, the limit was not exceeded.
        // Send the SMS code...
    }
}
````

* `CheckAsync` increments the counter and throws `AbpOperationRateLimitingException` (HTTP 429) if the limit is exceeded.
* Each phone number has its own counter because we used `PartitionByParameter()`.
* Passing `phoneNumber` directly is a shortcut for `new OperationRateLimitingContext { Parameter = phoneNumber }`. Extension methods are provided for all four methods (`CheckAsync`, `IsAllowedAsync`, `GetStatusAsync`, `ResetAsync`) when you only need to pass a `parameter` string.

That's the basic usage. The following sections explain each concept in detail.

## Declarative Usage (Attribute)

Instead of injecting `IOperationRateLimitingChecker` manually, you can use the `[OperationRateLimiting]` attribute to enforce a policy declaratively on Application Service methods or MVC Controller actions.

> **Application Services** are handled by the ABP interceptor (built into the Domain layer).
> **MVC Controllers** are handled by `AbpOperationRateLimitingActionFilter`, which is automatically registered when you reference the `Volo.Abp.OperationRateLimiting.AspNetCore` package.

### Applying to an Application Service

````csharp
public class SmsAppService : ApplicationService
{
    [OperationRateLimiting("SendSmsCode")]
    public virtual async Task SendCodeAsync([RateLimitingParameter] string phoneNumber)
    {
        // Rate limit is checked automatically before this line executes.
        await _smsSender.SendAsync(phoneNumber, GenerateCode());
    }
}
````

### Applying to an MVC Controller

````csharp
[Route("api/account")]
public class AccountController : AbpController
{
    [HttpPost("send-sms-code")]
    [OperationRateLimiting("SendSmsCode")]
    public async Task<IActionResult> SendSmsCodeAsync([RateLimitingParameter] string phoneNumber)
    {
        // Rate limit is checked automatically before this line executes.
        await _smsSender.SendAsync(phoneNumber, GenerateCode());
        return Ok();
    }
}
````

### Resolving the Parameter Value

The `[OperationRateLimiting]` attribute resolves `OperationRateLimitingContext.Parameter` automatically using the following priority order:

1. **`[RateLimitingParameter]`** — a method parameter marked with this attribute. Its `ToString()` value is used as the partition key.
2. **`IHasOperationRateLimitingParameter`** — a method parameter whose type implements this interface. The value returned by `GetPartitionParameter()` is used as the partition key.
3. **`null`** — no parameter is resolved; suitable for policies that use `PartitionByCurrentUser`, `PartitionByClientIp`, etc.

#### Using `[RateLimitingParameter]`

Mark a single parameter to use its value as the partition key:

````csharp
[OperationRateLimiting("SendSmsCode")]
public virtual async Task SendCodeAsync([RateLimitingParameter] string phoneNumber)
{
    // partition key = phoneNumber
}
````

#### Using `IHasOperationRateLimitingParameter`

Implement the interface on an input DTO when the partition key is a property of the DTO:

````csharp
public class SendSmsCodeInput : IHasOperationRateLimitingParameter
{
    public string PhoneNumber { get; set; }
    public string Language { get; set; }

    public string? GetPartitionParameter() => PhoneNumber;
}
````

````csharp
[OperationRateLimiting("SendSmsCode")]
public virtual async Task SendCodeAsync(SendSmsCodeInput input)
{
    // partition key = input.GetPartitionParameter() = input.PhoneNumber
}
````

#### No Partition Parameter

If no parameter is marked and no DTO implements the interface, the policy is checked without a `Parameter` value. This is appropriate for policies that use `PartitionByCurrentUser`, `PartitionByClientIp`, or `PartitionByCurrentTenant`:

````csharp
// Policy uses PartitionByCurrentUser — no explicit parameter needed.
[OperationRateLimiting("GenerateReport")]
public virtual async Task<ReportDto> GenerateMonthlyReportAsync()
{
    // Rate limit is checked per current user automatically.
}
````

> If the method has parameters but none is resolved, a **warning log** is emitted to help you catch misconfigured usages early.

### Applying to a Class

You can also place `[OperationRateLimiting]` on the class to apply it to **all public methods** of that class:

````csharp
[OperationRateLimiting("MyServiceLimit")]
public class MyAppService : ApplicationService
{
    public virtual async Task MethodAAsync([RateLimitingParameter] string key) { ... }

    public virtual async Task MethodBAsync([RateLimitingParameter] string key) { ... }
}
````

> A method-level attribute takes precedence over the class-level attribute.

## Defining Policies

Policies are defined using `AbpOperationRateLimitingOptions` in the `ConfigureServices` method of your [module class](../framework/architecture/modularity/basics.md). Each policy has a unique name, one or more rules, and a partition strategy.

### Single-Rule Policies

For simple scenarios, use the `WithFixedWindow` shortcut directly on the policy builder:

````csharp
options.AddPolicy("SendSmsCode", policy =>
{
    policy.WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 1)
          .PartitionByParameter();
});
````

### Multi-Rule Policies

Use `AddRule` to combine multiple rules. All rules are checked together (**AND** logic) — a request is allowed only when **all** rules pass:

````csharp
options.AddPolicy("Login", policy =>
{
    // Rule 1: Max 5 attempts per 5 minutes per username
    policy.AddRule(rule => rule
        .WithFixedWindow(TimeSpan.FromMinutes(5), maxCount: 5)
        .PartitionByParameter());

    // Rule 2: Max 20 attempts per hour per IP
    policy.AddRule(rule => rule
        .WithFixedWindow(TimeSpan.FromHours(1), maxCount: 20)
        .PartitionByClientIp());
});
````

> When multiple rules are present, the module uses a **two-phase check**: it first verifies all rules without incrementing counters, then increments only if all rules pass. This prevents wasted quota when one rule would block the request.

### Overriding an Existing Policy

If a reusable module (e.g., ABP's Account module) defines a policy with default rules, you have two ways to customize it in your own module's `ConfigureServices`.

**Option 1 — Full replacement with `AddPolicy`:**

Call `AddPolicy` with the same name. The last registration wins and completely replaces all rules:

````csharp
// In your application module — runs after the Account module
Configure<AbpOperationRateLimitingOptions>(options =>
{
    options.AddPolicy("Account.SendPasswordResetCode", policy =>
    {
        // Replaces all rules defined by the Account module for this policy
        policy.AddRule(rule => rule
            .WithFixedWindow(TimeSpan.FromMinutes(5), maxCount: 3)
            .PartitionByEmail());
    });
});
````

> `AddPolicy` stores policies in a dictionary keyed by name, so calling it again with the same name fully replaces the previous policy and all its rules.

**Option 2 — Partial modification with `ConfigurePolicy`:**

Use `ConfigurePolicy` to modify an existing policy without replacing it entirely. The builder is pre-populated with the existing rules, so you only need to express what changes:

````csharp
Configure<AbpOperationRateLimitingOptions>(options =>
{
    // Only override the error code, keeping the module's original rules
    options.ConfigurePolicy("Account.SendPasswordResetCode", policy =>
    {
        policy.WithErrorCode("MyApp:SmsCodeLimit");
    });
});
````

You can also add a rule on top of the existing ones:

````csharp
options.ConfigurePolicy("Account.SendPasswordResetCode", policy =>
{
    // Keep the module's per-email rule and add a per-IP rule on top
    policy.AddRule(rule => rule
        .WithFixedWindow(TimeSpan.FromHours(1), maxCount: 20)
        .PartitionByClientIp());
});
````

Or clear all inherited rules first and define entirely new ones using `ClearRules()`:

````csharp
options.ConfigurePolicy("Account.SendPasswordResetCode", policy =>
{
    policy.ClearRules()
          .WithFixedWindow(TimeSpan.FromMinutes(5), maxCount: 3)
          .PartitionByEmail();
});
````

`ConfigurePolicy` returns `AbpOperationRateLimitingOptions`, so you can chain multiple calls:

````csharp
options
    .ConfigurePolicy("Account.SendPasswordResetCode", p => p.WithErrorCode("MyApp:SmsLimit"))
    .ConfigurePolicy("Account.Login", p => p.WithErrorCode("MyApp:LoginLimit"));
````

> `ConfigurePolicy` throws `AbpException` if the policy name is not found. Use `AddPolicy` first (in the module that owns the policy), then `ConfigurePolicy` in downstream modules to customize it.

### Custom Error Code

By default, the exception uses the error code `Volo.Abp.OperationRateLimiting:010001`. You can override it per policy:

````csharp
options.AddPolicy("SendSmsCode", policy =>
{
    policy.WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 1)
          .PartitionByParameter()
          .WithErrorCode("App:SmsCodeLimit");
});
````

## Partition Types

Each rule must specify a **partition type** that determines how requests are grouped. Requests with different partition keys have independent counters.

### PartitionByParameter

Uses the `Parameter` value from the context you pass to `CheckAsync`:

````csharp
policy.WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 1)
      .PartitionByParameter();

// Each phone number has its own counter
await checker.CheckAsync("SendSmsCode",
    new OperationRateLimitingContext { Parameter = phoneNumber });
````

### PartitionByCurrentUser

Uses `ICurrentUser.Id` as the partition key. The user must be authenticated:

````csharp
policy.WithFixedWindow(TimeSpan.FromHours(1), maxCount: 10)
      .PartitionByCurrentUser();
````

> If you need to check rate limits for a specific user (e.g., admin checking another user's limit), use `PartitionByParameter()` and pass the user ID as the `Parameter`.

### PartitionByCurrentTenant

Uses `ICurrentTenant.Id` as the partition key. Uses `"host"` for the host side when no tenant is active:

````csharp
policy.WithFixedWindow(TimeSpan.FromHours(1), maxCount: 100)
      .PartitionByCurrentTenant();
````

### PartitionByClientIp

Uses `IWebClientInfoProvider.ClientIpAddress` as the partition key:

````csharp
policy.WithFixedWindow(TimeSpan.FromMinutes(15), maxCount: 10)
      .PartitionByClientIp();
````

> This requires an ASP.NET Core environment. In non-web scenarios, the IP address cannot be determined and an exception will be thrown. Use `PartitionByParameter()` if you need to pass the IP explicitly.

### PartitionByEmail

Resolves from `context.Parameter` first, then falls back to `ICurrentUser.Email`:

````csharp
policy.WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 1)
      .PartitionByEmail();

// For unauthenticated users, pass the email explicitly:
await checker.CheckAsync("SendEmailCode",
    new OperationRateLimitingContext { Parameter = email });
````

### PartitionByPhoneNumber

Works the same way as `PartitionByEmail`: resolves from `context.Parameter` first, then falls back to `ICurrentUser.PhoneNumber`.

### Custom Partition (PartitionBy)

You can register a named custom resolver to generate the partition key. The resolver is an async function, so you can perform database queries or other I/O operations. Because the resolver is stored by name (not as an anonymous delegate), it can be serialized and managed from a UI or database.

**Step 1 — Register the resolver by name:**

````csharp
Configure<AbpOperationRateLimitingOptions>(options =>
{
    options.AddPartitionKeyResolver("ByDevice", ctx =>
        Task.FromResult($"{ctx.Parameter}:{ctx.ExtraProperties["DeviceId"]}"));
});
````

**Step 2 — Reference it in a policy:**

````csharp
policy.WithFixedWindow(TimeSpan.FromHours(1), maxCount: 100)
      .PartitionBy("ByDevice");
````

You can also register and reference in one step (inline):

````csharp
policy.WithFixedWindow(TimeSpan.FromHours(1), maxCount: 100)
      .PartitionBy("ByDevice", ctx =>
          Task.FromResult($"{ctx.Parameter}:{ctx.ExtraProperties["DeviceId"]}"));
````

> If you call `PartitionBy("name")` with a resolver name that hasn't been registered, an exception is thrown at configuration time (not at runtime), so typos are caught early.

To replace an existing resolver (e.g., in a downstream module), use `ReplacePartitionKeyResolver`:

````csharp
options.ReplacePartitionKeyResolver("ByDevice", ctx =>
    Task.FromResult($"v2:{ctx.Parameter}:{ctx.ExtraProperties["DeviceId"]}"));
````

### Named Rules (WithName)

By default, a rule's store key is derived from its `Duration`, `MaxCount`, and `PartitionType`. This means that if you change a rule's parameters (e.g., increase `maxCount` from 5 to 10), the counter resets because the key changes.

To keep a stable key across parameter changes, give the rule a name:

````csharp
policy.AddRule(rule => rule
    .WithName("HourlyLimit")
    .WithFixedWindow(TimeSpan.FromHours(1), maxCount: 100)
    .PartitionByCurrentUser());
````

When a name is set, it is used as the store key instead of the content-based descriptor. This is particularly useful when rules are managed from a database or UI — changing the `maxCount` or `duration` will not reset existing counters.

> Rule names must be unique within a policy. Duplicate names cause an exception at build time.

## Multi-Tenancy

By default, partition keys do not include tenant information — for partition types like `PartitionByParameter`, `PartitionByCurrentUser`, `PartitionByClientIp`, etc., counters are shared across tenants unless you call `WithMultiTenancy()`. Note that `PartitionByCurrentTenant()` is inherently per-tenant since the partition key is the tenant ID itself, and `PartitionByClientIp()` is typically kept global since the same IP should share a counter regardless of tenant.

You can enable tenant isolation for a rule by calling `WithMultiTenancy()`:

````csharp
policy.AddRule(rule => rule
    .WithFixedWindow(TimeSpan.FromHours(1), maxCount: 5)
    .WithMultiTenancy()
    .PartitionByParameter());
````

When multi-tenancy is enabled, the cache key includes the tenant ID, so each tenant has independent counters:

* **Global key format:** `orl:{PolicyName}:{RuleKey}:{PartitionKey}`
* **Tenant-isolated key format:** `orl:t:{TenantId}:{PolicyName}:{RuleKey}:{PartitionKey}`

## Checking the Limit

Inject `IOperationRateLimitingChecker` to interact with rate limits. It provides four methods:

### CheckAsync

The primary method. It checks the rate limit and **increments the counter** if allowed. Throws `AbpOperationRateLimitingException` (HTTP 429) if the limit is exceeded:

````csharp
await checker.CheckAsync("SendSmsCode",
    new OperationRateLimitingContext { Parameter = phoneNumber });
````

### IsAllowedAsync

A read-only check that returns `true` or `false` **without incrementing** the counter. Useful for UI pre-checks (e.g., disabling a button before the user clicks):

````csharp
var isAllowed = await checker.IsAllowedAsync("SendSmsCode",
    new OperationRateLimitingContext { Parameter = phoneNumber });
````

### GetStatusAsync

Returns detailed status information **without incrementing** the counter:

````csharp
var status = await checker.GetStatusAsync("SendSmsCode",
    new OperationRateLimitingContext { Parameter = phoneNumber });

// status.IsAllowed      - whether the next request would be allowed
// status.RemainingCount  - how many requests are left in this window
// status.RetryAfter      - time until the window resets
// status.MaxCount        - maximum allowed count
// status.CurrentCount    - current usage count
````

### ResetAsync

Resets the counter for a specific policy and context. This can be useful for administrative operations:

````csharp
await checker.ResetAsync("SendSmsCode",
    new OperationRateLimitingContext { Parameter = phoneNumber });
````

## The Exception

When a rate limit is exceeded, `CheckAsync` throws `AbpOperationRateLimitingException`. This exception:

* Extends `BusinessException` and implements `IHasHttpStatusCode` with status code **429** (Too Many Requests).
* Is automatically handled by ABP's exception handling pipeline and serialized into the HTTP response.

The exception uses one of two error codes depending on the policy type:

| Error Code | Constant | When Used |
|---|---|---|
| `Volo.Abp.OperationRateLimiting:010001` | `AbpOperationRateLimitingErrorCodes.ExceedLimit` | Regular rate limit exceeded (has a retry-after window) |
| `Volo.Abp.OperationRateLimiting:010002` | `AbpOperationRateLimitingErrorCodes.ExceedLimitPermanently` | Ban policy (`maxCount: 0`, permanently denied) |

You can override the error code per policy using `WithErrorCode()`. When a custom code is set, it is always used regardless of the policy type.

The exception includes the following data properties:

| Key | Type | Description |
|-----|------|-------------|
| `PolicyName` | string | Name of the triggered policy |
| `MaxCount` | int | Maximum allowed count |
| `CurrentCount` | int | Current usage count |
| `RemainingCount` | int | Remaining allowed count |
| `RetryAfterSeconds` | int | Seconds until the window resets (`0` for ban policies) |
| `RetryAfterMinutes` | int | Minutes until the window resets, rounded down (`0` for ban policies) |
| `RetryAfter` | string | Localized retry-after description (e.g., "5 minutes"); absent for ban policies |
| `WindowDurationSeconds` | int | Total window duration in seconds |
| `WindowDescription` | string | Localized window description |
| `RuleDetails` | List | Per-rule details (for multi-rule policies) |

## Configuration

### AbpOperationRateLimitingOptions

`AbpOperationRateLimitingOptions` is the main options class for the operation rate limiting system:

````csharp
Configure<AbpOperationRateLimitingOptions>(options =>
{
    options.IsEnabled = true;
    options.LockTimeout = TimeSpan.FromSeconds(5);
});
````

* **`IsEnabled`** (`bool`, default: `true`): Global switch to enable or disable rate limiting. When set to `false`, all `CheckAsync` calls pass through without checking. This is useful for disabling rate limiting in development (see [below](#disabling-in-development)).
* **`LockTimeout`** (`TimeSpan`, default: `5 seconds`): Timeout for acquiring the distributed lock during counter increment operations.

## Advanced Usage

### Disabling in Development

You may want to disable rate limiting during development to avoid being blocked while testing:

````csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    var hostEnvironment = context.Services.GetHostingEnvironment();

    Configure<AbpOperationRateLimitingOptions>(options =>
    {
        if (hostEnvironment.IsDevelopment())
        {
            options.IsEnabled = false;
        }
    });
}
````

### Ban Policy (maxCount: 0)

Setting `maxCount` to `0` creates a ban policy that permanently denies all requests regardless of the window duration. The `RetryAfter` value will be `null` since there is no window to wait for. The exception uses the error code `Volo.Abp.OperationRateLimiting:010002` (`AbpOperationRateLimitingErrorCodes.ExceedLimitPermanently`) with the message "Operation rate limit exceeded. This request is permanently denied.":

````csharp
options.AddPolicy("BlockedUser", policy =>
{
    policy.WithFixedWindow(TimeSpan.FromHours(24), maxCount: 0)
          .PartitionByParameter();
});
````

### Passing Extra Properties

Use `ExtraProperties` on `OperationRateLimitingContext` to pass additional context data. These values are available in custom partition resolvers and are included in the exception data when the limit is exceeded:

````csharp
await checker.CheckAsync("ApiCall", new OperationRateLimitingContext
{
    Parameter = apiEndpoint,
    ExtraProperties =
    {
        ["DeviceId"] = deviceId,
        ["ClientVersion"] = clientVersion
    }
});
````

### Pre-checking Before Expensive Operations

Use `IsAllowedAsync` or `GetStatusAsync` to check the limit **before** performing expensive work (e.g., validating input or querying the database):

````csharp
public async Task<SendCodeResultDto> SendCodeAsync(string phoneNumber)
{
    var context = new OperationRateLimitingContext { Parameter = phoneNumber };

    // Check limit before doing any work
    var status = await _rateLimitChecker.GetStatusAsync("SendSmsCode", context);

    if (!status.IsAllowed)
    {
        return new SendCodeResultDto
        {
            Success = false,
            RetryAfterSeconds = (int)(status.RetryAfter?.TotalSeconds ?? 0)
        };
    }

    // Now do the actual work and increment the counter
    await _rateLimitChecker.CheckAsync("SendSmsCode", context);

    await _smsSender.SendAsync(phoneNumber, GenerateCode());
    return new SendCodeResultDto { Success = true };
}
````

> `IsAllowedAsync` and `GetStatusAsync` are read-only — they do not increment the counter. Only `CheckAsync` increments.

### Checking on Behalf of Another User

`PartitionByCurrentUser()`, `PartitionByCurrentTenant()`, and `PartitionByClientIp()` always resolve from their respective services (`ICurrentUser`, `ICurrentTenant`, `IWebClientInfoProvider`) and do not accept explicit overrides. This design avoids partition key conflicts in [composite policies](#multi-rule-policies) where `Parameter` is shared across all rules.

If you need to check or enforce rate limits for a **specific user, tenant, or IP**, define the policy with `PartitionByParameter()` and pass the value explicitly:

````csharp
// Policy definition: use PartitionByParameter for explicit control
options.AddPolicy("UserApiLimit", policy =>
{
    policy.WithFixedWindow(TimeSpan.FromHours(1), maxCount: 100)
          .PartitionByParameter();
});
````

````csharp
// Check current user's limit
await checker.CheckAsync("UserApiLimit",
    new OperationRateLimitingContext { Parameter = CurrentUser.Id.ToString() });

// Admin checking another user's limit
await checker.CheckAsync("UserApiLimit",
    new OperationRateLimitingContext { Parameter = targetUserId.ToString() });

// Check a specific IP in a background job
await checker.CheckAsync("UserApiLimit",
    new OperationRateLimitingContext { Parameter = ipAddress });
````

This approach gives you full flexibility while keeping the API simple — `PartitionByCurrentUser()` is a convenience shortcut for "always use the current authenticated user", and `PartitionByParameter()` is for "I want to specify the value explicitly".

### Combining with ASP.NET Core Rate Limiting

This module and ASP.NET Core's built-in [rate limiting middleware](https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit) serve different purposes and can be used together:

| | ASP.NET Core Rate Limiting | Operation Rate Limiting |
|---|---|---|
| **Level** | HTTP request pipeline | Application/domain code |
| **Scope** | All incoming requests | Specific business operations |
| **Usage** | Middleware (automatic) | `[OperationRateLimiting]` attribute or explicit `CheckAsync` calls |
| **Typical use** | API throttling, DDoS protection | Business logic limits (SMS, reports) |

A common pattern is to use ASP.NET Core middleware for broad API protection and this module for fine-grained business operation limits.

## Extensibility

### Custom Store

The default store uses ABP's `IDistributedCache`. You can replace it by implementing `IOperationRateLimitingStore`:

````csharp
public class MyCustomStore : IOperationRateLimitingStore, ITransientDependency
{
    public Task<OperationRateLimitingStoreResult> IncrementAsync(
        string key, TimeSpan duration, int maxCount)
    {
        // Your custom implementation (e.g., Redis Lua script for atomicity)
    }

    public Task<OperationRateLimitingStoreResult> GetAsync(
        string key, TimeSpan duration, int maxCount)
    {
        // Read-only check
    }

    public Task ResetAsync(string key)
    {
        // Reset the counter
    }
}
````

ABP's [dependency injection](../framework/fundamentals/dependency-injection.md) system will automatically use your implementation since it replaces the default one.

### Custom Rule

You can implement custom rate limiting algorithms (e.g., sliding window, token bucket) by implementing `IOperationRateLimitingRule` and registering it with `AddRule<TRule>()`:

````csharp
policy.AddRule<MySlidingWindowRule>();
````

### Custom Formatter

Replace `IOperationRateLimitingFormatter` to customize how time durations are displayed in error messages (e.g., "5 minutes", "2 hours 30 minutes").

### Custom Policy Provider

Replace `IOperationRateLimitingPolicyProvider` to load policies from a database or external configuration source instead of the in-memory options.

When loading pre-built policies from an external source, use the `AddPolicy` overload that accepts an `OperationRateLimitingPolicy` object directly (bypassing the builder):

````csharp
options.AddPolicy(new OperationRateLimitingPolicy
{
    Name = "DynamicPolicy",
    Rules =
    [
        new OperationRateLimitingRuleDefinition
        {
            Name = "HourlyLimit",
            Duration = TimeSpan.FromHours(1),
            MaxCount = 100,
            PartitionType = OperationRateLimitingPartitionType.CurrentUser
        }
    ]
});
````

To remove a policy (e.g., when it is deleted from the database), use `RemovePolicy`:

````csharp
options.RemovePolicy("DynamicPolicy");
````

## See Also

* [ASP.NET Core Rate Limiting Middleware](https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit)
* [Distributed Caching](../framework/fundamentals/caching.md)
* [Exception Handling](../framework/fundamentals/exception-handling.md)
