# 自定义应用模块: 扩展实体

在某些情况下你可能希望为依赖模块中定义的实体添加一些额外的属性(和数据库字段). 本节将介绍一些实现这一目标的不同方法.

## Extra Properties

[Extra properties](Entities.md)是一种存储实体的一些额外数据但不用更改实体的方式. 实体应该实现 `IHasExtraProperties` 接口. 所有预构建模块定义的聚合根实体都实现了 `IHasExtraProperties` 接口,所以你可以在这些实体中存储额外的属性.

示例:

````csharp
//SET AN EXTRA PROPERTY
var user = await _identityUserRepository.GetAsync(userId);
user.SetProperty("Title", "My custom title value!");
await _identityUserRepository.UpdateAsync(user);

//GET AN EXTRA PROPERTY
var user = await _identityUserRepository.GetAsync(userId);
return user.GetProperty<string>("Title");
````

这种方法开箱即用并且非常简单,你可以使用不同的属性名称(如这里的`Title`)在同一时间存储多个属性.

对于EF Core额外的属性被格式化成单个 `JSON` 字符值串存储在数据库中. 对于MongoDB它们做为单独的字段存储.

参阅[实体文档](Entities.md)了解更多关于额外系统.

> 可以基于额外的属性执行**业务逻辑**. 你可以[重写服务方法](Customizing-Application-Modules-Overriding-Services.md). 然后获取或设置如上所示的值.

## 实体扩展 (EF Core)

如上所述,实体所有的额外属性都作为单个JSON对象存储在数据库表中. 它不适用复杂的场景,特别是在你需要的时候.

* 使用额外属性创建**索引**和**外键**.
* 使用额外属性编写**SQL**或**LINQ**(例如根据属性值搜索).
* 创建你**自己的实体**映射到相同的表,但在实体中定义一个额外属性做为 **常规属性**(参阅 [EF Core迁移文档](Entity-Framework-Core-Migrations.md)了解更多).

为了解决上面的问题,用于EF Core的ABP框架实体扩展系统允许你使用上面定义相同的额外属性API,但将所需的属性存储在单独的数据库表字段中.

假设你想要添加 `SocialSecurityNumber` 到[身份模块](Modules/Identity.md)的 `IdentityUser` 实体. 你可以使用 `ObjectExtensionManager` 类:

````csharp
ObjectExtensionManager.Instance
    .MapEfCoreProperty<IdentityUser, string>(
        "SocialSecurityNumber",
        (entityBuilder, propertyBuilder) =>
        {
            propertyBuilder.HasMaxLength(32);
        }
    );
````

