## ABP OpenIddict Modules

## How to Install

TODO:

## User Interface

This module implements the domain logic and database integrations, but not provides any UI. Management UI is useful if you need to add applications and scopes on the fly. In this case, you may build the management UI yourself or consider to purchase the [ABP Commercial](https://commercial.abp.io/) which provides the management UI for this module.

## Relations to Other Modules

This module is based on the [Identity Module](Identity.md) and have an [integration package](https://www.nuget.org/packages/Volo.Abp.Account.Web.OpenIddict) with the [Account Module](Account.md).

## The module

### Demo projects

In the module's `app` directory there are six projects(including `angular`)

* `OpenIddict.Demo.Server`: An abp application with integrated modules (has two `clients` and a `scope`). 
* `OpenIddict.Demo.API`: ASP NET Core API application using JwtBearer authentication
* `OpenIddict.Demo.Client.Mvc`: ASP NET Core MVC application using `OpenIdConnect` for authentication
* `OpenIddict.Demo.Client.Console`: Use `IdentityModel` to test OpenIddict's various endpoints, and call the api of `OpenIddict.Demo.API`
* `OpenIddict.Demo.Client.BlazorWASM:` ASP NET Core Blazor application using `OidcAuthentication` for authentication
* `angular`: An angular application that integrates the abp ng modules and uses oauth for authentication

#### How to run?

Confirm the connection string of `appsettings.json` in the `OpenIddict.Demo.Server` project. Running the project will automatically create the database and initialize the data. 
After running the `OpenIddict.Demo.API` project, then you can run the rest of the projects to test.

### Domain module

There are four main entities included in this module.

* OpenIddictApplication: **Represents applications(client)**
* OpenIddictScope: **Represents scopes**
* OpenIddictAuthorization: **Represents authorizations, Track of logical chains of tokens and user consent..**
* OpenIddictToken: **Represents various tokens.**

Domain also implements four store interfaces in OpenIddict, OpenIddict uses store to manage entities, corresponding to the above four entities, Custom entity repository is used in the store.


```cs
//Manager
OpenIddictApplicationManager
OpenIddictScopeManager
OpenIddictAuthorizationManager
OpenIddictTokenManager

//Store
IOpenIddictApplicationStore
IOpenIddictScopeStore
IOpenIddictAuthorizationStore
IOpenIddictTokenStore

//Repository
IOpenIddictApplicationRepository
IOpenIddictScopeRepository
IOpenIddictAuthorizationRepository
IOpenIddictTokenRepository
```

We enabled most of OpenIddict's features in the `AddOpenIddict` method, You can change OpenIddict's related builder options via `PreConfigure`.

```cs
PreConfigure<OpenIddictBuilder>(builder =>
{
    //builder
});

PreConfigure<OpenIddictCoreBuilder>(builder =>
{
    //builder
});

PreConfigure<OpenIddictServerBuilder>(builder =>
{
    //builder
});
```

#### AbpOpenIddictAspNetCoreOptions

`UpdateAbpClaimTypes(default: true)`:  Updates AbpClaimTypes to be compatible with identity server claims.
`AddDevelopmentEncryptionAndSigningCertificate(default: true)`:  Registers (and generates if necessary) a user-specific development encryption/development signing certificate.

You can also change this options via `PreConfigure`.

#### Automatically removing orphaned tokens/authorizations

There is a background task in the `Domain` module (`enabled by default`) that automatically removes orphaned tokens/authorizations, you can configure `TokenCleanupOptions` to manage it.

### ASP NET Core module

This module integrates ASP NET Core, with built-in MVC controllers for four protocols. It uses OpenIddict's [Pass-through mode](https://documentation.openiddict.com/guides/index.html#pass-through-mode).

```cs
AuthorizeController -> connect/authorize
TokenController     -> connect/token
LogoutController    -> connect/logout
UserInfoController  -> connect/userinfo
```

> We will implement the related functions of **device flow** in the PRO module..

#### How to control claims in access_token and id_token

You can use the [Claims Principal Factory](https://docs.abp.io/en/abp/latest/Authorization#claims-principal-factory) to add/remove claims to the `ClaimsPrincipal`.

The `AbpDefaultOpenIddictClaimDestinationsProvider` service will add `Name`, `Email` and `Role` types of Claims to `access_token` and `id_token`, other claims are only added to `access_token` by default, and remove the `SecurityStampClaimType` secret claim of `Identity`.

You can create a service that inherits from `IAbpOpenIddictClaimDestinationsProvider` and add it to DI to fully control the destinations of claims

```cs
public class MyClaimDestinationsProvider : IAbpOpenIddictClaimDestinationsProvider, ITransientDependency
{
    public virtual Task SetDestinationsAsync(AbpOpenIddictClaimDestinationsProviderContext context)
    {
		// ...
        return Task.CompletedTask;
    }
}

Configure<AbpOpenIddictClaimDestinationsOptions>(options =>
{
    options.ClaimDestinationsProvider.Add<MyClaimDestinationsProvider>();
});
```

For detailed information, please refer to: [OpenIddict claim destinations](https://documentation.openiddict.com/configuration/claim-destinations.html)

### EF Core module

Implements the above four repository interfaces.

### MongoDB module

Implements the above four repository interfaces.


## OpenIddict

### Documentation

For more details about OpenIddict, please refer to its official documentation and Github.

https://documentation.openiddict.com

https://github.com/openiddict/openiddict-core#resources

### Disable AccessToken Encryption

ABP disables the `access token encryption` by default for compatibility, you can manually enable it if needed.

```cs
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<OpenIddictServerBuilder>(builder =>
    {
        builder.Configure(options => options.DisableAccessTokenEncryption = false);
    });
}
```

https://documentation.openiddict.com/configuration/token-formats.html#disabling-jwt-access-token-encryption


### PKCE

https://documentation.openiddict.com/configuration/proof-key-for-code-exchange.html

### Request/Response process

I will briefly introduce the principle of OpenIddict so that everyone can quickly understand it.

The `OpenIddict.Server.AspNetCore` adds an authentication scheme(`Name: OpenIddict.Server.AspNetCore, handler: OpenIddictServerAspNetCoreHandler`) and implements the `IAuthenticationRequestHandler` interface.

It will be executed first in `AuthenticationMiddleware` and can short-circuit the current request. Otherwise, `DefaultAuthenticateScheme` will be called and continue to execute the pipeline.

`OpenIddictServerAspNetCoreHandler` will call various built-in handlers(Handling requests and responses), And the handler will process according to the context or skip logic that has nothing to do with it.

Example a token request: 

```
POST /connect/token HTTP/1.1
Content-Type: application/x-www-form-urlencoded

    grant_type=password&
    client_id=AbpApp&
    client_secret=1q2w3e*&
    username=admin&
    password=1q2w3E*&
    scope=AbpAPI offline_access
```

This request will be processed by various handlers. They will confirm the endpoint type of the request, check `http/https`, verify that the request parameters (`client. scope etc`) are valid and exist in the database, etc. Various protocol checks. And build a `OpenIddictRequest` object, If there are any errors, the response content may be set and directly short-circuit the current request.

If everything is ok, the request will go to our processing controller(eg `TokenController`), we can get an `OpenIddictRequest` from the http request at this time. The rest of our work will be based on this object.

We may check the `username` and `password` in the request. If it is correct we create a `ClaimsPrincipal` object and return a `SignInResult`, which uses the `OpenIddict.Validation.AspNetCore` authentication scheme name, will calls `OpenIddictServerAspNetCoreHandler` for processing. 

`OpenIddictServerAspNetCoreHandler` do some checks to generate json and replace the http response content.

The `ForbidResult` `ChallengeResult` are all the above types of processing.

If you need to customize OpenIddict, you need to replace/delete/add new handlers and make it execute in the correct order.

Please refer to:
https://documentation.openiddict.com/guides/index.html#events-model

## Sponsor

Please consider sponsoring this project: https://github.com/sponsors/kevinchalet
