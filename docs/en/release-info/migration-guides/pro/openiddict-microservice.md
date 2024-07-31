# Migrating Microservice Application to OpenIddict Guide

This guide provides guidance for migrating your existing microservice application to OpenIddict. Since OpenIddict is only available with ABP v6.0, you will need to update your existing application in order to apply OpenIddict changes.

## History
We are not removing Identity Server packages and we will continue to release new versions of IdentityServer-related NuGet/NPM packages. That means you won't have an issue while upgrading to v6.0 when the stable version releases. We will continue to fix bugs in our packages for a while. ABP 7.0 will be based on .NET 7. If Identity Server continues to work with .NET 7, we will also continue to ship NuGet packages for our IDS integration.

On the other hand, Identity Server ends support for the open-source Identity Server at the end of 2022. The Identity Server team has decided to move to Duende IDS and ABP will not be migrated to the commercial Duende IDS. You can see the Duende Identity Server announcement from [this link](https://blog.duendesoftware.com/posts/20220111_fair_trade). 

## OpenIddict Migration Steps

Use the `abp update` command to update your existing application. See [Upgrading docs](../upgrading-startup-template.md) for more info. Apply required migrations by following the [Migration Guides](index.md) based on your application version.

### AuthServer Application

- In **MyApplication.AuthServer.csproj** replace **project reference**:

  ```csharp
  <PackageReference Include="Volo.Abp.Account.Pro.Public.Web.IdentityServer" Version="6.0.*" />
  <PackageReference Include="Volo.Abp.AspNetCore.Authentication.JwtBearer" Version="6.0.*" />
  ```

  with   

  ```csharp
  <PackageReference Include="Volo.Abp.Account.Pro.Public.Web.OpenIddict" Version="6.0.*" />
  ```

- In **MyApplicationAuthServerModule.cs** replace usings and **module dependencies:**

  ```csharp
  using IdentityServer4.Extensions;
  using Volo.Abp.IdentityServer;
  ...
  typeof(AbpAspNetCoreAuthenticationJwtBearerModule)
  typeof(AbpAccountPublicWebIdentityServerModule)
  ```

  with 

  ```csharp
  using Volo.Abp.OpenIddict;
  ...
  typeof(AbpAccountPublicWebOpenIddictModule),
  ```

- In **MyApplicationAuthServerModule.cs** update **PreConfigureServices** method like below:

  ```csharp
  public override void PreConfigureServices(ServiceConfigurationContext context)
      {
          var hostingEnvironment = context.Services.GetHostingEnvironment();
          var configuration = context.Services.GetConfiguration();
  
          PreConfigure<OpenIddictBuilder>(builder =>
          {
              builder.AddValidation(options =>
              {
                  options.AddAudiences("AccountService");
                  options.UseLocalServer();
                  options.UseAspNetCore();
              });
          });
  
          if (!hostingEnvironment.IsDevelopment())
          {
              PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
              {
                  options.AddDevelopmentEncryptionAndSigningCertificate = false;
              });
  
              PreConfigure<OpenIddictServerBuilder>(builder =>
              {
                  builder.AddSigningCertificate(GetSigningCertificate(hostingEnvironment, configuration));
                  builder.AddEncryptionCertificate(GetSigningCertificate(hostingEnvironment, configuration));
              });
          }
      }
  ```

- In **MyApplicationAuthServerModule.cs** remove **JwtBeaer authentication options**:

  ```csharp
  context.Services.AddAuthentication() // Remove this configuration
              .AddJwtBearer(options =>
              {
                  options.Authority = configuration["AuthServer:Authority"];
                  options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                  options.Audience = "AccountService";
              });
  ```

- In **MyApplicationAuthServerModule.cs** `OnApplicationInitialization` method **replace IdentityServer and JwtToken midware**:

    ```csharp
    app.UseJwtTokenMiddleware();
    app.UseIdentityServer();
    ```

    with 

    ```csharp
    app.UseAbpOpenIddictValidation();
    ```

- To use the new AuthServer page, replace **Index.cshtml.cs** with [AuthServer Index.cshtml.cs](https://gist.github.com/gterdem/878ea99edaf998bb9f360bbf1a674a87#file-index-cshtml-cs) and **Index.cshtml** file with [AuthServer Index.cshtml](https://gist.github.com/gterdem/878ea99edaf998bb9f360bbf1a674a87#file-index-cshtml). 

  > Note: It can be found under the *Pages* folder.

### AdministrationService Domain Layer

- In **MyApplication.AdministrationService.Domain.csproj** replace **project reference**:
  ```csharp
  <PackageReference Include="Volo.Abp.PermissionManagement.Domain.IdentityServer" Version="6.0.*" />
  ```
  with   
  ```csharp
  <PackageReference Include="Volo.Abp.PermissionManagement.Domain.OpenIddict" Version="6.0.*" />
  ```

- In **MyApplicationDomainSharedModule.cs** replace usings and **module dependencies:**

  ```csharp
  using Volo.Abp.PermissionManagement.IdentityServer;
  ...
  typeof(AbpPermissionManagementDomainIdentityServerModule)
  ```
  with 
  ```csharp
  using Volo.Abp.PermissionManagement.OpenIddict;
  ...
  typeof(AbpPermissionManagementDomainOpenIddictModule)

### IdentityService Domain Shared Layer

- In **MyApplication.IdentityService.Domain.Shared.csproj** replace **project references**:

  ```csharp
  <PackageReference Include="Volo.Abp.IdentityServer.Domain.Shared" Version="6.0.*" />
  ```

  with   

  ```csharp
  <PackageReference Include="Volo.Abp.OpenIddict.Domain.Shared" Version="6.0.*" />
  ```

- In **MyApplicationDomainModule.cs** replace usings and **module dependencies**:

  ```csharp
  using Volo.Abp.IdentityServer;
  ...
  typeof(AbpIdentityServerDomainModule)
  ```

  with 

  ```csharp
  using Volo.Abp.OpenIddict;
  ...
  typeof(AbpOpenIddictDomainModule)
  ```

### IdentityService Domain Layer

- In **MyApplication.IdentityService.Application.Contracts.csproj** replace **project references**:

  ```csharp
  <PackageReference Include="Volo.Abp.IdentityServer.Domain" Version="6.0.*" />
  ```

  with   

  ```csharp
  <PackageReference Include="Volo.Abp.OpenIddict.Domain" Version="6.0.*" />
  ```

- In **IdentityServiceDomainModule.cs** replace usings and **module dependencies**:

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
  ```

### IdentityService Application.Contracts Layer

- In **MyApplication.IdentityService.Application.Contracts.csproj** replace **project references**:

  ```csharp
  <PackageReference Include="Volo.Abp.IdentityServer.Application.Contracts" Version="6.0.*" />
  ```
  
  with   
  
  ```csharp
  <PackageReference Include="Volo.Abp.OpenIddict.Pro.Application.Contracts" Version="6.0.*" />
  ```
  
- In **IdentityServiceApplicationContractsModule.cs.cs** replace usings and **module dependencies**:

  ```csharp
  using Volo.Abp.IdentityServer;
  ...
  typeof(AbpIdentityServerApplicationContractsModule)
  ```
  
  with 

  ```csharp
  using Volo.Abp.OpenIddict;
  ...
  typeof(AbpOpenIddictProApplicationContractsModule)
  ```

### IdentityService Application Layer

- In **MyApplication.IdentityService.Application.csproj** replace **project references**:

  ```csharp
  <PackageReference Include="Volo.Abp.IdentityServer.Application" Version="6.0.*" />
  ```

  with   

  ```csharp
  <PackageReference Include="Volo.Abp.OpenIddict.Pro.Application" Version="6.0.*" />
  ```

- In **IdentityServiceApplicationModule.cs** replace usings and **module dependencies**:

  ```csharp
  using Volo.Abp.IdentityServer;
  ...
  typeof(AbpIdentityServerApplicationModule)
  ```

  with 

  ```csharp
  using Volo.Abp.OpenIddict;
  ...
  typeof(AbpOpenIddictProApplicationModule)
  ```

### IdentityService EntityFrameworkCore Layer

- In **MyApplication.IdentityService.EntityFrameworkCore.csproj** replace **project reference**:

  ```csharp
  <PackageReference Include="Volo.Abp.IdentityServer.EntityFrameworkCore" Version="6.0.*" />
  ```

  with   

  ```csharp
  <PackageReference Include="Volo.Abp.OpenIddict.EntityFrameworkCore" Version="6.0.*" />
  ```

- In **IdentityServiceEntityFrameworkCoreModule.cs.cs** replace usings and **module dependencies**:

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

- In **IdentityServiceEntityFrameworkCoreModule.cs.cs** replace `AddAbpDbContext` **options**:

  ```csharp
  options.ReplaceDbContext<IIdentityServerDbContext>();
  ```
  
  with 
  
  ```csharp
  options.ReplaceDbContext<IOpenIddictDbContext>();
  ```
  
- In **IdentityServiceDbContext.cs** replace implemented **dbcontexts**:

  ```csharp
  public class IdentityServiceDbContext : AbpDbContext<IdentityServiceDbContext>, IIdentityDbContext, IIdentityServerDbContext
  ```
  
  with 
  
  ```csharp
  public class IdentityServiceDbContext : AbpDbContext<IdentityServiceDbContext>, IIdentityDbContext, IOpenIddictDbContext
  ```
  
  Afterwards you should have the DbSets like below:
  
  ```csharp
  public class IdentityServiceDbContext : AbpDbContext<IdentityServiceDbContext>, IIdentityDbContext, IOpenIddictDbContext
  {
      public DbSet<IdentityUser> Users { get; set; }
      public DbSet<IdentityRole> Roles { get; set; }
      public DbSet<IdentityClaimType> ClaimTypes { get; set; }
      public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
      public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
      public DbSet<IdentityLinkUser> LinkUsers { get; set; }
      public DbSet<OpenIddictApplication> Applications { get; set; }
      public DbSet<OpenIddictAuthorization> Authorizations { get; set; }
      public DbSet<OpenIddictScope> Scopes { get; set; }
      public DbSet<OpenIddictToken> Tokens { get; set; }
  }
  ```
  
- In **IdentityServiceDbContext.cs** replace usings and **fluent api configurations**:

  ```csharp
  using Volo.Abp.IdentityServer.EntityFrameworkCore;
  ...
  using Volo.Abp.OpenIddict.EntityFrameworkCore;
  ...
  protected override void OnModelCreating(ModelBuilder builder)
  {
      base.OnModelCreating(builder);
  
      /* Include modules to your migration db context */
  
      builder.ConfigureIdentityPro();
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
  
      builder.ConfigureIdentityPro();
      builder.ConfigureOpenIddict();
  ```

> Don't forget to create new migration using `donet ef` command

### IdentityService HttpApi Layer

- In **MyApplication.IdentityService.HttpApi.csproj** replace **project reference**:

  ```csharp
  <PackageReference Include="Volo.Abp.IdentityServer.HttpApi" Version="6.0.*" />
  ```

  with   

  ```csharp
  <PackageReference Include="Volo.Abp.OpenIddict.Pro.HttpApi" Version="6.0.*" />
  ```

- In **IdentityServiceHttpApiModule.cs** replace usings and **module dependencies**:

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

### IdentityService HttpApi.Client Layer

- In **MyApplication.IdentityService.HttpApi.Client.csproj** replace **project reference**:

  ```csharp
  <PackageReference Include="Volo.Abp.IdentityServer.HttpApi.Client" Version="6.0.*" />
  ```

  with   

  ```csharp
  <PackageReference Include="Volo.Abp.OpenIddict.Pro.HttpApi.Client" Version="6.0.*" />
  ```

- In **IdentityServiceHttpApiClientModule.cs** replace usings and **module dependencies**:

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

### IdentityServer Data Seeder

It is recommended to update the `IdentityServerDataSeeder.cs` in the **MyApplication.IdentityService.HttpApi.Host** project under the *DbMigrations* folder.

Use the [Updated IdentityServerDataSeeder](https://gist.github.com/gterdem/6417a914722e715b9d7fb28e75eeee51#file-identityserverdataseeder-cs) to update your existing IdentityServer data seeder. Replace the `Acme.BookStore` namespace to your application namespace.

In `appsettings.json` replace **IdentityServer** section with **OpenIddict** and **Clients** to **Applications**. You should have something like:

```json
"OpenIddict": {
    "Applications": {
      "MyApplication_Web": {
        "ClientId": "MyApplication_Web",
        "ClientSecret": "1q2w3e*",
        "RootUrl": "https://localhost:44384/"
      },
        ...
    }
  }
```

> If you are using DbMigrator, you should make the same changes in `IdentityServerDataSeeder` and appsettings.json located under DbMigrator project.

> Note: Don't forget to have a trailing slash `/` at the end of the root urls.

### IdentityService Web Layer

- In **MyApplication.IdentityService.HttpApi.Client.csproj** replace **project reference**:

  ```csharp
  <PackageReference Include="Volo.Abp.IdentityServer.Web" Version="6.0.*" />
  ```

  with   

  ```csharp
  <PackageReference Include="Volo.Abp.OpenIddict.Pro.Web" Version="6.0.*" />
  ```

- In **IdentityServiceWebModule.cs** replace usings and **module dependencies**:

  ```csharp
  using Volo.Abp.IdentityServer.Web;
  ...
  typeof(AbpIdentityServerWebModule),
  ```

  with 

  ```csharp
  using Volo.Abp.OpenIddict.Pro.Web;
  ...
  typeof(AbpOpenIddictProWebModule),
  ```

- In **IdentityServiceWebModule.cs** add object mapping configurations:

  ```csharp
  context.Services.AddAutoMapperObjectMapper<IdentityServiceWebModule>();
          Configure<AbpAutoMapperOptions>(options =>
          {
              options.AddMaps<IdentityServiceWebModule>(validate: true);
          });
  ```


### Shared Hosting Module

- In **MyApplicationSharedHostingModule** replace the **database configuration**:

  ```csharp
  options.Databases.Configure("IdentityService", database =>
              {
                  database.MappedConnections.Add("AbpIdentity");
                  database.MappedConnections.Add("AbpIdentityServer");
              });
  ```

  with   

  ```csharp
  options.Databases.Configure("IdentityService", database =>
              {
                  database.MappedConnections.Add("AbpIdentity");
                  database.MappedConnections.Add("OpenIddict");
              });
  ```

### Public Web Application

In the AddAbpOpenIdConnect configuration options, update `options.Scope.Add("role");` to `options.Scope.Add("roles");`.

### Back-Office Application

Based on your UI choice, you need to update OpenIdConnect options as in the Public-Web application. Update **role** scope to **roles**. 

To update the menu, navigate to **MyApplicationMenuContributor.cs** (or NavigationContributor) and replace the menu names:
  ```csharp
  using Volo.Abp.IdentityServer.Web.Navigation;
  ...
  //Administration->Identity Server
  administration.SetSubItemOrder(AbpIdentityServerMenuNames.GroupName, 2);
  ```

  with 

  ```csharp
  using Volo.Abp.OpenIddict.Pro.Web.Menus;
  ...
  //Administration->OpenIddict
  administration.SetSubItemOrder(OpenIddictProMenus.GroupName, 2);
  ```

## See Also

* [ABP Version 6.0 Migration Guide](v6-0.md)