# Customizing the Application Modules: Overriding Services

You may need to **change behavior (business logic)** of a depended module for your application. In this case, you can use the power of the [dependency injection system](Dependency-Injection.md) to replace a service, controller or even a page model of the depended module by your own implementation.

**Replacing a service** is possible for any type of class registered to the dependency injection, including services of the ABP Framework.

You have different options can be used based on your requirement those will be explained in the next sections.

> Notice that some service methods may not be virtual, so you may not be able to override. We make all virtual by design. If you find any method that is not overridable, please [create an issue](https://github.com/abpframework/abp/issues/new) or do it yourself and send a **pull request** on GitHub.

## Replacing an Interface

If given service defines an interface, like the `IdentityUserAppService` class implements the `IIdentityUserAppService`, you can re-implement the same interface and replace the current implementation by your class. Example:

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

## Overriding a Service Class

In most cases, you will want to change one or a few methods of the current implementation for a service. Re-implementing the complete interface would not be efficient in this case. As a better approach, inherit from the original class and override the desired method.

### Example: Overriding an Application Service

````csharp
[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IIdentityUserAppService), typeof(IdentityUserAppService), typeof(MyIdentityUserAppService))]
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

    public async override Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
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

This class **overrides** the `CreateAsync` method of the `IdentityUserAppService` [application service](Application-Services.md) to check the phone number. Then calls the base method to continue to the **underlying business logic**. In this way, you can perform additional business logic **before** and **after** the base logic.

You could completely **re-write** the entire business logic for a user creation without calling the base method.

### Example: Overriding a Domain Service

````csharp
[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IdentityUserManager))]
public class MyIdentityUserManager : IdentityUserManager
{
        public MyIdentityUserManager(
            IdentityUserStore store,
            IIdentityRoleRepository roleRepository,
            IIdentityUserRepository userRepository,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<IdentityUser> passwordHasher,
            IEnumerable<IUserValidator<IdentityUser>> userValidators,
            IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<IdentityUserManager> logger,
            ICancellationTokenProvider cancellationTokenProvider) :
            base(store,
                roleRepository,
                userRepository,
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

    public async override Task<IdentityResult> CreateAsync(IdentityUser user)
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

This example class inherits from the `IdentityUserManager` [domain service](Domain-Services.md) and overrides the `CreateAsync` method to perform the same phone number check implemented above. The result is same, but this time we've implemented it inside the domain service assuming that this is a **core domain logic** for our system.

> `[ExposeServices(typeof(IdentityUserManager))]`  attribute is **required** here since `IdentityUserManager` does not define an interface (like `IIdentityUserManager`) and dependency injection system doesn't expose services for inherited classes (like it does for the implemented interfaces) by convention.

Check the [localization system](Localization.md) to learn how to localize the error messages.

### Example: Overriding a Controller

````csharp
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Account;
using Volo.Abp.DependencyInjection;

namespace MyProject.Controllers
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(AccountController))]
    public class MyAccountController : AccountController
    {
        public MyAccountController(IAccountAppService accountAppService)
            : base(accountAppService)
        {

        }

        public async override Task SendPasswordResetCodeAsync(
            SendPasswordResetCodeDto input)
        {
            Logger.LogInformation("Your custom logic...");

            await base.SendPasswordResetCodeAsync(input);
        }
    }
}
````

This example replaces the `AccountController` (An API Controller defined in the [Account Module](Modules/Account.md)) and overrides the `SendPasswordResetCodeAsync` method.

**`[ExposeServices(typeof(AccountController))]` is essential** here since it registers this controller for the `AccountController` in the dependency injection system. `[Dependency(ReplaceServices = true)]` is also recommended to clear the old registration (even the ASP.NET Core DI system selects the last registered one).

In addition, the `MyAccountController` will be removed from [`ApplicationModel`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.applicationmodels.applicationmodel.controllers) because it defines `ExposeServicesAttribute`.

If `IncludeSelf = true` is specified, i.e. `[ExposeServices(typeof(AccountController), IncludeSelf = true)]`, then `AccountController` will be removed instead. This is useful for **extending** a controller.

If you don't want to remove either controller, you can configure `AbpAspNetCoreMvcOptions`:

```csharp
Configure<AbpAspNetCoreMvcOptions>(options =>
{
    options.IgnoredControllersOnModelExclusion.AddIfNotContains(typeof(MyAccountController));
});
```

