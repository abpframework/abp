# 工作单元

ABP框架的工作单元(UOW)实现提供了对应用程序中的**数据库连接和事务范围**的抽象和控制.

一旦一个新的UOW启动,它将创建一个**环境作用域**,当前作用域中执行的**所有数据库操作**都将参与该作用域并将其视为单个事务边界. 操作一起**提交**(成功时)或**回滚**(异常时).

ABP的UOW系统是;

* **按约定工作**, 所以大部分情况下你不需要处理UOW.
* **数据库提供者独立**.
* **Web独立**, 这意味着你可以在Web应用程序/服务之外的任何类型的应用程序中创建工作单元作用域.

## 约定

以下方法类型被认为是一个工作单元:

* ASP.NET Core MVC **Controller Actions**.
* ASP.NET Core Razor **Page Handlers**.
* **应用程序** 方法.
* **仓储方法**.

UOW自动针对这些方法开始,除非**周围已经有一个(环境)**UOW在运行.示例;

* 如果你调用一个[仓储](Repositories.md)方法,但还没有启动UOW,它将自动**启动一个新的事务UOW**,其中包括在仓储方法中完成的所有操作,如果仓储方法没有抛出任何异常,则**提交事务**. 仓储方法根本不知道UOW或事务. 它只在一个常规的数据库对象上工作(例如用于[EF Core](Entity-Framework-Core.md)的`DbContext`),而UOW由ABP框架处理.
* 如果调用[应用服务](Application-Services.md)方法,则相同的UOW系统将按上述说明工作. 如果应用服务方法使用某些仓储,这些仓储**不会开始新的UOW**,而是**参与由ABP框架为应用程序服务方法启动的当前工作单元中**.
* ASP.NET Core控制器操作也是如此. 如果操作以控制器action开始,**UOW范围是控制器action的方法主体**.

所有这些都是由ABP框架自动处理的.

### 数据库事务行为

虽然上一节解释了UOW是数据库事务,但实际上UOW不必是事务性的. 默认情况下;

* **HTTP GET**请求不会启动事务性UOW. 它们仍然启动UOW,但**不创建数据库事务**.
* 如果底层数据库提供程序支持数据库事务,那么所有其他HTTP请求类型都使用数据库事务启动UOW.

这是因为HTTP GET请求不会(也不应该)在数据库中进行任何更改. 你可以使用下面解释的选项来更改此行为.

## 默认选项

`AbpUnitOfWorkDefaultOptions` 用于配置工作单元系统的默认选项.在你的[模块](Module-Development-Basics.md)的 `ConfigureServices` 方法中配置选项.

**示例: 完全禁用数据库事务**

````csharp
Configure<AbpUnitOfWorkDefaultOptions>(options =>
{
    options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled;
});
````

### 选项属性

