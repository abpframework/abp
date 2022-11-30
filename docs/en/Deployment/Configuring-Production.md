# Configuring for Production

ABP Framework has a lot of options to configure and fine-tune its features. They are all explained their own documents. Default values for these options are pretty well for most of the deployment environments. However, you may need to care about some options based on how you've structured your deployment environment. In this document, we will highlight these kind of options. So, it is highly recommended to read this document to not have unexpected behaviors in your system in production.

## Distributed Cache Prefix

ABP's [distributed cache infrastructure](../Caching.md) provides an option to set a key prefix for all of your data saved into your distributed cache provider. The default value of this option is not set (it is `null`). If you are using a distributed cache server that shared by different applications, then you can set a prefix value to isolate an application's cache data from others.

````csharp
Configure<AbpDistributedCacheOptions>(options =>
{
    options.KeyPrefix = "MyCrmApp";
});
````

That's all. ABP, then will add this prefix to all of your cache keys in your application as along as you use ABP's `IDistributedCache<TCacheItem>` or `IDistributedCache<TCacheItem,TKey>` services. See the [Caching documentation](../Caching.md) if you are new to the distributed caching.

> **Warning #1**: If you use ASP.NET Core's standard `IDistributedCache` service, it's your responsibility to add the key prefix (you can get the value by injecting `IOptions<AbpDistributedCacheOptions>`). ABP can not do it.

> **Warning #2**: Even if you have never used distributed caching in your own codebase, ABP still uses it for some features. So, you should always configure this prefix if your caching server is shared among multiple systems.

> **Warning #3**: Some of the ABP's startup templates are pre-configured to set a prefix value for the distributed cache. So, please check your application code if it is already configured.

## Distributed Lock Prefix

ABP's [distributed locking infrastructure](../Distributed-Locking.md) provides an option to set a prefix for all keys you are using in the distributed lock server. The default value of this option is not set (it is `null`). If you are using a distributed lock server that is shared by different applications, then you can set a prefix value to isolate an application's locks from others.

````csharp
Configure<AbpDistributedLockOptions>(options =>
{
    options.KeyPrefix = "MyCrmApp";
});
````

That's all. ABP, then will add this prefix to all of your keys in your application. See the [Distributed Locking documentation](../Distributed-Locking.md) if you are new to the distributed locking.

> **Warning #1**: Even if you have never used distributed locking in your own codebase, ABP still uses it for some features. So, you should always configure this prefix if your distributed lock server is shared among multiple systems.

> **Warning #2**: Some of the ABP's startup templates are pre-configured to set a prefix value for the distributed locking. So, please check your application code if it is already configured.