### Overriding Other Classes

Overriding controllers, framework services, view component classes and any other type of classes registered to dependency injection can be overridden just like the examples above.

## Extending Data Transfer Objects

**Extending [entities](Entities.md)** is possible as described in the [Extending Entities document](Customizing-Application-Modules-Extending-Entities.md). In this way, you can add **custom properties** to entities and perform **additional business logic** by overriding the related services as described above.

It is also possible to extend Data Transfer Objects (**DTOs**) used by the application services. In this way, you can get extra properties from the UI (or client) and return extra properties from the service.

### Example

Assuming that you've already added a `SocialSecurityNumber` as described in the [Extending Entities document](Customizing-Application-Modules-Extending-Entities.md) and want to include this information while getting the list of users from the `GetListAsync`  method of the `IdentityUserAppService`.

You can use the [object extension system](Object-Extensions.md) to add the property to the `IdentityUserDto`. Write this code inside the `YourProjectNameDtoExtensions` class comes with the application startup template:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<IdentityUserDto, string>(
        "SocialSecurityNumber"
    );
````

This code defines a `SocialSecurityNumber` to the `IdentityUserDto` class as a `string` type. That's all. Now, if you call the `/api/identity/users` HTTP API (which uses the `IdentityUserAppService` internally) from a REST API client, you will see the `SocialSecurityNumber` value in the `extraProperties` section.

````json
{
    "totalCount": 1,
    "items": [{
        "tenantId": null,
        "userName": "admin",
        "name": "admin",
        "surname": null,
        "email": "admin@abp.io",
        "emailConfirmed": false,
        "phoneNumber": null,
        "phoneNumberConfirmed": false,
        "twoFactorEnabled": false,
        "lockoutEnabled": true,
        "lockoutEnd": null,
        "concurrencyStamp": "b4c371a0ab604de28af472fa79c3b70c",
        "isDeleted": false,
        "deleterId": null,
        "deletionTime": null,
        "lastModificationTime": "2020-04-09T21:25:47.0740706",
        "lastModifierId": null,
        "creationTime": "2020-04-09T21:25:46.8308744",
        "creatorId": null,
        "id": "8edecb8f-1894-a9b1-833b-39f4725db2a3",
        "extraProperties": {
            "SocialSecurityNumber": "123456789"
        }
    }]
}
````

Manually added the `123456789` value to the database for now.

All pre-built modules support extra properties in their DTOs, so you can configure easily.

### Definition Check

When you [define](Customizing-Application-Modules-Extending-Entities.md) an extra property for an entity, it doesn't automatically appear in all the related DTOs, because of the security. The extra property may contain a sensitive data and you may not want to expose it to the clients by default.

So, you need to explicitly define the same property for the corresponding DTO if you want to make it available for the DTO (as just done above). If you want to allow to set it on user creation, you also need to define it for the `IdentityUserCreateDto`.

If the property is not so secure, this can be tedious. Object extension system allows you to ignore this definition check for a desired property. See the example below:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<IdentityUser, string>(
        "SocialSecurityNumber",
        options =>
        {
            options.MapEfCore(b => b.HasMaxLength(32));
            options.CheckPairDefinitionOnMapping = false;
        }
    );
````

This is another approach to define a property for an entity (`ObjectExtensionManager` has more, see [its document](Object-Extensions.md)). This time, we set `CheckPairDefinitionOnMapping` to false to skip definition check while mapping entities to DTOs and vice verse.

If you don't like this approach but want to add a single property to multiple objects (DTOs) easier, `AddOrUpdateProperty` can get an array of types to add the extra property:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<string>(
        new[]
        {
            typeof(IdentityUserDto),
            typeof(IdentityUserCreateDto),
            typeof(IdentityUserUpdateDto)
        },
        "SocialSecurityNumber"
    );
````

### About the User Interface

This system allows you to add extra properties to entities and DTOs and execute custom business code, however it does nothing related to the User Interface.

See [Overriding the User Interface](Customizing-Application-Modules-Overriding-User-Interface.md) guide for the UI part.

## How to Find the Services?

[Module documents](Modules/Index.md) includes the list of the major services they define. In addition, you can investigate [their source code](https://github.com/abpframework/abp/tree/dev/modules) to explore all the services.