* `TransactionBehavior` (`enum`: `UnitOfWorkTransactionBehavior`). 配置事务行为的全局点. 默认值为 `Auto` ,按照上面"*数据库事务行为"*一节的说明工作. 你可以使用此选项启用(甚至对于HTTP GET请求)或禁用事务.
* `TimeOut` (`int?`): 用于设置UOW的超时值. **默认值是 `null`** 并使用基础数据库提供程序的默认值.
* `IsolationLevel` (`IsolationLevel?`): 如果UOW是事务性的用于设置数据库事务的[隔离级别](https://docs.microsoft.com/en-us/dotnet/api/system.data.isolationlevel).

## 控制工作单元

在某些情况下你可能希望更改常规事务作用域,创建内部作用域或精细控制事务行为. 下面几节将介绍这些可能性.

### IUnitOfWorkEnabled 接口

这是为不是按照上面解释的约定作为工作单元的类(或类的层次结构)启用UOW的一种简单方法.

**示例: 为任意服务实现 `IUnitOfWorkEnabled`**

````csharp
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace AbpDemo
{
    public class MyService : ITransientDependency, IUnitOfWorkEnabled
    {
        public virtual async Task FooAsync()
        {
            //this is a method with a UOW scope
        }
    }
}
````

然后 `MyService`(和它的派生类)方法都将是UOW.

但是为了使它工作,**有些规则应该被遵守**;

* 如果你不是通过接口(如`IMyService`)注入服务,则服务的方法必须是 `virtual` 的(否则[动态代理/拦截](Dynamic-Proxying-Interceptors.md)系统将无法工作).
* 仅异步方法(返回`Task`或`Task<T>`的方法)被拦截. 因此同步方法无法启动UOW.

> 注意,如果 `FooAsync` 在UOW作用域内被调用,那么它已经参与了UOW,不需要 `IUnitOfWorkEnabled` 或其他配置.

### UnitOfWorkAttribute

`UnitOfWork` attribute提供了更多的可能性,比如启用或禁用UOW和控制事务行为.

`UnitOfWork` attribute可以用于**类**或**方法**级别.

**示例: 为类的特定方法启用UOW**

````csharp
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace AbpDemo
{
    public class MyService : ITransientDependency
    {
        [UnitOfWork]
        public virtual async Task FooAsync()
        {
            //this is a method with a UOW scope
        }
        
        public virtual async Task BarAsync()
        {
            //this is a method without UOW
        }
    }
}
````

**示例: 为类的所有方法启用UOW**

````csharp
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace AbpDemo
{
    [UnitOfWork]
    public class MyService : ITransientDependency
    {
        public virtual async Task FooAsync()
        {
            //this is a method with a UOW scope
        }
        
        public virtual async Task BarAsync()
        {
            //this is a method with a UOW scope
        }
    }
}
````

**同样的规则**也适用于此:

* 如果你不是通过接口(如`IMyService`)注入服务,则服务的方法必须是 `virtual` 的(否则[动态代理/拦截](Dynamic-Proxying-Interceptors.md)系统将无法工作).
* 仅异步方法(返回`Task`或`Task<T>`的方法)被拦截. 因此同步方法无法启动UOW.

#### UnitOfWorkAttribute 属性

* `IsTransactional` (`bool?`): 用于设置UOW是否是事务性的. **默认值为 `null`**. 如果你让它为 `null`,它会通过约定和配置自动确定.
* `TimeOut` (`int?`): 用于设置UOW的超时值. **默认值为 `null`**并回退到默认配置值.
* `IsolationLevel` (`IsolationLevel?`): 如果UOW是事务的,用于设置数据库事务的[隔离级别](https://docs.microsoft.com/en-us/dotnet/api/system.data.isolationlevel). 如果未设置,则使用默认值.
* `IsDisabled` (`bool`): 用于禁用当前方法/类的UOW.

> 如果在环境UOW作用域内调用方法,将忽略 `UnitOfWork` 属性,并且该方法参与周围的事务.

**示例: 为控制器action禁用UOW**

````csharp
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Uow;

namespace AbpDemo.Web
{
    public class MyController : AbpController
    {
        [UnitOfWork(IsDisabled = true)]
        public virtual async Task FooAsync()
        {
            //...
        }
    }
}
````

## IUnitOfWorkManager

`IUnitOfWorkManager` 是用于控制工作单元系统的主要服务. 下面的部分解释了如何使用此服务(大多数时候你并不需要).

### 开始新的工作单元

`IUnitOfWorkManager.Begin` 方法用于创建一个新的UOW作用域.

**示例: 创建一个新的非事务性UOW作用域**

````csharp
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace AbpDemo
{
    public class MyService : ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public MyService(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }
        
        public virtual async Task FooAsync()
        {
            using (var uow = _unitOfWorkManager.Begin(
                requiresNew: true, isTransactional: false
            ))
            {
                //...
                
                await uow.CompleteAsync();
            }
        }
    }
}
````

`Begin` 方法有以下可选参数:

* `requiresNew` (`bool`): 设置为 `true` 可忽略周围的工作单元,并使用提供的选项启动新的UOW. **默认值为`false`. 如果为`false`,并且周围有UOW,则 `Begin` 方法实际上不会开始新的UOW,而是以静默方式参与现有的UOW**.
* `isTransactional` (`bool`). 默认为 `false`.
* `isolationLevel` (`IsolationLevel?`): 如果UOW是事务的,用于设置数据库事务的[隔离级别](https://docs.microsoft.com/en-us/dotnet/api/system.data.isolationlevel). 如果未设置,则使用默认值.
* `TimeOut` (`int?`): 用于设置UOW的超时值. **默认值为 `null`**并回退到默认配置值.

### 当前工作单元

如上所述UOW是环境的. 如果需要访问当前的工作单元,可以使用 `IUnitOfWorkManager.Current` 属性.

**示例: 获取当前UOW**

````csharp
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace AbpDemo
{
    public class MyProductService : ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public MyProductService(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }
        
        public async Task FooAsync()
        {
            var uow = _unitOfWorkManager.Current;
            //...
        }
    }
}
````

`Current` 属性返回一个 `IUnitOfWork` 对象.

> 如果没有周围的工作单元,则**当前工作单元可以为`null`**. 如上所述,如果你的类是常规的UOW类,你将其手动设置为UOW或在UOW作用域内调用它,那么该值就不会为 `null`.

#### SaveChangesAsync

`IUnitOfWork.SaveChangesAsync()` 方法将到目前为止的所有更改保存到数据库中. 如果你正在使用EF Core,它的行为完全相同. 如果当前UOW是事务性的,即使已保存的更改也可以在错误时回滚(对于支持的数据库提供程序).

**示例: 插入实体后保存更改以获取其自动增量ID**

````csharp
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace AbpDemo
{
    public class CategoryAppService : ApplicationService, ICategoryAppService
    {
        private readonly IRepository<Category, int> _categoryRepository;

        public CategoryAppService(IRepository<Category, int> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<int> CreateAsync(string name)
        {
            var category = new Category {Name = name};
            await _categoryRepository.InsertAsync(category);
            
            //Saving changes to be able to get the auto increment id
            await UnitOfWorkManager.Current.SaveChangesAsync();
            
            return category.Id;
        }
    }
}
````

示例的 `Category` [实体](Entities.md)使用自动递增的 `int` 主键. 自动增量PK需要将实体保存到数据库中来获得新实体的ID.

示例是从基类 `ApplicationService` 派生的[应用服务](Application-Services.md), `IUnitOfWorkManager` 服务已经作为 `UnitOfWorkManager` 属性注入,所以无需手动注入.

获取当前UOW非常常见,所以还有一个 `UnitOfWorkManager.Current` 的快捷属性 `CurrentUnitOfWork`. 所以可以对上面的例子进行以下更改:

````csharp
await CurrentUnitOfWork.SaveChangesAsync();
````

##### SaveChanges() 的替代方法

由于经常需要在插入,更新或删除实体后保存更改,相应的[仓储](Repositories.md)方法有一个可选的 `autoSave` 参数. 可以将上面的 `CreateAsync` 方法按如下重写:

````csharp
public async Task<int> CreateAsync(string name)
{
    var category = new Category {Name = name};
    await _categoryRepository.InsertAsync(category, autoSave: true);
    return category.Id;
}
````

如果你的目的只是在创建/更新/删除实体后保存更改,建议你使用 `autoSave` 选项,而不是手动使用 `CurrentUnitOfWork.SaveChangesAsync()`.

> **Note-1**: 当工作单元结束而没有任何错误时,所有更改都会自动保存. 所以除非确实需要,否则不要调用 `SaveChangesAsync()` 和设置 `autoSave` 为 `true`.
> 
> **Note-2**: 如果你使用 `Guid` 作为主键,则无需插入时保存来获取生成的id,因为 `Guid` 主键是在应用程序中设置的,创建新实体后立即可用.

#### IUnitOfWork 其他属性/方法

* `OnCompleted` 方法获得一个回调动作,当工作单元成功完成时调用(在这里你可以确保所有更改都保存了).
* `Failed` 和 `Disposed` 事件可以用于UOW失败和被销毁的通知.
* `Complete` 和 `Rollback` 方法用于完成(提交)或回滚当前 UOW, 通常ABP框架在内部使用,如果你使用 `IUnitOfWorkManager.Begin` 方法手动启动事务,那么你可以手动使用这些方法.
* `Options` 可用于获取启动UOW时使用的选项.
* `Items` 字典可用于在同一工作单元内存储和获取任意对象,可以实现自定义逻辑.

## ASP.NET Core 集成

工作单元系统已完全集成到ASP.NET Core. 它为UOW系统定义了动作过滤器和页面过滤器. 当你使用ASP.NET Core MVC控制器或Razor页面时,它可以正常工作.

> 使用ASP.NET Core时,通常你不需要做任何操作配置UOW.

### 工作单元中间件

`AbpUnitOfWorkMiddleware` 是可以在ASP.NET Core请求管道中启用UOW的中间件. 如果你需要扩大UOW范围以涵盖其他一些中间件,可以这样做.

**示例:**

````csharp
app.UseUnitOfWork();
app.UseConfiguredEndpoints();
````
