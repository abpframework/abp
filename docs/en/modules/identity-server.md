```json
//[doc-seo]
{
    "Description": "Explore the IdentityServer module for ABP Framework, enabling advanced authentication features with seamless integration and database support."
}
```

# IdentityServer Module

IdentityServer module provides a full integration with the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) (IDS) framework, which provides advanced authentication features like single sign-on and API access control. This module persists clients, resources and other IDS-related objects to a database.

> **Legacy module:** The ABP startup templates have used the [OpenIddict module](./openiddict.md) instead of IdentityServer since ABP v6.0. IdentityServer4 is archived and no longer maintained by its owners. ABP still ships the IdentityServer integration packages for applications that already depend on them, but new applications should use OpenIddict. See the [IdentityServer to OpenIddict migration guide](../release-info/migration-guides/identityserver-to-openiddict.md) when upgrading an existing application.

> Note: You cannot use the IdentityServer and OpenIddict modules together. They are separate OpenID provider libraries for the same job.

## How to Install

You don't need this module when you are using the OpenIddict module. If an existing application must keep using IdentityServer4, install the corresponding IdentityServer packages and remove the OpenIddict modules. You can use the released packages or include the module source code in your solution (see the `get-source` [CLI](../cli) command) to customize it.

### The Source Code

The source code of this module can be accessed [here](https://github.com/abpframework/abp/tree/dev/modules/identityserver). The source code is licensed with [MIT](https://choosealicense.com/licenses/mit/), so you can freely use and customize it.

## User Interface

This module implements the domain logic and database integrations, but not provides any UI. Management UI is useful if you need to add clients and resources on the fly. In this case, you may build the management UI yourself or consider to purchase the [ABP](https://abp.io/) which provides the management UI for this module.

## Relations to Other Modules

This module is based on the [Identity Module](./identity.md) and have an [integration package](https://www.nuget.org/packages/Volo.Abp.Account.Web.IdentityServer) with the [Account Module](account.md).

## Options

### AbpIdentityServerBuilderOptions

`AbpIdentityServerBuilderOptions` can be configured in `PreConfigureServices` method of your Identity Server [module](../framework/architecture/modularity/basics.md). Example:

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
* `AddIdentityServerCookieAuthentication` (default: true): Adds IdentityServer's default cookie authentication handlers. Set it to `false` when the host registers and configures these handlers itself.

`IIdentityServerBuilder` can be configured in `PreConfigureServices` method of your Identity Server [module](../framework/architecture/modularity/basics.md). Example:

````csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
	PreConfigure<IIdentityServerBuilder>(builder =>
	{
    	builder.AddSigningCredential(...);	
	});
}
````

### AbpClaimsServiceOptions

`AbpClaimsServiceOptions.RequestedClaims` adds claim types to the set requested from the profile service while IdentityServer creates tokens. The module adds ABP's tenant and edition claim types by default. You can append application-specific claim types in `ConfigureServices`:

````csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<AbpClaimsServiceOptions>(options =>
    {
        options.RequestedClaims.Add("department_id");
    });
}
````

The profile service must also issue the claim for the current user; adding its name to this list does not create the claim value.

### TokenCleanupOptions

The module registers a background worker that removes expired persisted grants and device-flow codes. Configure it in `ConfigureServices`:

````csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<TokenCleanupOptions>(options =>
    {
        options.IsCleanupEnabled = true;
        options.CleanupPeriod = 3_600_000;
    });
}
````

* `IsCleanupEnabled` (default: `true`) controls whether the worker is registered. The global [background worker](../framework/infrastructure/background-workers/index.md) switch must also be enabled for it to run.
* `CleanupPeriod` (default: `3,600,000` milliseconds) sets the interval between cleanup passes.

The worker uses the distributed lock named `TokenCleanupBackgroundWorker`, so only the instance that acquires the lock performs a cleanup pass. `CleanupBatchSize` and `CleanupLoopCount` are obsolete and are no longer used by the cleanup service.

### Wildcard Subdomains for Client URLs

IdentityServer normally requires exact redirect URI and CORS origin matches. For a multi-tenant application that uses subdomains, the module provides replacement validators for client values containing a `{0}` placeholder, such as `https://{0}.mydomain.com/signin-oidc`:

````csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.AddAbpStrictRedirectUriValidator();
    context.Services.AddAbpClientConfigurationValidator();
    context.Services.AddAbpWildcardSubdomainCorsPolicyService();
}
````

Register all three services when both redirect URLs and CORS origins use the placeholder. Keep the scheme, host suffix and port as restrictive as possible; these validators expand the configured client URL boundary.

`AbpStrictRedirectUriValidator` also accepts the placeholder-free form when the configured URL with `{0}.` removed contains the requested URI. For example, configuring `http://{0}.ng.abp.io/index.html` also accepts `http://ng.abp.io`. This fallback can accept a requested URI with a shorter path than the configured value. Do not register the wildcard validator for clients that require exact redirect-path matching; keep IdentityServer's default strict validator and enumerate their exact redirect URIs instead.

