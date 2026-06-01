# Integration Services in ABP — What they are, when to use them, and how they behave 🚦

If you’ve been building with ABP for a while, you’ve probably used Application Services for your UI and APIs in your .NET and ASP.NET Core apps. Integration Services are similar—but with a different mission: they exist for service-to-service or module-to-module communication, not for end users.

If you want the formal spec, see the official doc: [Integration Services](../../framework/api-development/integration-services.md). This post is the practical, no-fluff guide.

## What is an Integration Service?

An Integration Service is an application service or ASP.NET Core MVC controller marked with the `[IntegrationService]` attribute. That marker tells ABP “this endpoint is for internal communication.”

- They are not exposed by default (safer for reusable modules and monoliths).
- When exposed, their route prefix is `/integration-api` (so you can easily protect them at your gateway or firewall).
- Auditing is disabled by default for them (less noise for machine-to-machine calls).

Quick look:

```csharp
[IntegrationService]
public interface IProductIntegrationService : IApplicationService
{
    Task<List<ProductDto>> GetProductsByIdsAsync(List<Guid> ids);
}

public class ProductIntegrationService : ApplicationService, IProductIntegrationService
{
    public Task<List<ProductDto>> GetProductsByIdsAsync(List<Guid> ids)
    {
        // fetch and return minimal product info for other services/modules
    }
}
```

## Are they HTTP endpoints?

- By default: no (they won’t be reachable over HTTP in the ASP.NET Core routing pipeline).
- If you need them over HTTP (typically for microservices), explicitly enable:

```csharp
Configure<AbpAspNetCoreMvcOptions>(options =>
{
    options.ExposeIntegrationServices = true;
});
```

Once exposed, ABP puts them under `/integration-api/...` instead of `/api/...` in the ASP.NET Core routing pipeline. That’s your hint to restrict them from public internet access.

## Enable auditing (optional)

If you want audit logs for integration calls, enable it explicitly:

```csharp
Configure<AbpAuditingOptions>(options =>
{
    options.IsEnabledForIntegrationServices = true;
});
```

## When should you use Integration Services?

- Internal, synchronous operations between services or modules.
- You need a “thin” API designed for other services (not for UI): minimal DTOs, no view concerns, predictable contracts.
- You want to hide these endpoints from public clients, or only allow them inside your private network or k8s cluster.
- You’re packaging a reusable module that might be used in both monolith and microservice deployments.

## When NOT to use them

- Public APIs or anything intended for browsers/mobile apps → use regular application services/controllers.
- Asynchronous cross-service workflows → consider domain events + outbox/inbox; use Integration Services for sync calls.
- Complex, chatty UI endpoints → those belong to your external API surface, not internal integration.

## Common use-cases and examples

- Identity lookups across services: an Ordering service needs basic user info from the Identity service.
- Permission checks from another module: a CMS module asks a Permission service for access decisions.
- Product data hydrations: a Cart service needs minimal product details (price, name) from Catalog.
- Internal admin/maintenance operations that aren’t meant for end users but are needed by other services.

## Example: microservice-to-microservice call

1) Mark and expose the integration service in the target service:

```csharp
[IntegrationService]
public interface IUserIntegrationService : IApplicationService
{
    Task<UserBriefDto?> FindByIdAsync(Guid id);
}

Configure<AbpAspNetCoreMvcOptions>(o => o.ExposeIntegrationServices = true);
```

2) In the caller service, add an HTTP client proxy only for Integration Services if you like to keep things clean:

```csharp
services.AddHttpClientProxies(
    typeof(TargetServiceApplicationModule).Assembly,
    remoteServiceConfigurationName: "TargetService",
    asDefaultServices: true,
    applicationServiceTypes: ApplicationServiceTypes.IntegrationServices);
```

3) Call it just like a local service (ABP’s HTTP proxy handles the wire):

```csharp
public class OrderAppService : ApplicationService
{
    private readonly IUserIntegrationService _userIntegrationService;

    public OrderAppService(IUserIntegrationService userIntegrationService)
    {
        _userIntegrationService = userIntegrationService;
    }

    public async Task PlaceOrderAsync(CreateOrderDto input)
    {
        var user = await _userIntegrationService.FindByIdAsync(CurrentUser.GetId());
        // validate user status, continue placing order...
    }
}
```

## Monolith vs. Microservices

- Monolith: keep them unexposed and call via DI in-process. You get the same clear contract with zero network overhead.
- Microservices: expose them and route behind your gateway. The `/integration-api` prefix makes it easy to firewall/gateway-restrict.

## Practical tips

- Keep integration DTOs lean and stable. These are machine contracts—don’t mix UI concerns.
- Name them clearly (e.g., `UserIntegrationService`) so intent is obvious.
- Guard your ASP.NET Core gateway application: block `/integration-api/*` from public traffic.
- Enable auditing only if you truly need the logs for these calls.

## Further reading

- Official docs: [Integration Services](../../framework/api-development/integration-services.md)

That’s it! Integration Services give you a clean, intentional way to design internal APIs—great in monoliths, essential in microservices.
