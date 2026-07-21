```json
//[doc-seo]
{
    "Description": "Learn how to utilize ASP.NET Core's logging system in ABP Framework for effective application logging and monitoring."
}
```

# Logging

ABP doesn't implement any logging infrastructure. It uses the [ASP.NET Core's logging system](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging).

> .NET Core's logging system is actually independent from the ASP.NET Core. It is usable in any type of application.

## Serilog Request Enrichers

When the ABP ASP.NET Core Serilog integration is installed, its middleware enriches request log events with the current `TenantId`, `UserId`, `ClientId` and `CorrelationId` values when they are available.

`AbpAspNetCoreSerilogOptions.EnricherPropertyNames` can align these property names with an existing observability schema:

````csharp
Configure<AbpAspNetCoreSerilogOptions>(options =>
{
    options.EnricherPropertyNames.TenantId = "tenant_id";
    options.EnricherPropertyNames.UserId = "user_id";
    options.EnricherPropertyNames.ClientId = "client_id";
    options.EnricherPropertyNames.CorrelationId = "correlation_id";
});
````

The default names are `TenantId`, `UserId`, `ClientId` and `CorrelationId`.
