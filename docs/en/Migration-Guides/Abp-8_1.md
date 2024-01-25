# ABP Version 8.1 Migration Guide

This document is a guide for upgrading ABP v8.0 solutions to ABP v8.1. There are some changes in this version that may affect your applications, please read it carefully and apply the necessary changes to your application.

## Added `NormalizedName` property to `Tenant`

The `Tenant` entity has a new property called `NormalizedName`. It is used to find/cache a tenant by its name in a case-insensitive way.
This property is automatically set when a tenant is created or updated. It gets the normalized name of the tenant name by `UpperInvariantTenantNormalizer(ITenantNormalizer)` service. You can implement this service to change the normalization logic.

### `ITenantStore` 

The `ITenantStore` will use the `NormalizedName` parameter to get tenants, Please use the `ITenantNormalizer` to normalize the tenant name before calling the `ITenantStore` methods.

### Update `NormalizedName` in `appsettings.json`

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

### Update `NormalizedName` in the database

Please add a sql script to your migration to set the `NormalizedName` property of the existing tenants. You can use the following script:

> This script is for the SQL Server database. You can change it for your database.

> The table name `SaasTenants` is used for ABP commercial Saas module. `AbpTenants` is for the ABP open-source Tenant Management module. 

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

