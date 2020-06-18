# Distributed Event Bus

Distributed Event bus system allows to **publish** and **subscribe** to events that can be **transferred across application/service boundaries**. You can use the distributed event bus to asynchronously send and receive messages between **microservices** or **applications**.

## Providers

Distributed event bus system provides an **abstraction** that can be implemented by any vendor/provider. There are two providers implemented out of the box:

* `LocalDistributedEventBus` is the default implementation that implements the distributed event bus to work as in-process. Yes! The **default implementation works just like the [local event bus](Local-Event-Bus.md)**, if you don't configure a real distributed provider.
* `RabbitMqDistributedEventBus` implements the distributed event bus with the [RabbitMQ](https://www.rabbitmq.com/). See the [RabbitMQ integration document](Distributed-Event-Bus-RabbitMQ-Integration.md) to learn how to configure it.

Using a local event bus as default has a few important advantages. The most important one is that: It allows you to write your code compatible to distributed architecture. You can write a monolithic application now that can be split into microservices later. It is a good practice to communicate between bounded contexts (or between application modules) via distributed events instead of local events.

For example, [pre-built application modules](Modules/Index.md) is designed to work as a service in a distributed system while they can also work as a module in a monolithic application without depending an external message broker.

## Publishing Events

There are two ways of publishing distributed events explained in the following sections.

### IDistributedEventBus

`IDistributedEventBus` can be [injected](Dependency-Injection.md) and used to publish a distributed event.

**Example: Publish a distributed event when the stock count of a product changes**

````csharp
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace AbpDemo
{
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
                new StockCountChangedEvent
                {
                    ProductId = productId,
                    NewCount = newCount
                }
            );
        }
    }
}
````

`PublishAsync` method gets a single parameter: the event object, which is responsible to hold the data related to the event. It is a simple plain class:

````csharp
using System;

namespace AbpDemo
{
    [EventName("MyApp.Product.StockChange")]
    public class StockCountChangedEto
    {
        public Guid ProductId { get; set; }
        
        public int NewCount { get; set; }
    }
}
````

Even if you don't need to transfer any data, you need to create a class (which is an empty class in this case).

> `Eto` is a suffix for **E**vent **T**ransfer **O**bjects we use by convention. While it is not required, we find it useful to identify such event classes (just like [DTOs](Data-Transfer-Objects.md) on the application layer).

#### Event Name

`EventName` attribute is optional, but suggested. If you don't declare it, the event name will be the full name of the event class, `AbpDemo.StockCountChangedEto` in this case.

#### About Serialization for the Event Objects

Event transfer objects **must be serializable** since they will be serialized/deserialized to JSON or other format when it is transferred to out of the process.

Avoid circular references, polymorphism, private setters and provide default (empty) constructors if you have any other constructor as a good practice (while some serializers may tolerate it), just like the DTOs.

### Inside Entity / Aggregate Root Classes

[Entities](Entities.md) can not inject services via dependency injection, but it is very common to publish distributed events inside entity / aggregate root classes.

**Example: Publish a distributed event inside an aggregate root method**

````csharp
using System;
using Volo.Abp.Domain.Entities;

namespace AbpDemo
{
    public class Product : AggregateRoot<Guid>
    {
        public string Name { get; set; }
        
        public int StockCount { get; private set; }

        private Product() { }

        public Product(Guid id, string name)
            : base(id)
        {
            Name = name;
        }

        public void ChangeStockCount(int newCount)
        {
            StockCount = newCount;
            
            //ADD an EVENT TO BE PUBLISHED
            AddDistributedEvent(
                new StockCountChangedEto
                {
                    ProductId = Id,
                    NewCount = newCount
                }
            );
        }
    }
}
````

`AggregateRoot` class defines the `AddDistributedEvent` to add a new distributed event, that is published when the aggregate root object is saved (created, updated or deleted) into the database.

> If an entity publishes such an event, it is a good practice to change the related properties in a controlled manner, just like the example above - `StockCount` can only be changed by the `ChangeStockCount` method which guarantees publishing the event.

#### IGeneratesDomainEvents Interface

Actually, adding distributed events are not unique to the `AggregateRoot` class. You can implement `IGeneratesDomainEvents` for any entity class. But, `AggregateRoot` implements it by default and makes it easy for you.

> It is not suggested to implement this interface for entities those are not aggregate roots, since it may not work for some database providers for such entities. It works for EF Core, but not works for MongoDB for example.

#### How It Was Implemented?

Calling the `AddDistributedEvent` doesn't immediately publish the event. The event is published when you save changes to the database;

* For EF Core, it is published on `DbContext.SaveChanges`.
* For MongoDB, it is published when you call repository's `InsertAsync`, `UpdateAsync` or `DeleteAsync` methods (since MongoDB has not a change tracking system).

## Subscribing to Events

A service can implement the `IDistributedEventHandler<TEvent>` to handle the event.

**Example: Handle the `StockCountChangedEto` defined above**

````csharp
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace AbpDemo
{
    public class MyHandler
        : IDistributedEventHandler<StockCountChangedEto>,
          ITransientDependency
    {
        public async Task HandleEventAsync(StockCountChangedEto eventData)
        {
            var productId = eventData.ProductId;
        }
    }
}
````

That's all.

* `MyHandler` is **automatically discovered** by the ABP Framework and `HandleEventAsync` is called whenever a `StockCountChangedEto` event occurs.
* If you are using a distributed message broker, like RabbitMQ, ABP automatically **subscribes to the event on the message broker**, gets the message, executes the handler.
* It sends **confirmation (ACK)** to the message broker if the event handler was successfully executed (did not throw any exception).

You can inject any service and perform any required logic here. A single event handler class can **subscribe to multiple events** but implementing the `IDistributedEventHandler<TEvent>` interface for each event type.

> The handler class must be registered to the dependency injection (DI). The sample above uses the `ITransientDependency` to accomplish it. See the [DI document](Dependency-Injection.md) for more options.
