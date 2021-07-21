# 分布式事件总线

分布式事件总线系统允许**发布**和**订阅跨应用/服务边界**传输的事件. 你可以使用分布式事件总线在**微服务**或**应用程序**之间异步发送和接收消息.

## 提供程序

分布式事件总线系统提供了一个可以被任何提供程序实现的**抽象**. 有四种开箱即用的提供程序:

* `LocalDistributedEventBus` 是默认实现,实现作为进程内工作的分布式事件总线. 是的!如果没有配置真正的分布式提供程序,**默认实现的工作方式与[本地事件总线](Local-Event-Bus.md)一样**.
* `RabbitMqDistributedEventBus` 通过[RabbitMQ](https://www.rabbitmq.com/)实现分布式事件总线. 请参阅[RabbitMQ集成文档](Distributed-Event-Bus-RabbitMQ-Integration.md)了解如何配置它.
* `KafkaDistributedEventBus` 通过[Kafka](https://kafka.apache.org/)实现分布式事件总线. 请参阅[Kafka集成文档](Distributed-Event-Bus-Kafka-Integration.md)了解如何配置它.
* `RebusDistributedEventBus` 通过[Rebus](http://mookid.dk/category/rebus/)实现分布式事件总线. 请参阅[Rebus集成文档](Distributed-Event-Bus-Rebus-Integration.md)了解如何配置它.

使用本地事件总线作为默认具有一些重要的优点. 最重要的是:它允许你编写与分布式体系结构兼容的代码. 您现在可以编写一个整体应用程序,以后可以拆分成微服务. 最好通过分布式事件而不是本地事件在边界上下文之间(或在应用程序模块之间)进行通信.

例如,[预构建的应用模块](Modules/Index.md)被设计成在分布式系统中作为服务工作,同时它们也可以在独立应用程序中作为模块工作,而不依赖于外部消息代理.

## 发布事件

以下介绍了两种发布分布式事件的方法.

### IDistributedEventBus

可以[注入](Dependency-Injection.md) `IDistributedEventBus` 并且使用发布分布式事件.

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

## 预定义的事件

如果你配置,ABP框架会为[实体](Entities.md)**自动发布创建,更新和删除**分布式事件.

### 事件类型

有三种预定义的事件类型:

* `EntityCreatedEto<T>` 是实体 `T` 创建后发布.
* `EntityUpdatedEto<T>` 是实体 `T` 更新后发布.
* `EntityDeletedEto<T>` 是实体 `T` 删除后发布.

这些都是泛型的, `T` 实际上是**E**vent **T**ransfer **O**bject (ETO)的类型,而不是实体的类型,因为实体对象不能做为事件数据传输,所以通常会为实体类定义一个ETO类,如为 `Product` 实体定义 `ProductEto`.

### 订阅事件

订阅自动事件与订阅常规分布式事件相同.

**示例: 产品更新后获取通知**

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

* `MyHandler` 实现了 `IDistributedEventHandler<EntityUpdatedEto<ProductEto>>`.

### 配置

你可以在[模块](Module-Development-Basics.md)的 `ConfigureServices` 中配置 `AbpDistributedEntityEventOptions`添加选择器.

**示例: 配置示例**

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

* 最后一个提供了灵活性来决定是否应该针对给定的实体类型发布事件. 返回 `true` 代表为该 `Type` 发布事件.

你可以添加多个选择器. 如果选择器之一与实体类型匹配,则将其选中.

### 事件传输对象

一旦你为一个实体启用了**自动事件**,ABP框架就会为实体上的更改发布事件. 如果你没有为实体指定对应的**E**vent **T**ransfer **O**bject(ETO), ABP框架会使用一个标准类型 `EntityEto`,它只有两个属性:

* `EntityType` (`string`): 实体类的全名(包括命令空间).
* `KeysAsString` (`string`): 已更改实体的主键.如果它只有一个主键,这个属性将是主键值. 对于复合键,它包含所有用`,`(逗号)分隔的键.

因此可以实现 `IDistributedEventHandler<EntityUpdatedEto<EntityEto>>` 订阅事件. 但是订阅这样的通用事件不是一个好方法,你可以为实体类型定义对应的ETO.

**示例: 为 `Product` 声明使用 `ProductDto`**

````csharp
Configure<AbpDistributedEntityEventOptions>(options =>
{
    options.AutoEventSelectors.Add<Product>();
    options.EtoMappings.Add<Product, ProductEto>();
});
````

在这个示例中;

* 添加选择器允许发布 `Product` 实体的创建,更新和删除事件.
* 配置为使用 `ProductEto` 作为事件传输对象来发布与 `Product` 相关的事件.

分布式事件系统使用[对象到对象的映射](Object-To-Object-Mapping.md)系统来映射 `Product` 对象到 `ProductEto` 对象,你需要配置映射. 请参阅可以对象到对象映射文档了解所有选项,下面的示例展示了如何使用[AutoMapper](https://automapper.org/)库配置它.

**示例: 使用AutoMapper配置 `Product` 到 `ProductEto` 映射**

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

此示例使用AutoMapper的 `AutoMap` 属性配置的映射. 你可以创建一个配置文件类代替. 请参阅AutoMapper文档了解更多选项.

## 异常处理

ABP提供了异常处理, 它会进行重试并且重试失败后移动到死信队列.

启用异常处理:

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<AbpEventBusOptions>(options =>
    {
        options.EnabledErrorHandle = true;
        options.UseRetryStrategy();
    });
}
```

* `EnabledErrorHandle` 用于启用异常处理.
* `UseRetryStrategy` 用于启用重试.

当一个异常抛出,它会每3秒重试一次直到最大重试次数(默认是3)并且移动到错误队列, 你可以更改重试次数,重试间隔和死信队列名称:

```csharp
PreConfigure<AbpEventBusOptions>(options =>
{
    options.DeadLetterName = "dead_queue";
    options.UseRetryStrategy(retryStrategyOptions =>
    {
        retryStrategyOptions.IntervalMillisecond = 0;
        retryStrategyOptions.MaxRetryAttempts = 1;
    });
});
```

### 错误处理选择器

默认所有的事件类型都会被处理, 你可以使用 `AbpEventBusOptions` 的 `ErrorHandleSelector` 来更改它:

```csharp
PreConfigure<AbpEventBusOptions>(options =>
{
    options.ErrorHandleSelector = type => type == typeof(MyExceptionHandleEventData);
});
```

`options.ErrorHandleSelector` 实际上是一个类型类型谓词列表. 你可以编写lambda来定义你的过滤.

### 自定义异常处理

ABP定义了 `IEventErrorHandler` 接口并且由提供程序实现, 你可以通过[依赖注入](Dependency-Injection.md)替换它.