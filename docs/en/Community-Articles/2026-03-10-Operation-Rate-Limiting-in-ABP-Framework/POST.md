# Operation Rate Limiting in ABP

Almost every user-facing system eventually runs into the same problem: **some operations cannot be allowed to run without limits**.

Sometimes it's a cost issue — sending an SMS costs money, and generating a report hammers the database. Sometimes it's security — a login endpoint with no attempt limit is an open invitation for brute-force attacks. And sometimes it's a matter of fairness — your paid plan says "up to 100 data exports per month," and you need to actually enforce that.

What all these cases have in common is that the thing being limited isn't an HTTP request — it's a *business operation*, performed by a specific *who*, doing a specific *what*, against a specific *resource*.

ASP.NET Core ships with a built-in [rate limiting middleware](https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit) that sits in the HTTP pipeline. It's excellent for broad API protection — throttling requests per IP to fend off bots or DDoS traffic. But it only sees HTTP requests. It can tell you how many requests came from an IP address; it cannot tell you:

- **"How many verification codes has this phone number received today?"** The moment the user switches networks, the counter resets — completely useless
- **"How many reports has this user exported today?"** Switching from mobile to desktop gives them a fresh counter
- **"How many times has someone tried to log in as `alice`?"** An attacker rotating through dozens of IPs will never hit the per-IP limit

There's another gap: some rate-limiting logic has no corresponding HTTP endpoint at all — it lives inside an application service method called by multiple endpoints, or triggered by a background job. HTTP middleware has no place to hook in.

Real-world requirements tend to look like this:

- The same phone number can receive at most 3 verification codes per hour, regardless of which device or IP the request comes from
- Each user can generate at most 2 monthly sales reports per day, because a single report query scans millions of records
- Login attempts are limited to 5 failures per username per 5 minutes, *and* 20 failures per IP per hour — two independent counters, both enforced simultaneously
- Free-tier users get 50 AI calls per month, paid users get 500 — this is a product-defined quota, not a security measure
- Your system integrates with an LLM provider (OpenAI, Azure OpenAI, etc.) where every call has a real dollar cost. Without per-user or per-tenant limits, a single user can exhaust your monthly budget overnight

The pattern is clear: the identity being throttled is a **business identity** — a user, a phone number, a resource ID — not an IP address. And the action being throttled is a **business operation**, not an HTTP request.

ABP's **Operation Rate Limiting** module is built for exactly this. It lets you enforce limits directly in your application or domain layer, with full awareness of who is doing what.

