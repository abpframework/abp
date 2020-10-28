# Redis Cache

ABP Framework [Caching System](Caching.md) extends the [ASP.NET Core distributed cache](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed). So, **any provider** supported by the standard ASP.NET Core distributed cache can be usable in your application and can be configured just like **documented by Microsoft**.

However, ABP provides an **integration package** for Redis Cache: [Volo.Abp.Caching.StackExchangeRedis](https://www.nuget.org/packages/Volo.Abp.Caching.StackExchangeRedis). There are two reasons for using this package, instead of the standard [Microsoft.Extensions.Caching.StackExchangeRedis](https://www.nuget.org/packages/Microsoft.Extensions.Caching.StackExchangeRedis/) package.

1. It implements `SetManyAsync` and `GetManyAsync` methods. These are not standard methods of the Microsoft Caching library, but added by the ABP Framework [Caching](Caching.md) system. They **significiantly increases the performance** when you need to set/get multiple cache items with a single method call.
2. It **simplifies** the Redis cache **configuration** (will be explained below).

> Volo.Abp.Caching.StackExchangeRedis is already uses the Microsoft.Extensions.Caching.StackExchangeRedis package, but extends and improves it.

## Installation

> This package is already installed in the application startup template if it is using Redis.

Open a command line in the folder of your `.csproj` file and type the following ABP CLI command:

````bash
abp add-package Volo.Abp.Caching.StackExchangeRedis
````

## Configuration

Volo.Abp.Caching.StackExchangeRedis package automatically gets the redis [configuration](Configuration.md) from the `IConfiguration`. So, for example, you can set your configuration inside the `appsettings.json`:

````js
"Redis": { 
 "IsEnabled": "true",
 "Configuration": "127.0.0.1"
}
````
The setting `IsEnabled` is optional and will be considered `true` if it is not set.

Alternatively you can configure the standard [RedisCacheOptions](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.caching.stackexchangeredis.rediscacheoptions) [options](Options.md) class in the `ConfigureServices` method of your [module](Module-Development-Basics.md):

````csharp
Configure<RedisCacheOptions>(options =>
{
    //...
});
````

## See Also

* [Caching](Caching.md)