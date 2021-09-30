# ABP Framework 4.x to 5.0 Migration Guide

## MongoDB

Framework will serializer the datetime based on [AbpClockOptions](https://docs.abp.io/en/abp/latest/Timing#clock-options) start from 5.0, before `DateTime` values in MongoDB are [always saved as UTC](https://mongodb.github.io/mongo-csharp-driver/2.13/reference/bson/mapping/#datetime-serialization-options).

You can disable this behavior by configure `AbpMongoDbOptions`.
```cs
services.Configure<AbpMongoDbOptions>(x => x.UseAbpClockHandleDateTime = false);
```
