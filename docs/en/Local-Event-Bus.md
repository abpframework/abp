# Local Event Bus

The Local Event Bus allows services to publish and subscribe to **in-process events**. That means it is suitable if two services (publisher and subscriber) are running in the same process.

## Publishing Events

There are two ways of publishing local events explained in the following sections.

### ILocalEventBus

`ILocalEventBus` can be [injected](Dependency-Injection.md) and used to publish a local event.

**Example: Publish an event when the stock count of a product changes**

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

### Inside Entity / Aggregate Root Classes

[Entities](Entities.md) can not inject services via dependency injection, but it is very common to publish events inside entity / aggregate root classes.

**Example: Publish an event inside an aggregate root method**

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

`AggregateRoot` class defines the `AddLocalEvent` to add a new local event, that is published when the aggregate root object is saved (created or updated) into the database.

> If an entity publishes such an event, it is a good practice to change the related properties in a controlled manner, just like the example above - `StockCount` can only be changed by the `ChangeStockCount` which guarantees publishing the event.

#### IGeneratesDomainEvents Interface

Actually, adding local events are not unique to the `AggregateRoot` class. You can implement `IGeneratesDomainEvents` for any entity class. But, `AggregateRoot` implements it by default and makes it easy for you.

> It is not suggested to implement this interface for entities those are not aggregate roots, since it may not work for some database providers for such entities. It works for EF Core, but not works for MongoDB for example.

#### How It Was Implemented?

Calling the `AddLocalEvent` doesn't immediately publish the event. The event is published when you save changes to the database;

* For EF Core, it is published on `DbContext.SaveChanges`.
* For MongoDB, it is published when you call repository's `InsertAsync` or `UpdateAsync` methods (since MongoDB has no such a change tracking system).

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
            //TODO: your code that does somthing on the event
        }
    }
}
````

That's all. `MyHandler` is **automatically discovered** by the ABP Framework and `HandleEventAsync` is called whenever a `StockCountChangedEvent` occurs.  You can inject any service and perform any required logic here.

There can be zero or more handlers subscribed to the same event.

> The handler class must be registered to the dependency injection (DI). The sample above uses the `ITransientDependency` to accomplish it. See the [DI document](Dependency-Injection.md) for more options.

## Transaction & Exception Behavior

When an event published, subscribed event handlers are immediately executed. So;

* If a handler **throws an exception**, it effects the code that published the event. That means it gets the exception on the `PublishAsync` call. So, **use try-catch yourself** in the event handler if you want to hide the error.
* If the event publishing code is being executed inside a [Unit Of Work](Unit-Of-Work.md) scope, the event handlers also covered by the unit of work. That means if your UOW is transactional and a handler throws an exception, the transaction is rolled back.

## Pre-Built Events

TODO