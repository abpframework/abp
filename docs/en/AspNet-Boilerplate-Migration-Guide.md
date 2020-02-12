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

ABP Framework and the ASP.NET Boilerplate both have the `IEntity` and `IEntity<T>` interfaces and `Entity` and `Entity<T>` base classes to define entities but they have some differences.

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

### Repositories

TODO

## Missing Features

TODO: Notification... etc.