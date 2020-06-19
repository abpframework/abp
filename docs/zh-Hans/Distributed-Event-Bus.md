# 分布式事件总线

分布式事件总线系统允许**发布**和**订阅跨应用/服务边界**传输的事件. 你可以使用分布式事件总线在**微服务**或**应用程序**之间异步发送和接收消息.

## 提供程序

分布式事件总线系统提供了一个可以被任何提供程序实现的**抽象**. 有两种开箱即用的提供程序:

* `LocalDistributedEventBus` 是默认实现,实现作为进程内工作的分布式事件总线. 是的!如果没有配置真正的分布式提供程序,**默认实现的工作方式与[本地事件总线](Local-Event-Bus.md)一样**.
* `RabbitMqDistributedEventBus` 通过[RabbitMQ](https://www.rabbitmq.com/)实现分布式事件总线. 请参阅[RabbitMQ集成文档](Distributed-Event-Bus-RabbitMQ-Integration.md)了解如何配置它.

使用本地事件总线作为默认具有一些重要的优点. 最重要的是:它允许你编写与分布式体系结构兼容的代码. 您现在可以编写一个整体应用程序,以后可以拆分成微服务. 最好通过分布式事件而不是本地事件在边界上下文之间(或在应用程序模块之间)进行通信.

例如,[预构建的应用模块](Modules/Index.md)被设计成在分布式系统中作为服务工作,同时它们也可以在独立应用程序中作为模块工作,而不依赖于外部消息代理.

## 发布事件

以下介绍了两种发布分布式事件的方法.

### IDistributedEventBus

`IDistributedEventBus` can be [injected](Dependency-Injection.md) and used to publish a distributed event.

**示例: 产品的存货数量发生变化时发布分布式事件**

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

`PublishAsync` 方法需要一个参数:事件对象,它负责保持与事件相关的数据,是一个简单的普通类:


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

即使你不需要传输任何数据也需要创建一个类(在这种情况下为空类).

> `Eto` 是我们按照约定使用的**E**vent **T**ransfer **O**bjects(事件传输对象)的后缀. s虽然这不是必需的,但我们发现识别这样的事件类很有用(就像应用层上的[DTO](Data-Transfer-Objects.md) 一样).

#### 事件名称

`EventName`attribute是可选的,但建议使用. 如果不声明,事件名将事件名称将是事件类的全名. 这里是 `AbpDemo.StockCountChangedEto`.

#### 关于序列化的事件对象

事件传输对象**必须是可序列化**的,因为将其传输到流程外时，它们将被序列化/反序列化为JSON或其他格式.

避免循环引用,多态,私有setter,并提供默认(空)构造函数,如果你有其他的构造函数.(虽然某些序列化器可能会正常工作),就像DTO一样.

### 实体/聚合根类

[实体](Entities.md)不能通过依赖注入注入服务,但是在实体/聚合根类中发布分布式事件是非常常见的.

**示例: 在聚合根方法内发布分布式事件**

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

`AggregateRoot` 类定义了 `AddDistributedEvent` 来添加一个新的分布式事件,事件在聚合根对象保存(创建,更新或删除)到数据库时发布.

> 如果实体发布这样的事件,以可控的方式更改相关属性是一个好的实践,就像上面的示例一样 - `StockCount`只能由保证发布事件的 `ChangeStockCount` 方法来更改.

#### IGeneratesDomainEvents 接口

实际上添加分布式事件并不是 `AggregateRoot` 类独有的. 你可以为任何实体类实现 `IGeneratesDomainEvents`. 但是 `AggregateRoot` 默认实现了它简化你的工作.

> 不建议为不是聚合根的实体实现此接口,因为它可能不适用于此类实体的某些数据库提供程序. 例如它适用于EF Core,但不适用于MongoDB.

#### 它是如何实现的?

调用 `AddDistributedEvent` 不会立即发布事件. 当你将更改保存到数据库时发布该事件;

* 对于 EF Core, 它在 `DbContext.SaveChanges` 中发布.
* 对于 MongoDB, 它在你调用仓储的 `InsertAsync`, `UpdateAsync` 或 `DeleteAsync` 方法时发由 (因为MongoDB没有更改跟踪系统).

## 订阅事件

一个服务可以实现 `IDistributedEventHandler<TEvent>` 来处理事件.

**示例: 处理上面定义的`StockCountChangedEto`**

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

这就是全部.

* `MyHandler` 由ABP框架**自动发现**,并在发生 `StockCountChangedEto` 事件时调用 `HandleEventAsync`.
* 如果你使用的是分布式消息代理,比如RabbitMQ,ABP会自动**订阅消息代理上的事件**,获取消息执行处理程序.
* 如果事件处理程序成功执行(没有抛出任何异常),它将向消息代理发送**确认(ACK)**.

你可以在处理程序注入任何服务来执行所需的逻辑. 一个事件处理程序可以**订阅多个事件**,但是需要为每个事件实现 `IDistributedEventHandler<TEvent>`  接口.

> 事件处理程序类必须注册到依赖注入(DI),示例中使用了 `ITransientDependency`. 参阅[DI文档](Dependency-Injection.md)了解更多选项.
