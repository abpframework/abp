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

While using the extra properties approach is **easy to use** and suitable for some scenarios, it has some drawbacks described in the [entities document](Entities.md).

Another approach can be **creating your own entity** mapped to **the same database table** (or collection for a MongoDB database).

`AppUser` entity in the [application startup template](Startup-Templates/Application.md) already implements this approach. [EF Core Migrations document](Entity-Framework-Core-Migrations.md) describes how to implement it and manage **EF Core database migrations** in such a case. It is also possible for MongoDB, while this time you won't deal with the database migration problems.

#### Creating a New Entity with Its Own Database Table/Collection

Mapping your entity to an **existing table** of a depended module has a few disadvantages;

* You deal with the **database migration structure** for EF Core. While it is possible, you should extra care about the migration code especially when you want to add **relations** between entities.
* Your application database and the module database will be the **same physical database**. Normally, a module database can be separated if needed, but using the same table restricts it.

If you want to **loose couple** your entity with the entity defined by the module, you can create your own database table/collection and map your entity to your own table in your own database.

In this case, you need to deal with the **synchronization problems**, especially if you want to **duplicate** some properties/fields of the related entity. There are a few solutions;

* If you are building a **monolithic** application (or managing your entity and the related module entity within the same process), you can use the [local event bus](Local-Event-Bus.md) to listen changes.
* If you are building a **distributed** system where the module entity is managed (created/updated/deleted) on a different process/service than your entity is managed, then you can subscribe to the [distributed event bus](Distributed-Event-Bus.md) for change events.

Once you handle the event, you can update your own entity in your own database.

##### Subscribing to Local Events

[Local Event Bus](Local-Event-Bus.md) system is a way to publish and subscribe to events occurring in the same application.

Assume that you want to get informed when a `IdentityUser` entity changes (created, updated or deleted). You can create a class that implements the `ILocalEventHandler<EntityChangedEventData<IdentityUser>>` interface.

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

* `EntityChangedEventData<T>` covers create, update and delete events for the given entity. If you need, you can subscribe to create, update and delete events individually (in the same class or different classes).
* This code will be executed **out of the local transaction**, because it listens the `EntityChanged` event. You can subscribe to the `EntityChangingEventData<T>` to perform your event handler in **the same local (in-process) transaction** if the current [unit of work](Unit-Of-Work.md) is transactional.

> Reminder: This approach needs to change the `IdentityUser`  entity in the same process contains the handler class. It perfectly works even for a clustered environment (when multiple instances of the same application are running on multiple servers).

##### Subscribing to Distributed Events

[Distributed Event Bus](Distributed-Event-Bus.md) system is a way to publish an event in one application and receive the event in the same or different application running on the same or different server.

Assume that you want to get informed when a `IdentityUser` entity created, updated or deleted. You can create a class like below:

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

* It implements multiple `IDistributedEventHandler` interfaces: **Created**, **Updated** and **Deleted**. Because, the distributed event bus system publishes events individually. There is no "Changed" event like the local event bus.
* It subscribes to `EntityEto`, which is a generic event class that is **automatically published** for all type of entities by the ABP framework. This is why it checks the **entity type** (checking the entity type as string since we assume that there is no type safe reference to the `IdentityUser` entity).

Pre-built application modules do not define specialized event types yet (like `IdentityUserEto` - "ETO" means "Event Transfer Object"). This feature is on the road map and will be available in a short term ([follow this issue](https://github.com/abpframework/abp/issues/3033)). Once it is implemented, you will be able to subscribe to individual entity types. Example:

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

* This handler is executed only when a new user has been created.

> The only pre-defined specialized event class is the `UserEto`. For example, you can subscribe to the `EntityCreatedEto<UserEto>` to get notified when a user has created. This event also works for the Identity module.

### Overriding Services

You may need to **change behavior (business logic)** of a depended module for your application. You can use the power of the [dependency injection system](Dependency-Injection.md) to replace a service, controller or even a page model of the depended module by your own implementation.

Replacing a service is possible for any type of class registered to the dependency injection, including services of the ABP Framework.

You have different options can be used based on your requirement those will be explained in the next sections.

#### Replacing an Interface

If given service defines an interface, like the `IdentityUserAppService` class implements the `IIdentityAppService`, you can re-implement the same interface and replace the current implementation by your class. Example:

````csharp
public class MyIdentityUserAppService : IIdentityUserAppService, ITransientDependency
{
    //...
}
````

`MyIdentityUserAppService` replaces the `IIdentityUserAppService` by naming convention (since both ends with `IdentityUserAppService`). If your class name doesn't match, you need to manually expose the service interface:

````csharp
[ExposeServices(typeof(IIdentityUserAppService))]
public class TestAppService : IIdentityUserAppService, ITransientDependency
{
    //...
}
````

The dependency injection system allows to register multiple services for the same interface. The last registered one is used when the interface is injected. It is a good practice to explicitly replace the service. 

Example:

````csharp
[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IIdentityUserAppService))]
public class TestAppService : IIdentityUserAppService, ITransientDependency
{
    //...
}
````

