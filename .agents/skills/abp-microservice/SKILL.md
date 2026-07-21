---
name: abp-microservice
description: ABP Microservice solution template - service structure, Integration Services ([IntegrationService]), inter-service HTTP proxies, distributed events with Outbox/Inbox, Entity Cache, RabbitMQ/Redis/YARP setup. Use when working with the ABP microservice solution template or inter-service communication patterns.
---

# ABP Microservice Solution Template

> **Docs**: https://abp.io/docs/latest/solution-templates/microservice

## Solution Structure

```
MyMicroservice/
├── apps/                           # UI applications
│   ├── web/                        # Web application
│   ├── public-web/                 # Public website
│   └── auth-server/                # Authentication server (OpenIddict)
├── gateways/                       # BFF pattern - one gateway per UI
│   └── web-gateway/                # YARP reverse proxy
├── services/                       # Microservices
│   ├── administration/             # Permissions, settings, features
│   ├── identity/                   # Users, roles
│   └── [your-services]/            # Your business services
└── etc/
    ├── docker/                     # Docker compose for local infra
    └── helm/                       # Kubernetes deployment
```

## Microservice Structure (NOT Layered!)

Each microservice has simplified structure - everything in one project:

```
services/ordering/
├── OrderingService/                # Main project
│   ├── Entities/
│   ├── Services/
│   ├── IntegrationServices/        # For inter-service communication
│   ├── Data/                       # DbContext (implements IHasEventInbox, IHasEventOutbox)
│   └── OrderingServiceModule.cs
├── OrderingService.Contracts/      # Interfaces, DTOs, ETOs (shared)
└── OrderingService.Tests/
```

## Inter-Service Communication

### 1. Integration Services (Synchronous HTTP)

For synchronous calls, use **Integration Services** - NOT regular application services.

#### Step 1: Provider Service - Create Integration Service

```csharp
// In CatalogService.Contracts project
[IntegrationService]
public interface IProductIntegrationService : IApplicationService
{
    Task<List<ProductDto>> GetProductsByIdsAsync(List<Guid> ids);
}

// In CatalogService project
[IntegrationService]
public class ProductIntegrationService : ApplicationService, IProductIntegrationService
{
    public async Task<List<ProductDto>> GetProductsByIdsAsync(List<Guid> ids)
    {
        var products = await _productRepository.GetListAsync(p => ids.Contains(p.Id));
        return ObjectMapper.Map<List<Product>, List<ProductDto>>(products);
    }
}
```

#### Step 2: Provider Service - Expose Integration Services

```csharp
// In CatalogServiceModule.cs
Configure<AbpAspNetCoreMvcOptions>(options =>
{
    options.ExposeIntegrationServices = true;
});
```

#### Step 3: Consumer Service - Add Package Reference

Add reference to provider's Contracts project (via ABP Studio or manually):
- Right-click OrderingService → Add Package Reference → Select `CatalogService.Contracts`

#### Step 4: Consumer Service - Generate Proxies

```bash
# Run ABP CLI in consumer service folder
abp generate-proxy -t csharp -u http://localhost:44361 -m catalog --without-contracts
```

Or use ABP Studio: Right-click service → ABP CLI → Generate Proxy → C#

#### Step 5: Consumer Service - Register HTTP Client Proxies

```csharp
// In OrderingServiceModule.cs
[DependsOn(typeof(CatalogServiceContractsModule))] // Add module dependency
public class OrderingServiceModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Register static HTTP client proxies
        context.Services.AddStaticHttpClientProxies(
            typeof(CatalogServiceContractsModule).Assembly,
            "CatalogService");
    }
}
```

#### Step 6: Consumer Service - Configure Remote Service URL

```json
// appsettings.json
"RemoteServices": {
    "CatalogService": {
        "BaseUrl": "http://localhost:44361"
    }
}
```

#### Step 7: Use Integration Service

```csharp
public class OrderAppService : ApplicationService
{
    private readonly IProductIntegrationService _productIntegrationService;

    public async Task<List<OrderDto>> GetListAsync()
    {
        var orders = await _orderRepository.GetListAsync();
        var productIds = orders.Select(o => o.ProductId).Distinct().ToList();

        // Call remote service via generated proxy
        var products = await _productIntegrationService.GetProductsByIdsAsync(productIds);
        // ...
    }
}
```

> **Why Integration Services?** Application services are for UI - they have different authorization, validation, and optimization needs. Integration services are designed specifically for inter-service communication.

**When to use:** Need immediate response, data required to complete current operation (e.g., get product details to display in order list).

### 2. Distributed Events (Asynchronous)

Use RabbitMQ-based events for loose coupling.

**When to use:**
- Notifying other services about state changes (e.g., "order placed", "stock updated")
- Operations that don't need immediate response
- When services should remain independent and decoupled

```csharp
// Define ETO in Contracts project
[EventName("Product.StockChanged")]
public class StockCountChangedEto
{
    public Guid ProductId { get; set; }
    public int NewCount { get; set; }
}

// Publish
await _distributedEventBus.PublishAsync(new StockCountChangedEto { ... });

// Subscribe in another service
public class StockChangedHandler : IDistributedEventHandler<StockCountChangedEto>, ITransientDependency
{
    public async Task HandleEventAsync(StockCountChangedEto eventData) { ... }
}
```

DbContext must implement `IHasEventInbox`, `IHasEventOutbox` for Outbox/Inbox pattern.

## Performance: Entity Cache

For frequently accessed data from other services, use Entity Cache:

```csharp
// Register
context.Services.AddEntityCache<Product, ProductDto, Guid>();

// Use - auto-invalidates on entity changes
private readonly IEntityCache<ProductDto, Guid> _productCache;

public async Task<ProductDto> GetProductAsync(Guid id)
{
    return await _productCache.GetAsync(id);
}
```

## Pre-Configured Infrastructure

- **RabbitMQ** - Distributed events with Outbox/Inbox
- **Redis** - Distributed cache and locking
- **YARP** - API Gateway
- **OpenIddict** - Auth server

## Best Practices

- **Choose communication wisely** - Synchronous for queries needing immediate data, asynchronous for notifications and state changes
- **Use Integration Services** - Not application services for inter-service calls
- **Cache remote data** - Use Entity Cache or IDistributedCache for frequently accessed data
- **Share only Contracts** - Never share implementations
- **Idempotent handlers** - Events may be delivered multiple times
- **Database per service** - Each service owns its database
