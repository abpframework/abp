# Unit of Work

ABP Framework's Unit Of Work (UOW) implementation provides an abstraction and control on a **database transaction** scope in an application.

Once a new UOW started, it creates an **ambient scope** that is participated by **all the database operations** performed in the current scope and considered as a **single transaction**, **committed** (on success) or **rolled back** (on exception) all together.

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

A UOW automatically begins for these methods except if there is already a **surrounding (ambient)** UOW in action. Examples;

* If you call a [repository](Repositories.md) method and there is no UOW started yet, it automatically **begins a new transactional UOW** that involves all the operations done in the repository method and **commits the transaction** if the repository method **doesn't throw any exception.** The repository method doesn't know about UOW or transaction at all. It just works on a regular database objects (DbContext for [EF Core](Entity-Framework-Core.md), for example) and the UOW is handled by the ABP Framework.
* If you call an [application service](Application-Services.md) method, the same UOW system works just as explained above. If the application service method uses some repositories, the repositories **don't begin a new UOW**, but **participates to the current unit of work** started by the ABP Framework for the application service method.
* The same is true for an ASP.NET Core controller action. If the operation has started with a controller action, then the **UOW scope is the controller action method body**.

All of these are automatically handled by the ABP Framework. The rest of this document explains the UOW system in details and options provided to a fine control the UOW system.

