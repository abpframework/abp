# Dynamic Claims

## What is Dynamic Claims and Why do we need it

We use claims-based authentication in ASP.NET Core, It will be store the claims in the cookie or token. But the claims are static, it will be not change after the user re-login. If the user changed its username or role, we still get the old claims.

The `Dynamic Claims` feature is used to dynamically generate claims for the user in each request. You can always get the latest user claims.

## How to use it

This feature is disabled by default. You can enable it by following code:

````csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
    {
        options.IsDynamicClaimsEnabled = true;
    });
}
````

If you are using the tiered solution you need to set the `RemoteRefreshUrl` to the Auth Server url in the UI project.

````csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
    {
        options.IsDynamicClaimsEnabled = true;
        options.RemoteRefreshUrl = configuration["AuthServerUrl"] + options.RemoteRefreshUrl;
    });
}
````

Then add the `DynamicClaims` middleware.

````csharp
public override void OnApplicationInitialization(ApplicationInitializationContext context)
{
    //...
    app.UseAuthentication();

    // Add this line after UseAuthentication
    app.UseDynamicClaims();

    app.UseAuthorization();
    //...
}
````

## How it works

The `DynamicClaims` middleware will use `IAbpClaimsPrincipalFactory` to dynamically generate claims for the current user(`HttpContext.User`) in each request.

There are two implementations of `IAbpDynamicClaimsPrincipalContributor` for different scenarios.

### IdentityDynamicClaimsPrincipalContributor

This implementation is used for the `Monolithic` solution. It will get the dynamic claims from the `IUserClaimsPrincipalFactory` and add/replace the current user claims.
It uses cache to improve performance. the cache will be invalidated when the user entity changed.

### RemoteDynamicClaimsPrincipalContributor

This implementation is used for the `Tiered` solution. It will get the dynamic claims from the cache of the Auth Server. It will call the `RemoteRefreshUrl` of the Auth Server to refresh the cache when the cache is invalid.

## IAbpDynamicClaimsPrincipalContributor

If you want to add your own dynamic claims contributor, you can a class that implement the `IAbpDynamicClaimsPrincipalContributor` interface. The framework will call the `ContributeAsync` method when get the dynamic claims.

## AbpClaimsPrincipalFactoryOptions

* `IsDynamicClaimsEnabled`: Enable or disable the dynamic claims feature.
* `RemoteRefreshUrl`: The url of the Auth Server to refresh the cache. It will be used by the `RemoteDynamicClaimsPrincipalContributor`. The default value is `/api/account/dynamic-claims/refresh`.
* `DynamicClaims`: A list of dynamic claim types, `DynamicClaims contributor`` will only handle the claim type in this list.
* `ClaimsMap`: A dictionary to map the claim types. This is used when the claim types are different between the Auth Server and the client. Already set up for common claim types by default

## See Also

* [Authorization](Authorization.md)
* [Claims-based authorization in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/claims)
* [Mapping, customizing, and transforming claims in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/claims)