`AbpWildcardSubdomainCorsPolicyService` has equivalent placeholder-free behavior for origins: configuring `https://{0}.abp.io` also accepts the base origin `https://abp.io`. CORS origins have no path component, so constrain the scheme, host suffix and port.

## Internals

### Domain Layer

#### Aggregates

##### ApiResource

API Resources are needed for allowing clients to request access tokens.

* `ApiResource` (aggregate root): Represents an API resource in the system.
  * `ApiResourceSecret` (collection): secrets of the API resource.
  * `ApiResourceScope` (collection): scope names associated with the API resource.
  * `ApiResourceClaim` (collection): claims of the API resource.

##### ApiScope

API scopes model the scopes that clients can request independently from API resources.

* `ApiScope` (aggregate root): Represents an API scope.
  * `ApiScopeClaim` (collection): Claims included for the scope.
  * `ApiScopeProperty` (collection): Custom properties of the scope.

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

##### DeviceFlowCodes

* `DeviceFlowCodes` (aggregate root): Stores the user and device codes and serialized data used by the device authorization flow until they expire.

##### IdentityResource

Identity resources are data like user ID, name, or email address of a user.

* `IdentityResource` (aggregate root): Represents an Identity Server identity resource.
  * `IdentityResourceClaim` (collection): Claims of the identity resource.

#### Repositories

Following custom repositories are defined for this module:

* `IApiResourceRepository`
* `IApiScopeRepository`
* `IClientRepository`
* `IDeviceFlowCodesRepository`
* `IPersistentGrantRepository`
* `IIdentityResourceRepository`

#### Domain Services

The module integrates the following IdentityServer services:

* `AbpProfileService` (Used when `AbpIdentityServerBuilderOptions.IntegrateToAspNetIdentity` is true)
* `AbpClaimsService`
* `AbpCorsPolicyService`

### Settings

This module doesn't define any settings.

### Application Layer

The open-source module doesn't provide application services or HTTP APIs for administration. The [Identity Server Pro module](identity-server-pro.md) provides the management application and user interfaces.

### Database Providers

#### Common

##### Table/Collection Prefix & Schema

All tables/collections use the `IdentityServer` prefix by default. Set static properties on the `AbpIdentityServerDbProperties` class if you need to change the table prefix or set a schema name (if supported by your database provider).

##### Connection String

This module uses `AbpIdentityServer` for the connection string name. If you don't define a connection string with this name, it fallbacks to the `Default` connection string.

See the [connection strings](../framework/fundamentals/connection-strings.md) documentation for details.

IdentityServer configuration is host data. The built-in EF Core and MongoDB contexts ignore the current tenant, and `ConfigureIdentityServer()` skips the model when an EF Core database is configured as tenant-only. Keep the IdentityServer tables or collections in the host/shared database when tenants use separate databases.

The EF Core and MongoDB provider modules replace IdentityServer's in-memory stores with database-backed stores. If no persistence provider registers a client store, resource store, persisted-grant store or device-flow store, the domain module falls back to values from the `IdentityServer:Clients`, `IdentityServer:ApiResources` and `IdentityServer:IdentityResources` configuration sections and to in-memory grant/device stores. Do not rely on these fallback stores for production persistence.

#### Entity Framework Core

##### Tables

* **IdentityServerApiResources**
  * IdentityServerApiResourceSecrets
  * IdentityServerApiResourceScopes
  * IdentityServerApiResourceClaims
  * IdentityServerApiResourceProperties
* **IdentityServerApiScopes**
  * IdentityServerApiScopeClaims
  * IdentityServerApiScopeProperties
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
* **IdentityServerDeviceFlowCodes**
* **IdentityServerIdentityResources**
  * IdentityServerIdentityResourceClaims
  * IdentityServerIdentityResourceProperties

#### MongoDB

##### Collections

* **IdentityServerApiResources**
* **IdentityServerApiScopes**
* **IdentityServerClients**
* **IdentityServerPersistedGrants**
* **IdentityServerDeviceFlowCodes**
* **IdentityServerIdentityResources**

## Relations to Permission Management

The optional `Volo.Abp.PermissionManagement.Domain.IdentityServer` integration lets applications grant permissions to a client by using the `GetForClientAsync`, `GetAllForClientAsync` and `SetForClientAsync` extensions on `IPermissionManager` and `IResourcePermissionManager`. Client permission values are stored on the host side. When a client is deleted, the integration removes both its ordinary and resource permission grants through the client's distributed deletion event.

## Entity Extensions

The module's [module entity extension](../framework/architecture/modularity/extending/module-entity-extensions.md) API supports the `Client`, `ApiResource` and `IdentityResource` aggregate roots. The configuration API also exposes a method for `ApiScope`, but the module does not apply that configuration to the entity. Configure these extensions before application startup, and create an EF Core migration when an extra property is mapped to a database column.
