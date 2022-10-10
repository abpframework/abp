## ABP OpenIddict Module

OpenIddict module provides an integration with the [OpenIddict](https://github.com/openiddict/openiddict-core) which provides advanced authentication features like single sign-on, single log-out, and API access control. This module persists applications, scopes, and other OpenIddict-related objects to the database.

## How to Install

This module comes as pre-installed (as NuGet/NPM packages) when you [create a new solution](https://abp.io/get-started) with the ABP Framework. You can continue to use it as a package and get updates easily, or you can include its source code into your solution (see `get-source` [CLI](../CLI.md) command) to develop your custom module.

### The Source Code

The source code of this module can be accessed [here](https://github.com/abpframework/abp/tree/dev/modules/openiddict). The source code is licensed by [MIT](https://choosealicense.com/licenses/mit/), so you can freely use and customize it.

## User Interface

This module implements the domain logic and database integrations but does not provide any UI. Management UI is useful if you need to add applications and scopes on the fly. In this case, you may build the management UI yourself or consider purchasing the [ABP Commercial](https://commercial.abp.io/) which provides the management UI for this module.

## Relations to Other Modules

This module is based on the [Identity Module](Identity.md) and has an [integration package](https://www.nuget.org/packages/Volo.Abp.Account.Web.OpenIddict) with the [Account Module](Account.md).

## Options

### OpenIddictBuilder

`OpenIddictBuilder` can be configured in the `PreConfigureServices` method of your OpenIddict [module](https://docs.abp.io/en/abp/latest/Module-Development-Basics). 

Example:

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
	PreConfigure<OpenIddictBuilder>(builder =>
	{
    	//Set options here...		
	});
}
```

`OpenIddictBuilder` contains various extension methods to configure the OpenIddict services:

- `AddServer()` registers the OpenIddict token server services in the DI container. Contains `OpenIddictServerBuilder` configurations.
- `AddCore()` registers the OpenIddict core services in the DI container. Contains `OpenIddictCoreBuilder` configurations.
- `AddValidation()` registers the OpenIddict token validation services in the DI container. Contains `OpenIddictValidationBuilder` configurations.

### OpenIddictCoreBuilder

`OpenIddictCoreBuilder` contains extension methods to configure the OpenIddict core services. 

Example:

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
	PreConfigure<OpenIddictCoreBuilder>(builder =>
	{
    	//Set options here...		
	});
}
```

These services contain:

- Adding `ApplicationStore`, `AuthorizationStore`, `ScopeStore`, `TokenStore`.
- Replacing `ApplicationManager`, `AuthorizationManager`, `ScopeManager`, `TokenManager`.
- Replacing `ApplicationStoreResolver`, `AuthorizationStoreResolver`, `ScopeStoreResolver`, `TokenStoreResolver`.
- Setting `DefaultApplicationEntity`, `DefaultAuthorizationEntity`, `DefaultScopeEntity`, `DefaultTokenEntity`.

### OpenIddictServerBuilder

`OpenIddictServerBuilder` contains extension methods to configure OpenIddict server services.

Example:

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
	PreConfigure<OpenIddictServerBuilder>(builder =>
	{
    	//Set options here...		
	});
}
```

These services contain:

- Registering claims, scopes.
- Setting the `Issuer` URI that is used as the base address for the endpoint URIs returned from the discovery endpoint.
- Adding development signing keys, encryption/signing keys, credentials, and certificates.
- Adding/removing event handlers.
- Enabling/disabling grant types.
- Setting authentication server endpoint URIs.

### OpenIddictValidationBuilder

`OpenIddictValidationBuilder` contains extension methods to configure OpenIddict validation services.

Example:

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
	PreConfigure<OpenIddictValidationBuilder>(builder =>
	{
    	//Set options here...		
	});
}
```

These services contain:

- `AddAudiances()` for resource servers.
- `SetIssuer()` URI that is used to determine the actual location of the OAuth 2.0/OpenID Connect configuration document when using provider discovery.
- `SetConfiguration()` to configure `OpenIdConnectConfiguration`.
- `UseIntrospection()` to use introspection instead of local/direct validation.
- Adding encryption key, credentials, and certificates.
- Adding/removing event handlers.
- `SetClientId() ` to set the client identifier `client_id ` when communicating with the remote authorization server (e.g for introspection).
- `SetClientSecret()` to set the identifier `client_secret` when communicating with the remote authorization server (e.g for introspection).
- `EnableAuthorizationEntryValidation()` to enable authorization validation to ensure the `access token` is still valid by making a database call for each API request. *Note:* This may have a negative impact on performance and can only be used with an OpenIddict-based authorization server.
- `EnableTokenEntryValidation()` to enable authorization validation to ensure the `access token` is still valid by making a database call for each API request. *Note:* This may have a negative impact on performance and it is required when the OpenIddict server is configured to use reference tokens.
- `UseLocalServer()` to register the OpenIddict validation/server integration services.
- `UseAspNetCore()` to register the OpenIddict validation services for ASP.NET Core in the DI container.

## Internals

### Domain Layer

#### Aggregates

##### OpenIddictApplication

OpenIddictApplications represent the applications that can request tokens from your OpenIddict Server.

- `OpenIddictApplications` (aggregate root): Represents an OpenIddict application.
  - `ClientId` (string): The client identifier associated with the current application.
  - `ClientSecret` (string): The client secret associated with the current application. Maybe hashed or encrypted for security reasons.
  - `ConsentType` (string): The consent type associated with the current application.
  - `DisplayName` (string): The display name associated with the current application.
  - `DisplayNames` (string): The localized display names associated with the current application serialized as a JSON object.
  - `Permissions` (string): The permissions associated with the current application, serialized as a JSON array.
  - `PostLogoutRedirectUris` (string): The logout callback URLs associated with the current application, serialized as a JSON array.
  - `Properties` (string): The additional properties associated with the current application serialized as a JSON object or null.
  - `RedirectUris` (string): The callback URLs associated with the current application, serialized as a JSON array.
  - `Requirements` (string): The requirements associated with the current application
  - `Type` (string): The application type associated with the current application.
  - `ClientUri` (string): URI to further information about client.
  - `LogoUri` (string): URI to client logo.

##### OpenIddictAuthorization

OpenIddictAuthorizations are used to keep the allowed scopes, authorization flow types.

- `OpenIddictAuthorization` (aggregate root): Represents an OpenIddict authorization.

  - `ApplicationId` (Guid?): The application associated with the current authorization.

  - `Properties` (string): The additional properties associated with the current authorization serialized as a JSON object or null.

  - `Scopes` (string): The scopes associated with the current authorization, serialized as a JSON array.

  - `Status` (string): The status of the current authorization.

  - `Subject` (string): The subject associated with the current authorization.

  - `Type` (string): The type of the current authorization.

##### OpenIddictScope

OpenIddictScopes are used to keep the scopes of resources.

- `OpenIddictScope` (aggregate root): Represents an OpenIddict scope.

  - `Description` (string): The public description associated with the current scope.

  - `Descriptions` (string): The localized public descriptions associated with the current scope, serialized as a JSON object.

  - `DisplayName` (string): The display name associated with the current scope.

  - `DisplayNames` (string): The localized display names associated with the current scope serialized as a JSON object.

  - `Name` (string): The unique name associated with the current scope.
  - `Properties` (string): The additional properties associated with the current scope serialized as a JSON object or null.
  - `Resources` (string): The resources associated with the current scope, serialized as a JSON array.

##### OpenIddictToken

OpenIddictTokens are used to persist the application tokens.

- `OpenIddictToken` (aggregate root): Represents an OpenIddict token.

  - `ApplicationId` (Guid?): The application associated with the current token.
  - `AuthorizationId` (Guid?): The application associated with the current token.
  - `CreationDate` (DateTime?): The UTC creation date of the current token.
  - `ExpirationDate` (DateTime?): The UTC expiration date of the current token.
  - `Payload` (string): The payload of the current token, if applicable. Only used for reference tokens and may be encrypted for security reasons.

  - `Properties` (string): The additional properties associated with the current token serialized as a JSON object or null.
  - `RedemptionDate` (DateTime?): The UTC redemption date of the current token.
  - `Status` (string): The status of the current authorization.

  - `ReferenceId` (string): The reference identifier associated with the current token, if applicable. Only used for reference tokens and may be hashed or encrypted for security reasons.

  - `Status` (string): The status of the current token.

  - `Subject` (string): The subject associated with the current token.

  - `Type` (string): The type of the current token.

#### Stores

This module implements OpenIddict stores:

- `IAbpOpenIdApplicationStore`
- `IOpenIddictAuthorizationStore`
- `IOpenIddictScopeStore`
- `IOpenIddictTokenStore`

##### Repositories

The following custom repositories are defined in this module:

- `IOpenIddictApplicationRepository`
- `IOpenIddictAuthorizationRepository`
- `IOpenIddictScopeRepository`
- `IOpenIddictTokenRepository`

##### Domain Services

This module doesn't contain any domain service but overrides the service below:

- `AbpApplicationManager` used to populate/get `AbpApplicationDescriptor` information that contains `ClientUri` and `LogoUri`.

### Database Providers

#### Common

##### Table/Collection Prefix & Schema

All tables/collections use the `OpenIddict` prefix by default. Set static properties on the `AbpOpenIddictDbProperties` class if you need to change the table prefix or set a schema name (if supported by your database provider).

##### Connection String

This module uses `AbpOpenIddict` for the connection string name. If you don't define a connection string with this name, it fallbacks to the `Default` connection string.

See the [connection strings](https://docs.abp.io/en/abp/latest/Connection-Strings) documentation for details.

#### Entity Framework Core

##### Tables

- **OpenIddictApplications**
- **OpenIddictAuthorizations**
- **OpenIddictScopes**
- **OpenIddictTokens**

#### MongoDB

##### Collections

- **OpenIddictApplications**
- **OpenIddictAuthorizations**
- **OpenIddictScopes**
- **OpenIddictTokens**

## ASP.NET Core Module

This module integrates ASP NET Core, with built-in MVC controllers for four protocols. It uses OpenIddict's [Pass-through mode](https://documentation.openiddict.com/guides/index.html#pass-through-mode).

```cs
AuthorizeController -> connect/authorize
TokenController     -> connect/token
LogoutController    -> connect/logout
UserInfoController  -> connect/userinfo
```

> **Device flow** implementation will be done in the commercial module.

#### AbpOpenIddictAspNetCoreOptions

`AbpOpenIddictAspNetCoreOptions` can be configured in the `PreConfigureServices` method of your OpenIddict [module](https://docs.abp.io/en/abp/latest/Module-Development-Basics). 

Example:

```csharp
PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
{
    //Set options here...
});
```

`AbpOpenIddictAspNetCoreOptions` properties:

- `UpdateAbpClaimTypes(default: true)`:  Updates `AbpClaimTypes` to be compatible with the Openiddict claims.
- `AddDevelopmentEncryptionAndSigningCertificate(default: true)`:  Registers (and generates if necessary) a user-specific development encryption/development signing certificate.

#### Automatically Removing Orphaned Tokens/Authorizations

The background task that automatically removes orphaned tokens/authorizations. This can be configured by `TokenCleanupOptions` to manage it.

`TokenCleanupOptions` can be configured in the `PreConfigureServices` method of your OpenIddict [module](https://docs.abp.io/en/abp/latest/Module-Development-Basics). 

Example:

```csharp
PreConfigure<TokenCleanupOptions>(options =>
{
    //Set options here...	
});
```

`TokenCleanupOptions` properties:

- `IsCleanupEnabled` (default: true): Enable/disable token clean up.
- `CleanupPeriod` (default: 3,600,000 ms):  Setting clean up period.
- `DisableAuthorizationPruning`: Setting a boolean indicating whether authorizations pruning should be disabled.
- `DisableTokenPruning`: Setting a boolean indicating whether token pruning should be disabled.
- `MinimumAuthorizationLifespan` (default: 14 days): Setting the minimum lifespan authorizations must have to be pruned. Cannot be less than 10 minutes.
- `MinimumTokenLifespan` (default: 14 days): Setting the minimum lifespan tokens must have to be pruned. Cannot be less than 10 minutes.

#### Updating Claims In Access_token and Id_token

[Claims Principal Factory](https://docs.abp.io/en/abp/latest/Authorization#claims-principal-factory) can be used to add/remove claims to the `ClaimsPrincipal`.

The `AbpDefaultOpenIddictClaimDestinationsProvider` service will add `Name`, `Email,` and `Role` types of Claims to `access_token` and `id_token`, other claims are only added to `access_token` by default, and remove the `SecurityStampClaimType` secret claim of `Identity`.

Create a service that inherits from `IAbpOpenIddictClaimDestinationsProvider` and add it to DI to fully control the destinations of claims.

```cs
public class MyClaimDestinationsProvider : IAbpOpenIddictClaimDestinationsProvider, ITransientDependency
{
    public virtual Task SetDestinationsAsync(AbpOpenIddictClaimDestinationsProviderContext context)
    {
        foreach (var claim in context.Claims)
        {
            if (claim.Type == MyClaims.MyClaimsType)
            {
                claim.SetDestinations(OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken);
            }
	    
	    if (claim.Type == MyClaims.MyClaimsType2)
            {
                claim.SetDestinations(OpenIddictConstants.Destinations.AccessToken);
            }
        }

        return Task.CompletedTask;
    }
}

Configure<AbpOpenIddictClaimDestinationsOptions>(options =>
{
    options.ClaimDestinationsProvider.Add<MyClaimDestinationsProvider>();
});
```

For detailed information, please refer to:  [OpenIddict claim destinations](https://documentation.openiddict.com/configuration/claim-destinations.html)

#### Disable AccessToken Encryption

ABP disables the `access token encryption` by default for compatibility, it can be enabled manually if needed.

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

### Request/Response Process

The `OpenIddict.Server.AspNetCore` adds an authentication scheme(`Name: OpenIddict.Server.AspNetCore, handler: OpenIddictServerAspNetCoreHandler`) and implements the `IAuthenticationRequestHandler` interface.

It will be executed first in `AuthenticationMiddleware` and can short-circuit the current request. Otherwise, `DefaultAuthenticateScheme` will be called and continue to execute the pipeline.

`OpenIddictServerAspNetCoreHandler` will call various built-in handlers (handling requests and responses), And the handler will process according to the context or skip logic that has nothing to do with it.

Example of a token request: 

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

This request will be processed by various handlers. They will confirm the endpoint type of the request, check `HTTP/HTTPS`, verify that the request parameters (`client. scope, etc`) are valid and exist in the database, etc. Various protocol checks. And build a `OpenIddictRequest` object, If there are any errors, the response content may be set and directly short-circuit the current request.

If everything is ok, the request will go to our processing controller(eg `TokenController`), we can get an `OpenIddictRequest` from the HTTP request at this time. The rest will be based on this object.

Check the `username` and `password` in the request. If it is correct create a `ClaimsPrincipal` object and return a `SignInResult`, which uses the `OpenIddict.Validation.AspNetCore` authentication scheme name, will calls `OpenIddictServerAspNetCoreHandler` for processing. 

`OpenIddictServerAspNetCoreHandler` do some checks to generate json and replace the http response content.

The `ForbidResult` `ChallengeResult` are all the above types of processing.

If you need to customize OpenIddict, you need to replace/delete/add new handlers and make it execute in the correct order.

Please refer to:
https://documentation.openiddict.com/guides/index.html#events-model

### PKCE

https://documentation.openiddict.com/configuration/proof-key-for-code-exchange.html

## Demo projects

In the module's `app` directory there are six projects(including `angular`)

* `OpenIddict.Demo.Server`: An abp application with integrated modules (has two `clients` and a `scope`). 
* `OpenIddict.Demo.API`: ASP NET Core API application using JwtBearer authentication.
* `OpenIddict.Demo.Client.Mvc`: ASP NET Core MVC application using `OpenIdConnect` for authentication.
* `OpenIddict.Demo.Client.Console`: Use `IdentityModel` to test OpenIddict's various endpoints, and call the api of `OpenIddict.Demo.API`.
* `OpenIddict.Demo.Client.BlazorWASM:` ASP NET Core Blazor application using `OidcAuthentication` for authentication.
* `angular`: An angular application that integrates the abp ng modules and uses oauth for authentication.

#### How to run?

Confirm the connection string of `appsettings.json` in the `OpenIddict.Demo.Server` project. Running the project will automatically create the database and initialize the data. 
After running the `OpenIddict.Demo.API` project, then you can run the rest of the projects to test.

## Migrating Guide

[Migrating from IdentityServer to OpenIddict Step by Step Guide ](../Migration-Guides/OpenIddict-Step-by-Step.md)
