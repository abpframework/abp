# Migrating from IdentityServer to OpenIddict Step by Step Guide

This guide provides layer-by-layer guidance for migrating your existing application to OpenIddict. Since OpenIddict is only available with ABP v6.0, you will need to update your existing application in order to apply OpenIddict changes.

Use the `abp update` command to update your existing application. See [Upgrading docs](../Upgrading.md) for more info. Apply required migrations by following the [Migration Guides](Index.md) based on your application version.

## Domain.Shared Layer

- In **MyApplication.Domain.Shared.csproj** replace **project reference**:
  ```csharp
  <PackageReference Include="Volo.Abp.IdentityServer.Domain.Shared" Version="6.0.0-rc.1" />
  ```
  with   
  ```csharp
  <PackageReference Include="Volo.Abp.OpenIddict.Domain.Shared" Version="6.0.0-rc.1" />
  ```

- In **MyApplicationDomainSharedModule.cs** replace usings and **module dependencies:**

  ```csharp
  using Volo.Abp.IdentityServer;
  ...
  typeof(AbpIdentityServerDomainSharedModule)
  ```
  with 
  ```csharp
  using Volo.Abp.OpenIddict;
  ...
  typeof(AbpOpenIddictDomainSharedModule)

## Domain Layer

- In **MyApplication.Domain.csproj** replace **project references**:

  ```csharp
  <PackageReference Include="Volo.Abp.IdentityServer.Domain" Version="6.0.0-rc.1" />
  <PackageReference Include="Volo.Abp.PermissionManagement.Domain.IdentityServer" Version="6.0.0-rc.1" />
  ```

  with   

  ```csharp
  <PackageReference Include="Volo.Abp.OpenIddict.Domain" Version="6.0.0-rc.1" />
  <PackageReference Include="Volo.Abp.PermissionManagement.Domain.OpenIddict" Version="6.0.0-rc.1" />
  ```

- In **MyApplicationDomainModule.cs** replace usings and **module dependencies**:

  ```csharp
  using Volo.Abp.IdentityServer;
  using Volo.Abp.PermissionManagement.IdentityServer;
  ...
  typeof(AbpIdentityServerDomainModule),
  typeof(AbpPermissionManagementDomainIdentityServerModule),
  ```

  with 

  ```csharp
  using Volo.Abp.OpenIddict;
  using Volo.Abp.PermissionManagement.OpenIddict;
  ...
  typeof(AbpOpenIddictDomainModule),
  typeof(AbpPermissionManagementDomainOpenIddictModule),
  ```

### OpenIddictDataSeedContributor

- Create a folder named *OpenIddict* under the Domain project and copy the [OpenIddictDataSeedContributor.cs](https://github.com/abpframework/abp-samples/blob/master/Ids2OpenId/src/Ids2OpenId.Domain/OpenIddict/OpenIddictDataSeedContributor.cs) under this folder. Rename all the `Ids2OpenId` with your project name.
- Delete *IdentityServer* folder that contains `IdentityServerDataSeedContributor.cs` which is no longer needed.

## EntityFrameworkCore Layer

If you are using MongoDB, skip this step and check the *MongoDB* layer section.

- In **MyApplication.EntityFrameworkCore.csproj** replace **project reference**:

  ```csharp
  <PackageReference Include="Volo.Abp.IdentityServer.EntityFrameworkCore" Version="6.0.0-rc.1" />
  ```

  with   

  ```csharp
  <PackageReference Include="Volo.Abp.OpenIddict.EntityFrameworkCore" Version="6.0.0-rc.1" />
  ```

- In **MyApplicationEntityFrameworkCoreModule.cs** replace usings and **module dependencies**:

  ```csharp
  using Volo.Abp.IdentityServer.EntityFrameworkCore;
  ...
  typeof(AbpIdentityServerEntityFrameworkCoreModule),
  ```

  with 

  ```csharp
  using Volo.Abp.OpenIddict.EntityFrameworkCore;
  ...
  typeof(AbpOpenIddictEntityFrameworkCoreModule),
  ```

- In **MyApplicationDbContext.cs** replace usings and **fluent api configurations**:

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

  with 

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

## MongoDB Layer

If you are using EntityFrameworkCore, skip this step and check the *EntityFrameworkCore* layer section.

- In **MyApplication.MongoDB.csproj** replace **project reference**:

  ```csharp
  <PackageReference Include="Volo.Abp.IdentityServer.MongoDB" Version="6.0.0-rc.1" />
  ```

  with   

  ```csharp
  <PackageReference Include="Volo.Abp.OpenIddict.MongoDB" Version="6.0.0-rc.1" />
  ```

- In **MyApplicationMongoDbModule.cs** replace usings and **module dependencies**:

  ```csharp
  using Volo.Abp.IdentityServer.MongoDB;
  ...
  typeof(AbpIdentityServerMongoDbModule),
  ```

  with 

  ```csharp
  using Volo.Abp.OpenIddict.MongoDB;
  ...
  typeof(AbpOpenIddictMongoDbModule),
  ```

## DbMigrator Project

- In **MyApplication.DbMigrator.csproj** **add project reference**:

  ```csharp
  <PackageReference Include="Microsoft.Extensions.Hosting" Version="6.0.1" />
  ```

for creating the host builder.

- In `appsettings.json` **replace IdentityServer section with OpenIddict:**

  ```json
  "OpenIddict": {
      "Applications": {
        "MyApplication_Web": {
          "ClientId": "MyApplication_Web",
          "ClientSecret": "1q2w3e*",
          "RootUrl": "https://localhost:44384"
        },
        "MyApplication_App": {
          "ClientId": "MyApplication_App",
          "RootUrl": "http://localhost:4200"
        },
        "MyApplication_BlazorServerTiered": {
          "ClientId": "MyApplication_BlazorServerTiered",
          "ClientSecret": "1q2w3e*",
          "RootUrl": "https://localhost:44346"
        },
        "MyApplication_Swagger": {
          "ClientId": "MyApplication_Swagger",
          "RootUrl": "https://localhost:44391"
        }
      }
    }
  ```

  Replace **MyApplication** with your application name.

## Application Contracts Layer (Commercial only)

- In **MyApplication.Application.Contracts.csproj** replace **project reference**:

  ```csharp
  <PackageReference Include="Volo.Abp.IdentityServer.Application.Contracts" Version="6.0.0-rc.1" />
  ```

  with   

  ```csharp
  <PackageReference Include="Volo.Abp.OpenIddict.Pro.Application.Contracts" Version="6.0.0-rc.1" />
  ```

- In **MyApplicationApplicationContractsModule.cs** replace usings and **module dependencies**:

  ```csharp
  using Volo.Abp.IdentityServer;
  ...
  typeof(AbpIdentityServerApplicationContractsModule),
  ```

  with 

  ```csharp
  using Volo.Abp.OpenIddict;
  ...
  typeof(AbpOpenIddictProApplicationContractsModule),
  ```

## Application Layer (Commercial only)

- In **MyApplication.Application.csproj** replace **project reference**:

  ```csharp
  <PackageReference Include="Volo.Abp.IdentityServer.Application" Version="6.0.0-rc.1" />
  ```

  with   

  ```csharp
  <PackageReference Include="Volo.Abp.OpenIddict.Pro.Application" Version="6.0.0-rc.1" />
  ```

- In **MyApplicationApplicationModule.cs** replace usings and **module dependencies**:

  ```csharp
  using Volo.Abp.IdentityServer;
  ...
  typeof(AbpIdentityServerApplicationModule),
  ```

  with 

  ```csharp
  using Volo.Abp.OpenIddict;
  ...
  typeof(AbpOpenIddictProApplicationModule),
  ```

## HttpApi Layer (Commercial only)

- In **MyApplication.HttpApi.csproj** replace **project reference**:

  ```csharp
  <PackageReference Include="Volo.Abp.IdentityServer.HttpApi" Version="6.0.0-rc.1" />
  ```

  with   

  ```csharp
  <PackageReference Include="Volo.Abp.OpenIddict.Pro.HttpApi" Version="6.0.0-rc.1" />
  ```

- In **MyApplicationHttpApiModule.cs** replace usings and **module dependencies**:

  ```csharp
  using Volo.Abp.IdentityServer;
  ...
  typeof(AbpIdentityServerHttpApiModule),
  ```

  with 

  ```csharp
  using Volo.Abp.OpenIddict;
  ...
  typeof(AbpOpenIddictProHttpApiModule),
  ```

## HttpApi.Client Layer (Commercial only)

- In **MyApplication.HttpApi.Client.csproj** replace **project reference**:

  ```csharp
  <PackageReference Include="Volo.Abp.IdentityServer.HttpApi.Client" Version="6.0.0-rc.1" />
  ```

  with   

  ```csharp
  <PackageReference Include="Volo.Abp.OpenIddict.Pro.HttpApi.Client" Version="6.0.0-rc.1" />
  ```

- In **MyApplicationHttpApiModule.cs** replace usings and **module dependencies**:

  ```csharp
  using Volo.Abp.IdentityServer;
  ...
  typeof(AbpIdentityServerHttpApiClientModule),
  ```

  with 

  ```csharp
  using Volo.Abp.OpenIddict;
  ...
  typeof(AbpOpenIddictProHttpApiClientModule),
  ```

## UI Layer

- [Angular UI Migration](OpenIddict-Angular.md)
- [MVC/Razor UI Migration](OpenIddict-Mvc.md)
- [Blazor-Server UI Migration](OpenIddict-Blazor-Server.md)
- [Blazor-Wasm UI Migration](OpenIddict-Blazor.md)

## See Also

* [ABP Version 6.0 Migration Guide](Abp-6_0.md)