This module is used by the Account (Pro) modules internally and comes pre-installed in the latest startup templates. You must have an [ABP Team or a higher license](https://abp.io/pricing) to use this module.

## Defining a Policy

The model is straightforward: define a named policy in `ConfigureServices`, then call `CheckAsync` wherever you need to enforce it.

Name your policies after the business action they protect — `"SendSmsCode"`, `"GenerateReport"`, `"CallAI"`. A clear name makes the intent obvious at the call site, and avoids the mystery of something like `"policy1"`.

```csharp
Configure<AbpOperationRateLimitingOptions>(options =>
{
    options.AddPolicy("SendSmsCode", policy =>
    {
        policy.WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 1)
              .PartitionByParameter();
    });
});
```

- `WithFixedWindow` sets the time window and maximum count — here, at most 1 call per minute
- `PartitionByParameter` means each distinct value you pass at call time (such as a phone number) gets its own independent counter

Then inject `IOperationRateLimitingChecker` and call `CheckAsync` at the top of the method you want to protect:

```csharp
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

        // Limit not exceeded — proceed with sending the SMS
    }
}
```

`CheckAsync` checks the current usage against the limit and throws `AbpOperationRateLimitingException` (HTTP 429) if the limit is already exceeded. If the check passes, it then increments the counter and proceeds. ABP's exception pipeline catches this automatically and returns a standard error response. Put `CheckAsync` first — the rate limit check is the gate, and everything else only runs if it passes.

## Declarative Usage with `[OperationRateLimiting]`

The explicit `CheckAsync` approach is useful when you need fine-grained control — for example, when you want to check the limit conditionally, or when the parameter value comes from somewhere other than a method argument. But for the common case where you simply want to enforce a policy on every invocation of a specific method, there's a cleaner way: the `[OperationRateLimiting]` attribute.

```csharp
public class SmsAppService : ApplicationService
{
    [OperationRateLimiting("SendSmsCode")]
    public virtual async Task SendCodeAsync([RateLimitingParameter] string phoneNumber)
    {
        // Rate limit is enforced automatically — no manual CheckAsync needed.
        await _smsSender.SendAsync(phoneNumber, GenerateCode());
    }
}
```

The attribute works on both **Application Service methods** (via ABP's interceptor) and **MVC Controller actions** (via an action filter). No manual injection of `IOperationRateLimitingChecker` required.

### Providing the Partition Key

When using the attribute, the partition key is resolved from the method's parameters automatically:

- Mark a parameter with `[RateLimitingParameter]` to use its `ToString()` value as the key — this is the most common case when the key is a single primitive like a phone number or email.
- Have your input DTO implement `IHasOperationRateLimitingParameter` and provide a `GetPartitionParameter()` method — useful when the key is a property buried inside a complex input object.

```csharp
public class SendSmsCodeInput : IHasOperationRateLimitingParameter
{
    public string PhoneNumber { get; set; }
    public string Language { get; set; }

    public string? GetPartitionParameter() => PhoneNumber;
}

[OperationRateLimiting("SendSmsCode")]
public virtual async Task SendCodeAsync(SendSmsCodeInput input)
{
    // input.GetPartitionParameter() = input.PhoneNumber is used as the partition key.
}
```

If neither is provided, `Parameter` is `null` — which is perfectly valid for policies that use `PartitionByCurrentUser`, `PartitionByClientIp`, or similar partition types that don't rely on an explicit value.

```csharp
// Policy uses PartitionByCurrentUser — no partition key needed.
[OperationRateLimiting("GenerateReport")]
public virtual async Task<ReportDto> GenerateMonthlyReportAsync()
{
    // Rate limit is checked per current user, automatically.
}
```

> The resolution order is: `[RateLimitingParameter]` first, then `IHasOperationRateLimitingParameter`, then `null`. If the method has parameters but none is resolved, a warning is logged to help you catch the misconfiguration early.

You can also place `[OperationRateLimiting]` on the class itself to apply the policy to all public methods:

```csharp
[OperationRateLimiting("MyServiceLimit")]
public class MyAppService : ApplicationService
{
    public virtual async Task MethodAAsync([RateLimitingParameter] string key) { ... }

    public virtual async Task MethodBAsync([RateLimitingParameter] string key) { ... }
}
```

A method-level attribute always takes precedence over the class-level one.

## Choosing a Partition Type

The partition type controls **how counters are isolated from each other** — it's the most important decision when setting up a policy, because it determines *what dimension you're counting across*.

Getting this wrong can make your rate limiting completely ineffective. Using `PartitionByClientIp` for SMS verification? An attacker just needs to switch networks. Using `PartitionByCurrentUser` for a login endpoint? There's no current user before login, so the counter has nowhere to land.

- **`PartitionByParameter`** — uses the value you explicitly pass as the partition key. This is the most flexible option. Pass a phone number, an email address, a resource ID, or any business identifier you have at hand. It's the right choice whenever you know exactly what the "who" is.
- **`PartitionByCurrentUser`** — uses the authenticated user's ID, with no value to pass. Perfect for "each user gets N per day" scenarios where user identity is all you need.
- **`PartitionByClientIp`** — uses the client's IP address. Don't rely on this alone — it's too easy to rotate. Use it as a secondary layer alongside another partition type, as in the login example below.
- **`PartitionByEmail`** and **`PartitionByPhoneNumber`** — designed for pre-authentication flows where the user isn't logged in yet. They prefer the `Parameter` value you explicitly pass, and fall back to the current user's email or phone number if none is provided.
- **`PartitionBy`** — a named custom resolver that can produce any partition key you need. Register a resolver function under a unique name via `options.AddPartitionKeyResolver("MyResolver", ctx => ...)`, then reference it by name: `.PartitionBy("MyResolver")`. You can also register and reference in one step: `.PartitionBy("MyResolver", ctx => ...)`. When the built-in options don't fit, you're free to implement whatever logic makes sense: look up a resource's owner in the database, derive a key from the user's subscription tier, partition by tenant — anything that returns a string. Because the resolver is stored by name (not as an anonymous delegate), it can be serialized and managed from a UI or database.

> The rule of thumb: partition by the identity of whoever's behavior you're trying to limit.

## Combining Rules in One Policy

A single rule covers most cases, but sometimes you need to enforce limits across multiple dimensions simultaneously. Login protection is the textbook example: throttling by username alone doesn't stop an attacker from targeting many accounts; throttling by IP alone doesn't stop an attacker with a botnet. You need both, at the same time.

```csharp
options.AddPolicy("Login", policy =>
{
    // Rule 1: at most 5 attempts per username per 5-minute window
    policy.AddRule(rule => rule
        .WithFixedWindow(TimeSpan.FromMinutes(5), maxCount: 5)
        .PartitionByParameter());

    // Rule 2: at most 20 attempts per IP per hour, counted independently
    policy.AddRule(rule => rule
        .WithFixedWindow(TimeSpan.FromHours(1), maxCount: 20)
        .PartitionByClientIp());
});
```

The two counters are completely independent. If `alice` fails 5 times, her account is locked — but other accounts from the same IP are unaffected. If an IP accumulates 20 failures, it's blocked — but `alice` can still be targeted from other IPs until their own counters fill up.

When multiple rules are present, the module uses a two-phase approach: it checks all rules first, and only increments counters if every rule passes. This prevents a rule from consuming quota on a request that would have been rejected by another rule anyway.

## Customizing Policies from Reusable Modules

ABP modules (including your own) can ship with built-in rate limiting policies. For example, an Account module might define a `"Account.SendPasswordResetCode"` policy with conservative defaults that make sense for most applications. When you need different rules in your specific application, you have two options.

**Complete replacement with `AddPolicy`:** call `AddPolicy` with the same name and the second registration wins, replacing all rules from the module:

```csharp
Configure<AbpOperationRateLimitingOptions>(options =>
{
    options.AddPolicy("Account.SendPasswordResetCode", policy =>
    {
        policy.AddRule(rule => rule
            .WithFixedWindow(TimeSpan.FromMinutes(5), maxCount: 3)
            .PartitionByEmail());
    });
});
```

**Partial modification with `ConfigurePolicy`:** when you only want to tweak part of a policy — change the error code, add a secondary rule, or tighten the window — use `ConfigurePolicy`. The builder starts pre-populated with the module's existing rules, so you only express what changes.

For example, keep the module's default rules but assign your own localized error code:

```csharp
Configure<AbpOperationRateLimitingOptions>(options =>
{
    options.ConfigurePolicy("Account.SendPasswordResetCode", policy =>
    {
        policy.WithErrorCode("MyApp:PasswordResetLimit");
    });
});
```

Or add a secondary IP-based rule on top of what the module already defined, without touching it:

```csharp
Configure<AbpOperationRateLimitingOptions>(options =>
{
    options.ConfigurePolicy("Account.SendPasswordResetCode", policy =>
    {
        policy.AddRule(rule => rule
            .WithFixedWindow(TimeSpan.FromHours(1), maxCount: 20)
            .PartitionByClientIp());
    });
});
```

If you want a clean slate, call `ClearRules()` first and then define entirely new rules — this gives you the same result as `AddPolicy` but makes the intent explicit:

```csharp
Configure<AbpOperationRateLimitingOptions>(options =>
{
    options.ConfigurePolicy("Account.SendPasswordResetCode", policy =>
    {
        policy.ClearRules()
              .WithFixedWindow(TimeSpan.FromMinutes(10), maxCount: 5)
              .PartitionByEmail();
    });
});
```

`ConfigurePolicy` throws if the policy name doesn't exist — which catches typos at startup rather than silently doing nothing.

The general rule: use `AddPolicy` for full replacements, `ConfigurePolicy` for surgical modifications.

## Beyond Just Checking

Not every scenario calls for throwing an exception. `IOperationRateLimitingChecker` provides three additional methods for more nuanced control.

**`IsAllowedAsync`** performs a read-only check — it returns `true` or `false` without touching any counter. The most common use case is UI pre-checking: when a user opens the "send verification code" page, check the limit first. If they've already hit it, disable the button and show a countdown immediately, rather than making them click and get an error. That's a meaningfully better experience.

```csharp
var isAllowed = await _rateLimitChecker.IsAllowedAsync("SendSmsCode", phoneNumber);
```

**`GetStatusAsync`** also reads without incrementing, but returns richer data: `RemainingCount`, `RetryAfter`, and `CurrentCount`. This is what you need to build quota displays — "You have 2 exports remaining today" or "Please try again in 47 seconds" — which are far friendlier than a raw 429.

```csharp
var status = await _rateLimitChecker.GetStatusAsync("SendSmsCode", phoneNumber);
// status.RemainingCount, status.RetryAfter, status.IsAllowed ...
```

**`ResetAsync`** clears the counter for a given policy and context. Useful in admin panels where support staff can manually unblock a user, or in test environments where you need to reset state between runs.

```csharp
await _rateLimitChecker.ResetAsync("SendSmsCode", phoneNumber);
```

## When the Limit Is Hit

When `CheckAsync` triggers, it throws `AbpOperationRateLimitingException`, which:

- Inherits from `BusinessException` and maps to HTTP **429 Too Many Requests**
- Is handled automatically by ABP's exception pipeline
- Carries useful metadata: `RetryAfterSeconds`, `RemainingCount`, `MaxCount`, `CurrentCount`

By default, the error code sent to the client is a generic one from the module. If you want each operation to produce its own localized message — "Too many verification code requests, please wait before trying again" instead of a generic error — assign a custom error code to the policy:

```csharp
options.AddPolicy("SendSmsCode", policy =>
{
    policy.WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 1)
          .PartitionByParameter()
          .WithErrorCode("App:SmsCodeLimit");
});
```

> For details on mapping error codes to localized messages, see [Exception Handling](https://abp.io/docs/latest/framework/fundamentals/exception-handling) in the ABP docs.

## Turning It Off in Development

Rate limiting and local development don't mix well. When you're iterating quickly and calling the same endpoint a dozen times to test something, getting blocked by a 429 every few seconds is genuinely painful. Disable the module in your development environment:

```csharp
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
```

## Summary

ABP's Operation Rate Limiting fills the gap that ASP.NET Core's HTTP middleware can't: rate limiting with real awareness of *who* is doing *what*. Define a named policy, pick a time window, a max count, and a partition type. Then either call `CheckAsync` explicitly, or just add `[OperationRateLimiting]` to your method and let the framework handle the rest. Counter storage, distributed locking, and exception handling are all taken care of.

## References

- [Operation Rate Limiting (Pro)](https://abp.io/docs/latest/modules/operation-rate-limiting)
- [ASP.NET Core Rate Limiting Middleware](https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit)
- [Exception Handling](https://abp.io/docs/latest/framework/fundamentals/exception-handling)
