## Module Development

### Introduction

This document describes the best practices for who want to develop modules that satisfies the following specifications:

* Develop the module that applies the **Domain Driven Design** patterns & best practices.
* Develop the module as DBMS and **ORM independent**.
* Develop the module that can be used as a remote service / **microservice** as well as can be integrated to a **monolithic** application.

### Data Access Layer

The module should be completely independent of any DBMS and and ORM.

- **Do not** use `IQueryable<TEntity>` features in the application code (domain, application... layers) except the data access layer.
- **Do** create separated packages (projects/assemblies/libraries) for each ORM integration (like *Company.Module.EntityFrameworkCore* and *Company.Module.MongoDB*).
- **Do** always use the specifically created repository interface (like `IIdentityUserRepository`) from the application code (as developed specified below). **Do not** use generic repository interfaces (like `IRepository<IdentityUser, Guid>`).

#### Repositories

* **Do** define a repository interface (and create its corresponding implementations) for each aggregate root.

For the example aggregate root:

````C#
public class IdentityUser : AggregateRoot<Guid>
{
    //...
}
````

Define the repository interface as below:

````C#
public interface IIdentityUserRepository : IBasicRepository<IdentityUser, Guid>
{
    //...
}
````

* **Do** define repository interfaces in the **domain layer**.
* **Do not** inherit the repository interface from `IRepository` interfaces. Because it inherits `IQueryable` and the repository should not expose `IQueryable` to the application.
* **Do** inherit the repository interface from `IBasicRepository<TEntity, TKey>` (as normally) or a lower-featured interface, like `IReadOnlyRepository<TEntity, TKey>` (if it's needed).
* **Do not** define repositories for entities those are **not aggregate roots**.
* **Do** define all repository methods as **asynchronous**.
* **Do** add a `cancellationToken` parameter to every method of the repository. 
* **Do** create a **synchronous extension** method for each asynchronous repository method.
* ...

#### Integrations

##### Entity Framework Core

##### MongoDB