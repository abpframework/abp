```json
//[doc-seo]
{
    "Description": "Explore the ABP OpenIddict Module for seamless authentication, including SSO and API access control. Easily integrate and customize for your needs."
}
```

# ABP OpenIddict Module

OpenIddict module provides an integration with the [OpenIddict](https://github.com/openiddict/openiddict-core) which provides advanced authentication features like single sign-on, single log-out, and API access control. This module persists applications, scopes, and other OpenIddict-related objects to the database.

## How to Install

This module comes as pre-installed (as NuGet/NPM packages). You can continue to use it as a package and get updates easily, or you can include its source code into your solution (see `get-source` [CLI](../cli) command) to develop your custom module.

### The Source Code

The source code of this module can be accessed [here](https://github.com/abpframework/abp/tree/dev/modules/openiddict). The source code is licensed by [MIT](https://choosealicense.com/licenses/mit/), so you can freely use and customize it.

## User Interface

This module implements the domain logic and database integrations but does not provide a management UI. Management UI is useful if you need to add applications and scopes on the fly. In this case, you may build the management UI yourself or consider purchasing the [ABP](https://abp.io/) which provides the management UI for this module. The ASP.NET Core integration includes the authorization consent view used by the protocol flow.

## Relations to Other Modules

This module is based on the [Identity Module](./identity.md) and has an [integration package](https://www.nuget.org/packages/Volo.Abp.Account.Web.OpenIddict) with the [Account Module](account.md).

The optional `Volo.Abp.PermissionManagement.Domain.OpenIddict` package integrates OpenIddict applications with the [Permission Management Module](./permission-management.md). It provides client permission managers and providers. Its distributed event handlers also move client permission grants when a client ID changes and delete them when the application is deleted.

## Options

### OpenIddictBuilder

`OpenIddictBuilder` can be configured in the `PreConfigureServices` method of your OpenIddict [module](../framework/architecture/modularity/basics.md). 

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

- `AddAudiences()` for resource servers.
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

OpenIddict applications represent the clients that can request tokens from your OpenIddict server.

- `OpenIddictApplication` (aggregate root): Represents an OpenIddict application.
  - `ApplicationType` (string): The application type associated with the application.
  - `ClientId` (string): The client identifier associated with the current application.
  - `ClientSecret` (string): The client secret associated with the current application. Maybe hashed or encrypted for security reasons.
  - `ClientType` (string): The client type associated with the current application.
  - `ConsentType` (string): The consent type associated with the current application.
  - `DisplayName` (string): The display name associated with the current application.
  - `DisplayNames` (string): The localized display names associated with the current application serialized as a JSON object.
  - `JsonWebKeySet` (string): The JSON Web Key Set associated with the application, serialized as a JSON object.
  - `Permissions` (string): The permissions associated with the current application, serialized as a JSON array.
  - `PostLogoutRedirectUris` (string): The logout callback URLs associated with the current application, serialized as a JSON array.
  - `Properties` (string): The additional properties associated with the current application serialized as a JSON object or null.
  - `RedirectUris` (string): The callback URLs associated with the current application, serialized as a JSON array.
  - `Requirements` (string): The requirements associated with the current application, serialized as a JSON array.
  - `Settings` (string): The settings associated with the current application, serialized as a JSON object.
  - `FrontChannelLogoutUri` (string): The front-channel logout URI associated with the application.
  - `ClientUri` (string): URI to further information about client.
  - `LogoUri` (string): URI to client logo.

##### OpenIddictAuthorization

OpenIddictAuthorizations are used to keep the allowed scopes, authorization flow types.

- `OpenIddictAuthorization` (aggregate root): Represents an OpenIddict authorization.

  - `ApplicationId` (Guid?): The application associated with the current authorization.

  - `CreationDate` (DateTime?): The UTC creation date of the current authorization.

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
  - `AuthorizationId` (Guid?): The authorization associated with the current token.
  - `CreationDate` (DateTime?): The UTC creation date of the current token.
  - `ExpirationDate` (DateTime?): The UTC expiration date of the current token.
  - `Payload` (string): The payload of the current token, if applicable. Only used for reference tokens and may be encrypted for security reasons.

  - `Properties` (string): The additional properties associated with the current token serialized as a JSON object or null.
  - `RedemptionDate` (DateTime?): The UTC redemption date of the current token.

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

#### AbpOpenIddictStoreOptions

`AbpOpenIddictStoreOptions` controls the transaction isolation levels used by destructive store operations:

- `PruneIsolationLevel` defaults to `IsolationLevel.RepeatableRead` and is used while pruning tokens and authorizations.
- `DeleteIsolationLevel` defaults to `IsolationLevel.Serializable` and is used when deleting applications and authorizations together with their related records.

You can change these values when your database requires a different isolation level:

```csharp
Configure<AbpOpenIddictStoreOptions>(options =>
{
    options.PruneIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
    options.DeleteIsolationLevel = System.Data.IsolationLevel.RepeatableRead;
});
```

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

This module uses `AbpOpenIddict` for the connection string name. If you don't define a connection string with this name, it falls back to the `Default` connection string.

See the [connection strings](../framework/fundamentals/connection-strings.md) documentation for details.

The built-in EF Core and MongoDB contexts are marked with `IgnoreMultiTenancy`, so OpenIddict data belongs to the host/shared database rather than to tenant databases. In addition, the EF Core `ConfigureOpenIddict()` extension skips the OpenIddict model when the current database is configured as tenant-only.

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

This module integrates with ASP.NET Core and provides pass-through MVC controllers for the authorization, token, end-session and userinfo endpoints. It uses OpenIddict's [pass-through mode](https://documentation.openiddict.com/guides/index.html#pass-through-mode).

| Controller | Route |
| --- | --- |
| `AuthorizeController` | `/connect/authorize` |
| `TokenController` | `/connect/token` |
| `LogoutController` | `/connect/endsession` |
| `UserInfoController` | `/connect/userinfo` |

The server enables authorization code, hybrid, implicit, password, client credentials, refresh token, device authorization, none and token exchange flows by default. A client application still needs the corresponding endpoint, grant type, scope and response type permissions. This module handles device-code token requests and registers the end-user verification endpoint, but it does not provide the end-user verification UI.

### AbpOpenIddictAspNetCoreOptions

`AbpOpenIddictAspNetCoreOptions` can be configured in the `PreConfigureServices` method of your OpenIddict [module](../framework/architecture/modularity/basics.md). 

Example:

```csharp
PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
{
    //Set options here...
});
```

`AbpOpenIddictAspNetCoreOptions` properties:

- `UpdateAbpClaimTypes(default: true)`:  Updates `AbpClaimTypes` to be compatible with the OpenIddict claims.
- `AddDevelopmentEncryptionAndSigningCertificate(default: true)`:  Registers (and generates if necessary) a user-specific development encryption/development signing certificate. This is a certificate used for signing and encrypting the tokens and for **development environment only**. You must set it to **false** for non-development environments.
- `AttachCultureInfo` (default: true): Adds the current `culture` and `ui-culture` values to authorization responses when those parameters have not already been set.
- `SelectAccountPage` (default: `~/Account/SelectAccount`): Sets the page used when an authorization request specifies `prompt=select_account`. The open-source module performs the redirect but doesn't provide a page at this path. If clients can send this prompt, implement the page in your host and set this option to its route.
- `UseDefaultScopesForClientCredentials(default: false)`: When set to `true`, the access token issued for the `client_credentials` grant automatically grants the scopes configured on the client application (permissions prefixed with `oi_scp:`) when the client does not explicitly request any scope.
- `UseDefaultScopesForPassword(default: false)`: When set to `true`, the token response for the `password` grant automatically grants the scopes configured on the client application when the client does not explicitly request any scope. If the configured scopes include `openid`/`profile`/`email`/`roles`, the corresponding `id_token` and claim destinations are affected as well.
- `UseDefaultScopesForTokenExchange(default: false)`: When set to `true`, the token response for the `urn:ietf:params:oauth:grant-type:token-exchange` grant automatically grants the scopes configured on the client application when the client does not explicitly request any scope. If the configured scopes include `openid`/`profile`/`email`/`roles`, the corresponding `id_token` and claim destinations are affected as well.

Example to enable the default-scope fallback for the `client_credentials` grant:

```csharp
PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
{
    options.UseDefaultScopesForClientCredentials = true;
});
```

> `AddDevelopmentEncryptionAndSigningCertificate` cannot be used in applications deployed on IIS or Azure App Service: trying to use them on IIS or Azure App Service will result in an exception being thrown at runtime (unless the application pool is configured to load a user profile). To avoid that, consider creating self-signed certificates and storing them in the X.509 certificates store of the host machine(s). Please refer to: https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html#registering-a-development-certificate

#### Automatically Removing Invalid Tokens and Authorizations

The cleanup worker prunes old invalid or expired tokens first, and then prunes old invalid authorizations and ad-hoc authorizations that no longer have a token. It uses a distributed lock, so only one application instance performs a cleanup pass at a time.

`TokenCleanupOptions` can be configured in the `ConfigureServices` method of your OpenIddict [module](../framework/architecture/modularity/basics.md). 

Example:

```csharp
Configure<TokenCleanupOptions>(options =>
{
    //Set options here...	
});
```

`TokenCleanupOptions` properties:

- `IsCleanupEnabled` (default: true): Controls whether the cleanup worker is registered.
- `CleanupPeriod` (default: 3,600,000 ms): Sets the interval between cleanup passes.
- `DisableAuthorizationPruning` (default: false): Disables authorization pruning when set to `true`.
- `DisableTokenPruning` (default: false): Disables token pruning when set to `true`.
- `MinimumAuthorizationLifespan` (default: 14 days): Sets the minimum age of authorizations that can be pruned.
- `MinimumTokenLifespan` (default: 14 days): Sets the minimum age of tokens that can be pruned.

The worker is registered during application initialization only when `IsCleanupEnabled` is `true`. The global [background worker](../framework/infrastructure/background-workers/index.md) switch must also be enabled for it to run.

#### Updating Claims In Access_token and Id_token

[Claims Principal Factory](../framework/fundamentals/authorization/index.md#claims-principal-factory) can be used to add/remove claims to the `ClaimsPrincipal`.

The `AbpDefaultOpenIddictClaimsPrincipalHandler` service will add `Name`, `Email,` and `Role` types of Claims to `access_token` and `id_token`, other claims are only added to `access_token` by default, and remove the `SecurityStampClaimType` secret claim of `Identity`.

Create a service that inherits from `IAbpOpenIddictClaimsPrincipalHandler` and add it to DI to fully control the destinations of claims.

```cs
public class MyClaimDestinationsHandler : IAbpOpenIddictClaimsPrincipalHandler, ITransientDependency
{
    public virtual Task HandleAsync(AbpOpenIddictClaimsPrincipalHandlerContext context)
    {
        foreach (var claim in context.Principal.Claims)
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

Configure<AbpOpenIddictClaimsPrincipalOptions>(options =>
{
    options.ClaimsPrincipalHandlers.Add<MyClaimDestinationsHandler>();
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

### Disable Transport Security Requirement

By default, OpenIddict requires the use of HTTPS for all endpoints. You can disable it if it's needed. You just need to configure the `OpenIddictServerAspNetCoreOptions` and set `DisableTransportSecurityRequirement` as **true**:

```cs
Configure<OpenIddictServerAspNetCoreOptions>(options =>
{
    options.DisableTransportSecurityRequirement = true;
});
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

After validating the `username` and `password`, the controller creates a `ClaimsPrincipal` and returns a `SignInResult` that uses the `OpenIddict.Server.AspNetCore` authentication scheme. `OpenIddictServerAspNetCoreHandler` then processes the result.

`OpenIddictServerAspNetCoreHandler` do some checks to generate json and replace the http response content.

The `ForbidResult` `ChallengeResult` are all the above types of processing.

If you need to customize OpenIddict, you need to replace/delete/add new handlers and make it execute in the correct order.

Please refer to:
https://documentation.openiddict.com/guides/index.html#events-model

### Custom Token Grant Types

Implement `ITokenExtensionGrant` to handle a custom token grant. Register the grant type with OpenIddict and add the handler instance to `AbpOpenIddictExtensionGrantsOptions`:

```csharp
PreConfigure<OpenIddictServerBuilder>(builder =>
{
    builder.Configure(options =>
    {
        options.GrantTypes.Add(MyTokenExtensionGrant.GrantType);
    });
});

Configure<AbpOpenIddictExtensionGrantsOptions>(options =>
{
    options.Grants.Add(
        MyTokenExtensionGrant.GrantType,
        new MyTokenExtensionGrant()
    );
});
```

When a request uses an otherwise unhandled grant type, `TokenController` resolves the registered `ITokenExtensionGrant` and calls its `HandleAsync` method with the current `HttpContext` and `OpenIddictRequest`. See [How to add a custom grant type in OpenIddict](../Community-Articles/2022-11-14-How-to-add-a-custom-grant-type-in-OpenIddict/POST.md) for a longer example.

### PKCE

https://documentation.openiddict.com/configuration/proof-key-for-code-exchange.html

### Setting Tokens Lifetime

Update `PreConfigureServices` method of AuthServerModule (or HttpApiHostModule if you don't have tiered/separate-authserver) file:

```csharp
PreConfigure<OpenIddictServerBuilder>(builder =>
{
    builder.SetAuthorizationCodeLifetime(TimeSpan.FromMinutes(30));
    builder.SetAccessTokenLifetime(TimeSpan.FromMinutes(30));
    builder.SetIdentityTokenLifetime(TimeSpan.FromMinutes(30));
    builder.SetRefreshTokenLifetime(TimeSpan.FromDays(14));
});
```

### Refresh Token

To use refresh token, it must be supported by OpenIddictServer and the `refresh_token` must be requested by the application.

> **Note:** Angular application is already configured to use `refresh_token`.

#### Configuring OpenIddictServer

Update the **OpenIddictDataSeedContributor**, add `OpenIddictConstants.GrantTypes.RefreshToken` to grant types in `CreateApplicationAsync` method:

```csharp
await CreateApplicationAsync(
    ...
    grantTypes: new List<string> //Hybrid flow
    {
        OpenIddictConstants.GrantTypes.AuthorizationCode,
        OpenIddictConstants.GrantTypes.Implicit,
        OpenIddictConstants.GrantTypes.RefreshToken,
    },
    ...
```

> **Note:** The current startup template updates the permissions and redirect URI values of an existing client when data seeding runs. Run the database migrator or the data seeder after changing the contributor. If a custom or older contributor only creates missing clients, add an existing-client update path or recreate the client.

#### Configuring Application:

You need to request the **offline_access scope** to be able to receive `refresh_token`. 

In **Razor/MVC, Blazor-Server applications**, add `options.Scope.Add("offline_access");` to **OpenIdConnect** options. These application templates are using cookie authentication by default and has default cookie expire options set as:

```csharp
.AddCookie("Cookies", options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(365);
})
```

[Cookie ExpireTimeSpan will ignore access_token expiration](https://learn.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions.ExpireTimeSpan?view=aspnetcore-7.0&viewFallbackFrom=net-7.0) and expired access_token will still be valid if it is set to higher value than the `refresh_token lifetime`. It is recommended to keep **Cookie ExpireTimeSpan** and the **Refresh Token lifetime** same, hence the new token will be persisted in the cookie.

In **Blazor wasm** applications, add `options.ProviderOptions.DefaultScopes.Add("offline_access");` to **AddOidcAuthentication** options.

In **Angular** applications, add `offline_access` to **oAuthConfig**  scopes in *environment.ts* file. (Angular applications already have this configuration).

## About localization

We don't localize any error messages in the OpenIddict module, Because the OAuth 2.0 specification restricts the charset you're allowed to use for the error and error_description parameters:

> A.7. "error" Syntax
> The "error" element is defined in Sections 4.1.2.1, 4.2.2.1, 5.2, 7.2, and 8.5:

```
error = 1*NQSCHAR
```

> A.8. "error_description" Syntax
>T he "error_description" element is defined in Sections 4.1.2.1, 4.2.2.1, 5.2, and 7.2:

```
error-description = 1*NQSCHAR
NQSCHAR = %x20-21 / %x23-5B / %x5D-7E
```

## Demo projects

In the module's `app` directory there are six projects(including `angular`)

* `OpenIddict.Demo.Server`: An abp application with integrated modules (has two `clients` and a `scope`). 
* `OpenIddict.Demo.API`: ASP NET Core API application using JwtBearer authentication.
* `OpenIddict.Demo.Client.Mvc`: ASP NET Core MVC application using `OpenIdConnect` for authentication.
* `OpenIddict.Demo.Client.Console`: Use `IdentityModel` to test OpenIddict's various endpoints, and call the api of `OpenIddict.Demo.API`.
* `OpenIddict.Demo.Client.BlazorWASM:` ASP NET Core Blazor application using `OidcAuthentication` for authentication.
* `angular`: An angular application that integrates the abp ng modules and uses oauth for authentication.

### How to run?

Confirm the connection string of `appsettings.json` in the `OpenIddict.Demo.Server` project. Running the project will automatically create the database and initialize the data. 
After running the `OpenIddict.Demo.API` project, then you can run the rest of the projects to test.

## Migrating Guide

[Migrating from IdentityServer to OpenIddict Step by Step Guide ](../release-info/migration-guides/openiddict-step-by-step.md)
