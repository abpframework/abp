# Migrating from Open Source Templates

````json
//[doc-params]
{
    "DB": ["EF", "Mongo"]
}
````

This guide provides you a step-by-step guidance to migrating your existing application (that uses the ABP) to ABP. Since ABP uses the main structure of the ABP and is built on top of that, this process is pretty straightforward, you can apply the steps mentioned in each step and easily migrate your project to ABP.

> After following this documentation, you should be able to migrate your project to ABP. However, if you have any problems or cannot migrate your project, we are providing paid consultancy, which you can find details at [https://abp.io/additional-services](https://abp.io/additional-services). On this page, you can find related pieces of information about our trainings, custom project development, and porting existing projects services, and you can fill-out the contact form, so we can reach out to you.

## ABP Migration Steps

In this guide, we assume that you have a middle-complex ABP based solution and want to migrate to ABP. Throughout this documentation, `Acme.BookStore` application will be used as a reference solution (example application that is described in ABP's tutorial documents){{if DB == "EF"}}, which you can find at [https://github.com/abpframework/abp-samples/tree/master/BookStore-Mvc-EfCore](https://github.com/abpframework/abp-samples/tree/master/BookStore-Mvc-EfCore){{end}} but all of these steps are applicable for your own applications, only some of them can be changed according to your project choose and structure. However, the migration flow is the same.

There are 4 main steps to migrating from ABP to ABP, and each one of them is explained in the following sections, step-by-step and project-based:

### 1. License Transition

The first step is to obtain the necessary license for ABP to be able to get the benefit of the pro modules and unlock the additional features. To do that, you should first get your `ApiKey` from the [organization's detail page](https://abp.io/my-organizations).

You can update the **NuGet.Config** file in the root directory of your solution and add the *packageSource* as follows (don't forget to replace `<api-key>` placeholder): 

```diff
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
+   <add key="ABP NuGet Source" value="https://nuget.abp.io/<api-key>/v3/index.json" />
  </packageSources>
</configuration>
```

After that, you can obtain an `AbpLicenseCode` by creating a startup template and copying the code from the `appsettings.secrets.json` file. Then, you can open the `appsettings.secrets.json` files under the `*.DbMigrator` and `*.Domain` projects and add your `AbpLicenseCode`:

```json
{
    //...
    
    "AbpLicenseCode": "<AbpLicenseCode>"
}

```

> `ApiKey` is needed to be able to use ABP's NuGet packages and `AbpLicenseCode` is needed for license checks per module.

### 2. Installing the ABP Modules

After, you have added the `ApiKey` and `AbpLicenseCode` to the relevant places, now you can add [ABP's modules](../modules) to your solution. ABP provides plenty of modules that extend the ABP modules, such as the `Account Pro` module over the `Account` module or the `Identity Pro` module over the `Identity` module. 

To replace these modules and also add the additional modules provided by ABP, you can use the `abp add-module` command (and then remove the free modules as described in the next section). This command finds all packages of the specified module, finds the related projects in the solution, and adds each package to the corresponding project in the solution. Therefore, by using this command, you don't need to manually add the package references to the `*.csproj` files and add related `[DependsOn(typeof(<>))]` statements to the module classes, instead, this command does this on behalf of you.

You can run the following commands one after another in your solution directory and add all the related modules into your solution as you would have started with [one of the startup templates of ABP](../solution-templates):

1. `abp add-module Volo.Identity.Pro --skip-db-migrations` → [Identity Module](../modules/identity.md)
2. `abp add-module Volo.OpenIddict.Pro --skip-db-migrations` → [OpenIddict Module](../modules/openiddict.md)
3. `abp add-module Volo.Saas --skip-db-migrations` → [SaaS Module](../modules/saas.md)
4. `abp add-module Volo.AuditLogging.Ui --skip-db-migrations` → [Audit Logging UI Module](../modules/audit-logging.md)
5. `abp add-module Volo.Account.Pro --skip-db-migrations` → [Account Module](../modules/account.md)
6. `abp add-module Volo.TextTemplateManagement --skip-db-migrations` → [Text Template Management Module](../modules/text-template-management.md)
7. `abp add-module Volo.LanguageManagement --skip-db-migrations` → [Language Management Module](../modules/language-management.md)
8. `abp add-module Volo.Gdpr --skip-db-migrations` → [GDPR Module](../modules/gdpr.md)
9. `abp add-module Volo.Abp.BlobStoring.Database --skip-db-migrations` → [Blob Storing - Database Provider](../framework/infrastructure/blob-storing/database.md)

> These 9 modules are pre-installed on the [startup templates of ABP](../solution-templates). Therefore, you can install all of them if you want to align your project with the startup templates, but it's totally optional, so you can skip running the command above for a module that you don't want to add to your solution.

After running the commands above, all of the related commercial packages and their dependencies will be added to your solution. In addition to these module packages, you can add `Volo.Abp.Commercial.SuiteTemplates` package into your domain application to be able to use ABP Suite later on. By doing that you will be able to add your solution from [ABP Suite UI](../suite) and generate CRUD pages for your applications whenever you want. 

So, open your `*Domain.csproj` file and add the line below (don't forget to replace the `<Version>` placeholder):

```xml
<PackageReference Include="Volo.Abp.Commercial.SuiteTemplates" Version="<Version>" />
```

Then, for the final step, you need to add the related `DependsOn` statement to the `*DomainModule.cs` file as follows:

```cs
using Volo.Abp.Commercial.SuiteTemplates;

// ...

[DependsOn(typeof(VoloAbpCommercialSuiteTemplatesModule))]
public class BookStoreDomainModule : AbpModule
{
    //omited for code abbreviation...
}
```

### 3. Removing the ABP Module References & Updating Configurations

After the license transition and installing the ABP Modules, now you can remove the unnecessary free modules. For example, now you don't need the `Identity` module in your solution, because you have added the `Identity PRO` module in the previous section and it already has dependency on the free module and extends it.

You should remove various dependencies and references in different projects in your solution. All of the required changes are listed below in different sections, please apply the following steps to remove the unnecessary ABP Modules:

#### 3.1 - Domain.Shared Project

Remove the unnecessary references from the `*Domain.Shared.csproj`:

```diff
-    <PackageReference Include="Volo.Abp.Identity.Domain.Shared" Version="8.0.4" />
-    <PackageReference Include="Volo.Abp.TenantManagement.Domain.Shared" Version="8.0.4" />
-    <PackageReference Include="Volo.Abp.OpenIddict.Domain.Shared" Version="8.0.4" />  
```

Remove the unnecessary namespaces, and **DependsOn** statements from `*DomainSharedModule.cs`:

```diff
- using Volo.Abp.TenantManagement;

-    typeof(AbpIdentityDomainSharedModule),
-    typeof(AbpOpenIddictDomainSharedModule),
-    typeof(AbpTenantManagementDomainSharedModule)  
```

#### 3.2 - Domain Project

Remove the unnecessary references from the `*Domain.csproj`:

```diff
-    <PackageReference Include="Volo.Abp.Identity.Domain" Version="8.0.4" />
-    <PackageReference Include="Volo.Abp.TenantManagement.Domain" Version="8.0.4" />
-    <PackageReference Include="Volo.Abp.OpenIddict.Domain" Version="8.0.4" />  
```

Remove the unnecessary namespaces, and **DependsOn** statements from `*DomainModule.cs`:

```diff
- using Volo.Abp.TenantManagement;

-    typeof(AbpIdentityDomainModule),
-    typeof(AbpOpenIddictDomainModule),
-    typeof(AbpTenantManagementDomainModule),
```

After removing the unnecessary references, we should update the namespaces in the `BookStoreDbMigrationService` class under the **Data** folder:

```diff
- using Volo.Abp.TenantManagement;
+ using Volo.Saas.Tenants;
```

{{ if DB == "EF" }}

#### 3.3 - EntityFrameworkCore Project

Remove the unnecessary references from the  `*EntityFrameworkCore.csproj`:

```diff
-    <PackageReference Include="Volo.Abp.Identity.EntityFrameworkCore" Version="8.0.4" />
-    <PackageReference Include="Volo.Abp.TenantManagement.EntityFrameworkCore" Version="8.0.4" />
-    <PackageReference Include="Volo.Abp.OpenIddict.EntityFrameworkCore" Version="8.0.4" />  
```

Remove the unnecessary namespaces from `*EntityFrameworkCoreModule.cs`:

```diff
- using Volo.Abp.TenantManagement.EntityFrameworkCore;

-    typeof(AbpIdentityEntityFrameworkCoreModule),
-    typeof(AbpOpenIddictEntityFrameworkCoreModule),
-    typeof(AbpTenantManagementEntityFrameworkCoreModule)  
```

Then, update the`*DbContext.cs` and make the related configurations:

```diff
- using Volo.Abp.TenantManagement;
- using Volo.Abp.TenantManagement.EntityFrameworkCore;
+ using Volo.Saas.Editions;
+ using Volo.Saas.EntityFrameworkCore;
+ using Volo.Saas.Tenants;
+ using Volo.Abp.LanguageManagement.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
- [ReplaceDbContext(typeof(ITenantManagementDbContext))]
+ [ReplaceDbContext(typeof(ISaasDbContext))]
[ConnectionStringName("Default")]
public class BookStoreDbContext :
    AbpDbContext<BookStoreDbContext>,
    IIdentityDbContext,
-   ITenantManagementDbContext
+   ISaasDbContext
{
    //...

-    // Tenant Management
-    public DbSet<Tenant> Tenants { get; set; }
-    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

+    // SaaS
+    public DbSet<Tenant> Tenants { get; set; }
+    public DbSet<Edition> Editions { get; set; }
+    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    //...

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //...

-        builder.ConfigureIdentity();
+        builder.ConfigureIdentityPro();
-        builder.ConfigureOpenIddict();
+        builder.ConfigureOpenIddictPro();
-        builder.ConfigureTenantManagement();
+        builder.ConfigureSaas();

+        builder.ConfigureLanguageManagement();

    }
}
```

{{ else }}

#### 3.3 - MongoDB Project

Remove the unnecessary references from the  `*MongoDb.csproj`:

```diff
-    <PackageReference Include="Volo.Abp.Identity.MongoDb" Version="8.0.4" />
-    <PackageReference Include="Volo.Abp.TenantManagement.MongoDb" Version="8.0.4" />
-    <PackageReference Include="Volo.Abp.OpenIddict.MongoDb" Version="8.0.4" />  
```

Remove the unnecessary namespaces from `*MongoDbModule.cs`:

```diff
- using Volo.Abp.TenantManagement.MongoDb;

-    typeof(AbpIdentityMongoDbModule),
-    typeof(AbpOpenIddictMongoDbModule),
-    typeof(AbpTenantManagementMongoDbModule)  
```

{{ end }}

#### 3.4 - Application.Contracts Project

Remove the unnecessary references from the `*Application.Contracts.csproj`:

```diff
-    <PackageReference Include="Volo.Abp.Account.Application.Contracts" Version="8.0.4" />
-    <PackageReference Include="Volo.Abp.Identity.Application.Contracts" Version="8.0.4" />
-    <PackageReference Include="Volo.Abp.TenantManagement.Application.Contracts" Version="8.0.4" />
```

Remove the unnecessary namespaces, and **DependsOn** statements from `*ApplicationContractsModule.cs`:

```diff
- using Volo.Abp.TenantManagement;

-    typeof(AbpAccountApplicationContractsModule),
-    typeof(AbpTenantManagementApplicationContractsModule),
```

#### 3.5 - Application Project

Remove the unnecessary references from the `*Application.csproj`:

```diff
-    <PackageReference Include="Volo.Abp.Account.Application" Version="8.0.4" />
-    <PackageReference Include="Volo.Abp.Identity.Application" Version="8.0.4" />
-    <PackageReference Include="Volo.Abp.TenantManagement.Application" Version="8.0.4" />
```

Remove the unnecessary namespaces, and **DependsOn** statements from `*ApplicationModule.cs`:

```diff
- using Volo.Abp.TenantManagement;

-    typeof(AbpAccountApplicationModule),
-    typeof(AbpTenantManagementApplicationModule),
```

#### 3.6 - HttpApi Project

Remove the unnecessary references from the `*HttpApi.csproj`:

```diff
-    <PackageReference Include="Volo.Abp.Account.HttpApi" Version="8.0.4" />
-    <PackageReference Include="Volo.Abp.Identity.HttpApi" Version="8.0.4" />
-    <PackageReference Include="Volo.Abp.TenantManagement.HttpApi" Version="8.0.4" />
```

Remove the unnecessary namespaces, and **DependsOn** statements from `*HttpApiModule.cs`:

```diff
- using Volo.Abp.TenantManagement;

-    typeof(AbpAccountHttpApiModule),
-    typeof(AbpTenantManagementHttpApiModule),
```

#### 3.7 - HttpApi.Client Project

Remove the unnecessary references from the `*HttpApi.Client.csproj`:

```diff
-    <PackageReference Include="Volo.Abp.Account.HttpApi.Client" Version="8.0.4" />
-    <PackageReference Include="Volo.Abp.Identity.HttpApi.Client" Version="8.0.4" />
-    <PackageReference Include="Volo.Abp.TenantManagement.HttpApi.Client" Version="8.0.4" />
```

Remove the unnecessary namespaces, and **DependsOn** statements from `*HttpApiClientModule.cs`:

```diff
- using Volo.Abp.TenantManagement;

-    typeof(AbpAccountHttpApiClientModule),
-    typeof(AbpTenantManagementHttpApiClientModule),
```

#### 3.8 - Web Project

Remove the unnecessary references from the `*Web.csproj`:

```diff
-    <PackageReference Include="Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite" Version="3.0.*-*" />
+    <PackageReference Include="Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX" Version="3.0.*-*" />

-    <PackageReference Include="Volo.Abp.Identity.Web" Version="8.0.4" />
-    <PackageReference Include="Volo.Abp.TenantManagement.Web" Version="8.0.4" />

-    <PackageReference Include="Volo.Abp.Account.Web.OpenIddict" Version="8.0.4" />
+    <PackageReference Include="Volo.Abp.Account.Pro.Public.Web.OpenIddict" Version="8.0.4" />

```

> Notice, that you have also changed the LeptonXLite theme reference with the [LeptonX Theme](../ui-themes/lepton-x), which is a commercial theme provided by ABP and has superior features to the LeptonX Lite theme.

Update namespaces for the `*WebModule.cs`:

```diff
- using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
- using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
+ using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX;
+ using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Bundling;

- using Volo.Abp.TenantManagement.Web;
+ using Volo.Abp.Gdpr.Web.Extensions;
+ using Volo.Abp.LeptonX.Shared;
+ using Volo.Abp.PermissionManagement;
```

Then, we can update the configurations and add missing middlewares to the request pipeline in the same file, as follows:

```diff
-    typeof(AbpAccountWebOpenIddictModule),
+    typeof(AbpAccountPublicWebOpenIddictModule),
-    typeof(AbpTenantManagementWebModule),
-    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
+    typeof(AbpAspNetCoreMvcUiLeptonXThemeModule),

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //other configurations...
        
+        context.Services.AddAbpCookieConsent(options =>
+        {
+            options.IsEnabled = true;
+            options.CookiePolicyUrl = "/CookiePolicy";
+            options.PrivacyPolicyUrl = "/PrivacyPolicy";
+        });

+        Configure<LeptonXThemeOptions>(options =>
+        {
+            options.DefaultStyle = LeptonXStyleNames.System;
+        });

+        Configure<LeptonXThemeMvcOptions>(options =>
+        {
+            options.ApplicationLayout = LeptonXMvcLayouts.SideMenu;
+        });

+        Configure<PermissionManagementOptions>(options =>
+        {
+            options.IsDynamicPermissionStoreEnabled = true;
+        });
    }

    //...

        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
-               LeptonXLiteThemeBundles.Styles.Global,
+               LeptonXThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
        });

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        //...

+       app.UseAbpCookieConsent();
        app.UseCorrelationId();
+       app.UseAbpSecurityHeaders();
        app.UseStaticFiles();

        //...
    }    
```

> **Note:** In the startup templates of ABP, besides these configurations, there are some additional configurations, such as [configuring impersonation](../modules/account/impersonation.md), [configuring external providers](https://docs.abp.io/en/abp/latest/Modules/Account#configure-the-provider), and configuring health checks. These configurations are optional, and for the sake of simplicity, in this documentation, we did not mention them. You can apply the related configurations by checking the related documentation and from the default startup templates.

Update the namespaces in the `BookStoreMenuContributor` file as follows:

```diff
- using Volo.Abp.TenantManagement.Web.Navigation;
+ using Volo.Abp.TextTemplateManagement.Web.Navigation;
+ using Volo.Abp.AuditLogging.Web.Navigation;
+ using Volo.Abp.LanguageManagement.Navigation;
+ using Volo.Abp.OpenIddict.Pro.Web.Menus;
```

Then, we can update the `ConfigureMainMenuAsync` method in this file to specify the order of the menu items:

```csharp
private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
{
    //other configurations for menu items...

    //Administration
    var administration = context.Menu.GetAdministration();
    administration.Order = 5;

    //Administration->Identity
    administration.SetSubItemOrder(IdentityMenuNames.GroupName, 1);

    //Administration->OpenIddict
    administration.SetSubItemOrder(OpenIddictProMenus.GroupName, 2);

    //Administration->Language Management
    administration.SetSubItemOrder(LanguageManagementMenuNames.GroupName, 3);

    //Administration->Text Template Management
    administration.SetSubItemOrder(TextTemplateManagementMainMenuNames.GroupName, 4);

    //Administration->Audit Logs
    administration.SetSubItemOrder(AbpAuditLoggingMainMenuNames.GroupName, 5);

    //Administration->Settings
    administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 6);

    return Task.CompletedTask;
}
```

Replace LeptonX Lite npm package with LeptonX package in `package.json` file:
```diff
-    "@abp/aspnetcore.mvc.ui.theme.leptonxlite": "~3.0.3",
+    "@volo/abp.aspnetcore.mvc.ui.theme.leptonx": "~3.0.3",
```

### 4. Creating Migrations & Running Application

That's it, you have applied the all related steps to migrate your application from ABP to ABP. Now, you can create a new migration, apply it to your database, and run your application!

To create a new migration, open a terminal in your {{ if DB == "EF" }}`*.EntityFrameworkCore`{{else}}`*.MongoDb`{{end}} project directory, and run the following command:

```bash
dotnet ef migrations add Migrated_To_ABP_Commercial
```

Then, to apply the database into your database and seed the initial data, you can run the `*.DbMigrator` project. After it's completed, you can run the `*.Web` project to see your application as working.

> **Note:** If you have an existing database, then creating a new migration and applying it to the database may not happen correctly. At that point, if it's possible you can drop the existing database and create a new one, or you can have a backup of your existing db, and after applying the new migration, you can synchronize the database with the backup.

## Consultancy

If you find the migration process challenging or prefer professional assistance, we offer a [paid consultancy service](https://abp.io/additional-services). Our experienced consultants can help ensure a smooth transition to ABP, addressing any specific needs or challenges your project may encounter. For detailed guidance and support, feel free to [reach out](https://abp.io/contact).
