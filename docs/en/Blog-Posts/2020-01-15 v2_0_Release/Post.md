# ABP Framework v2.0 Has Been Released

ABP Framework v2.0 has been released in this week. This post explains why we've released an early major version and what is changed with the v2.0.

## Why 2.0 instead of 1.2?

Actually, it was planned to release v1.2 after the [v1.1.2](https://github.com/abpframework/abp/releases/tag/1.1.2) release. However, [it is reported](https://github.com/abpframework/abp/issues/2026) that v1.x has some **performance** and **stability** problems on Linux, especially when you deploy your application into **Linux** containers with **low CPU and memory** resources.

We have investigated the problem deeply and have seen that the root cause of the problem is related to the implementation of **intercepting async methods**. In addition, there were some **async over sync** usages effected the thread pool optimization.

Finally, we **solved all the problems** with the huge help of the **community**. But we also had some important **design decisions** which cause some **breaking changes** and we had to change the major version number of the framework because of the **semantic versioning**.

Most of the applications won't be effected by [the breaking changes](https://github.com/abpframework/abp/releases), or it will be trivial to make necessary changes.

## Breaking Changes

### Removed Some Sync APIs

We've [removed some sync APIs](https://github.com/abpframework/abp/pull/2464) because they eventually causes async over sync problems. Because some of the interceptors need to use async APIs and if they intercept sync methods they need to call async over sync.

**Async over sync** problem is a classic problem of C# when you need to **call an async method inside a sync method**. While there are some solutions to this problem, they all have **disadvantages** and it is suggested to **not write** such code at all. You can find plenty of documents related to this topic on the web, so I will not write more about it.

So, to not cause this problem;

* Removed sync [Repository](https://docs.abp.io/en/abp/latest/Repositories) methods (like Insert, Update... etc).
* Removed sync APIs of the [Unit Of Work](https://docs.abp.io/en/abp/latest/Unit-Of-Work).
* Removed sync API of the [background jobs](https://docs.abp.io/en/abp/latest/Background-Jobs).
* Removed sync APIs of [Audit logging](https://docs.abp.io/en/abp/latest/Audit-Logging).

Also removed some other rarely used sync APIs. If you get any compile error, just use the async versions of these APIs.

### Always Async!

Beginning from the v2.0, ABP framework assumes that you are writing your application code async. Otherwise, some framework funtionalities may not properly work.

It is suggested to write async for all your [application services](https://docs.abp.io/en/abp/latest/Application-Services), [repository methods](https://docs.abp.io/en/abp/latest/Repositories), controller actions, page handlers.

Even if your application service method doesn't need to be async, write it as async, because interceptors perform async operations (for authorization, unit of work... etc.). You can return `Task.Completed` from a method that doesn't make an async call.

Example:

````csharp
public Task<int> GetValueAsync()
{
    ...
    return Task.CompletedTask(42);
}
````

The example above doesn't need to be async because it doesn't perform an async call to any service. However, making it async helps to the ABP framework to run interceptors without async over sync calls.

This rule doesn't force you to write every method async. This would not be good and would be tedious. It is only needed for the intercepted services (especially for [application services](https://docs.abp.io/en/abp/latest/Application-Services) and [repository methods](https://docs.abp.io/en/abp/latest/Repositories))

### Other Breaking Changes

See [the release notes](https://github.com/abpframework/abp/releases/tag/2.0.0) for the other breaking changes while most of them will not effect your application code.

## New Features

This release also contains a few new features and tens of enhancements. Some of them are;

* [#2597](https://github.com/abpframework/abp/pull/2597) New  Volo.Abp.AspNetCore.Serilog  package.
* [#2526](https://github.com/abpframework/abp/issues/2526) Client side validation for the dynamic C# client proxies.
* [#2374](https://github.com/abpframework/abp/issues/2374) Async background jobs.
* [#265](https://github.com/abpframework/abp/issues/265) Managing the application shutdown.
* [#2472](https://github.com/abpframework/abp/issues/2472) Implemented  DeviceFlowCodes and TokenCleanupService for the IdentityServer module.

See [the release notes](https://github.com/abpframework/abp/releases/tag/2.0.0) for the other features, enhancements and bug fixes.

## Documentation

We've completed some missing documentation with the v2.0 release. In the next weeks, we will mostly focus on the basic documentation and tutorials.

