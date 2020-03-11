# Customizing the Existing Modules

ABP Framework provides was designed to support to build fully [modular applications](Module-Development-Basics.md) and systems. It also provides some [pre-built application modules](Modules/Index.md) those are **ready to use** in any kind of application.

For example, you can **re-use** the [Identity Management Module](Modules/Identity.md) to add user, role and permission management to your application. The [application startup template](Startup-Templates/Application.md) already comes with Identity and some other modules **pre-installed**.

## Re-Using an Application Module

You have two options to re-use an application module.

### As Package References

You can add **NuGet** & **NPM** package references of the related module to your application and configure the module (based on its documentation) to integrate to your application.

As mentioned before, the [application startup template](Startup-Templates/Application.md) already comes with some **fundamental modules pre-installed**. It uses the modules as NuGet & NPM package references.

This approach has the following benefits:

* Your solution will be **clean** and only contains your **own application code**.
* You can **easily upgrade** a module when a new version is available. `abp update` [CLI](CLI.md) command makes it even easier. In this way, you can continue to get **new features and bug fixes**.

However, there is a drawback:

* You may not able to **customize** the module source code as it is in your own solution.

This document explains **how to customize or extend** a depended module without need to change its source code. While it is limited compared to a full source code change opportunity, there are still some good ways to make some customizations.

If you don't think to make huge changes on the pre-built modules, re-using them as package reference is the recommended way.

### Including the Source Code

If you want to make **huge changes** or add **major features** on a pre-built module, but the available extension points are not enough, you can consider to directly work the source code of the depended module.

In this case, you typically **add the source code** of the module to your solution and **replace package references** by local project references. **[ABP CLI](CLI.md)** automates this process for you.

#### Separating the Module Solution

You may prefer to not include the module source code **directly into your solution**. Every module consists of 10+ project files and adding **multiple modules** may impact on the **size** of your solution **load & development time.** Also, you may have different development teams working on different modules, so you don't want to make the module code available to the application development team.

In any case, you can create a **separate solution** for the desired module and depend on the module as project references out of the solution. We do it like that for the [abp repository](https://github.com/abpframework/abp/).

> One problem we see is  Visual Studio doesn't play nice with this kind of approach (it doesn't support well to have references to local projects out of the solution directory). If you get error while building the application (depends on an external module), run `dotnet restore` in the command line after opening the application's solution in the Visual Studio.

#### Publishing the Customized Module as Packages

One alternative scenario could be re-packaging the module source code (as NuGet/NPM packages) and using as package references. You can use a local private NuGet/NPM server for your company.

## Module Customization / Extending Approaches

This section suggests some approaches if you decided to use pre-built application modules as NuGet/NPM package references.

### Extending Entities

In some cases, you may want to add some additional properties (and database fields) for an entity defined in a depended module. This section will cover some different approaches to make this possible.

#### Extra Properties

[Extra properties](Entities.md) is a way of storing some additional data on an entity without changing it. The entity should implement the `IHasExtraProperties` interface to allow it. All the aggregate root entities defined in the pre-built modules implement the `IHasExtraProperties` interface, so you can store extra properties on these entities.

Example:

````csharp
//SET AN EXTRA PROPERTY
var user = await _identityUserRepository.GetAsync(userId);
user.SetProperty("Title", "My custom title value!");
await _identityUserRepository.UpdateAsync(user);

//GET AN EXTRA PROPERTY
var user = await _identityUserRepository.GetAsync(userId);
return user.GetProperty<string>("Title");
````

This approach is very easy to use and available out of the box. No extra code needed. You can store more than one property at the same time by using different property names (like `Title` here).

Extra properties are stored as a single `JSON` formatted string value in the database for the EF Core. For MongoDB, they are stored as separate fields of the document.

See the [entities document](Entities.md) for more about the extra properties system.

> It is possible to perform a **business logic** based on the value of an extra property. You can **override** a service method and get or set the value as shown above. Overriding services will be discussed below.

#### Creating a New Entity Maps to the Same Database Table/Collection

While using the extra properties approach is easy to use and suitable for some scenarios, it has some drawbacks described in the [entities document](Entities.md).

Another approach can be creating your own entity mapped to the same database table (or collection for a MongoDB database).

`AppUser` entity in the [application startup template](Startup-Templates/Application.md) already implements this approach. [EF Core Migrations document](Entity-Framework-Core-Migrations.md) describes how to implement it and manage database migrations in such a case. It is also possible for MongoDB, while this time you won't deal with the database migration problems.

#### Creating a New Entity with Its Own Database Table/Collection

