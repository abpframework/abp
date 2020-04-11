# ASP.NET Boilerplate v5+ to ABP Framework Migration

ABP Framework is **the successor** of the open source [ASP.NET Boilerplate](https://aspnetboilerplate.com/) framework. This guide aims to help you to **migrate your existing solutions** (you developed with the ASP.NET Boilerplate framework) to the ABP Framework.

## Introduction

**ASP.NET Boilerplate** is being **actively developed** [since 2013](https://github.com/aspnetboilerplate/aspnetboilerplate/graphs/contributors). It is loved, used and contributed by the community. It started as a side project of [a developer](http://halilibrahimkalkan.com/), but now it is officially maintained and improved by the company [Volosoft](https://volosoft.com/) in addition to the great community support.

ABP Framework has the same goal of the ASP.NET Boilerplate framework: **Don't Repeat Yourself**! It provides infrastructure, tools and startup templates to make a developer's life easier while developing enterprise software solutions.

See [the introduction blog post](https://blog.abp.io/abp/Abp-vNext-Announcement) if you wonder why we needed to re-write the ASP.NET Boilerplate framework.

### Should I Migrate?

No, you don't have to!

* ASP.NET Boilerplate is still in active development and maintenance.
* It also works on the latest ASP.NET Core and related libraries and tools. It is up to date.

However, if you want to take the advantage of the new ABP Framework [features](https://abp.io/features) and the new architecture opportunities (like support for NoSQL databases, microservice compatibility, advanced modularity), you can use this document as a guide.

### What About the ASP.NET Zero?

[ASP.NET Zero](https://aspnetzero.com/) is a commercial product developed by the core ASP.NET Boilerplate team, on top of the ASP.NET Boilerplate framework. It provides pre-built application [features](https://aspnetzero.com/Features), code generation tooling and a nice looking modern UI. It is trusted and used by thousands of companies from all around the World.

We have created the [ABP Commercial](https://commercial.abp.io/) as an alternative to the ASP.NET Zero. ABP Commercial is more modular and upgradeable compared to the ASP.NET Zero. It currently has less features compared to ASP.NET Zero, but the gap will be closed by the time (it also has some features don't exist in the ASP.NET Zero).

We think ASP.NET Zero is still a good choice while starting a new application. It is production ready and mature solution delivered as a full source code. It is being actively developed and we are constantly adding new features.

We don't suggest to migrate your ASP.NET Zero based solution to the ABP Commercial if;

* Your ASP.NET Zero solution is mature and it is in maintenance rather than a rapid development.
* You don't have enough development time to perform the migration.
* A monolithic solution fits in your business.
* You've customized existing ASP.NET Zero features too much based on your requirements.

We also suggest you to compare the features of two products based on your needs.

If you have an ASP.NET Zero based solution and want to migrate to the ABP Commercial, this guide will also help you.

### ASP.NET MVC 5.x Projects

The ABP Framework doesn't support ASP.NET MVC 5.x, it only works with ASP.NET Core. So, if you migrate your ASP.NET MVC 5.x based projects, you will also deal with the .NET Core migration.

## The Migration Progress

We've designed the ABP Framework by **getting the best parts** of the ASP.NET Boilerplate framework, so it will be familiar to you if you've developed ASP.NET Boilerplate based applications.

In the ASP.NET Boilerplate, we have not worked much on the UI side, but used some free themes (we've used [metronic theme](https://keenthemes.com/metronic/) for ASP.NET Zero on the other side). In the ABP Framework, we worked a lot on the UI side (especially for the MVC / Razor Pages UI, because Angular already has a good modular system of its own). So, the **most challenging part** of the migration will be the **User Interface** of your solution.

ABP Framework is (and ASP.NET Boilerplate was) designed based on the [Domain Driven Design](https://docs.abp.io/en/abp/latest/Domain-Driven-Design) patterns & principles and the startup templates are layered based on the DDD layers. So, this guide respects to that layering model and explains the migration layer by layer.

## Creating the Solution

First step of the migration is to create a new solution. We suggest you to create a fresh new project using [the startup templates](https://abp.io/get-started) (see [this document](https://docs.abp.io/en/commercial/latest/getting-started) for the ABP Commercial).

After creating the project and running the application, you can copy your code from your existing solution to the new solution step by step, layer by layer.

### About Pre-Built Modules

The startup projects for the ABP Framework use the [pre-built modules](https://docs.abp.io/en/abp/latest/Modules/Index) (not all of them, but the essentials) and themes as NuGet/NPM packages. So, you don't see the source code of the modules/themes in your solution. This has an advantage that you can easily update these packages when a new version is released. However, you can not easily customize them as their source code in your hands.

We suggest to continue to use these modules as package references, in this way you can get new features easily (see [abp update command](https://docs.abp.io/en/abp/latest/CLI#update)). In this case, you have a few options to customize or extend the functionality of the used modules;

* You can create your own entity and share the same database table with an entity in a used module. An example of this is the `AppUser` entity comes in the startup template.
* You can [replace](https://docs.abp.io/en/abp/latest/Dependency-Injection#replace-a-service) a domain service, application service, controller, page model or other types of services with your own implementation. We suggest you to inherit from the existing implementation and override the method you need.
* You can replace a `.cshtml` view, page, view component, partial view... with your own one using the [Virtual File System](https://docs.abp.io/en/abp/latest/Virtual-File-System).
* You can override javascript, css, image or any other type of static files using the [Virtual File System](https://docs.abp.io/en/abp/latest/Virtual-File-System).

More extend/customization options will be developed and documented by the time. However, if you need to fully change the module implementation, it is best to add the [source code](https://github.com/abpframework/abp/tree/dev/modules) of the related module into your own solution and remove the package dependencies.

The source code of the modules and the themes are [MIT](https://opensource.org/licenses/MIT) licensed, you can fully own and customize it without any limitation (for the ABP Commercial, you can download the source code of a [module](https://commercial.abp.io/modules)/[theme](https://commercial.abp.io/themes) if you have a [license](https://commercial.abp.io/pricing) type that includes the source code).

## The Domain Layer

Most of your domain layer code will remain same, while you need to perform some minor changes in your domain objects.

### Aggregate Roots & Entities

The ABP Framework and the ASP.NET Boilerplate both have the `IEntity` and `IEntity<T>` interfaces and `Entity` and `Entity<T>` base classes to define entities but they have some differences.

If you have an entity in the ASP.NET Boilerplate application like that:

````csharp
public class Person : Entity //Default PK is int for the ASP.NET Boilerplate
{
    ...
}
````

Then your primary key (the `Id` property in the base class) is `int` which is the **default primary key** (PK) type for the ASP.NET Boilerplate. If you want to set another type of PK, you need to explicitly declare it:

````csharp
public class Person : Entity<Guid> //Set explicit PK in the ASP.NET Boilerplate
{
    ...
}
````

ABP Framework behaves differently and expects to **always explicitly set** the PK type:

````csharp
public class Person : Entity<Guid> //Set explicit PK in the ASP.NET Boilerplate
{
    ...
}
````

`Id` property (and the corresponding PK in the database) will be `Guid` in this case.

#### Composite Primary Keys

ABP Framework also has a non-generic `Entity` base class, but this time it has no `Id` property. Its purpose is to allow you to create entities with composite PKs. See [the documentation](https://docs.abp.io/en/abp/latest/Entities#entities-with-composite-keys) to learn more about the composite PKs.

#### Aggregate Root

It is best practice now to use the `AggregateRoot` base class instead of `Entity` for aggregate root entities. See [the documentation](https://docs.abp.io/en/abp/latest/Entities#aggregateroot-class) to learn more about the aggregate roots.

In opposite to the ASP.NET Boilerplate, the ABP Framework creates default repositories (`IRepository<T>`) **only for the aggregate roots**. It doesn't create for other types derived from the `Entity`.

If you still want to create default repositories for all entity types, find the *YourProjectName*EntityFrameworkCoreModule class in your solution and change `options.AddDefaultRepositories()` to `options.AddDefaultRepositories(includeAllEntities: true)` (it may be already like that for the application startup template).

#### Migrating the Existing Entities

We suggest & use the GUID as the PK type for all the ABP Framework modules. However, you can continue to use your existing PK types to migrate your database tables easier.

The challenging part will be the primary keys of the ASP.NET Boilerplate related entities, like Users, Roles, Tenants, Settings... etc. Our suggestion is to copy data from existing database to the new database tables using a tool or in a manual way (be careful about the foreign key values).

#### Documentation

See the documentation for details on the entities:

* [ASP.NET Boilerplate - Entity documentation](https://aspnetboilerplate.com/Pages/Documents/Entities)
* [ABP Framework - Entity documentation](https://docs.abp.io/en/abp/latest/Entities)

### Repositories

> ABP Framework creates default repositories (`IRepository<T>`) **only for the aggregate roots**. It doesn't create for other types derived from the `Entity`. See the "Aggregate Root" section above for more information.

The ABP Framework and the ASP.NET Boilerplate both have the default generic repository system, but has some differences.

#### Injecting the Repositories

In the ASP.NET Boilerplate, there are two default repository interfaces you can directly inject and use:

* `IRepository<TEntity>` (e.g. `IRepository<Person>`) is used for entities with `int` primary key (PK) which is the default PK type.
* `IRepository<TEntity, TKey>` (e.g. `IRepository<Person, Guid>`) is used for entities with other types of PKs.

ABP Framework doesn't have a default PK type, so you need to **explicitly declare the PK type** of your entity, like `IRepository<Person, int>` or `IRepository<Person, Guid>`.

ABP Framework also has the `IRepository<TEntity>` (without PK), but it is mostly used when your entity has a composite PK (because this repository has no methods work with the `Id` property). See [the documentation](https://docs.abp.io/en/abp/latest/Entities#entities-with-composite-keys) to learn more about the **composite PKs**.

#### Restricted Repositories

ABP Framework additionally provides a few repository interfaces:

* `IBasicRepository<TEntity, TKey>` has the same methods with the `IRepository` except it doesn't have `IQueryable` support. It can be useful if you don't want to expose complex querying code to the application layer. In this case, you typically want to create custom repositories to encapsulate the querying logic. It is also useful for database providers those don't support `IQueryable`.
* `IReadOnlyRepository<TEntity,TKey>` has the methods get data from the database, but doesn't contain any method change the database.
* `IReadOnlyBasicRepository<TEntity, TKey>` is similar to the read only repository but also doesn't support `IQueryable`.

All the interfaces also have versions without `TKey` (like ``IReadOnlyRepository<TEntity>`) those can be used for composite PKs just like explained above.

#### GetAll() vs IQueryable

ASP.NET Boilerplate's repository has a `GetAll()` method that is used to obtain an `IQueryable` object to execute LINQ on it. An example application service calls the `GetAll()` method:

````csharp
public class PersonAppService : ApplicationService, IPersonAppService
{
    private readonly IRepository<Person, Guid> _personRepository;

    public PersonAppService(IRepository<Person, Guid> personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task DoIt()
    {
        var people = await _personRepository
            .GetAll() //GetAll() returns IQueryable
            .Where(p => p.BirthYear > 2000) //Use LINQ extension methods
            .ToListAsync();
    }
}
````

ABP Framework's repository doesn't have this method. Instead, it implements the `IQueryable` itself. So, you can directly use LINQ on the repository:

````csharp
public class PersonAppService : ApplicationService, IPersonAppService
{
    private readonly IRepository<Person, Guid> _personRepository;

    public PersonAppService(IRepository<Person, Guid> personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task DoIt()
    {
        var people = await _personRepository
            .Where(p => p.BirthYear > 2000) //Use LINQ extension methods
            .ToListAsync();
    }
}
````

> Note that in order to use the async LINQ extension methods (like `ToListAsync` here), you may need to depend on the database provider (like EF Core) since these methods are defined in the database provider package, they are not standard LINQ methods.

#### FirstOrDefault(predicate), Single()... Methods

ABP Framework repository has not such methods get predicate (expression) since the repository itself is `IQueryable` and all these methods are already standard LINQ extension methods those can be directly used.

However, it provides the following methods those can be used to query a single entity by its Id:

* `FindAsync(id)` returns the entity or null if not found.
* `GetAsync(id)` method returns the entity or throws an `EntityNotFoundException` (which causes HTTP 404 status code) if not found.

#### Sync vs Async 

ABP Framework repository has no sync methods (like `Insert`). All the methods are async (like `InsertAsync`). So, if your application has sync repository method usages, convert them to async versions. 

In general, ABP Framework forces you to completely use async everywhere, because mixing async & sync methods is not a recommended approach.

#### Documentation

See the documentation for details on the repositories:

* [ASP.NET Boilerplate - Repository documentation](https://aspnetboilerplate.com/Pages/Documents/Repositories)
* [ABP Framework - Repository documentation](https://docs.abp.io/en/abp/latest/Repositories)

### Domain Services

Your domain service logic mostly remains same on the migration. ABP Framework also defines the base `DomainService` class and the `IDomainService` interface just works like the ASP.NET Boilerplate.

## The Application Layer

Your application service logic remains similar on the migration. ABP Framework also defines the base `ApplicationService` class and the `IApplicationService` interface just works like the ASP.NET Boilerplate, but there are some differences in details.

### Declarative Authorization

ASP.NET Boilerplate has `AbpAuthorize` and `AbpMvcAuthorize` attributes for declarative authorization. Example usage:

````csharp
[AbpAuthorize("MyUserDeletionPermissionName")]
public async Task DeleteUserAsync(...)
{
    ...
}
````

ABP Framework doesn't has such a custom attribute. It uses the standard `Authorize` attribute in all layers.

````csharp
[Authorize("MyUserDeletionPermissionName")]
public async Task DeleteUserAsync(...)
{
    ...
}
````

This is possible with the better integration to the Microsoft Authorization Extensions libraries. See the Authorization section below for more information about the authorization system.

### CrudAppService and AsyncCrudAppService Classes

ASP.NET Boilerplate has `CrudAppService` (with sync service methods) and `AsyncCrudAppService` (with async service methods) classes.

ABP Framework only has the `CrudAppService` which actually has only the async methods (instead of sync methods).

ABP Framework's `CrudAppService` method signatures are slightly different than the old one. For example, old update method signature was ` Task<TEntityDto> UpdateAsync(TUpdateInput input) ` while the new one is ` Task<TGetOutputDto> UpdateAsync(TKey id, TUpdateInput input) `. The main difference is that it gets the Id of the updating entity as a separate parameter instead of including in the input DTO.

### Data Transfer Objects (DTOs)

There are similar base DTO classes (like `EntityDto`) in the ABP Framework too. So, you can find the corresponding DTO base class if you need.

#### Validation

You can continue to use the data annotation attributes to validate your DTOs just like in the ASP.NET Boilerplate.

ABP Framework doesn't include the ` ICustomValidate ` that does exists in the ASP.NET Boilerplate. Instead, you should implement the standard `IValidatableObject` interface for your custom validation logic.

## The Infrastructure Layer

### Namespaces

ASP.NET Boilerplate uses the `Abp.*` namespaces while the ABP Framework uses the `Volo.Abp.*` namespaces for the framework and pre-built fundamental modules.

In addition, there are also some pre-built application modules (like docs and blog modules) those are using the `Volo.*` namespaces (like `Volo.Blogging.*` and `Volo.Docs.*`). We consider these modules as standalone open source products developed by Volosoft rather than add-ons or generic modules completing the ABP Framework and used in the applications. We've developed them as a module to make them re-usable as a part of a bigger solution.

### Module System

Both of the ASP.NET Boilerplate and the ABP Framework have the `AbpModule` while they are a bit different.

ASP.NET Boilerplate's `AbpModule` class has `PreInitialize`, `Initialize` and `PostInitialize` methods you can override and configure the framework and the depended modules. You can also register and resolve dependencies in these methods.

ABP Framework's `AbpModule` class has the `ConfigureServices` and `OnApplicationInitialization` methods (and their Pre and Post versions). It is similar to ASP.NET Core's Startup class. You configure other services and register dependencies in the `ConfigureServices`. However, you can now resolve dependencies in that point. You can resolve dependencies and configure the ASP.NET Core pipeline in the `OnApplicationInitialization` method while you can not register dependencies here. So, the new module classes separate dependency registration phase from dependency resolution phase since it follows the ASP.NET Core's approach.

### Dependency Injection

#### The DI Framework

ASP.NET Boilerplate is using the [Castle Windsor](http://www.castleproject.org/projects/windsor/) as the dependency injection framework. This is a fundamental dependency of the ASP.NET Boilerplate framework. We've got a lot of feedback to make the ASP.NET Boilerplate DI framework agnostic, but it was not so easy because of the design.

ABP Framework is dependency injection framework independent since it uses Microsoft's [Dependency Injection Extensions](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection) library as an abstraction. None of the ABP Framework or module packages depends on any specific library.

However, ABP Framework doesn't use the Microsoft's base DI library because it has some missing features ABP Framework needs to: Property Injection and Interception. All the startup templates and the samples are using the [Autofac](https://autofac.org/) as the DI library and it is the only [officially integrated](Autofac-Integration.md) library to the ABP Framework. We suggest you to use the Autofac with the ABP Framework if you have not a good reason. If you have a good reason, please create an [issue](https://github.com/abpframework/abp/issues/new) on GitHub to request it or just implement it and send a pull request :)

#### Registering the Dependencies

Registering the dependencies are similar and mostly handled by the framework conventionally (like repositories, application services, controllers... etc). Implement the same `ITransientDependency`, `ISingletonDependency` and `IScopedDependency` interfaces for the services not registered by conventions.

When you need to manually register dependencies, use the `context.Services` in the `ConfigureServices` method of your module. Example:

````csharp
public class BlogModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //Register an instance as singleton
        context.Services.AddSingleton<TaxCalculator>(new TaxCalculator(taxRatio: 0.18));

        //Register a factory method that resolves from IServiceProvider
        context.Services.AddScoped<ITaxCalculator>(
            sp => sp.GetRequiredService<TaxCalculator>()
        );
    }
}
````

See the ABP Framework [dependency injection document](https://docs.abp.io/en/abp/latest/Dependency-Injection) for details.

### Configuration vs Options System

ASP.NET Boilerplate has its own configuration system to configure the framework and the modules. For example, you could disable the audit logging in the `Initialize` method of your [module](https://aspnetboilerplate.com/Pages/Documents/Module-System):

````csharp
public override void Initialize()
{
    Configuration.Auditing.IsEnabled = false;
}
````

ABP Framework uses [the options pattern](Options.md) to configure the framework and the modules. You typically configure the options in the `ConfigureServices` method of your [module](Module-Development-Basics.md):

````csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<AbpAuditingOptions>(options =>
    {
        options.IsEnabled = false;
    });
}
````

Instead of a central configuration object, there are separated option classes for every module and feature those are defined in the related documents.

### IAbpSession vs ICurrentUser and ICurrentTenant

ASP.NET Boilerplate's `IAbpSession` service is used to obtain the current user and tenant information, like ` UserId ` and `TenantId`.

ABP Framework doesn't have the same service. Instead, use `ICurrentUser` and `ICurrentTenant` services. These services are defined as base properties in some common classes (like `ApplicationService` and `AbpController`), so you generally don't need to manually inject them. They also have much properties compared to the `IAbpSession`.

### Authorization

ABP Framework extends the [ASP.NET Core Authorization](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/introduction) by adding **permissions** as auto [policies](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies) and allowing the authorization system to be usable in the [application services](Application-Services.md) too.

#### AbpAutorize vs Autorize

Use the standard `[Autorize]` and `[AllowAnonymous]` attributes instead of ASP.NET Boilerplate's custom `[AbpAutorize]` and `[AbpAllowAnonymous]` attributes.

#### IPermissionChecker vs IAuthorizationService

Use the standard `IAuthorizationService` to check permissions instead of the ASP.NET Boilerplate's `IPermissionChecker` service. While `IPermissionChecker` also exists in the ABP Framework, it is used to explicitly use the permissions. Using `IAuthorizationService` is the recommended way since it covers other type of policy checks too.

#### AuthorizationProvider vs PermissionDefinitionProvider

You inherit from the `AuthorizationProvider` in the ASP.NET Boilerplate to define your permissions. ABP Framework replaces it by the `PermissionDefinitionProvider` base class. So, define your permissions by inheriting from the `PermissionDefinitionProvider` class.

### Unit of Work

Unit of work system has been designed to work seamlessly. For most of the cases, you don't need to change anything.

`UnitOfWork` attribute of the ABP Framework doesn't have the `ScopeOption` (type of `TransactionScopeOption`) property. Instead, use `IUnitOfWorkManager.Begin()` method with `requiresNew = true`  to create an independent inner transaction in a transaction scope.

#### Data Filters

ASP.NET Boilerplate implements the data filtering system as a part of the unit of work. ABP Framework has a separate `IDataFilter` service.

See the [data filtering document](Data-Filtering.md) to learn how to enable/disable a filter.

See [the UOW documentation](Unit-Of-Work.md) for more about the UOW system.

### Multi-Tenancy

#### IMustHaveTenant & IMayHaveTenant vs IMultiTenant

ASP.NET Boilerplate defines `IMustHaveTenant` and `IMayHaveTenant` interfaces to implement them for your entities. In this way, your entities are automatically filtered according to the current tenant. Because of the design, there was a problem: You had to create a "Default" tenant in the database with "1" as the Id if you want to create a non multi-tenant application (this "Default" tenant was used as the single tenant).

ABP Framework has a single interface for multi-tenant entities: `IMultiTenant` which defines a nullable `TenantId` property of type `Guid`. If your application is not multi-tenant, then your entities will have null TenantId (instead of a default one).

On the migration, you need to change the TenantId field type and replace these interfaces with the `IMultiTenant`

#### Switch Between Tenants

In some cases you might need to switch to a tenant for a code scope and work with the tenant's data in this scope.

In ASP.NET Boilerplate, it is done using the `IUnitOfWorkManager` service:

````csharp
public async Task<List<Product>> GetProducts(int tenantId)
{
    using (_unitOfWorkManager.Current.SetTenantId(tenantId))
    {
        return await _productRepository.GetAllListAsync();
    }
}
````

In the ABP Framework it is done with the `ICurrentTenant` service:

````csharp
public async Task<List<Product>> GetProducts(Guid tenantId)
{
    using (_currentTenant.Change(tenantId))
    {
        return await _productRepository.GetListAsync();
    }
}
````

Pass `null` to the `Change` method to switch to the host side.

### Caching

ASP.NET Boilerplate has its [own distributed caching abstraction](https://aspnetboilerplate.com/Pages/Documents/Caching) which has in-memory and Redis implementations. You typically inject the `ICacheManager` service and use its `GetCache(...)` method to obtain a cache, then get and set objects in the cache.

ABP Framework uses and extends ASP.NET Core's [distributed caching abstraction](Caching.md). It defines the `IDistributedCache<T>` services to inject a cache and get/set objects.

### Logging

ASP.NET Boilerplate uses Castle Windsor's [logging facility](http://docs.castleproject.org/Windsor.Logging-Facility.ashx) as an abstraction and supports multiple logging providers including Log4Net (the default one comes with the startup projects) and Serilog. You typically property-inject the logger:

````csharp
using Castle.Core.Logging; //1: Import Logging namespace

public class TaskAppService : ITaskAppService
{    
    //2: Getting a logger using property injection
    public ILogger Logger { get; set; }

    public TaskAppService()
    {
        //3: Do not write logs if no Logger supplied.
        Logger = NullLogger.Instance;
    }

    public void CreateTask(CreateTaskInput input)
    {
        //4: Write logs
        Logger.Info("Creating a new task with description: " + input.Description);
        //...
    }
}
````

ABP Framework depends on Microsoft's [logging extensions](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging) library which is also an abstraction and there are many providers implement it. Startup templates are using the Serilog as the pre-configured logging libary while it is easy to change in your project. The usage pattern is similar:

````csharp
//1: Import the Logging namespaces
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

public class TaskAppService : ITaskAppService
{
    //2: Getting a logger using property injection
    public ILogger<TaskAppService> Logger { get; set; }

    public TaskAppService()
    {
        //3: Do not write logs if no Logger supplied.
        Logger = NullLogger<TaskAppService>.Instance;
    }

    public void CreateTask(CreateTaskInput input)
    {
        //4: Write logs
        Logger.Info("Creating a new task with description: " + input.Description);
        //...
    }
}
````

You inject the `ILogger<T>` instead of the `ILogger`.

### Object to Object Mapping

#### IObjectMapper Service

ASP.NET Boilerplate defines an `IObjectMapper` service ([see](https://aspnetboilerplate.com/Pages/Documents/Object-To-Object-Mapping)) and has an integration to the [AutoMapper](https://automapper.org/) library.

Example usage: Create a `User` object with the given `CreateUserInput` object:

````csharp
public void CreateUser(CreateUserInput input)
{
    var user = ObjectMapper.Map<User>(input);
    ...
}
````

Example: Update an existing `User` properties with the given `UpdateUserInput` object:

````csharp
public async Task UpdateUserAsync(Guid id, UpdateUserInput input)
{
    var user = await _userRepository.GetAsync(id);
    ObjectMapper.Map(input, user);
}
````

ABP Framework has the same `IObjectMapper` service ([see](Object-To-Object-Mapping.md)) and the AutoMapper integration with a slightly different mapping methods.

Example usage: Create a `User` object with the given `CreateUserInput` object:

````csharp
public void CreateUser(CreateUserInput input)
{
    var user = ObjectMapper.Map<CreateUserInput, User>(input);
}
````

This time you need to explicitly declare the source type and target type (while ASP.NET Boilerplate was requiring only the target type).

Example: Update an existing `User` properties with the given `UpdateUserInput` object:

````csharp
public async Task UpdateUserAsync(Guid id, UpdateUserInput input)
{
    var user = await _userRepository.GetAsync(id);
    ObjectMapper.Map<UpdateUserInput, User>(input, user);
}
````

Again, ABP Framework expects to explicitly set the source and target types.

#### AutoMapper Integration

##### Auto Mapping Attributes

ASP.NET Boilerplate has `AutoMapTo`, `AutoMapFrom` and `AutoMap` attributes to automatically create mappings for the declared types. Example:

````csharp
[AutoMapTo(typeof(User))]
public class CreateUserInput
{
    public string Name { get; set; }
    public string Surname { get; set; }
    ...
}
````

ABP Framework has no such attributes, because AutoMapper as a [similar attribute](https://automapper.readthedocs.io/en/latest/Attribute-mapping.html) now. You need to switch to AutoMapper's attribute.

##### Mapping Definitions

ABP Framework follows AutoMapper principles closely. You can define classes derived from the `Profile` class to define your mappings.

##### Configuration Validation

Configuration validation is a best practice for the AutoMapper to maintain your mapping configuration in a safe way.

See [the documentation](Object-To-Object-Mapping.md) for more information related to the object mapping.

### Setting Management

#### Defining the Settings

In an ASP.NET Boilerplate based application, you create a class deriving from the `SettingProvider` class, implement the `GetSettingDefinitions` method and add your class to the `Configuration.Settings.Providers` list.

In the ABP Framework, you need to derive your class from the `SettingDefinitionProvider` and implement the `Define` method. You don't need to register your class since the ABP Framework automatically discovers it.

#### Getting the Setting Values

ASP.NET Boilerplate provides the `ISettingManager` to read the setting values in the server side and `abp.setting.get(...)` method in the JavaScript side.

ABP Framework has the `ISettingProvider` service to read the setting values in the server side and `abp.setting.get(...)` method in the JavaScript side.

#### Setting the Setting Values

For ASP.NET Boilerplate, you use the same `ISettingManager` service to change the setting values.

ABP Framework separates it and provides the setting management module (pre-added to the startup projects) which has the ` ISettingManager ` to change the setting values. This separation was introduced to support tiered deployment scenarios (where `ISettingProvider` can also work in the client application while `ISettingManager ` can also work in the server (API) side).

### Clock

ASP.NET Boilerplate has a static `Clock` service ([see](https://aspnetboilerplate.com/Pages/Documents/Timing)) which is used to abstract the `DateTime` kind, so you can easily switch between Local and UTC times. You don't inject it, but just use the `Clock.Now` static method to obtain the current time.

ABP Framework has the `IClock` service ([see](Clock.md)) which has a similar goal, but now you need to inject it whenever you need it.

### Event Bus

ASP.NET Boilerplate has an in-process event bus system. You typically inject the `IEventBus` (or use the static instance `EventBus.Default`) to trigger an event. It automatically triggers events for entity changes (like `EntityCreatingEventData` and `EntityUpdatedEventData`). You create a class by implementing the `IEventHandler<T>` interface.

ABP Framework separates the event bus into two services: `ILocalEventBus` and `IDistributedEventBus`.

The local event bus is similar to the event bus of the ASP.NET Boilerplate while the distributed event bus is new feature introduced in the ABP Framework.

So, to migrate your code;

* Use the `ILocalEventBus` instead of the `IEventBus`.
* Implement the `ILocalEventHandler` instead of the `IEventHandler`.

> Note that ABP Framework has also an `IEventBus` interface, but it does exists to be a common interface for the local and distributed event bus. It is not injected and directly used.

### Feature Management

Feature system is used in multi-tenant applications to define features of your application check if given feature is available for the current tenant.

#### Defining Features

In the ASP.NET Boilerplate ([see](https://aspnetboilerplate.com/Pages/Documents/Feature-Management)), you create a class inheriting from the `FeatureProvider`, override the `SetFeatures` method and add your class to the `Configuration.Features.Providers` list.

In the ABP Framework ([see](Features.md)), you derive your class from the `FeatureDefinitionProvider` and override the `Define` method. No need to add your class to the configuration, it is automatically discovered by the framework.

#### Checking Features

You can continue to use the `RequiresFeature` attribute and `IFeatureChecker` service to check if a feature is enabled for the current tenant.

#### Changing the Feature Values

In the ABP Framework you use the `IFeatureManager` to change a feature value for a tenant.

### Audit Logging

The ASP.NET Boilerplate ([see](https://aspnetboilerplate.com/Pages/Documents/Audit-Logging)) and the ABP Framework ([see](Audit-Logging.md)) has similar audit logging systems. ABP Framework requires to add `UseAuditing()` middleware to the ASP.NET Core pipeline, which is already added in the startup templates. So, most of the times it will be work out of the box.

### Localization

ASP.NET Boilerplate supports XML and JSON files to define the localization key-values for the UI ([see](https://aspnetboilerplate.com/Pages/Documents/Localization)). ABP Framework only supports the JSON formatter localization files ([see](Localization.md)). So, you need to convert your XML file to JSON.

The ASP.NET Boilerplate has its own the `ILocalizationManager` service to be injected and used for the localization in the server side.

The ABP Framework uses [Microsoft localization extension](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization) library, so it is completely integrated to ASP.NET Core. You use the `IStringLocalizer<T>` service to get a localized text. Example:

````csharp
public class MyService
{
    private readonly IStringLocalizer<TestResource> _localizer;

    public MyService(IStringLocalizer<TestResource> localizer)
    {
        _localizer = localizer;
    }

    public void Foo()
    {
        var str = _localizer["HelloWorld"]; //Get a localized text
    }
}
````

So, you need to replace `ILocalizationManager` usage by the `IStringLocalizer`.

It also provides API used in the client side:

````js
var testResource = abp.localization.getResource('Test');
var str = testResource('HelloWorld');
````

It was like `abp.localization.localize(...)` in the ASP.NET Boilerplate.

### Navigation vs Menu

In ASP.NET you create a class deriving from the `NavigationProvider` to define your menu elements. Menu items has `requiredPermissionName` attributes to restrict access to a menu element. Menu items were static and your class is executed only one time.

Int the ABP Framework you need to create a class implements the `IMenuContributor` interface. Your class is executed whenever the menu needs to be rendered. So, you can conditionally add menu items.

As an example, this is the menu contributor of the tenant management module:

````csharp
public class AbpTenantManagementWebMainMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        //Add items only to the main menu
        if (context.Menu.Name != StandardMenus.Main)
        {
            return;
        }

        //Get the standard administration menu item
        var administrationMenu = context.Menu.GetAdministration();

        //Resolve some needed services from the DI container
        var authorizationService = context.ServiceProvider
            .GetRequiredService<IAuthorizationService>();
        var l = context.ServiceProvider
            .GetRequiredService<IStringLocalizer<AbpTenantManagementResource>>();

        var tenantManagementMenuItem = new ApplicationMenuItem(
            TenantManagementMenuNames.GroupName,
            l["Menu:TenantManagement"],
            icon: "fa fa-users");
        
        administrationMenu.AddItem(tenantManagementMenuItem);

        //Conditionally add the "Tenants" menu item based on the permission
        if (await authorizationService
            .IsGrantedAsync(TenantManagementPermissions.Tenants.Default))
        {
            tenantManagementMenuItem.AddItem(
                new ApplicationMenuItem(
                    TenantManagementMenuNames.Tenants,
                    l["Tenants"], 
                    url: "/TenantManagement/Tenants"));
        }
    }
}
````

So, you need to check permission using the `IAuthorizationService` if you want to show a menu item only when the user has the related permission.

> Navigation/Menu system is only for ASP.NET Core MVC / Razor Pages applications. Angular applications has a different system implemented in the startup templates.

## Missing Features

The following features are not present for the ABP Framework. Here, a list of some major missing features (and the related issue for that feature waiting on the ABP Framework GitHub repository):

* [Multi-Lingual Entities](https://aspnetboilerplate.com/Pages/Documents/Multi-Lingual-Entities) ([#1754](https://github.com/abpframework/abp/issues/1754))
* [Real time notification system](https://aspnetboilerplate.com/Pages/Documents/Notification-System) ([#633](https://github.com/abpframework/abp/issues/633))
* [NHibernate Integration](https://aspnetboilerplate.com/Pages/Documents/NHibernate-Integration) ([#339](https://github.com/abpframework/abp/issues/339)) - We don't intent to work on this, but any community contribution welcome.

Some of these features will eventually be implemented. However, you can implement them yourself if they are important for you. If you want, you can [contribute](Contribution/Index.md) to the framework, it is appreciated.