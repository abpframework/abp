# IdentityServer Module

IdentityServer module provides a full integration with the [IdentityServer](https://github.com/IdentityServer/IdentityServer4) (IDS) framework, which provides advanced authentication features like single sign-on and API access control. This module persists clients, resources and other IDS-related objects to database.

## How to Install

This module comes as pre-installed (as NuGet/NPM packages) when you [create a new solution](https://abp.io/get-started) with the ABP Framework. You can continue to use it as package and get updates easily, or you can include its source code into your solution (see `get-source` [CLI](../CLI.md) command) to develop your custom module.

### The Source Code

The source code of this module can be accessed [here](https://github.com/abpframework/abp/tree/dev/modules/identityserver). The source code is licensed with [MIT](https://choosealicense.com/licenses/mit/), so you can freely use and customize it.

## User Interface

This module implements the domain logic and database integrations, but not provides any UI. Management UI is useful if you need to add clients and resources on the fly. In this case, you may build the management UI yourself or consider to purchase the [ABP Commercial](https://commercial.abp.io/) which provides the management UI for this module.

## Relations to Other Modules

This module is based on the [Identity Module](Identity.md) and have an [integration package](https://www.nuget.org/packages/Volo.Abp.Account.Web.IdentityServer) with the [Account Module](Account.md).

## Options

### AbpIdentityServerBuilderOptions

`AbpIdentityServerBuilderOptions` can be configured in `PreConfigureServices` method of your Identity Server [module](https://docs.abp.io/en/abp/latest/Module-Development-Basics). Example:

````csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
	PreConfigure<AbpIdentityServerBuilderOptions>(builder =>
	{
    	//Set options here...		
	});
}
````

`AbpIdentityServerBuilderOptions` properties:

* `UpdateJwtSecurityTokenHandlerDefaultInboundClaimTypeMap` (default: true): Updates `JwtSecurityTokenHandler.DefaultInboundClaimTypeMap` to be compatible with Identity Server claims.
* `UpdateAbpClaimTypes` (default: true): Updates `AbpClaimTypes` to be compatible with identity server claims.
* `IntegrateToAspNetIdentity` (default: true): Integrate to ASP.NET Identity.
* `AddDeveloperSigningCredential` (default: true): Set false to suppress AddDeveloperSigningCredential() call on the IIdentityServerBuilder.

`IIdentityServerBuilder` can be configured in `PreConfigureServices` method of your Identity Server [module](https://docs.abp.io/en/abp/latest/Module-Development-Basics). Example:

````csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
	PreConfigure<IIdentityServerBuilder>(builder =>
	{
    	builder.AddSigningCredential(...);	
	});
}
````

## Internals

### Domain Layer

#### Aggregates

##### ApiResource

API Resources are needed for allowing clients to request access tokens.

* `ApiResource` (aggregate root): Represents an API resource in the system.
  * `ApiSecret` (collection): secrets of the API resource.
  * `ApiScope` (collection): scopes of the API resource.
  * `ApiResourceClaim` (collection): claims of the API resource.

##### Client

Clients represent applications that can request tokens from your Identity Server.

* `Client` (aggregate root): Represents an Identity Server client application.
  * `ClientScope` (collection): Scopes of the client.
  * `ClientSecret` (collection): Secrets of the client.
  * `ClientGrantType` (collection): Grant types of the client.
  * `ClientCorsOrigin` (collection): CORS origins of the client.
  * `ClientRedirectUri` (collection): redirect URIs of the client.
  * `ClientPostLogoutRedirectUri` (collection): Logout redirect URIs of the client.
  * `ClientIdPRestriction` (collection): Provider restrictions of the client.
  * `ClientClaim` (collection): Claims of the client.
  * `ClientProperty` (collection): Custom properties of the client.

##### PersistedGrant

Persisted Grants stores AuthorizationCodes, RefreshTokens and UserConsent.

* `PersistedGrant` (aggregate root): Represents PersistedGrant for identity server.

##### IdentityResource

Identity resources are data like user ID, name, or email address of a user.

* `IdentityResource` (aggregate root): Represents and Identity Server identity resource.
  * `IdentityClaim` (collection): Claims of identity resource.

#### Repositories

Following custom repositories are defined for this module:

* `IApiResourceRepository`
* `IClientRepository`
* `IPersistentGrantRepository`
* `IIdentityResourceRepository`

#### Domain Services

This module doesn't contain any domain service but overrides the services below;

* `AbpProfileService` (Used when `AbpIdentityServerBuilderOptions.IntegrateToAspNetIdentity` is true)
* `AbpClaimsService`
* `AbpCorsPolicyService`

### Settings

This module doesn't define any settings.

### Application Layer

#### Application Services

* `ApiResourceAppService` (implements `IApiResourceAppService`): Implements the use cases of the API resource management UI.
* `IdentityServerClaimTypeAppService` (implement `IIdentityServerClaimTypeAppService`): Used to get list of claims.
* `ApiResourceAppService` (implements `IApiResourceAppService`): Implements the use cases of the API resource management UI.
* `IdentityResourceAppService` (implements `IIdentityResourceAppService`): Implements the use cases of the Identity resource management UI.

### Database Providers

#### Common

##### Table/Collection Prefix & Schema

All tables/collections use the `IdentityServer` prefix by default. Set static properties on the `AbpIdentityServerDbProperties` class if you need to change the table prefix or set a schema name (if supported by your database provider).

##### Connection String

This module uses `AbpIdentityServer` for the connection string name. If you don't define a connection string with this name, it fallbacks to the `Default` connection string.

See the [connection strings](https://docs.abp.io/en/abp/latest/Connection-Strings) documentation for details.

#### Entity Framework Core

##### Tables

* **IdentityServerApiResources**
  * IdentityServerApiSecrets
  * IdentityServerApiScopes
    * IdentityServerApiScopeClaims
  * IdentityServerApiClaims
* **IdentityServerClients**
  * IdentityServerClientScopes
  * IdentityServerClientSecrets
  * IdentityServerClientGrantTypes
  * IdentityServerClientCorsOrigins
  * IdentityServerClientRedirectUris
  * IdentityServerClientPostLogoutRedirectUris
  * IdentityServerClientIdPRestrictions
  * IdentityServerClientClaims
  * IdentityServerClientProperties
* **IdentityServerPersistedGrants**
* **IdentityServerIdentityResources**
  * IdentityServerIdentityClaims

#### MongoDB

##### Collections

* **IdentityServerApiResources**
* **IdentityServerClients**
* **IdentityServerPersistedGrants**
* **IdentityServerIdentityResources**