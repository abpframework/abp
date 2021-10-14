# ABP Framework v4.x to v5.0 Migration Guide

## IdentityUser

`IsActive <bool>` property is added to the `IdentityUser`. This flag will be checked during the authentication of the users. See the related [PR](https://github.com/abpframework/abp/pull/10185). 
**After the migration, set this property to `true` for the existing users: `UPDATE AbpUsers SET IsActive=1`**

For EFCore you can set `defaultValue` to `true` in the migration class:
(This will add the column with `true` value for the existing records.)

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

For document base databases like MongoDB, you need to manually update the `IsActive` field for the existing user records.
 
## MongoDB

ABP Framework will serialize the datetime based on [AbpClockOptions](https://docs.abp.io/en/abp/latest/Timing#clock-options) starting from ABP v5.0. It was saving `DateTime` values as UTC in MongoDB. Check out [MongoDB Datetime Serialization Options](https://mongodb.github.io/mongo-csharp-driver/2.13/reference/bson/mapping/#datetime-serialization-options).

To revert back this feature, set `UseAbpClockHandleDateTime = false` in `AbpMongoDbOptions`:

```cs
services.Configure<AbpMongoDbOptions>(x => x.UseAbpClockHandleDateTime = false);
```

## IApiScopeRepository

`GetByNameAsync` method renamed as `FindByNameAsync`.

## Angular UI

See the [Angular UI 5.0 Migration Guide](Abp-5_0-Angular.md).
