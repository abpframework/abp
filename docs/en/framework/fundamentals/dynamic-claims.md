# Dynamic Claims

When a client authenticates and obtains an access token or an authentication cookie, the claims in that token or cookie are not changed unless it re-authenticates. That is not a problem for most claims since the claim values do not frequently change. However, for some claims, it may be required to immediately see the impact after the claim values change in the current session. For example, if a role is revoked from a user, you want to see its effect in the next request. Otherwise, the user will continue to use that role's permissions until re-login to the application.

ABP's dynamic claims feature dynamically overrides the configured claim values in the client's authentication token/cookie with the latest values of these claims.

## How to Use

This feature is disabled by default. You should enable it for your application and use the Dynamic Claims middleware.

> **Beginning from the v8.0, all the [startup templates](../../solution-templates) are pre-configured and the dynamic claims feature is enabled by default. So, if you have created a solution with v8.0 and above, you don't need to make any configuration. Follow the instructions only if you've upgraded from a version lower than 8.0.**

### Enabling / Disabling the Dynamic Claims

You can enable it by the following code:

````csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
    {
        options.IsDynamicClaimsEnabled = true; //set it "true" to enable "Dynamic Claims" or "false" to disable it.
    });
}
````

This is typically done on the authentication server. In a monolith application, you will typically have a single application, so you can configure it. If you are using the tiered solution structure (where the UI part is hosted in a separate application) you will need to also set the `RemoteRefreshUrl` to the Authentication Server's URL in the UI application. Example:

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

> The `RemoteRefreshUrl` is already configured inside methods `AddAbpOpenIdConnect` and `AddAbpJwtBearer`. 


### The Dynamic Claims Middleware

Add the `DynamicClaims` middleware to all the applications that performs authentication (including the authentication server):

````csharp
public override void OnApplicationInitialization(
    ApplicationInitializationContext context)
{
    //...
    app.UseDynamicClaims(); // Add this line before UseAuthorization.
    app.UseAuthorization();
    //...
}
````

## How It Works

The `DynamicClaims` middleware will use `IAbpClaimsPrincipalFactory` to dynamically generate claims for the current user(`HttpContext.User`) in each request.

There are three pre-built implementations of `IAbpDynamicClaimsPrincipalContributor` for different scenarios:

* `IdentityDynamicClaimsPrincipalContributor`: Provided by the [Identity module](../../modules/identity.md) and generates and overrides the actual dynamic claims, and writes to the distributed cache. Typically works in the authentication server in a distributed system.
* `RemoteDynamicClaimsPrincipalContributor`: For distributed scenarios, this implementation works in the UI application. It tries to get dynamic claim values in the distributed cache. If not found in the distributed cache, it makes an HTTP call to the authentication server and requests filling it by the authentication server. `AbpClaimsPrincipalFactoryOptions.RemoteRefreshUrl` should be properly configure to make it running.
* `WebRemoteDynamicClaimsPrincipalContributor`: Similar to the `RemoteDynamicClaimsPrincipalContributor` but works in the microservice applications.

### IAbpDynamicClaimsPrincipalContributor

If you want to add your own dynamic claims contributor, you can create a class that implement the `IAbpDynamicClaimsPrincipalContributor` interface (and register it to the [dependency injection](./dependency-injection.md) system. ABP will call the `ContributeAsync` method to get the claims. It better to use a kind of cache to improve the performance since that is a frequently executed method (in every HTTP request).

## AbpClaimsPrincipalFactoryOptions

`AbpClaimsPrincipalFactoryOptions` is the main options class to configure the behavior of the dynamic claims system. It has the following properties:

* `IsDynamicClaimsEnabled`: Enable or disable the dynamic claims feature.
* `RemoteRefreshUrl`: The `url ` of the Auth Server to refresh the cache. It will be used by the `RemoteDynamicClaimsPrincipalContributor`. The default value is `/api/account/dynamic-claims/refresh ` and you should provide the full URL in the authentication server, like `http://my-account-server/api/account/dynamic-claims/refresh `.
* `DynamicClaims`: A list of dynamic claim types. Only the claims in that list will be overridden by the dynamic claims system.
* `ClaimsMap`: A dictionary to map the claim types. This is used when the claim types are different between the Auth Server and the client. Already set up for common claim types by default.

## WebRemoteDynamicClaimsPrincipalContributorOptions

`WebRemoteDynamicClaimsPrincipalContributorOptions` is the options class to configure the behavior of the `WebRemoteDynamicClaimsPrincipalContributor`. It has the following properties:

* `IsEnabled`: Enable or disable the `WebRemoteDynamicClaimsPrincipalContributor`. `false` by default.
* `AuthenticationScheme`: The authentication scheme to authenticate the HTTP call to the authentication server.
  
## See Also

* [Authorization](./authorization.md)
* [Claims-based authorization in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/claims)
* [Mapping, customizing, and transforming claims in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/claims)
