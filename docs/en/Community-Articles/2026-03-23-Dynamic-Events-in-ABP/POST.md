# Dynamic Events in ABP

> This feature is available since ABP 10.3.

ABP's Event Bus is a core infrastructure piece. The **Local Event Bus** handles in-process communication between services. The **Distributed Event Bus** handles cross-service communication over message brokers like RabbitMQ, Kafka, Azure Service Bus, and Rebus.

Both are fully type-safe — you define event types at compile time, register handlers via DI, and everything is wired up automatically. This works great, but it has one assumption: **you know all your event types at compile time**.

In practice, that assumption breaks down in several scenarios:

- You're building a **plugin system** where third-party modules register their own event types at runtime — you can't pre-define an `IDistributedEventHandler<TEvent>` for every possible plugin event
- Your system receives events from **external systems** (webhooks, IoT devices, partner APIs) where the event schema is defined by the external party, not by your codebase
- You're building a **low-code platform** where end users define event-driven workflows through a visual designer — the event names and payloads are entirely determined at runtime

ABP's **Dynamic Events** extend the existing `IEventBus` and `IDistributedEventBus` interfaces with string-based publishing and subscription. You can publish events by name, subscribe to events by name, and handle payloads without any compile-time type binding — all while coexisting seamlessly with the existing typed event system.

## Publishing Events by Name

The most straightforward use case: publish an event using a string name and an arbitrary payload.

```csharp
public class OrderAppService : ApplicationService
{
    private readonly IDistributedEventBus _eventBus;

    public OrderAppService(IDistributedEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task PlaceOrderAsync(PlaceOrderInput input)
    {
        // Business logic...

        // Publish a dynamic event — no event class needed
        await _eventBus.PublishAsync(
            "OrderPlaced",
            new { OrderId = input.Id, CustomerEmail = input.Email }
        );
    }
}
```

The payload can be any serializable object — an anonymous type, a `Dictionary<string, object>`, or even an existing typed class. The event bus serializes the payload and sends it to the broker with the string name as the routing key.

### What If a Typed Event Already Exists?

If the string name matches an existing typed event (via `EventNameAttribute`), the framework automatically converts the payload to the typed class and routes it through the **typed pipeline**. Both typed handlers and dynamic handlers are triggered.

```csharp
[EventName("OrderPlaced")]
public class OrderPlacedEto
{
    public Guid OrderId { get; set; }
    public string CustomerEmail { get; set; }
}

// This handler will still receive the event, with auto-converted data
public class OrderEmailHandler : IDistributedEventHandler<OrderPlacedEto>
{
    public Task HandleEventAsync(OrderPlacedEto eventData)
    {
        // eventData.OrderId and eventData.CustomerEmail are populated
        return Task.CompletedTask;
    }
}
```

Publishing by name with `new { OrderId = ..., CustomerEmail = ... }` triggers this typed handler — the framework handles the serialization round-trip. This is especially useful for scenarios where a service needs to emit events without taking a dependency on the project that defines the event type.

## Subscribing to Dynamic Events

Dynamic subscription lets you register event handlers at runtime, using a string event name.

The recommended approach is to use `IocEventHandlerFactory`, which is the same mechanism ABP uses internally for typed handlers. It creates a new DI scope for each event, resolves a fresh handler instance, calls `HandleEventAsync`, then disposes the scope — so the handler can use normal constructor injection without any manual scope management:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.AddTransient<PartnerOrderHandler>();
}

public override void OnApplicationInitialization(
    ApplicationInitializationContext context)
{
    var eventBus = context.ServiceProvider
        .GetRequiredService<IDistributedEventBus>();
    var scopeFactory = context.ServiceProvider
        .GetRequiredService<IServiceScopeFactory>();

    // Subscribe to a dynamic event — no event class needed
    eventBus.Subscribe("PartnerOrderReceived",
        new IocEventHandlerFactory(scopeFactory, typeof(PartnerOrderHandler)));
}
```

The handler implements `IDistributedEventHandler<DynamicEventData>` and injects its dependencies normally:

```csharp
public class PartnerOrderHandler : IDistributedEventHandler<DynamicEventData>
{
    private readonly IPartnerOrderProcessor _orderProcessor;

    public PartnerOrderHandler(IPartnerOrderProcessor orderProcessor)
    {
        _orderProcessor = orderProcessor;
    }

