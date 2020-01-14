# ABP Framework v2.0 and the ABP Commercial

ABP Framework v2.0 has been released in this week. This post explains why we've released an **early major version** and what is changed with the v2.0.

In addition to the v2.0 release, we've also announcing the **ABP Commercial** which is a set of professional modules, tools, themes and services built on top of the open source ABP framework

## ABP Framework v2.0

### Why 2.0 instead of 1.2?

Actually, it was planned to release v1.2 after the [v1.1.2](https://github.com/abpframework/abp/releases/tag/1.1.2) release. However, [it is reported](https://github.com/abpframework/abp/issues/2026) that v1.x has some **performance** and **stability** problems on Linux, especially when you deploy your application into **Linux** containers with **low CPU and memory** resources.

We have investigated the problem deeply and have seen that the root cause of the problem is related to the implementation of **intercepting async methods**. In addition, there were some **async over sync** usages effected the thread pool optimization.

Finally, we **solved all the problems** with the huge help of the **community**. But we also had some important **design decisions** which cause some **breaking changes** and we had to change the major version number of the framework because of the **semantic versioning**.

Most of the applications won't be effected by [the breaking changes](https://github.com/abpframework/abp/releases), or it will be trivial to make necessary changes.

### Breaking Changes

#### Removed Some Sync APIs

We've [removed some sync APIs](https://github.com/abpframework/abp/pull/2464) because they eventually causes async over sync problems. Because some of the interceptors need to use async APIs and if they intercept sync methods they need to call async over sync.

**Async over sync** problem is a classic problem of C# when you need to **call an async method inside a sync method**. While there are some solutions to this problem, they all have **disadvantages** and it is suggested to **not write** such code at all. You can find plenty of documents related to this topic on the web, so I will not write more about it.

So, to not cause this problem;

* Removed sync [Repository](https://docs.abp.io/en/abp/latest/Repositories) methods (like Insert, Update... etc).
* Removed sync APIs of the [Unit Of Work](https://docs.abp.io/en/abp/latest/Unit-Of-Work).
* Removed sync API of the [background jobs](https://docs.abp.io/en/abp/latest/Background-Jobs).
* Removed sync APIs of [Audit logging](https://docs.abp.io/en/abp/latest/Audit-Logging).

Also removed some other rarely used sync APIs. If you get any compile error, just use the async versions of these APIs.

#### Always Async!

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

#### Other Breaking Changes

See [the release notes](https://github.com/abpframework/abp/releases/tag/2.0.0) for the other breaking changes while most of them will not effect your application code.

### New Features

This release also contains a few new features and tens of enhancements. Some of them are;

* [#2597](https://github.com/abpframework/abp/pull/2597) New  Volo.Abp.AspNetCore.Serilog  package.
* [#2526](https://github.com/abpframework/abp/issues/2526) Client side validation for the dynamic C# client proxies.
* [#2374](https://github.com/abpframework/abp/issues/2374) Async background jobs.
* [#265](https://github.com/abpframework/abp/issues/265) Managing the application shutdown.
* [#2472](https://github.com/abpframework/abp/issues/2472) Implemented  DeviceFlowCodes and TokenCleanupService for the IdentityServer module.

See [the release notes](https://github.com/abpframework/abp/releases/tag/2.0.0) for the other features, enhancements and bug fixes.

### Documentation

We've completed some missing documentation with the v2.0 release. In the next weeks, we will mostly focus on the basic documentation and tutorials.

## ABP Commercial

[ABP Commercial](https://commercial.abp.io/) is a set of professional **modules, tools, themes and services** built on top of the open source ABP framework.

* It provides [professional modules](https://commercial.abp.io/modules) in addition to ABP Framework's free & [open source modules](https://docs.abp.io/en/abp/latest/Modules/Index).
* It includes a beautiful [UI theme](https://commercial.abp.io/themes).
* It provides [ABP Suite](https://commercial.abp.io/tools/suite), a tool to assist your development to make you more productive. It currently can create full-stack CRUD pages in a few seconds by configuring your entity properties. More functionalities will be added by the time.
* [Premium support](https://commercial.abp.io/support) for enterprise companies.

In addition to these standard set of features, we will provide customer basis services. See the [commercial.abp.io](https://commercial.abp.io/) web site for other details.

### ABP Framework vs the ABP Commercial

The ABP Commercial **is not a paid version** of the ABP Framework. You can think it as **additional benefits** for professional companies. If you have budget, you can use it to save your time and develop your product faster.

ABP Framework is open source & free and will always be like that.

As a principle, we build the main infrastructure as open source while we sell additional pre-built application features, themes and tools. If you were following the [ASP.NET Boilerplate](https://aspnetboilerplate.com/) & the [ASP.NET Zero](https://aspnetzero.com/) products, the main idea is similar.

Buying a commercial license saves your significant time and effort and you can focus on your own business much more, you take a dedicated and high priority support. Also, in this way, you support the ABP core team since we are spending most of our time to develop, maintain and support the open source ABP Framework.

### Pricing

You can build **unlimited projects/products**, sell to **unlimited customers**, host in **unlimited of servers** without any restriction. Pricing is mostly based on your **developer count**, required **support level** and **source code** access. There are three main packages;

* **Team license**: Includes all the modules, themes and tools. Allows to develop your product with 3 developers. You can buy additional developer licenses.
* **Business license**: Allows to download the source code of all the modules and the themes. Also, includes 5 developer licenses by default. You can buy additional developer licenses.
* **Enterprise license**: Provides unlimited and private support in addition to the benefits of the business license.

See the [pricing page](https://commercial.abp.io/pricing) for details. In addition to the standard packages, we are also providing custom services and custom licensing. [Contact us](https://commercial.abp.io/contact) if you have any questions.

#### License Comparison

The license price changes based on your developer count, required support level and source code access.

##### The Source Code

Team license doesn't include the source code of the pre-built modules & themes. It uses all these modules as NuGet & NPM packages. In this was, you can easily get new features and bug fixes by just updating the package dependencies. But you can't access their source code. So you don't have to possibility to embed a module's source code into your application and freely change the source code.

Pre-built modules provides some level of customizability and extensibility and allows you to override services, UI parts and so on. We are working on to make them much more customizable and extensible. So, if you don't need to make major changes on the pre-built modules, the team license will be ideal for you; Because it is cheaper and allows you to easily get new features and bug fixes.

Business and Enterprise licenses allow you to download the source code of any module or theme when you need. They also uses the same startup template with the team license, so all modules are used as NuGet and NPM packages. But in case of need, you can remove the package dependencies for a module and embed its source code into your own solution to completely customize it. In this case, upgrading the module will not be as easy as before when a new version is available. You don't have to upgrade it, surely. But if you want, you should do it yourself using some merge tool or Git branch system.

#### License Lifetime

ABP Commercial license is **perpetual**, that means you can **use it forever** and continue to develop your applications.

However, the following services are for one year:

* Premium **support** ends after one year. You can continue to get the community support.
* You can not get **updates** of the modules & themes after one year. You can continue to use the last obtained version. You can even get bug fixes and enhancements for your current major version.
* You can use the ABP **Suite** tooling for one year.

If you want to continue to get these benefits, you can extend your license period. Renewing price is 20% less than the regular price.