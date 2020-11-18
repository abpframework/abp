# 本地事件总线

本地事件总线允许服务发布和订阅**进程内事件**. 这意味着如果两个服务(发布者和订阅者)在同一个进程中运行,那么它是合适的.

## 发布事件

以下介绍了两种发布本地事件的方法.

### ILocalEventBus

可以[注入](Dependency-Injection.md) `ILocalEventBus` 并且使用发布本地事件.

**示例: 产品的存货数量发生变化时发布本地事件**

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

`PublishAsync` 方法需要一个参数:事件对象,它负责保持与事件相关的数据,是一个简单的普通类:

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

即使你不需要传输任何数据也需要创建一个类(在这种情况下为空类).

### 实体/聚合根类

[实体](Entities.md)不能通过依赖注入注入服务,但是在实体/聚合根类中发布本地事件是非常常见的.

**示例: 在聚合根方法内发布本地事件**

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

`AggregateRoot` 类定义了 `AddLocalEvent` 来添加一个新的本地事件,事件在聚合根对象保存(创建,更新或删除)到数据库时发布.

> 如果实体发布这样的事件,以可控的方式更改相关属性是一个好的实践,就像上面的示例一样 - `StockCount`只能由保证发布事件的 `ChangeStockCount` 方法来更改.

#### IGeneratesDomainEvents 接口

实际上添加本地事件并不是 `AggregateRoot` 类独有的. 你可以为任何实体类实现 `IGeneratesDomainEvents`. 但是 `AggregateRoot` 默认实现了它简化你的工作.

> 不建议为不是聚合根的实体实现此接口,因为它可能不适用于此类实体的某些数据库提供程序. 例如它适用于EF Core,但不适用于MongoDB.

#### 它是如何实现的?

调用 `AddLocalEvent` 不会立即发布事件. 当你将更改保存到数据库时发布该事件;

* 对于 EF Core, 它在 `DbContext.SaveChanges` 中发布.
* 对于 MongoDB, 它在你调用仓储的 `InsertAsync`, `UpdateAsync` 或 `DeleteAsync` 方法时发由 (因为MongoDB没有更改跟踪系统).

## 订阅事件

一个服务可以实现 `ILocalEventHandler<TEvent>` 来处理事件.

**示例: 处理上面定义的`StockCountChangedEvent`**

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

这就是全部,`MyHandler` 由ABP框架**自动发现**,并在发生 `StockCountChangedEvent` 事件时调用 `HandleEventAsync`.

* 事件可以由**0个或多个处理程序**订阅.
* 一个事件处理程序可以**订阅多个事件**,但是需要为每个事件实现 `ILocalEventHandler<TEvent>`  接口.

> 事件处理程序类必须注册到依赖注入(DI),示例中使用了 `ITransientDependency`. 参阅[DI文档](Dependency-Injection.md)了解更多选项.

如果您执行**数据库操作**并在事件处理程序中使用[仓储](Repositories.md),那么您可能需要创建一个[工作单元](Unit-Of-Work.md),因为一些存储库方法需要在**活动的工作单元**中工作. 确保处理方法设置为 `virtual`,并为该方法添加一个 `[UnitOfWork]` attribute. 或者手动使用 `IUnitOfWorkManager` 创建一个工作单元范围.

## 事务和异常行为

当一个事件发布,订阅的事件处理程序将立即执行.所以;

* 如果处理程序**抛出一个异常**,它会影响发布该事件的代码. 这意味着它在 `PublishAsync` 调用上获得异常. 因此如果你想隐藏错误,在事件处理程序中**使用try-catch**.
*如果在一个[工作单元](Unit-Of-Work.md)范围内执行的事件发布的代码,该事件处理程序也由工作单元覆盖. 这意味着,如果你的UOW是事务和处理程序抛出一个异常,事务会回滚.

## 预定义的事件

**发布实体创建,更新,删除事件**是常见的操作. ABP框架为所有的实体**自动**发布这些事件. 你只需要订阅相关的事件.

**示例: 订阅用户创建事件**

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

这个类订阅 `EntityCreatedEventData<IdentityUser>`,它在用户创建后发布. 你可能需要向新用户发送一封"欢迎"电子邮件.

这些事件有两种类型:过去时态的事件和进行时态的事件.

### 用过去时态事件

当相关工作单元完成且实体更改成功保存到数据库时,将发布带有过去时态的事件. 如果在这些事件处理程序上抛出异常,则**无法回滚**事务,因为事务已经提交.

事件类型;

* `EntityCreatedEventData<T>` 当实体创建成功后发布.
* `EntityUpdatedEventData<T>` 当实体更新成功后发布.
* `EntityDeletedEventData<T>` 当实体删除成功后发布.
* `EntityChangedEventData<T>` 当实体创建,更新,删除后发布. 如果你需要监听任何类型的更改,它是一种快捷方式 - 而不是订阅单个事件.

### 用于进行时态事件

带有进行时态的事件在完成事务之前发布(如果数据库事务由所使用的数据库提供程序支持). 如果在这些事件处理程序上抛出异常,它**会回滚**事务,因为事务还没有完成,更改也没有保存到数据库中.

事件类型;

* `EntityCreatingEventData<T>` 当新实体保存到数据库前发布.
* `EntityUpdatingEventData<T>` 当已存在实体更新到数据库前发布.
* `EntityDeletingEventData<T>` 删除实体前发布.
* `EntityChangingEventData<T>` 当实体创建,更新,删除前发布. 如果你需要监听任何类型的更改,它是一种快捷方式 - 而不是订阅单个事件.

#### 它是如何实现的?

在将更改保存到数据库时发布预构建事件;

* 对于 EF Core, 他们在 `DbContext.SaveChanges` 发布.
* 对于 MongoDB, 在你调用仓储的 `InsertAsync`, `UpdateAsync` 或 `DeleteAsync` 方法发布(因为MongoDB没有更改追踪系统).
