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

### Using IDistributedEventBus to Publish Events

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

`PublishAsync` method gets the event object, which is responsible to hold the data related to the event. It is a simple plain class:

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

`EventName` attribute is optional, but suggested. If you don't declare it for an event type (ETO class), the event name will be the full name of the event class, `AbpDemo.StockCountChangedEto` in this case.

#### About Serialization for the Event Objects

Event transfer objects (ETOs) **must be serializable** since they will be serialized/deserialized to JSON or other format when it is transferred to out of the process.

Avoid circular references, polymorphism, private setters and provide default (empty) constructors if you have any other constructor as a good practice (while some serializers may tolerate it), just like the DTOs.

### Publishing Events Inside Entity / Aggregate Root Classes

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
* It is required to register your handler class to the [dependency injection](Dependency-Injection.md) system. Implementing `ITransientDependency` like in this example is an easy way.

### Configuration

You can configure the `AbpDistributedEntityEventOptions` in the `ConfigureServices` of your [module](Module-Development-Basics.md) to add a selector.

**Example: Configuration samples**

````csharp
Configure<AbpDistributedEntityEventOptions>(options =>
{
    //Enable for all entities
    options.AutoEventSelectors.AddAll();

    //Enable for a single entity
    options.AutoEventSelectors.Add<Product>();

    //Enable for all entities in a namespace (and child namespaces)
    options.AutoEventSelectors.AddNamespace("MyProject.Products");

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

So, you can implement the `IDistributedEventHandler<EntityUpdatedEto<EntityEto>>` to subscribe the update events. However, it is not a good approach to subscribe to such a generic event, because you handle the update events for all entities in a single handler (since they all use the same ETO object). You can define the corresponding ETO type for the entity type.

**Example: Declare to use `ProductEto` for the `Product` entity**

````csharp
public class ProductEto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public float Price { get; set; }
}
````

Then you can use the `AbpDistributedEntityEventOptions.EtoMappings` option to map your `Product` entity to the `ProductEto`:

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

> Distributed event system use the [object to object mapping](Object-To-Object-Mapping.md) system to map `Product` objects to `ProductEto` objects. So, you need to configure the object mapping (`Product` -> `ProductEto`) too. You can check the [object to object mapping document](Object-To-Object-Mapping.md) to learn how to do it.

## Entity Synchronizer

In a distributed (or microservice) system, it is typical to subscribe to change events for an [entity](Entities.md) type of another service, so you can get notifications when the subscribed entity changes. In that case, you can use ABP's Pre-Defined Events as explained in the previous section.

If your purpose is to store your local copies of a remote entity, you typically subscribe to create, update and delete events of the remote entity and update your local database in your event handler. ABP provides a pre-built `EntitySynchronizer` base class to make that operation easier for you.

Assume that there is a `Product` entity (probably an aggregate root entity) in a Catalog microservice, and you want to keep copies of the products in your Ordering microservice, with a local `OrderProduct` entity. In practice, properties of the `OrderProduct` class will be a subset of the `Product` properties, because not all the product data is needed in the Ordering microservice (however, you can make a full copy if you need). Also, the `OrderProduct` entity may have additional properties that are populated and used in the Ordering microservice.

The first step to establish the synchronization is to define an ETO (Event Transfer Object) class in the Catalog microservice that is used to transfer the event data. Assuming the `Product` entity has a `Guid` key, your ETO can be as shown below:

````
[EventName("product")]
public class ProductEto : EntityEto<Guid>
{
    // Your Product properties here...
}
````

`ProductEto` can be put in a shared project (DLL) that is referenced by the Catalog and the Ordering microservices. Alternatively, you can put a copy of the `ProductEto` class in the Ordering microservice if you don't want to introduce a common project dependency between the services. In this case, the `EventName` attribute becomes critical to map the `ProductEto` classes across two services (you should use the same event name).

Once you define an ETO class, you should configure the ABP Framework to publish auto (create, update and delete) events for the `Product` entity, as explained in the previous section:

````csharp
Configure<AbpDistributedEntityEventOptions>(options =>
{
    options.AutoEventSelectors.Add<Product>();
    options.EtoMappings.Add<Product, ProductEto>();
});
````

Finally, you should create a class in the Ordering microservice, that is derived from the `EntitySynchronizer` class:

````csharp
public class ProductSynchronizer : EntitySynchronizer<OrderProduct, Guid, ProductEto>
{
    public ProductSynchronizer(
        IObjectMapper objectMapper,
        IRepository<OrderProduct, Guid> repository
        ) : base(objectMapper, repository)
    {
    }
}
````

The main point of this class is it subscribes to the create, update and delete events of the source entity and updates the local entity in the database. It uses the [Object Mapper](Object-To-Object-Mapping.md) system to create or update the `OrderProduct` objects from the `ProductEto` objects. So, you should also configure the object mapper to make it properly work. Otherwise, you should manually perform the object mapping by overriding the `MapToEntityAsync(TSourceEntityEto)` and `MapToEntityAsync(TSourceEntityEto,TEntity)` methods in your `ProductSynchronizer` class.

If your entity has a composite primary key (see the [Entities document](Entities.md)), then you should inherit from the `EntitySynchronizer<TEntity, TSourceEntityEto>` class (just don't use the `Guid` generic argument in the previous example) and implement `FindLocalEntityAsync` to find the entity in your local database using the `Repository`.

`EntitySynchronizer` is compatible with the *Entity Versioning* system (see the [Entities document](Entities.md)). So, it works as expected even if the events are received as disordered. If the entity's version in your local database is newer than the entity in the received event, then the event is ignored. You should implement the `IHasEntityVersion` interface for the entity and ETO classes (for this example, you should implement for the `Product`, `ProductEto` and `OrderProduct` classes).

If you want to ignore some type of change events, you can set `IgnoreEntityCreatedEvent`, `IgnoreEntityUpdatedEvent` and `IgnoreEntityDeletedEvent` in the constructor of your class. Example:

````csharp
public class ProductSynchronizer 
    : EntitySynchronizer<OrderProduct, Guid, ProductEto>
{
    public ProductSynchronizer(
        IObjectMapper objectMapper,
        IRepository<OrderProduct, Guid> repository
        ) : base(objectMapper, repository)
    {
        IgnoreEntityDeletedEvent = true;
    }
}
````

> Notice that the `EntitySynchronizer` can only create/update the entities after you use it. If you have an existing system with existing data, you should manually copy the data for one time, because the `EntitySynchronizer` starts to work.

## Transaction and Exception Handling

Distributed event bus works in-process (since default implementation is `LocalDistributedEventBus`) unless you configure an actual provider (e.g. [Kafka](Distributed-Event-Bus-Kafka-Integration.md) or [RabbitMQ](Distributed-Event-Bus-RabbitMQ-Integration.md)). In-process event bus always executes event handlers in the same [unit of work](Unit-Of-Work.md) scope that you publishes the events in. That means, if an event handler throws an exception, then the related unit of work (the database transaction) is rolled back. In this way, your application logic and event handling logic becomes transactional (atomic) and consistent. If you want to ignore errors in an event handler, you must use a `try-catch` block in your handler and shouldn't re-throw the exception.

When you switch to an actual distributed event bus provider (e.g. [Kafka](Distributed-Event-Bus-Kafka-Integration.md) or [RabbitMQ](Distributed-Event-Bus-RabbitMQ-Integration.md)), then the event handlers will be executed in different processes/applications as their purpose is to create distributed systems. In this case, the only way to implement transactional event publishing is to use the outbox/inbox patterns as explained in the *Outbox / Inbox for Transactional Events* section.

If you don't configure outbox/inbox pattern or use the `LocalDistributedEventBus`, then events are published at the end of the unit of work by default, just before the unit of work is completed (that means throwing exception in an event handler still rollbacks the unit of work), even if you publish them in the middle of unit of work. If you want to immediately publish the event, you can set `onUnitOfWorkComplete` to `false` while using `IDistributedEventBus.PublishAsync` method.

> Keeping the default behavior is recommended unless you don't have a unique requirement. `onUnitOfWorkComplete` option is not available when you publish events inside entity / aggregate root classes (see the *Publishing Events Inside Entity / Aggregate Root Classes* section).

## Outbox / Inbox for Transactional Events

The **[transactional outbox pattern](https://microservices.io/patterns/data/transactional-outbox.html)** is used to publishing distributed events within the **same transaction** that manipulates the application's database. When you enable outbox, distributed events are saved into the database inside the same transaction with your data changes, then sent to the actual message broker by a separate [background worker](Background-Workers.md) with a re-try system. In this way, it ensures the consistency between your database state and the published events.

The **transactional inbox pattern**, on the other hand, saves incoming events into database first. Then (in a [background worker](Background-Workers.md)) executes the event handler in a transactional manner and removes the event from the inbox queue in the same transaction. It ensures that the event is only executed one time by keeping the processed messages for a while and discarding the duplicate events received from the message broker.

Enabling the event outbox and inbox systems require a few manual steps for your application. Please apply  the instructions in the following sections to make them running.

> Outbox and Inbox can be separately enabled and configured, so you may only use one of them if you want.

### Pre-requirements

* The outbox/inbox system uses the distributed lock system to handle concurrency when you run multiple instances of your application/service. So, you should **configure the distributed lock system** with one of the providers as [explained in this document](Distributed-Locking.md).
* The outbox/inbox system supports [Entity Framework Core](Entity-Framework-Core.md) (EF Core) and [MongoDB](MongoDB.md) **database providers** out of the box. So, your applications should use one of these database providers. For other database providers, see the *Implementing a Custom Database Provider* section.

> If you are using MongoDB, be sure that you enabled multi-document database transactions  that was introduced in MongoDB version 4.0. See the *Transactions* section of the [MongoDB](MongoDB.md) document.

### Enabling event outbox

Open your `DbContext` class (EF Core or MongoDB), implement the `IHasEventOutbox` interface. You should end up by adding a `DbSet` property into your `DbContext` class:

```csharp
public DbSet<OutgoingEventRecord> OutgoingEvents { get; set; }
```

Add the following lines inside the `OnModelCreating` method of your `DbContext` class (only for EF Core):

```csharp
builder.ConfigureEventOutbox();
```

For EF Core, use the standard `Add-Migration` and `Update-Database` commands to apply changes into your database (you can skip this step for MongoDB). If you want to use the command-line terminal, run the following commands in the root directory of the database integration project:

```bash
dotnet ef migrations add "Added_Event_Outbox"
dotnet ef database update
```

Finally, write the following configuration code inside the `ConfigureServices` method of your [module class](Module-Development-Basics.md) (replace `YourDbContext` with your own `DbContext` class):

````csharp
Configure<AbpDistributedEventBusOptions>(options =>
{
    options.Outboxes.Configure(config =>
    {
        config.UseDbContext<YourDbContext>();
    });
});
````

### Enabling event inbox

Open your `DbContext` class (EF Core or MongoDB), implement the `IHasEventInbox` interface. You should end up by adding a `DbSet` property into your `DbContext` class:

```csharp
public DbSet<IncomingEventRecord> IncomingEvents { get; set; }
```

Add the following lines inside the `OnModelCreating` method of your `DbContext` class (only for EF Core):

```csharp
builder.ConfigureEventInbox();
```

For EF Core, use the standard `Add-Migration` and `Update-Database` commands to apply changes into your database (you can skip this step for MongoDB). If you want to use the command-line terminal, run the following commands in the root directory of the database integration project:

```bash
dotnet ef migrations add "Added_Event_Inbox"
dotnet ef database update
```

Finally, write the following configuration code inside the `ConfigureServices` method of your [module class](Module-Development-Basics.md) (replace `YourDbContext` with your own `DbContext` class):

````csharp
Configure<AbpDistributedEventBusOptions>(options =>
{
    options.Inboxes.Configure(config =>
    {
        config.UseDbContext<YourDbContext>();
    });
});
````

### Additional Configuration

> The default configuration will be enough for most cases. However, there are some options you may want to set for outbox and inbox.

#### Outbox configuration

Remember how outboxes are configured:

````csharp
Configure<AbpDistributedEventBusOptions>(options =>
{
    options.Outboxes.Configure(config =>
    {
        // TODO: Set options
    });
});
````

Here, the following properties are available on the `config` object:

* `IsSendingEnabled` (default: `true`): You can set to `false` to disable sending outbox events to the actual event bus. If you disable this, events can still be added to outbox, but not sent. This can be helpful if you have multiple applications (or application instances) writing to outbox, but use one of them to send the events.
* `Selector`: A predicate to filter the event (ETO) types to be used for this configuration. Should return `true` to select the event. It selects all the events by default. This is especially useful if you want to ignore some ETO types from the outbox, or want to define named outbox configurations and group events within these configurations. See the *Named Configurations* section.
* `ImplementationType`: Type of the class that implements the database operations for the outbox. This is normally set when you call `UseDbContext` as shown before. See *Implementing a Custom Outbox/Inbox Database Provider* section for advanced usages.

#### Inbox configuration

Remember how inboxes are configured:

````csharp
Configure<AbpDistributedEventBusOptions>(options =>
{
    options.Inboxes.Configure(config =>
    {
        // TODO: Set options
    });
});
````

Here, the following properties are available on the `config` object:

* `IsProcessingEnabled` (default: `true`): You can set to `false` to disable processing (handling) events in the inbox. If you disable this, events can still be received, but not executed. This can be helpful if you have multiple applications (or application instances), but use one of them to execute the event handlers.
* `EventSelector`: A predicate to filter the event (ETO) types to be used for this configuration. This is especially useful if you want to ignore some ETO types from the inbox, or want to define named inbox configurations and group events within these configurations. See the *Named Configurations* section.
* `HandlerSelector`: A predicate to filter the event handled types (classes implementing the `IDistributedEventHandler<TEvent>` interface) to be used for this configuration. This is especially useful if you want to ignore some event handler types from inbox processing, or want to define named inbox configurations and group event handlers within these configurations. See the *Named Configurations* section.
* `ImplementationType`: Type of the class that implements the database operations for the inbox. This is normally set when you call `UseDbContext` as shown before. See *Implementing a Custom Outbox/Inbox Database Provider* section for advanced usages.

#### AbpEventBusBoxesOptions

`AbpEventBusBoxesOptions` can be used to fine-tune how inbox and outbox systems work. For most of the systems, using the defaults would be more than enough, but you can configure it to optimize your system when it is needed.

Just like all the [options classes](Options.md), `AbpEventBusBoxesOptions` can be configured in the `ConfigureServices` method of your [module class](Module-Development-Basics.md) as shown in the following code block:

````csharp
Configure<AbpEventBusBoxesOptions>(options =>
{
    // TODO: configure the options
});
````

`AbpEventBusBoxesOptions` has the following properties to be configured:

* `BatchPublishOutboxEvents`: Can be used to enable or disable batch publishing events to the message broker. Batch publishing works if it is supported by the distributed event bus provider. If not supported, events are sent one by one as the fallback logic. Keep it as enabled since it has a great performance gain wherever possible. Default value is `true` (enabled).
* `PeriodTimeSpan`: The period of the inbox and outbox message processors to check if there is a new event in the database. Default value is 2 seconds (`TimeSpan.FromSeconds(2)`).
* `CleanOldEventTimeIntervalSpan`: The event inbox system periodically checks and deletes the old processed events from the inbox in the database. You can set this value to determine the check period. Default value is 6 hours (`TimeSpan.FromHours(6)`).
* `WaitTimeToDeleteProcessedInboxEvents`: Inbox events are not deleted from the database for a while even if they are successfully processed. This is for a system to prevent multiple process of the same event (if the event broker sends it twice). This configuration value determines the time to keep the processed events. Default value is 2 hours (`TimeSpan.FromHours(2)`).
* `InboxWaitingEventMaxCount`: The maximum number of events to query at once from the inbox in the database. Default value is 1000.
* `OutboxWaitingEventMaxCount`: The maximum number of events to query at once from the outbox in the database. Default value is 1000.
* `DistributedLockWaitDuration`: ABP uses [distributed locking](Distributed-Locking.md) to prevent concurrent access to the inbox and outbox messages in the database, when running multiple instance of the same application. If an instance of the application can not obtain the lock, it tries after a duration. This is the configuration of that duration. Default value is 15 seconds (`TimeSpan.FromSeconds(15)`).

### Skipping Outbox

`IDistributedEventBus.PublishAsync` method provides an optional parameter, `useOutbox`, which is set to `true` by default. If you bypass outbox and immediately publish an event, you can set it to `false` for a specific event publishing operation.

### Advanced Topics

#### Named Configurations

> All the concepts explained in this section is also valid for inbox configurations. We will show examples only for outbox to keep the document shorter.

See the following outbox configuration code:

````csharp
Configure<AbpDistributedEventBusOptions>(options =>
{
    options.Outboxes.Configure(config =>
    {
        //TODO
    });
});
````

This is equivalent of the following code:

````csharp
Configure<AbpDistributedEventBusOptions>(options =>
{
    options.Outboxes.Configure("Default", config =>
    {
        //TODO
    });
});
````

`Default` is this code indicates the configuration name. If you don't specify it (like in the previous code block), `Default` is used as the configuration name.

That means you can define more than one configuration for outbox (also for inbox) with different names. ABP runs all the configured outboxes.

Multiple outboxes can be needed if your application have more than one database and you want to run different outbox queues for different databases. In this case, you can use the `Selector` option to decide the events should be handled by an outbox. See the *Additional Configurations* section above.

#### Implementing a Custom Outbox/Inbox Database Provider

If your application or service is using a database provider other than [EF Core](Entity-Framework-Core.md) and [MongoDB](MongoDB.md), you should manually integrate outbox/inbox system with your database provider.

> Outbox and Inbox table/data must be stored in the same database with your application's data (since we want to create a single database transaction that includes application's database operations and outbox/inbox table operations). Otherwise, you should care about distributed (multi-database) transaction support which is not provided by most of the vendors and may require additional configuration.

ABP provides `IEventOutbox` and `IEventInbox` abstractions as extension point for the outbox/inbox system. You can create classes by implementing these interfaces and register them to [dependency injection](Dependency-Injection.md).

Once you implement your custom event boxes, you can configure `AbpDistributedEventBusOptions` to use your event box classes:

````csharp
Configure<AbpDistributedEventBusOptions>(options =>
{
    options.Outboxes.Configure(config =>
    {
        config.ImplementationType = typeof(MyOutbox); //Your Outbox class
    });
    
    options.Inboxes.Configure(config =>
    {
        config.ImplementationType = typeof(MyInbox); //Your Inbox class
    });
});
````

## See Also

* [Local Event Bus](Local-Event-Bus.md)