In this way, there will be a single implementation of the `IIdentityUserAppService` interface, while it doesn't change the result for this case. Replacing a service is also possible by code:

````csharp
context.Services.Replace(
    ServiceDescriptor.Transient<IIdentityUserAppService, MyIdentityUserAppService>()
);
````

You can write this inside the `ConfigureServices` method of your [module](Module-Development-Basics.md).

#### Overriding a Service Class

In most cases, you will want to change one or a few methods of the current implementation for a service. Re-implementing the complete interface would not be efficient in this case. As a better approach, inherit from the original class and override the desired method.

##### Example: Overriding an Application Service

````csharp
[Dependency(ReplaceServices = true)]
public class MyIdentityUserAppService : IdentityUserAppService
{
    //...
    public MyIdentityUserAppService(
        IdentityUserManager userManager,
        IIdentityUserRepository userRepository,
        IGuidGenerator guidGenerator
    ) : base(
        userManager,
        userRepository,
        guidGenerator)
    {
    }

    public override async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
    {
        if (input.PhoneNumber.IsNullOrWhiteSpace())
        {
            throw new AbpValidationException(
                "Phone number is required for new users!",
                new List<ValidationResult>
                {
                    new ValidationResult(
                        "Phone number can not be empty!",
                        new []{"PhoneNumber"}
                    )
                }
            );        }

        return await base.CreateAsync(input);
    }
}
````

This class **overrides** the `CreateAsync` method of the `IdentityUserAppService` to check the phone number. Then calls the base method to continue to the **underlying business logic**. In this way, you can perform additional business logic **before** and **after** the base logic.

You could completely **re-write** the entire business logic for a user creation without calling the base method.

##### Example: Overriding a Domain Service

````csharp
[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IdentityUserManager))]
public class MyIdentityUserManager : IdentityUserManager
{
    public MyIdentityUserManager(
        IdentityUserStore store, 
        IOptions<IdentityOptions> optionsAccessor, 
        IPasswordHasher<IdentityUser> passwordHasher,
        IEnumerable<IUserValidator<IdentityUser>> userValidators, 
        IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators, 
        ILookupNormalizer keyNormalizer, 
        IdentityErrorDescriber errors, 
        IServiceProvider services, 
        ILogger<IdentityUserManager> logger, 
        ICancellationTokenProvider cancellationTokenProvider
        ) : base(
            store, 
            optionsAccessor, 
            passwordHasher, 
            userValidators, 
            passwordValidators, 
            keyNormalizer, 
            errors, 
            services, 
            logger, 
            cancellationTokenProvider)
    {
    }

    public override async Task<IdentityResult> CreateAsync(IdentityUser user)
    {
        if (user.PhoneNumber.IsNullOrWhiteSpace())
        {
            throw new AbpValidationException(
                "Phone number is required for new users!",
                new List<ValidationResult>
                {
                    new ValidationResult(
                        "Phone number can not be empty!",
                        new []{"PhoneNumber"}
                    )
                }
            );
        }

        return await base.CreateAsync(user);
    }
}
````

This example class inherits from the `IdentityUserManager` and overrides the `CreateAsync` method to perform the same phone number check implemented above. The result is same, but this time we've implemented it inside the domain service assuming that this is a **core domain logic** for our system.

> `[ExposeServices(typeof(IdentityUserManager))]`  attribute is **required** here since `IdentityUserManager` does not define an interface (like `IIdentityUserManager`) and dependency injection system doesn't expose services for inherited classes (like it does for the implemented interfaces) by convention.

Check the [localization system](Localization.md) to learn how to localize the error messages.

##### Example: Overriding a Page Model

TODO