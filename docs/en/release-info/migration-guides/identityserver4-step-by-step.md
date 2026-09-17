```json
//[doc-seo]
{
    "Description": "Step-by-step guide for migrating from OpenIddict to IdentityServer4 in ABP Framework applications, ensuring a smooth transition for developers."
}
```

# Migrating from OpenIddict to IdentityServer4 Step by Step Guide

ABP startup templates use the `OpenIddict` OpenID provider from v6.0.0 by default, and `IdentityServer` projects were renamed to `AuthServer` in tiered/separated solutions. This guide provides the v6.0-era, layer-by-layer steps for migrating an existing [OpenIddict](https://github.com/openiddict/openiddict-core) application to IdentityServer4.

> IdentityServer4 is archived and no longer maintained by its owners. ABP doesn't support newer IdentityServer or Duende IdentityServer versions through this module. Use this guide only when an existing system has a compatibility requirement that prevents migration to OpenIddict; new applications should remain on OpenIddict.

## IdentityServer4 Migration Steps

These steps target an application that is already on ABP 6.0.x. Keep every `Volo.Abp.*` package on the exact 6.0.x patch version used by the application. Complete any version update separately with the CLI and migration guides for that release line before applying these provider-replacement steps.

### Domain.Shared Layer

- In **MyApplication.Domain.Shared.csproj** replace **project reference**:

```csharp
<PackageReference Include="Volo.Abp.OpenIddict.Domain.Shared" Version="6.0.*" />
```

  with   

```csharp
<PackageReference Include="Volo.Abp.IdentityServer.Domain.Shared" Version="6.0.*" />
```

- In **MyApplicationDomainSharedModule.cs** replace usings and **module dependencies:**

```csharp
using Volo.Abp.OpenIddict;
...
typeof(AbpOpenIddictDomainSharedModule)
```

  with 

```csharp
using Volo.Abp.IdentityServer;
...
typeof(AbpIdentityServerDomainSharedModule)
```

### Domain Layer

- In **MyApplication.Domain.csproj** replace **project references**:

```csharp
<PackageReference Include="Volo.Abp.OpenIddict.Domain" Version="6.0.*" />
<PackageReference Include="Volo.Abp.PermissionManagement.Domain.OpenIddict" Version="6.0.*" />
```

  with   

```csharp
<PackageReference Include="Volo.Abp.IdentityServer.Domain" Version="6.0.*" />
<PackageReference Include="Volo.Abp.PermissionManagement.Domain.IdentityServer" Version="6.0.*" />
```

- In **MyApplicationDomainModule.cs** replace usings and **module dependencies**:

```csharp
using Volo.Abp.OpenIddict;
using Volo.Abp.PermissionManagement.OpenIddict;
...
typeof(AbpOpenIddictDomainModule),
typeof(AbpPermissionManagementDomainOpenIddictModule),
```

  with 

```csharp
using Volo.Abp.IdentityServer;
using Volo.Abp.PermissionManagement.IdentityServer;
...
typeof(AbpIdentityServerDomainModule),
typeof(AbpPermissionManagementDomainIdentityServerModule),
```

#### OpenIddictDataSeedContributor

DataSeeder is the most important part for starting the application since it seeds the initial data for both OpenID providers. 

- Create a folder named *IdentityServer* under the Domain project and copy the [IdentityServerDataSeedContributor.cs](https://github.com/abpframework/abp-samples/blob/1dc297255ca22af02ef6d71092dbc1b394f9260a/Ids2OpenId/src/Ids2OpenId.Domain/IdentityServer/IdentityServerDataSeedContributor.cs) under this folder. **Rename** all the `OpenId2Ids` with your project name.
- Delete *OpenIddict* folder that contains `OpenIddictDataSeedContributor.cs` which is no longer needed.

### EntityFrameworkCore Layer

If you are using MongoDB, skip this step and check the *MongoDB* layer section.

- In **MyApplication.EntityFrameworkCore.csproj** replace **project reference**:

  ```csharp
  <PackageReference Include="Volo.Abp.OpenIddict.EntityFrameworkCore" Version="6.0.*" />
  ```

  with   

  ```csharp
  <PackageReference Include="Volo.Abp.IdentityServer.EntityFrameworkCore" Version="6.0.*" />
  ```

- In **MyApplicationEntityFrameworkCoreModule.cs** replace usings and **module dependencies**:

```csharp
using Volo.Abp.OpenIddict.EntityFrameworkCore;
...
typeof(AbpOpenIddictEntityFrameworkCoreModule),
```

  with 

```csharp
using Volo.Abp.IdentityServer.EntityFrameworkCore;
...
typeof(AbpIdentityServerEntityFrameworkCoreModule),
```

- In **MyApplicationDbContext.cs** replace usings and **fluent api configurations**:

  ```csharp
  using Volo.Abp.OpenIddict.EntityFrameworkCore;
  ...
  protected override void OnModelCreating(ModelBuilder builder)
  {
      base.OnModelCreating(builder);
  
      /* Include modules to your migration db context */
  
      ...
      builder.ConfigureOpenIddict();
  ```
  
  with 

  ```csharp
  using Volo.Abp.IdentityServer.EntityFrameworkCore;
  ...
  protected override void OnModelCreating(ModelBuilder builder)
  {
      base.OnModelCreating(builder);
  
      /* Include modules to your migration db context */
  
      ...
      builder.ConfigureIdentityServer();
  ```

> Note: Create a new migration after updating the fluent API. Navigate to the *EntityFrameworkCore* folder and run, for example, `dotnet ef migrations add Updated_To_IdentityServer`.

### MongoDB Layer

If you are using EntityFrameworkCore, skip this step and check the *EntityFrameworkCore* layer section.

- In **MyApplication.MongoDB.csproj** replace **project reference**:

  ```csharp
  <PackageReference Include="Volo.Abp.OpenIddict.MongoDB" Version="6.0.*" />
  ```

  with   

  ```csharp
  <PackageReference Include="Volo.Abp.IdentityServer.MongoDB" Version="6.0.*" />
  ```

- In **MyApplicationMongoDbModule.cs** replace usings and **module dependencies**:

```csharp
using Volo.Abp.OpenIddict.MongoDB;
...
typeof(AbpOpenIddictMongoDbModule),
```

  with 

```csharp
using Volo.Abp.IdentityServer.MongoDB;
...
typeof(AbpIdentityServerMongoDbModule),
```

### DbMigrator Project

- In `appsettings.json` **replace the OpenIddict section with IdentityServer** because IdentityServerDataSeeder uses this configuration for initial data seeding. Rename the `Applications` property to `Clients` and preserve its existing child entries. The minimal valid structure is:

  ```json
  {
    "IdentityServer": {
      "Clients": {}
    }
  }
  ```
  

### Test Project

- In **MyApplicationTestBaseModule.cs** **add** the IdentityServer related using and PreConfigurations:

  ```csharp
  using Volo.Abp.IdentityServer;
  ```

  and

  ```csharp
  PreConfigure<AbpIdentityServerBuilderOptions>(options =>
  {
      options.AddDeveloperSigningCredential = false;
  });
  
  PreConfigure<IIdentityServerBuilder>(identityServerBuilder =>
  {
      identityServerBuilder.AddDeveloperSigningCredential(false, System.Guid.NewGuid().ToString());
  });
  ```

  to `PreConfigureServices` to run authentication related unit tests.

### UI Layer

You can follow the migration guides from IdentityServer to OpenIddict in reverse order to update your UIs. You can also check the sample source for [Index.cshtml.cs](https://github.com/abpframework/abp-samples/blob/master/OpenId2Ids/src/OpenId2Ids.AuthServer/Pages/Index.cshtml.cs) and [Index.cshtml](https://github.com/abpframework/abp-samples/blob/master/OpenId2Ids/src/OpenId2Ids.AuthServer/Pages/Index.cshtml) in the **AuthServer** project.

- [Angular UI Migration](openiddict-angular.md)
- [MVC/Razor UI Migration](openiddict-mvc.md)
- [Blazor-Server UI Migration](openiddict-blazor-server.md)
- [Blazor-Wasm UI Migration](openiddict-blazor.md)

## Source code of samples and module

* [Open source tiered & separate auth server application migrate OpenIddict to Identity Server](https://github.com/abpframework/abp-samples/tree/master/OpenId2Ids)
* [IdentityServer module document](https://docs.abp.io/en/abp/6.0/Modules/IdentityServer)
* [IdentityServer module source code](https://github.com/abpframework/abp/tree/rel-6.0/modules/identityserver)
