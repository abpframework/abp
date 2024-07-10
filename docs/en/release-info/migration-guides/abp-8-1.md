# ABP Version 8.1 Migration Guide

This document is a guide for upgrading ABP v8.0 solutions to ABP v8.1. There are some changes in this version that may affect your applications, please read it carefully and apply the necessary changes to your application.

## Open-Source (Framework)

If you are using one of the open-source startup templates, then you can check the following sections to apply the related breaking changes:

### Added `NormalizedName` property to `Tenant`

The `Tenant` entity has a new property called `NormalizedName`. It is used to find/cache a tenant by its name in a case-insensitive way.
This property is automatically set when a tenant is created or updated. It gets the normalized name of the tenant name by `UpperInvariantTenantNormalizer(ITenantNormalizer)` service. You can implement this service to change the normalization logic.

#### `ITenantStore` 

The `ITenantStore` will use the `NormalizedName` parameter to get tenants, Please use the `ITenantNormalizer` to normalize the tenant name before calling the `ITenantStore` methods.

#### Update `NormalizedName` in `appsettings.json`

If your tenants defined in the `appsettings.json` file, you should add the `NormalizedName` property to your tenants.

````json
"Tenants": [
    {
      "Id": "446a5211-3d72-4339-9adc-845151f8ada0",
      "Name": "tenant1",
      "NormalizedName": "TENANT1" // <-- Add this property
    },
    {
      "Id": "25388015-ef1c-4355-9c18-f6b6ddbaf89d",
      "Name": "tenant2",
      "NormalizedName": "TENANT2", // <-- Add this property
      "ConnectionStrings": {
        "Default": "...tenant2's db connection string here..."
      }
    }
  ]
````

#### Update `NormalizedName` in the database

Please add a sql script to your migration to set the `NormalizedName` property of the existing tenants. You can use the following script:

> This script is for the SQL Server database. You can change it for your database.

> The table name `SaasTenants` is used for ABP Saas module. `AbpTenants` is for the ABP open-source Tenant Management module. 

```sql
UPDATE SaasTenants SET NormalizedName = UPPER(Name) WHERE NormalizedName IS NULL OR NormalizedName = ''
```

```csharp
/// <inheritdoc />
public partial class Add_NormalizedName : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "NormalizedName",
            table: "SaasTenants",
            type: "nvarchar(64)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.Sql("UPDATE SaasTenants SET NormalizedName = UPPER(Name) WHERE NormalizedName IS NULL OR NormalizedName = ''");

        migrationBuilder.CreateIndex(
            name: "IX_SaasTenants_NormalizedName",
            table: "SaasTenants",
            column: "NormalizedName");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_SaasTenants_NormalizedName",
            table: "SaasTenants");

        migrationBuilder.DropColumn(
            name: "NormalizedName",
            table: "SaasTenants");
    }
}
```

See https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/managing?tabs=dotnet-core-cli#adding-raw-sql to learn how to add raw SQL to migrations.

### Use `Asp.Versioning.Mvc` to replace `Microsoft.AspNetCore.Mvc.Versioning`

The Microsoft.AspNetCore.Mvc.Versioning packages are now deprecated and superseded by Asp.Versioning.Mvc.
See the announcement here: https://github.com/dotnet/aspnet-api-versioning/discussions/807

The namespace of the `[ControllerName]` attribute has changed to `using Asp.Versioning`, Please update your code to use the new namespace.

Related PR: https://github.com/abpframework/abp/pull/18380

### New asynchronous methods for `IAppUrlProvider`

The `IsRedirectAllowedUrl` method of `IAppUrlProvider` has been changed to `IsRedirectAllowedUrlAsync` and it is now an async method. 
You should update your usage of `IAppUrlProvider` to use the new method.

Related PR: https://github.com/abpframework/abp/pull/18492

### New attribute: `ExposeKeyedServiceAttribute`

The new `ExposeKeyedServiceAttribute` is used to control which keyed services are provided by the related class. Example:

````C#
[ExposeKeyedService<ITaxCalculator>("taxCalculator")]
[ExposeKeyedService<ICalculator>("calculator")]
public class TaxCalculator: ICalculator, ITaxCalculator, ICanCalculate, ITransientDependency
{
}
````

In the example above, the `TaxCalculator` class exposes the `ITaxCalculator` interface with the key `taxCalculator` and the `ICalculator` interface with the key `calculator`. That means you can get keyed services from the `IServiceProvider` as shown below:

````C#
var taxCalculator = ServiceProvider.GetRequiredKeyedService<ITaxCalculator>("taxCalculator");
var calculator = ServiceProvider.GetRequiredKeyedService<ICalculator>("calculator");
````

Also, you can use the [`FromKeyedServicesAttribute`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.fromkeyedservicesattribute?view=dotnet-plat-ext-8.0) to resolve a certain keyed service in the constructor:

```csharp
public class MyClass
{
    //...
    public MyClass([FromKeyedServices("taxCalculator")] ITaxCalculator taxCalculator)
    {
        TaxCalculator = taxCalculator;
    }
}
```

> Notice that the `ExposeKeyedServiceAttribute` only exposes the keyed services. So, you can not inject the `ITaxCalculator` or `ICalculator` interfaces in your application without using the `FromKeyedServicesAttribute` as shown in the example above. If you want to expose both keyed and non-keyed services, you can use the `ExposeServicesAttribute` and `ExposeKeyedServiceAttribute` attributes together as shown below:
````C#
[ExposeKeyedService<ITaxCalculator>("taxCalculator")]
[ExposeKeyedService<ICalculator>("calculator")]
[ExposeServices(typeof(ITaxCalculator), typeof(ICalculator))]
public class TaxCalculator: ICalculator, ITaxCalculator, ICanCalculate, ITransientDependency
{
}
````

This is a small **Breaking Change** because `IOnServiceExposingContext` has changed. You should update your usage of `IOnServiceExposingContext` if you have related code.

Related PR: https://github.com/abpframework/abp/pull/18819

## PRO

There is not a single breaking-change that affects the pro modules, nevertheless, please check the **Open-Source (Framework)** section above to ensure, there is not a change that you need to make in your application.