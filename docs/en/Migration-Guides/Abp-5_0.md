# ABP Framework v4.x to v5.0 Migration Guide

This document is a guide for upgrading ABP 4.x solutions to ABP 5.0. Please read them all since 5.0 has some important breaking changes.

## .NET 6.0

ABP 5.0 runs on .NET 6.0. So, please upgrade your solution to .NET 6.0 if you want to use ABP 5.0. You can see [Microsoft's migration guide](https://docs.microsoft.com/en-us/aspnet/core/migration/50-to-60).

## Bootstrap 5

ABP 5.0 uses the Bootstrap 5 as the fundamental HTML/CSS framework. We've migrated all the UI themes, tag helpers, UI components and the pages of the pre-built application modules. You may need to update your own pages by following the [Bootstrap's migration guide](https://getbootstrap.com/docs/5.0/migration/).

## ABP Framework

This section contains breaking changes in the ABP Framework.

### MongoDB

ABP Framework will serialize the datetime based on [AbpClockOptions](https://docs.abp.io/en/abp/latest/Timing#clock-options) starting from ABP v5.0. It was saving `DateTime` values as UTC in MongoDB. Check out [MongoDB Datetime Serialization Options](https://mongodb.github.io/mongo-csharp-driver/2.13/reference/bson/mapping/#datetime-serialization-options).

If you want to revert back this feature, set `UseAbpClockHandleDateTime = false` in `AbpMongoDbOptions`:

```cs
services.Configure<AbpMongoDbOptions>(x => x.UseAbpClockHandleDateTime = false);
```

### Removed Obsolete APIs

* `IRepository` doesn't inherit from `IQueryable` anymore. It was [made obsolete in 4.2](https://docs.abp.io/en/abp/latest/Migration-Guides/Abp-4_2#irepository-getqueryableasync).

## UI Providers

* [Angular UI 4.x to 5.0 Migration Guide](Abp-5_0-Angular.md)
* [ASP.NET Core MVC / Razor Pages UI 4.x to 5.0 Migration Guide]()
* [Blazor UI 4.x to 5.0 Migration Guide](Abp-5-0-Blazor.md)

## Modules

This section contains breaking and important changes in the application modules.

### Identity

An `IsActive` (`bool`) property is added to the `IdentityUser` entity. This flag will be checked during the authentication of the users. EF Core developers need to add a new database migration and update their databases.

**After the database migration, set this property to `true` for the existing users: `UPDATE AbpUsers SET IsActive=1`**. Otherwise, none of the users can login to the application.

Alternatively, you can set `defaultValue` to `true` in the migration class (after adding the migration).
This will add the column with `true` value for the existing records.

```cs
public partial class AddIsActiveToIdentityUser : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsActive",
            table: "AbpUsers",
            type: "bit",
            nullable: false,
            defaultValue: true); // Default is false.
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsActive",
            table: "AbpUsers");
    }
}
```

For MongoDB, you need to manually update the `IsActive` field for the existing users.

### IdentityServer

`IApiScopeRepository.GetByNameAsync` method renamed as `FindByNameAsync`.

## See Also

* [Angular UI 4.x to 5.0 Migration Guide](Abp-5_0-Angular.md)
* [ASP.NET Core MVC / Razor Pages UI 4.x to 5.0 Migration Guide]()
* [Blazor UI 4.x to 5.0 Migration Guide](Abp-5-0-Blazor.md)
