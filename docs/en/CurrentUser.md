# Current User

It is very common to retrieve the information about the logged in user in a web application. The current user is the active user related to the current request in a web application.

## ICurrentUser

`ICurrentUser` is the main service to get info about the current active user.

Example: [Injecting](Dependency-Injection.md) the `ICurrentUser` into a service:

````csharp
using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace AbpDemo
{
    public class MyService : ITransientDependency
    {
        private readonly ICurrentUser _currentUser;

        public MyService(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }
        
        public void Foo()
        {
            Guid? userId = _currentUser.Id;
        }
    }
}
````

Common base classes have already injected this service as a base property. For example, you can directly use the `CurrentUser` property in an [application service](Application-Services.md):

````csharp
using System;
using Volo.Abp.Application.Services;

namespace AbpDemo
{
    public class MyAppService : ApplicationService
    {
        public void Foo()
        {
            Guid? userId = CurrentUser.Id;
        }
    }
}
````

### Properties

Here are the fundamental properties of the `ICurrentUser` interface:

* **IsAuthenticated** (bool): Returns `true` if the current user has logged in (authenticated). If the user has not logged in then `Id` and `UserName` returns `null`.
* **Id** (Guid?): Id of the current user. Returns `null`, if the current user has not logged in.
* **UserName** (string): User name of the current user. Returns `null`, if the current user has not logged in.
* **TenantId** (Guid?): Tenant Id of the current user, which can be useful for a [multi-tenant](Multi-Tenancy.md) application. Returns `null`, if the current user is not assigned to a tenant.
* **Email** (string): Email address of the current user.Returns `null`, if the current user has not logged in or not set an email address.
* **EmailVerified** (bool): Returns `true`, if the email address of the current user has been verified.
* **PhoneNumber** (string): Phone number of the current user. Returns `null`, if the current user has not logged in or not set a phone number.
* **PhoneNumberVerified** (bool): Returns `true`, if the phone number of the current user has been verified.
* **Roles** (string[]): Roles of the current user. Returns a string array of the role names of the current user.

### Methods

`ICurrentUser` is implemented on the `ICurrentPrincipalAccessor` (see the section below) and works with the claims. So, all of the above properties are actually retrieved from the claims of the current authenticated user.

`ICurrentUser` has some methods to directly work with the claims, if you have custom claims or get other non-common claim types.

* **FindClaim**: Gets a claim with the given name. Returns `null` if not found.
* **FindClaims**: Gets all the claims with the given name (it is allowed to have multiple claim values with the same name).
* **GetAllClaims**: Gets all the claims.
* **IsInRole**: A shortcut method to check if the current user is in the specified role.

Beside these standard methods, there are some extension methods:

* **FindClaimValue**: Gets the value of the claim with the given name, or `null` if not found. It has a generic overload that also casts the value to a specific type.
* **GetId**: Returns `Id` of the current user. If the current user has not logged in, it throws an exception (instead of returning `null`) . Use this only if you are sure that the user has already authenticated in your code context.

### Authentication & Authorization

`ICurrentUser` works independently of how the user is authenticated or authorized. It seamlessly works with any authentication system that works with the current principal (see the section below).

## ICurrentPrincipalAccessor

`ICurrentPrincipalAccessor` is the service that should be used (by the ABP Framework and your application code) whenever the current principal of the current user is needed.

For a web application, it gets the `User` property of the current `HttpContext`. For a non-web application, it returns the `Thread.CurrentPrincipal`.

> You generally don't need to this low level `ICurrentPrincipalAccessor` service and directly work with the `ICurrentUser` explained above.

### Basic Usage

You can inject `ICurrentPrincipalAccessor` and use the `Principal` property to the the current principal:

````csharp
public class MyService : ITransientDependency
{
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

    public MyService(ICurrentPrincipalAccessor currentPrincipalAccessor)
    {
        _currentPrincipalAccessor = currentPrincipalAccessor;
    }
    
    public void Foo()
    {
        var allClaims = _currentPrincipalAccessor.Principal.Claims.ToList();
        //...
    }
}
````

### Changing the Current Principal

Current principal is not something you want to set or change, except at some advanced scenarios. If you need it, use the `Change` method of the `ICurrentPrincipalAccessor`. It takes a `ClaimsPrincipal` object and makes it "current" for a scope.

Example:

````csharp
public class MyAppService : ApplicationService
{
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

    public MyAppService(ICurrentPrincipalAccessor currentPrincipalAccessor)
    {
        _currentPrincipalAccessor = currentPrincipalAccessor;
    }

    public void Foo()
    {
        var newPrincipal = new ClaimsPrincipal(
            new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(AbpClaimTypes.UserId, Guid.NewGuid().ToString()),
                    new Claim(AbpClaimTypes.UserName, "john"),
                    new Claim("MyCustomCliam", "42")
                }
            )
        );

        using (_currentPrincipalAccessor.Change(newPrincipal))
        {
            var userName = CurrentUser.UserName; //returns "john"
            //...
        }
    }
}
````

Use the `Change` method always in a `using` statement, so it will be restored to the original value after the `using` scope ends.

This can be a way to simulate a user login for a scope of the application code, however try to use it carefully.

## AbpClaimTypes

`AbpClaimTypes` is a static class that defines the names of the standard claims and used by the ABP Framework.

* Default values for the `UserName`, `UserId`, `Role` and `Email` properties are set from the [System.Security.Claims.ClaimTypes](https://docs.microsoft.com/en-us/dotnet/api/system.security.claims.claimtypes) class, but you can change them.
* Other properties, like `EmailVerified`, `PhoneNumber`, `TenantId`... are defined by the ABP Framework by following the standard names wherever possible.

It is suggested to use properties of this class instead of magic strings for claim names.

