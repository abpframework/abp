# ABP Framework v4.x to v5.0 Migration Guide

## IdentityUser

We added an `IsActive(bool)` property to `IdentityUser` to [control whether it is available](https://github.com/abpframework/abp/pull/10185). **Please set it to `true` of the old user after the upgrade.**

For EF Core you can change `defaultValue` to `true` in the migration class:
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


## MongoDB

ABP Framework will serialize the datetime based on [AbpClockOptions](https://docs.abp.io/en/abp/latest/Timing#clock-options) start from 5.0, before `DateTime` values in MongoDB are [always saved as UTC](https://mongodb.github.io/mongo-csharp-driver/2.13/reference/bson/mapping/#datetime-serialization-options).

You can disable this behavior by configure `AbpMongoDbOptions`.
```cs
services.Configure<AbpMongoDbOptions>(x => x.UseAbpClockHandleDateTime = false);
```

## Angular UI

See the [Angular UI Migration Guide](Abp-5_0-Angular.md).
