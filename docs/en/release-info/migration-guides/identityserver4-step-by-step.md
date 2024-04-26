# Migrating from OpenIddict to IdentityServer4 Step by Step Guide

ABP startup templates use `OpenIddict` OpenID provider from v6.0.0 by default and `IdentityServer` projects are renamed to `AuthServer` in tiered/separated solutions. Since OpenIddict is the default OpenID provider library for ABP templates since v6.0, you may want to keep using [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) library, even it is **archived and no longer maintained by the owners**. ABP doesn't provide support for newer versions of IdentityServer. This guide provides layer-by-layer guidance for migrating your existing [OpenIddict](https://github.com/openiddict/openiddict-core) application to IdentityServer4. 

## IdentityServer4 Migration Steps

Use the `abp update` command to update your existing application. See [Upgrading docs](../upgrading.md) for more info. Apply required migrations by following the [Migration Guides](../migration-guides) based on your application version.

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

- Create a folder named *IdentityServer* under the Domain project and copy the [IdentityServerDataSeedContributor.cs](https://github.com/abpframework/abp-samples/blob/master/Ids2OpenId/src/Ids2OpenId.Domain/IdentityServer/IdentityServerDataSeedContributor.cs) under this folder. **Rename** all the `OpenId2Ids` with your project name.
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
  using Volo.Abp.OpenIddict.EntityFrameworkCore;
  ...
  protected override void OnModelCreating(ModelBuilder builder)
  {
      base.OnModelCreating(builder);
  
      /* Include modules to your migration db context */
  
      ...
      builder.ConfigureIdentityServer();
  ```

> Not: You need to create new migration after updating the fluent api. Navigate to *EntityFrameworkCore* folder and add a new migration. Ex, `dotnet ef migrations add Updated_To_IdentityServer `

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

- In `appsettings.json` **replace OpenIddict section with IdentityServer** since IdentityServerDataSeeder will be using these information for initial data seeding:

  ```json
  "IdentityServer": { // Rename OpenIddict to IdentityServer
      "Clients ": {	// Rename Applications to Clients
        ...
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

You can follow the migrations guides from IdentityServer to OpenIddict in **reverse order** to update your UIs. You can also check the source-code for [Index.cshtml.cs](https://github.com/abpframework/abp-samples/blob/master/OpenId2Ids/src/OpenId2Ids.AuthServer/Pages/Index.cshtml) and  [Index.cshtml](https://github.com/abpframework/abp-samples/blob/master/OpenId2Ids/src/OpenId2Ids.AuthServer/Pages/Index.cshtml.cs) files for **AuthServer** project.

- [Angular UI Migration](openiddict-angular.md)
- [MVC/Razor UI Migration](openiddict-mvc.md)
- [Blazor-Server UI Migration](openiddict-blazor-server.md)
- [Blazor-Wasm UI Migration](openiddict-blazor.md)

## Source code of samples and module

* [Open source tiered & separate auth server application migrate OpenIddict to Identity Server](https://github.com/abpframework/abp-samples/tree/master/OpenId2Ids)
* [IdentityServer module document](https://docs.abp.io/en/abp/6.0/Modules/IdentityServer)
* [IdentityServer module source code](https://github.com/abpframework/abp/tree/rel-6.0/modules/identityserver)
