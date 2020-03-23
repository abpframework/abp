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

This example class inherits from the `IdentityUserManager` [domain service](Domain-Services.md) and overrides the `CreateAsync` method to perform the same phone number check implemented above. The result is same, but this time we've implemented it inside the domain service assuming that this is a **core domain logic** for our system.

> `[ExposeServices(typeof(IdentityUserManager))]`  attribute is **required** here since `IdentityUserManager` does not define an interface (like `IIdentityUserManager`) and dependency injection system doesn't expose services for inherited classes (like it does for the implemented interfaces) by convention.

Check the [localization system](Localization.md) to learn how to localize the error messages.

### Overriding Other Classes

Overriding controllers, framework services, view component classes and any other type of classes registered to dependency injection can be overridden just like the examples above.

## How to Find the Services?

[Module documents](Modules/Index.md) includes the list of the major services they define. In addition, you can investigate [their source code](https://github.com/abpframework/abp/tree/dev/modules) to explore all the services.