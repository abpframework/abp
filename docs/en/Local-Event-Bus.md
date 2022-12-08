# Local Event Bus

The Local Event Bus allows services to publish and subscribe to **in-process events**. That means it is suitable if two services (publisher and subscriber) are running in the same process.

## Publishing Events

There are two ways of publishing local events explained in the following sections.

### Publishing Events Using the ILocalEventBus

`ILocalEventBus` can be [injected](Dependency-Injection.md) and used to publish a local event.

**Example: Publish a local event when the stock count of a product changes**

````csharp
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Local;

namespace AbpDemo
{
    public class MyService : ITransientDependency
    {
        private readonly ILocalEventBus _localEventBus;

        public MyService(ILocalEventBus localEventBus)
        {
            _localEventBus = localEventBus;
        }
        
        public virtual async Task ChangeStockCountAsync(Guid productId, int newCount)
        {
            //TODO: IMPLEMENT YOUR LOGIC...
            
            //PUBLISH THE EVENT
            await _localEventBus.PublishAsync(
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
    public class StockCountChangedEvent
    {
        public Guid ProductId { get; set; }
        
        public int NewCount { get; set; }
    }
}
````

Even if you don't need to transfer any data, you need to create a class (which is an empty class in this case).

### Publishing Events Inside Entity / Aggregate Root Classes

[Entities](Entities.md) can not inject services via dependency injection, but it is very common to publish local events inside entity / aggregate root classes.

**Example: Publish a local event inside an aggregate root method**

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
            AddLocalEvent(
                new StockCountChangedEvent
                {
                    ProductId = Id,
                    NewCount = newCount
                }
            );
        }
    }
}
````

`AggregateRoot` class defines the `AddLocalEvent` to add a new local event, that is published when the aggregate root object is saved (created, updated or deleted) into the database.

> Tip: If an entity publishes such an event, it is a good practice to change the related properties in a controlled manner, just like the example above - `StockCount` can only be changed by the `ChangeStockCount` method which guarantees publishing the event.

#### IGeneratesDomainEvents Interface

Actually, adding local events are not unique to the `AggregateRoot` class. You can implement `IGeneratesDomainEvents` for any entity class. But, `AggregateRoot` implements it by default and makes it easy for you.

> It is not suggested to implement this interface for entities those are not aggregate roots, since it may not work for some database providers for such entities. It works for EF Core, but not works for MongoDB for example.

#### How It Was Implemented?

Calling the `AddLocalEvent` doesn't immediately publish the event. The event is published when you save changes to the database;

* For EF Core, it is published on `DbContext.SaveChanges`.
* For MongoDB, it is published when you call repository's `InsertAsync`, `UpdateAsync` or `DeleteAsync` methods (since MongoDB has not a change tracking system).

## Subscribing to Events

A service can implement the `ILocalEventHandler<TEvent>` to handle the event.

**Example: Handle the `StockCountChangedEvent` defined above**

````csharp
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace AbpDemo
{
    public class MyHandler
        : ILocalEventHandler<StockCountChangedEvent>,
          ITransientDependency
    {
        public async Task HandleEventAsync(StockCountChangedEvent eventData)
        {
            //TODO: your code that does something on the event
        }
    }
}
````

That's all. `MyHandler` is **automatically discovered** by the ABP Framework and `HandleEventAsync` is called whenever a `StockCountChangedEvent` occurs.  You can inject any service and perform any required logic in your handler class.

* **One or more handlers** can subscribe to the same event.
* A single event handler class can **subscribe to multiple events** by implementing the `ILocalEventHandler<TEvent>` interface for each event type.

If you perform **database operations** and use the [repositories](Repositories.md) inside the event handler, you may need to create a [unit of work](Unit-Of-Work.md), because some repository methods need to work inside an **active unit of work**. Make the handle method `virtual` and add a `[UnitOfWork]` attribute for the method, or manually use the `IUnitOfWorkManager` to create a unit of work scope.

> The handler class must be registered to the dependency injection (DI). The sample above uses the `ITransientDependency` to accomplish it. See the [DI document](Dependency-Injection.md) for more options.

### Transaction & Exception Behavior

Event handlers are always executed in the same [unit of work](Unit-Of-Work.md) scope, that means in the same database transaction with the code that published the event. If an event handler throws an exception, the unit of work (database transaction) is rolled back. So, **use try-catch yourself** in the event handler if you want to hide the error.

When you call `ILocalEventBus.PublishAsync`, the event handlers are not immediately executed. Instead, they are executed just before the current unit of work completed (an unhandled exception in the handler still rollbacks the current unit of work). If you want to immediately execute the handlers, set the optional `onUnitOfWorkComplete` parameter to `false`.

> Keeping the default behavior is recommended unless you don't have a unique requirement. `onUnitOfWorkComplete` option is not available when you publish events inside entity / aggregate root classes (see the *Publishing Events Inside Entity / Aggregate Root Classes* section).

## Pre-Built Events

It is very common to **publish events on entity create, update and delete** operations. ABP Framework **automatically** publish these events for all entities. You can just subscribe to the related event.

**Example: Subscribe to an event that published when a user was created**

````csharp
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace AbpDemo
{
    public class MyHandler
        : ILocalEventHandler<EntityCreatedEventData<IdentityUser>>,
          ITransientDependency
    {
        public async Task HandleEventAsync(
            EntityCreatedEventData<IdentityUser> eventData)
        {
            var userName = eventData.Entity.UserName;
            var email = eventData.Entity.Email;
            //...
        }
    }
}
````

This class subscribes to the `EntityCreatedEventData<IdentityUser>`, which is published just after a user was created (but before the current transaction is completed). For example, you may want to send a "Welcome" email to the new user.

The pre-built event types are;

* `EntityCreatedEventData<T>` is published just after an entity was successfully created.
* `EntityUpdatedEventData<T>` is published just after an entity was successfully updated.
* `EntityDeletedEventData<T>` is published just after an entity was successfully deleted.
* `EntityChangedEventData<T>` is published just after an entity was successfully created, updated or deleted. It can be a shortcut if you need to listen any type of change - instead of subscribing to the individual events.

#### How It Was Implemented?

Pre-build events are published when you save changes to the database;

* For EF Core, they are published on `DbContext.SaveChanges`.
* For MongoDB, they are published when you call repository's `InsertAsync`, `UpdateAsync` or `DeleteAsync` methods (since MongoDB has not a change tracking system).

## See Also

* [Distributed Event Bus](Distributed-Event-Bus.md)
