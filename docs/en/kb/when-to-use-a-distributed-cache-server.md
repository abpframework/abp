```json
//[doc-seo]
{
    "Description": "Learn when to use a distributed cache server with ABP Framework to enhance performance in multi-instance and microservice applications."
}
```

# KB#0002: When to Use a Distributed Cache Server

ABP provides a [distributed cache service](../framework/fundamentals/caching.md) that is based on [ASP.NET Core's distributed cache](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed). This document explains when you need to have a separate cache server for your applications.

## Understanding the Default Cache Service

**Default implementation of the cache service works in-memory**. Memory cache is only useful if you are building a monolith application and you run a single instance of your application. For other cases, **you should use a real distributed cache server**.

Here are a few example cases where you should use a distributed cache server:

* You have a **monolith application**, but you run **multiple instances** of that application concurrently, for example, in a [clustered environment](../deployment/clustered-environment.md)
* You build a **microservice** or any kind of **distributed** system
* You have web **multiple applications** in your solution and they should share the same cache

The problem is obvious: If each application instance uses its internal in-memory cache, and if two or more applications cache the same data, it is probable that they will cache different copies of the data. In that case, there is no way to **invalidate/refresh** that data in every application's memory when the data changes.

## What is a Distributed Cache Server

A **distributed cache server** (e.g. [Redis](../framework/fundamentals/redis-cache.md)) stores cache objects in a separate server application and allows multiple applications/processes to share the same cache objects. In that way;

* All applications/services and all their instances use the same cache store and share the same cached objects. Once an application instance refreshes a cached object, all others use the new object.
* Even if your applications stop and restart, the cached objects are not lost, since they are managed by a separate cache server.

## How to Use a Distributed Cache Server

ABP [solution templates](../solution-templates/index.md) come with Redis configured when it is certainly necessary. For example;

* The [microservice startup template](../solution-templates/microservice/index.md) always comes with [Redis configured](../solution-templates/microservice/distributed-cache.md) and also included as a docker container.

* The application startup template comes with Redis configured when you select multiple applications, tiered architecture, or some other configuration that requires a distributed cache server.

In other cases, to keep the dependencies minimal, they come with the default (in-memory) cache configuration. In those cases, if you need a distributed cache server, you should manually switch to a distributed cache provider for your application.

See the *[Redis Cache](../framework/fundamentals/redis-cache.md)* document if you need to use Redis as the distributed cache server. See [ASP.NET Core's documentation](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed) to see how to switch to another cache provider.

### Installing a Redis Server to Your Local Environment

If you want to use Redis as your distributed cache provider in your development environment, you can simply use the [official Redis docker image](https://hub.docker.com/_/redis). Once you have [Docker](https://www.docker.com/products/docker-desktop/) in your local machine, you can use the following command to run a Redis container and map the default Redis port:

````bash
docker run -p 6379:6379 --name RedisServer -d redis
````

You can check the [official Redis docker image](https://hub.docker.com/_/redis) document for more options.

## See Also

* [ABP Distributed Cache](../framework/fundamentals/caching.md)
* [ASP.NET Core Distributed Cache](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed)