* 你提供了 `IdentityUser` 作为实体名(泛型参数), `string` 做为新属性的类型, `SocialSecurityNumber` 做为属性名(也是数据库表的字段名).
* 你还需要提供一个使用[EF Core Fluent API](https://docs.microsoft.com/en-us/ef/core/modeling/entity-properties)定义数据库映射属性的操作.

> 必须在使用相关的 `DbContext` 之前执行此代码. 应用程序启动模板定义了一个名为 `YourProjectNameEfCoreEntityExtensionMappings` 的静态类. 你可以在此类中定义扩展确保在正确的时间执行它. 否则你需要自己处理.

定义实体扩展后你需要使用EF Core的[Add-Migration](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/powershell#add-migration)和[Update-Database](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/powershell#update-database)命令来创建code first迁移类并更新数据库.

然后你可以使用上一部分中定义的相同额外属性系统来操纵实体上的属性.

## 创建新实体映射到同一个数据库表/Collection

另一个方法是**创建你自己的实体**映射到**同一个数据库库**(对于MongoDB数据库是collection)

[应用程序启动模板](Startup-Templates/Application.md)的 `AppUser` 已经实现了这种方法. [EF Core迁移文档](Entity-Framework-Core-Migrations.md)描述了在这些情况下如何实现和管理**EF Core数据库迁移**. 这种方法同样适用于MongoDB,但你不需要处理数据库迁移问题.

## 创建一个拥有自己数据库表/Collection的新实体

映射你的实体到依赖模块的**已存在的表**有一些缺点;

* 你需要处理EF Core的**数据库迁移架构**. 需要特别注意迁移代码,特别是当你需要在实体间添加**关系**时.
* 你的应用程序数据库和模块数据库将是 **同一个物理数据库**. 通常需要时可以将模块数据库分开,但使用相同的表会对其进行限制.

如果你想要使你的实体或模块定义的实体**低耦合**,那么可以创建自己的数据库表/collection并且将你的实体映射到自己的数据库表.

在这种情况下你需要处理**同步问题**,尤其是你要**复制**相关实体的某些属性/字段时,有一些解决方案;

* 如果你构建的是一个 **单体** 应用程序(或者在同一进程管理你的实体和依赖模块的实体),那么你可以使用[本地事件总线](Local-Event-Bus.md)监听实体更改.
* 如果你构建的是一个 **分布式** 系统,模块的实体和你的实体在不同的 进程/服务 管理(创建/更新/删除),那么你可以使用[分布式事件总线](Distributed-Event-Bus.md)订阅实体的更改事件.

在你处理事件时,你可以在自己的数据库中更改自己的实体.

### 订阅本地事件总线

[本地事件总线](Local-Event-Bus.md)系统是发布和订阅同一应用程序中发生的事件的方法.

假设你想要获取 `IdentityUser` 实体的更改信息(创建,更改或删除). 你可以创建一个类实现 `ILocalEventHandler<EntityChangedEventData<IdentityUser>>` 接口.

````csharp
public class MyLocalIdentityUserChangeEventHandler :
    ILocalEventHandler<EntityChangedEventData<IdentityUser>>,
    ITransientDependency
{
    public async Task HandleEventAsync(EntityChangedEventData<IdentityUser> eventData)
    {
        var userId = eventData.Entity.Id;
        var userName = eventData.Entity.UserName;
        //...
    }
}
````

* `EntityChangedEventData<T>` 涵盖了给定实体的创建,更新或删除事件. 如果你需要你可以分别订阅创建,更新或删除事件(在同一个类或不同的类中).
* 这里的代码在**本地事务之外执行**,因为它监听 `EntityChanged` 事件. 如果当前[工作单元](Unit-Of-Work.md)是事务性的,你可以订阅 `EntityChangingEventData<T>` 事件,它在**同一本地(进行)事务**中执行事件处理.

> 提醒:这些方法需要在包含处理类的同一进程中更改 `IdentityUser` 实体. 即使在集群环境(同一应用程序的多个实例在不同的服务器进行),它也完美工作.

### 订阅分布式事件总线

[分布式事件总线](Distributed-Event-Bus.md)是在一个应用程序中发布事件,并在相同服务器或不同服务器运行的相同应用程序或不同应用程序中接收事件的方法.

假设你想要获取 `IdentityUser` 实体的创建,更改或删除信息. 你可以像以下一样创建一个类:

````csharp
public class MyDistributedIdentityUserChangeEventHandler :
    IDistributedEventHandler<EntityCreatedEto<EntityEto>>,
    IDistributedEventHandler<EntityUpdatedEto<EntityEto>>,
    IDistributedEventHandler<EntityDeletedEto<EntityEto>>,
    ITransientDependency
{
    public async Task HandleEventAsync(EntityCreatedEto<EntityEto> eventData)
    {
        if (eventData.Entity.EntityType == "Volo.Abp.Identity.IdentityUser")
        {
            var userId = Guid.Parse(eventData.Entity.KeysAsString);
            //...handle the "created" event
        }
    }

    public async Task HandleEventAsync(EntityUpdatedEto<EntityEto> eventData)
    {
        if (eventData.Entity.EntityType == "Volo.Abp.Identity.IdentityUser")
        {
            var userId = Guid.Parse(eventData.Entity.KeysAsString);
            //...handle the "updated" event
        }
    }

    public async Task HandleEventAsync(EntityDeletedEto<EntityEto> eventData)
    {
        if (eventData.Entity.EntityType == "Volo.Abp.Identity.IdentityUser")
        {
            var userId = Guid.Parse(eventData.Entity.KeysAsString);
            //...handle the "deleted" event
        }
    }
}
````

* 它实现了多个 `IDistributedEventHandler` 接口: **创建**,**更改**和**删除**,因为分布式事件总线单独发布事件,没有本地事件总线那样的"Changed"事件.
* 它订阅了 `EntityEto`, 这是一个通用的事件类,ABP框架针对所有类型的实体**自动发布**. 这就是为什么它检查**实体类型**(因为我们没有假设有对 `IdentityUser` 实体有安全的类型引用,所以它是字符串类型的).

预构建应用模块没有定义专门的事件类型(如`IdentityUserEto` - "ETO" 意思是 "事件传输对象"). 此功能在路线图上([关注这个issue](https://github.com/abpframework/abp/issues/3033)),一旦完成后,你就可以订阅独立的实体类型:

````csharp
public class MyDistributedIdentityUserCreatedEventHandler :
    IDistributedEventHandler<EntityCreatedEto<IdentityUserEto>>,
    ITransientDependency
{
    public async Task HandleEventAsync(EntityCreatedEto<IdentityUserEto> eventData)
    {
        var userId = eventData.Entity.Id;
        var userName = eventData.Entity.UserName;
        //...handle the "created" event
    }

    //...
}
````

* 这个处理程序只会在新用户创建时执行.

> 唯一预定义的专门事件类是 `UserEto`, 你可以订阅 `EntityCreatedEto<UserEto>` 获取用户创建时的通知. 此事件也适用于身份模块.

## 另请参阅

* [自定义已存在的模块](Customizing-Application-Modules-Guide.md)