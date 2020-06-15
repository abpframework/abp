# Unit of Work

ABP Framework's Unit Of Work (UOW) implementation provides an abstraction and control on a **database connection and transaction** scope in an application.

Once a new UOW started, it creates an **ambient scope** that is participated by **all the database operations** performed in the current scope and considered as a **single transaction boundary**. The operations are **committed** (on success) or **rolled back** (on exception) all together.

ABP's UOW system is;

* **Works conventional**, so most of the times you don't deal with UOW at all.
* **Database provider independent**.
* **Web independent**, that means you can create unit of work scopes in any type of applications beside web applications/services.

## Conventions

The following method types are considered as a unit of work:

* ASP.NET Core MVC **Controller Actions**.
* ASP.NET Core Razor **Page Handlers**.
* **Application service** methods.
* **Repository methods**.

A UOW automatically begins for these methods **except** if there is already a **surrounding (ambient)** UOW in action. Examples;

* If you call a [repository](Repositories.md) method and there is no UOW started yet, it automatically **begins a new transactional UOW** that involves all the operations done in the repository method and **commits the transaction** if the repository method **doesn't throw any exception.** The repository method doesn't know about UOW or transaction at all. It just works on a regular database objects (`DbContext` for [EF Core](Entity-Framework-Core.md), for example) and the UOW is handled by the ABP Framework.
* If you call an [application service](Application-Services.md) method, the same UOW system works just as explained above. If the application service method uses some repositories, the repositories **don't begin a new UOW**, but **participates to the current unit of work** started by the ABP Framework for the application service method.
* The same is true for an ASP.NET Core controller action. If the operation has started with a controller action, then the **UOW scope is the controller action's method body**.

All of these are automatically handled by the ABP Framework.

### Database Transaction Behavior

While the section above explains the UOW as it is database transaction, actually a UOW doesn't have to be transactional. By default;

* **HTTP GET** requests don't start a transactional UOW. They still starts a UOW, but **doesn't create a database transaction**.
* All other HTTP request types start a UOW with a database transaction, if database level transactions are supported by the underlying database provider.

This is because an HTTP GET request doesn't (and shouldn't) make any change in the database. You can change this behavior using the options explained below.

## Default Options

...

## Controlling the Unit Of Work

In some cases, you may want to change the conventional transaction scope, create inner scopes or fine control the transaction behavior. The following sections cover these possibilities.

### IUnitOfWorkEnabled Interface

This is an easy way to enable UOW for a class (or a hierarchy of classes) that is not unit of work by the conventions explained above.

**Example: Implement `IUnitOfWorkEnabled` for an arbitrary service**

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

Then `MyService` (and any class derived from it) methods will be UOW.

However, there are **some rules should be followed** in order to make it working;

* If you are **not injecting** the service over an interface (like `IMyService`), then the methods of the service must be `virtual` (otherwise, [dynamic proxy / interception](Dynamic-Proxying-Interceptors.md) system can not work).
* Only `async` methods (methods returning a `Task` or `Task<T>`) are intercepted. So, sync methods can not start a UOW.

> Notice that if `FooAsync` is called inside a UOW scope, then it already participates to the UOW without needing to the `IUnitOfWorkEnabled` or any other configuration.

### UnitOfWorkAttribute

`UnitOfWork` attribute provides much more possibility like enabling or disabling UOW and controlling the transaction behavior.

`UnitOfWork` attribute can be used for a **class** or a **method** level.

**Example: Enable UOW for a specific method of a class**

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

**Example: Enable UOW for all the methods of a class**

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

Again, the **same rules** are valid here:

* If you are **not injecting** the service over an interface (like `IMyService`), then the methods of the service must be `virtual` (otherwise, [dynamic proxy / interception](Dynamic-Proxying-Interceptors.md) system can not work).
* Only `async` methods (methods returning a `Task` or `Task<T>`) are intercepted. So, sync methods can not start a UOW.

#### UnitOfWorkAttribute Properties

* `IsTransactional` (`bool?`): Used to set whether the UOW should be transactional or not. **Default value is `null`**. if you leave it `null`, it is determined automatically based on the conventions and the configuration.
* `TimeOut` (`int?`): Used to set the timeout value for this UOW. **Default value is `null`** and fallbacks to the default configured value.
* `IsolationLevel` (`IsolationLevel?`): Used to set the [isolation level](https://docs.microsoft.com/en-us/dotnet/api/system.data.isolationlevel) of the database transaction, if the UOW is transactional.
* `IsDisabled` (`bool`): Used to disable the UOW for the current method/class.

> If a method is called in an ambient UOW scope, then the `UnitOfWork` attribute is ignored and the method participates to the surrounding transaction in any way.

**Example: Disable UOW for a controller action**

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