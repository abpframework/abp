# Microservice Solution: Distributed Events

````json
//[doc-nav]
{
  "Next": {
    "Name": "Helm charts and Kubernetes in the Microservice solution",
    "Path": "solution-templates/microservice/helm-charts-and-kubernetes"
  }
}
````

> You must have an ABP Business or a higher license to be able to create a microservice solution.

The microservice solution template uses the [Distributed Event Bus](../../framework/infrastructure/event-bus/distributed/index.md) mechanism to enable asynchronous communication between microservices. The Event Bus employs a publish-subscribe pattern, allowing microservices to communicate without being aware of each other, which helps in decoupling them and making them more independent.

In the microservice solution template, [RabbitMQ](https://www.rabbitmq.com/) is used as the message broker to manage these events. The functionality is implemented in the `Volo.Abp.EventBus.RabbitMQ` package, which provides the necessary implementations to publish and subscribe to events using RabbitMQ. This setup is integrated into the microservice solution template and is used in microservice/application projects. You can change the RabbitMQ configuration in the `appsettings.json` file of the related project. The default configuration is as follows:

```json
  "RabbitMQ": {
    "Connections": {
      "Default": {
        "HostName": "localhost"
      }
    },
    "EventBus": {
      "ClientName": "ProjectName_MicroserviceName",
      "ExchangeName": "ProjectName"
    }
  }
```

*ExchangeName* is the name of the exchange used to publish and subscribe to events. Different microservices in an application should use the same exchange name to communicate with each other. *ClientName* is the name of the client used to connect to RabbitMQ. It is recommended to use the microservice name as the client name. This way, when a message is published, all clients subscribed to the exchange will receive the message.

## Publishing Events

To publish an event, inject the `IDistributedEventBus` service and call the `PublishAsync` method. The `PublishAsync` method takes the event object as a parameter. Here is an example of publishing an event:

```csharp
public class MyService : ITransientDependency
{
    private readonly IDistributedEventBus _distributedEventBus;

    public MyService(IDistributedEventBus distributedEventBus)
    {
        _distributedEventBus = distributedEventBus;
    }
    
    public virtual async Task ChangeStockCountAsync(Guid productId, int newCount)
    {
        await _distributedEventBus.PublishAsync(
            new StockCountChangedEto
            {
                ProductId = productId,
                NewCount = newCount
            }
        );
    }
}
```

## Subscribing to Events

To subscribe to an event, implement the `IDistributedEventHandler<TEvent>` interface. Here is an example of subscribing to an event:

```csharp
public class MyHandler : IDistributedEventHandler<StockCountChangedEto>, ITransientDependency
{
    public async Task HandleEventAsync(StockCountChangedEto eventData)
    {
        var productId = eventData.ProductId;
    }
}
```

## Outbox / Inbox Pattern

The outbox/inbox pattern ensures that messages are delivered safely and reliably. The outbox pattern stores messages in the database before sending them to the message broker. The inbox pattern stores messages in the database before processing them, ensuring that messages are not lost in case of a failure. The microservice solution template uses the outbox/inbox pattern to ensure safe and reliable message delivery. You can learn more about the outbox/inbox pattern in the [Distributed Event Bus](../../framework/infrastructure/event-bus/distributed/index.md#outbox--inbox-for-transactional-events) documentation.

You can see the configuration of the outbox/inbox pattern in the Module class of the related project. The default configuration is as follows:

```csharp
private void ConfigureDistributedEventBus()
{
    Configure<AbpDistributedEventBusOptions>(options =>
    {
        options.Inboxes.Configure(config =>
        {
            config.UseDbContext<MicroserviceDbContext>();
        });

        options.Outboxes.Configure(config =>
        {
            config.UseDbContext<MicroserviceDbContext>();
        });
    });
}
```

> One downside of using the outbox/inbox pattern is that since messages are stored in the database, each microservice needs its own database schema. This can be problematic if you use EntityFrameworkCore and want to share the same database connection between microservices. Having two different database schemas (DbContexts) with the same table name will cause conflicts.