    public async Task HandleEventAsync(DynamicEventData eventData)
    {
        // eventData.EventName = "PartnerOrderReceived"
        // eventData.Data = the raw payload from the broker
        await _orderProcessor.ProcessAsync(eventData.EventName, eventData.Data);
    }
}
```

`DynamicEventData` is a simple POCO with two properties:

- **`EventName`** — the string name that identifies the event
- **`Data`** — the raw event data payload (the deserialized `object` from the broker)

> `Subscribe` returns an `IDisposable`. Call `Dispose()` to unsubscribe the handler at runtime. For application-lifetime subscriptions, prefer module initialization (`OnApplicationInitialization` / `OnApplicationInitializationAsync`) over subscribing inside an application service.

## Mixed Typed and Dynamic Handlers

Typed and dynamic handlers coexist naturally. When both are registered for the same event name, **both are triggered** — the framework automatically converts the data to the appropriate format for each handler.

```csharp
var eventBus = context.ServiceProvider.GetRequiredService<IDistributedEventBus>();
var scopeFactory = context.ServiceProvider.GetRequiredService<IServiceScopeFactory>();

// Typed handler — receives OrderPlacedEto
eventBus.Subscribe<OrderPlacedEto, OrderEmailHandler>();

// Dynamic handler — receives DynamicEventData for the same event
eventBus.Subscribe("OrderPlaced",
    new IocEventHandlerFactory(scopeFactory, typeof(AuditLogHandler)));
```

When `OrderPlacedEto` is published (by type or by name), both handlers fire. The typed handler receives a fully deserialized `OrderPlacedEto` object. The dynamic handler receives a `DynamicEventData` wrapping the raw payload.

This enables a powerful pattern: the core business logic uses typed handlers for safety, while infrastructure concerns (auditing, logging, plugin hooks) use dynamic handlers for flexibility.

## Outbox Support

Dynamic events go through the same **outbox/inbox pipeline** as typed events. If you have outbox configured, dynamic events benefit from the same reliability guarantees — they are stored in the outbox table within the same database transaction as your business data, then reliably delivered to the broker by the background worker.

No additional configuration is needed. The outbox works transparently for both typed and dynamic events:

```csharp
// This dynamic event goes through the outbox if configured
using var uow = _unitOfWorkManager.Begin();
await _eventBus.PublishAsync(
    "OrderPlaced",
    new { OrderId = orderId },
    onUnitOfWorkComplete: true,
    useOutbox: true
);
await uow.CompleteAsync();
```

## Local Event Bus

Dynamic events work on the local event bus too, not just the distributed bus. The API is the same:

```csharp
var localEventBus = context.ServiceProvider
    .GetRequiredService<ILocalEventBus>();

// Subscribe dynamically
localEventBus.Subscribe("UserActivityTracked",
    new SingleInstanceHandlerFactory(
        new ActionEventHandler<DynamicEventData>(eventData =>
        {
            // Handle the event
            return Task.CompletedTask;
        })));

// Publish dynamically
await localEventBus.PublishAsync("UserActivityTracked", new
{
    UserId = currentUser.Id,
    Action = "PageView",
    Url = "/products/42"
});
```

## Provider Support

Dynamic events work with all distributed event bus providers:

| Provider | Dynamic Subscribe | Dynamic Publish |
|---|---|---|
| LocalDistributedEventBus (default) | ✅ | ✅ |
| RabbitMQ | ✅ | ✅ |
| Kafka | ✅ | ✅ |
| Rebus | ✅ | ✅ |
| Azure Service Bus | ✅ | ✅ |
| Dapr | ❌ | ❌ |

Dapr requires topic subscriptions to be declared at application startup and cannot add subscriptions at runtime. Calling `Subscribe(string, ...)` on the Dapr provider throws an `AbpException`.

## Summary

`IEventBus.PublishAsync(string, object)` and `IEventBus.Subscribe(string, handler)` let you publish and subscribe to events by name at runtime — no compile-time types required. If the event name matches a typed event, the framework auto-converts the payload and triggers both typed and dynamic handlers. Dynamic events go through the same outbox/inbox pipeline as typed events, so reliability guarantees are preserved. This works across all providers except Dapr, and coexists seamlessly with the existing typed event system.

## References

- [Local Event Bus](https://abp.io/docs/latest/framework/infrastructure/event-bus/local)
- [Distributed Event Bus](https://abp.io/docs/latest/framework/infrastructure/event-bus/distributed)
- [RabbitMQ Integration](https://abp.io/docs/latest/framework/infrastructure/event-bus/distributed/rabbitmq)
- [Kafka Integration](https://abp.io/docs/latest/framework/infrastructure/event-bus/distributed/kafka)
- [Dynamic Distributed Events Sample](https://github.com/abpframework/abp-samples/tree/master/DynamicDistributedEvents)
