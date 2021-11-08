# Distributed Event Bus

Distributed Event bus system allows to **publish** and **subscribe** to events that can be **transferred across application/service boundaries**. You can use the distributed event bus to asynchronously send and receive messages between **microservices** or **applications**.

## Providers

Distributed event bus system provides an **abstraction** that can be implemented by any vendor/provider. There are four providers implemented out of the box:

* `LocalDistributedEventBus` is the default implementation that implements the distributed event bus to work as in-process. Yes! The **default implementation works just like the [local event bus](Local-Event-Bus.md)**, if you don't configure a real distributed provider.
* `AzureDistributedEventBus` implements the distributed event bus with the [Azure Service Bus](https://azure.microsoft.com/en-us/services/service-bus/). See the [Azure Service Bus integration document](Distributed-Event-Bus-Azure-Integration.md) to learn how to configure it.
* `RabbitMqDistributedEventBus` implements the distributed event bus with the [RabbitMQ](https://www.rabbitmq.com/). See the [RabbitMQ integration document](Distributed-Event-Bus-RabbitMQ-Integration.md) to learn how to configure it.
* `KafkaDistributedEventBus` implements the distributed event bus with the [Kafka](https://kafka.apache.org/). See the [Kafka integration document](Distributed-Event-Bus-Kafka-Integration.md) to learn how to configure it.
* `RebusDistributedEventBus` implements the distributed event bus with the [Rebus](http://mookid.dk/category/rebus/). See the [Rebus integration document](Distributed-Event-Bus-Rebus-Integration.md) to learn how to configure it.

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
                new StockCountChangedEto
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

If you perform **database operations** and use the [repositories](Repositories.md) inside the event handler, you may need to create a [unit of work](Unit-Of-Work.md), because some repository methods need to work inside an **active unit of work**. Make the handle method `virtual` and add a `[UnitOfWork]` attribute for the method, or manually use the `IUnitOfWorkManager` to create a unit of work scope.

> The handler class must be registered to the dependency injection (DI). The sample above uses the `ITransientDependency` to accomplish it. See the [DI document](Dependency-Injection.md) for more options.


## Pre-Defined Events

ABP Framework **automatically publishes** distributed events for **create, update and delete** operations for an [entity](Entities.md) once you configure it.

### Event Types

There are three pre-defined event types:

* `EntityCreatedEto<T>` is published when an entity of type `T` was created.
* `EntityUpdatedEto<T>` is published when an entity of type `T` was updated.
* `EntityDeletedEto<T>` is published when an entity of type `T` was deleted.

These types are generics. `T` is actually the type of the **E**vent **T**ransfer **O**bject (ETO) rather than the type of the entity. Because, an entity object can not be transferred as a part of the event data. So, it is typical to define a ETO class for an entity class, like `ProductEto` for `Product` entity.

### Subscribing to the Events

Subscribing to the auto events is same as subscribing a regular distributed event.

**Example: Get notified once a product updated**

````csharp
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;

namespace AbpDemo
{
    public class MyHandler : 
        IDistributedEventHandler<EntityUpdatedEto<ProductEto>>,
        ITransientDependency
    {
        public async Task HandleEventAsync(EntityUpdatedEto<ProductEto> eventData)
        {
            var productId = eventData.Entity.Id;
            //TODO
        }
    }
}
````

* `MyHandler` implements the `IDistributedEventHandler<EntityUpdatedEto<ProductEto>>`.

### Configuration

You can configure the `AbpDistributedEntityEventOptions` in the `ConfigureServices` of your [module](Module-Development-Basics.md) to add a selector.

**Example: Configuration samples**

````csharp
Configure<AbpDistributedEntityEventOptions>(options =>
{
    //Enable for all entities
    options.AutoEventSelectors.AddAll();

    //Enable for a single entity
    options.AutoEventSelectors.Add<IdentityUser>();

    //Enable for all entities in a namespace (and child namespaces)
    options.AutoEventSelectors.AddNamespace("Volo.Abp.Identity");

    //Custom predicate expression that should return true to select a type
    options.AutoEventSelectors.Add(
        type => type.Namespace.StartsWith("MyProject.")
    );
});
````

* The last one provides flexibility to decide if the events should be published for the given entity type. Returns `true` to accept a `Type`.

You can add more than one selector. If one of the selectors match for an entity type, then it is selected.

### Event Transfer Object

Once you enable **auto events** for an entity, ABP Framework starts to publish events on the changes on this entity. If you don't specify a corresponding **E**vent **T**ransfer **O**bject (ETO) for the entity, ABP Framework uses a standard type, named `EntityEto`, which has only two properties:

* `EntityType` (`string`): Full name (including namespace) of the entity class.
* `KeysAsString` (`string`): Primary key(s) of the changed entity. If it has a single key, this property will be the primary key value. For a composite key, it will contain all keys separated by `,` (comma).

So, you can implement the `IDistributedEventHandler<EntityUpdatedEto<EntityEto>>` to subscribe the events. However, it is not a good approach to subscribe to such a generic event. You can define the corresponding ETO for the entity type.

**Example: Declare to use `ProductEto` for the `Product` entity**

````csharp
Configure<AbpDistributedEntityEventOptions>(options =>
{
    options.AutoEventSelectors.Add<Product>();
    options.EtoMappings.Add<Product, ProductEto>();
});
````

This example;

* Adds a selector to allow to publish the create, update and delete events for the `Product` entity.
* Configure to use the `ProductEto` as the event transfer object to publish for the `Product` related events.

Distributed event system use the [object to object mapping](Object-To-Object-Mapping.md) system to map `Product` objects to `ProductEto` objects. So, you need to configure the mapping. You can check the object to object mapping document for all options, but the following example shows how to configure it with the [AutoMapper](https://automapper.org/) library.

**Example: Configure `Product` to `ProductEto` mapping using the AutoMapper**

````csharp
using System;
using AutoMapper;
using Volo.Abp.Domain.Entities.Events.Distributed;

namespace AbpDemo
{
    [AutoMap(typeof(Product))]
    public class ProductEto : EntityEto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
````

This example uses the `AutoMap` attribute of the AutoMapper to configure the mapping. You could create a profile class instead. Please refer to the AutoMapper document for more options.