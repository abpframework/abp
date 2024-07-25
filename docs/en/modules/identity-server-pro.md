# Identity Server Module (Pro)

> You must have an ABP Team or a higher license to use this module.

This module provides integration and management functionality for Identity Server;

* Built on the [IdentityServer4 ](http://docs.identityserver.io/en/latest/) library.
* Manage **Clients**, **Identity resources** and **API resources** in the system. 
* Set **permissions** for clients.
* Create **standard identity resources** (like role, profile) easily.
* Create custom **identity resources**.
* Manage **API resources**

See [the module description page](https://abp.io/modules/Volo.identityserver.Ui) for an overview of the module features.

## How to Install

Identity Server is pre-installed in [the startup templates](../solution-templates). So, no need to manually install it.

## Packages

This module follows the [module development best practices guide](../framework/architecture/best-practices) and consists of several NuGet and NPM packages. See the guide if you want to understand the packages and relations between them.

You can visit [Identity module package list page](https://abp.io/packages?moduleName=Volo.Identity.Pro) to see list of packages related with this module.

## User Interface

### Menu Items

Identity Server module adds the following items to the "Main" menu, under the "Administration" menu item:

* **Clients**: Client management page.
* **Identity resources**: Identity resource management page.
* **API resources**: API resource management page.

`AbpIdentityServerMenuNames` class has the constants for the menu item names.

### Pages

#### Client Management

Clients page is used to manage Identity Server clients. A client represent applications that can request tokens from your Identity Server.

![identity-server-clients-page](../images/identity-server-clients-page.png)

You can create new clients or edit existing clients in this page:

![identity-server-edit-client-modal](../images/identity-server-edit-client-modal.png)

#### Identity Resource Management

Identity resource page is used to manage identity resources of Identity Server. Identity resources are data like user ID, name, or email address of a user.

![identity-server-identity-resources-page](../images/identity-server-identity-resources-page.png)

You can create a new identity resource or edit an existing identity resource in this page:

![identity-server-edit-identity-resource-modal](../images/identity-server-edit-identity-resource-modal.png)

This page allows creating standard identity resources (role, profile, phone, openid, email and address) using "Create standard resources" button.

#### API Resource Management

Identity Server module allows to manage API resources. To allow clients to request access tokens for APIs, you need to define API resources.

![identity-server-api-resources-page](../images/identity-server-api-resources-page.png)

You can create a new API resource or edit an existing API resource in this page:

![identity-server-edit-api-resource-modal](../images/identity-server-edit-api-resource-modal.png)

## Data Seed

This module adds some initial data (see [the data seed system](../framework/infrastructure/data-seeding.md)) to the database when you run the `.DbMigrator` application:

* Creates standard identity resources which are role, profile, phone, openid, email and address.

You can delete or edit created standard identity resources in the identity resource management page. You can also re-create standard identity resources in the identity resource management page using "Create standard resources" button. 

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

## Internals

### Domain Layer

#### Aggregates

This module follows the [Entity Best Practices & Conventions](../framework/architecture/best-practices/entities.md) guide.

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

This module follows the [Repository Best Practices & Conventions](../framework/architecture/best-practices/repositories.md) guide.

Following custom repositories are defined for this module:

* `IApiResourceRepository`
* `IClientRepository`
* `IPersistentGrantRepository`
* `IIdentityResourceRepository`

#### Domain Services

This module follows the [Domain Services Best Practices & Conventions](../framework/architecture/best-practices/domain-services.md) guide.

Identity Server module doesn't contain any domain service but overrides services below;

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

See the [connection strings](../framework/fundamentals/connection-strings.md) documentation for details.

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

### Permissions

See the `AbpIdentityServerPermissions` class members for all permissions defined for this module.

### Angular UI

#### Installation

In order to configure the application to use the `IdentityServerModule`, you first need to import `IdentityServerConfigModule` from `@volo/abp.ng.identity-server/config` to root module. `IdentityServerConfigModule` has a static `forRoot` method which you should call for a proper configuration.

```js
// app.module.ts
import { IdentityServerConfigModule } from '@volo/abp.ng.identity-server/config';

@NgModule({
  imports: [
    // other imports
    IdentityServerConfigModule.forRoot(),
    // other imports
  ],
  // ...
})
export class AppModule {}
```

The `IdentityServerModule` should be imported and lazy-loaded in your routing module. It has a static `forLazy` method for configuration. Available options are listed below. It is available for import from `@volo/abp.ng.identity-server`.

```js
// app-routing.module.ts
const routes: Routes = [
  // other route definitions
  {
    path: 'identity-server',
    loadChildren: () =>
      import('@volo/abp.ng.identity-server').then(m => m.IdentityServerModule.forLazy(/* options here */)),
  },
];

@NgModule(/* AppRoutingModule metadata */)
export class AppRoutingModule {}
```

> If you have generated your project via the startup template, you do not have to do anything, because it already has both `IdentityServerConfigModule` and `IdentityServerModule`.

<h4 id="h-identity-server-module-options">Options</h4>

You can modify the look and behavior of the module pages by passing the following options to `IdentityServerModule.forLazy` static method:

- **entityActionContributors:** Changes grid actions. Please check [Entity Action Extensions for Angular](../framework/ui/angular/entity-action-extensions.md) for details.
- **toolbarActionContributors:** Changes page toolbar. Please check [Page Toolbar Extensions for Angular](../framework/ui/angular/page-toolbar-extensions.md) for details.
- **entityPropContributors:** Changes table columns. Please check [Data Table Column Extensions for Angular](../framework/ui/angular/data-table-column-extensions.md) for details.
- **createFormPropContributors:** Changes create form fields. Please check [Dynamic Form Extensions for Angular](../framework/ui/angular/dynamic-form-extensions.md) for details.
- **editFormPropContributors:** Changes create form fields. Please check [Dynamic Form Extensions for Angular](../framework/ui/angular/dynamic-form-extensions.md) for details.


#### Services / Models

Identity Server module services and models are generated via `generate-proxy` command of the [ABP CLI](../cli). If you need the module's proxies, you can run the following command in the Angular project directory:

```bash
abp generate-proxy --module identityServer
```

#### Replaceable Components

`eIdentityServerComponents` enum provides all replaceable component keys. It is available for import from `@volo/abp.ng.identity-server`.

Please check [Component Replacement document](../framework/ui/angular/component-replacement.md) for details.


#### Remote Endpoint URL

The Identity Server module remote endpoint URL can be configured in the environment files.

```js
export const environment = {
  // other configurations
  apis: {
    default: {
      url: 'default url here',
    },
    AbpIdentityServer: {
      url: 'Identity Server remote url here'
    }
    // other api configurations
  },
};
```

The Identity Server module remote URL configuration shown above is optional. If you don't set a URL, the `default.url` will be used as fallback.


## Distributed Events

This module defines events for `Client` aggregate and `ClientCorsOrigin` entity. When a `Client` or  `ClientCorsOrigin` changes, `AllowedCorsOriginsCacheItemInvalidator` invalidates the cache for `AllowedCorsOriginsCacheItem`. See the [standard distributed events](../framework/infrastructure/event-bus/distributed) for more information about distributed events.
