# ABP Dapr Integration

> This document assumes that you are already familiar with [Dapr](https://dapr.io/) and you want to use it in your ABP based applications.

[Dapr](https://dapr.io/) (Distributed Application Runtime) provides APIs that simplify microservice connectivity. It is an open source project that is mainly backed by Microsoft. It is also a CNCF (Cloud Native Computing Foundation) project and trusted by community.

ABP and Dapr has some intersecting features like service-to-service communication, distributed message bus and distributed locking. However their purposes are totally different. ABP's goal is to provide an end-to-end developer experience by offering an opinionated architecture and providing the necessary infrastructure libraries, reusable modules and tools to implement that architecture properly. Dapr's purpose, on the other hand, is to provide a runtime to decouple common microservice communication patterns from your application logic.

ABP and Dapr can perfectly work together in the same application. ABP offers some packages to provide better integration where Dapr features intersect with ABP. You can use other Dapr features with no ABP integration packages based on [its own documentation](https://docs.dapr.io/).

## ABP Dapr Integration Packages

ABP provides the following NuGet packages for the Dapr integration:

* [Volo.Abp.Dapr](https://www.nuget.org/packages/Volo.Abp.Dapr): The main Dapr integration package. All other packages depend on this package.
* [Volo.Abp.Http.Client.Dapr](https://www.nuget.org/packages/Volo.Abp.Http.Client.Dapr): Integration package for ABP's [dynamic](../API/Dynamic-CSharp-API-Clients.md) and [static](../API/Static-CSharp-API-Clients.md) C# API Client Proxies systems with Dapr's [service invocation](https://docs.dapr.io/developing-applications/building-blocks/service-invocation/service-invocation-overview/) building block.
* [Volo.Abp.EventBus.Dapr](https://www.nuget.org/packages/Volo.Abp.EventBus.Dapr): Implements ABP's distributed event bus with Dapr's [publish & subscribe](https://docs.dapr.io/developing-applications/building-blocks/pubsub/) building block. With this package, you can send events, but can not receive.
* [Volo.Abp.AspNetCore.Mvc.Dapr.EventBus](https://www.nuget.org/packages/Volo.Abp.AspNetCore.Mvc.Dapr.EventBus): Provides the endpoints to receive events from Dapr's [publish & subscribe](https://docs.dapr.io/developing-applications/building-blocks/pubsub/) building block. Use this package to send and receive events.
* [Volo.Abp.DistributedLocking.Dapr](https://www.nuget.org/packages/Volo.Abp.DistributedLocking.Dapr): Uses Dapr's [distributed lock](https://docs.dapr.io/developing-applications/building-blocks/distributed-lock/) building block for [distributed locking](Distributed-Locking.md) service of the ABP Framework.

In the following sections, we will see how to use these packages to use Dapr in your ABP based solutions.

## Basics

### Installation

> This section explains how to add [Volo.Abp.Dapr](https://www.nuget.org/packages/Volo.Abp.Dapr), the core Dapr integration package to your project. If you are using one of the other Dapr integration packages, you can skip this section since this package will be indirectly added.

Use the ABP CLI to add [Volo.Abp.Dapr](https://www.nuget.org/packages/Volo.Abp.Dapr) NuGet package to your project:

* Install the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) if you haven't installed before.
* Open a command line (terminal) in the directory of the `.csproj` file you want to add the `Volo.Abp.Dapr` package.
* Run `abp add-package Volo.Abp.Dapr` command.

If you want to do it manually, install the [Volo.Abp.Dapr](https://www.nuget.org/packages/Volo.Abp.Dapr) NuGet package to your project and add `[DependsOn(typeof(AbpDaprModule))]` to the [ABP module](Module-Development-Basics.md) class inside your project.

### AbpDaprOptions

`AbpDaprOptions` is the main [options class](../Options.md) that you can configure the global Dapr settings. **All settings are optional and you mostly don't need to configure them.** If you need, you can configure it in the `ConfigureServices` method of your [module class](../Module-Development-Basics.md):

````csharp
Configure<AbpDaprOptions>(options =>
{
    // ...
});
````

Available properties of the `AbpDaprOptions` class:

* `HttpEndpoint` (optional): HTTP endpoint that is used while creating a `DaprClient` object. If you don't specify, the default value is used.
* `GrpcEndpoint` (optional): The gRPC endpoint that is used while creating a `DaprClient` object. If you don't specify, the default value is used.

Alternatively, you can configure the options in the `Dapr` section of your `appsettings.json` file. Example:

````csharp
"Dapr": {
  "HttpEndpoint": "http://localhost:3500/"
}
````

### IAbpDaprClientFactory

`IAbpDaprClientFactory` is used to create `DaprClient` or `HttpClient` objects to perform operations on Dapr. It uses `AbpDaprOptions`, so you can configure the settings in a central place.

**Example usages:**

````csharp
public class MyService
{
    private readonly IAbpDaprClientFactory _daprClientFactory;

    public MyService(IAbpDaprClientFactory daprClientFactory)
    {
        _daprClientFactory = daprClientFactory;
    }

    public async Task DoItAsync()
    {
        // Create a DaprClient object with default options
        DaprClient daprClient = await _daprClientFactory.CreateAsync();
        
        /* Create a DaprClient object with configuring
         * the DaprClientBuilder object */
        DaprClient daprClient2 = await _daprClientFactory
            .CreateAsync(builder =>
            {
                builder.UseDaprApiToken("...");
            });
        
        // Create an HttpClient object
        HttpClient httpClient = await _daprClientFactory
            .CreateHttpClientAsync("target-app-id");
    }
}
````

`CreateHttpClientAsync` method also gets optional `daprEndpoint` and `daprApiToken` parameters.

> ABP uses `IAbpDaprClientFactory` when it needs to create a Dapr client. You can also use Dapr API to create client objects in your application. Using `IAbpDaprClientFactory` is recommended, but not required.

## C# API Client Proxies Integration

ABP can [dynamically](../API/Dynamic-CSharp-API-Clients.md) or [statically](../API/Static-CSharp-API-Clients.md) generate proxy classes to invoke your HTTP APIs from a Dotnet client application. It makes dead simple to consume HTTP APIs in a distributed system. [Volo.Abp.Http.Client.Dapr](https://www.nuget.org/packages/Volo.Abp.Http.Client.Dapr) package configures the client-side proxies system, so it uses Dapr's service invocation building block for the communication between your applications.

### Installation

Use the ABP CLI to add [Volo.Abp.Http.Client.Dapr](https://www.nuget.org/packages/Volo.Abp.Http.Client.Dapr) NuGet package to your project (to the client side):

* Install the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) if you haven't installed before.
* Open a command line (terminal) in the directory of the `.csproj` file you want to add the `Volo.Abp.Http.Client.Dapr` package.
* Run `abp add-package Volo.Abp.Http.Client.Dapr` command.

If you want to do it manually, install the [Volo.Abp.Http.Client.Dapr](https://www.nuget.org/packages/Volo.Abp.Http.Client.Dapr) NuGet package to your project and add `[DependsOn(typeof(AbpHttpClientDaprModule))]` to the [ABP module](Module-Development-Basics.md) class inside your project.

### Configuration

One you install the [Volo.Abp.Http.Client.Dapr](https://www.nuget.org/packages/Volo.Abp.Http.Client.Dapr) NuGet package, all you need to do it to configure ABP's remote services option either in `appsettings.json` or using the `AbpRemoteServiceOptions` [options class](../Options.md).

**Example:**

````csharp
{
  "RemoteServices": {
    "Default": {
      "BaseUrl": "http://dapr-httpapi/"
    }
  }
}
````

`dapr-httpapi` in this example is the application id of the server application in your Dapr configuration.

The remove service name (`Default` in this example) should match the remote service name specified in the `AddHttpClientProxies` call for dynamic client proxies or `AddStaticHttpClientProxies` call for static client proxies. Using `Default` is fine if your client communicates to a single server. However, if your client uses multiple servers, you typically have multiple keys in the `RemoteServices` configuration. Once you configure the remote service endpoints as Dapr application ids, it will automatically work and make the HTTP calls through Dapr when you use ABP's client proxy system.

> See the [dynamic](../API/Dynamic-CSharp-API-Clients.md) and [static](../API/Static-CSharp-API-Clients.md) client proxy documents for details about the ABP's client proxy system.

## Distributed Event Bus Integration

[ABP's distributed event bus](../Distributed-Event-Bus.md) system provides a convenient abstraction to allow application communicate asynchronously via events. ABP has integration packages with various distributed messaging systems, like RabbitMQ, Kafka, and Azure. Dapr also has a [publish & subscribe building block](https://docs.dapr.io/developing-applications/building-blocks/pubsub/pubsub-overview/) for the same purpose: distributed messaging / events.

ABP's [Volo.Abp.EventBus.Dapr](https://www.nuget.org/packages/Volo.Abp.EventBus.Dapr) and [Volo.Abp.AspNetCore.Mvc.Dapr.EventBus](https://www.nuget.org/packages/Volo.Abp.AspNetCore.Mvc.Dapr.EventBus) packages make possible to use the Dapr infrastructure for ABP's distributed event bus.

The [Volo.Abp.EventBus.Dapr](https://www.nuget.org/packages/Volo.Abp.EventBus.Dapr) package can be used by any type of application (e.g., a Console or ASP.NET Core application) to publish events through Dapr. To be able to receive messages (by subscribing events), you need to have the [Volo.Abp.AspNetCore.Mvc.Dapr.EventBus](https://www.nuget.org/packages/Volo.Abp.AspNetCore.Mvc.Dapr.EventBus) package installed, and your application should be an ASP.NET Core application.

### Installation

If your application is an ASP.NET Core application and you want to send and receive events, you need to install the [Volo.Abp.AspNetCore.Mvc.Dapr.EventBus](https://www.nuget.org/packages/Volo.Abp.AspNetCore.Mvc.Dapr.EventBus) package as describe below:

You can use the ABP CLI to add [Volo.Abp.AspNetCore.Mvc.Dapr.EventBus](https://www.nuget.org/packages/Volo.Abp.AspNetCore.Mvc.Dapr.EventBus) NuGet package to your project (to the client side):

* Install the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) if you haven't installed before.
* Open a command line (terminal) in the directory of the `.csproj` file you want to add the `Volo.Abp.AspNetCore.Mvc.Dapr.EventBus` package.
* Run `abp add-package Volo.Abp.AspNetCore.Mvc.Dapr.EventBus` command.

If you want to do it manually, install the [Volo.Abp.AspNetCore.Mvc.Dapr.EventBus](https://www.nuget.org/packages/Volo.Abp.AspNetCore.Mvc.Dapr.EventBus) NuGet package to your project and add `[DependsOn(typeof(AbpAspNetCoreMvcDaprEventBusModule))]` to the [ABP module](Module-Development-Basics.md) class inside your project.

> **If you install the [Volo.Abp.AspNetCore.Mvc.Dapr.EventBus](https://www.nuget.org/packages/Volo.Abp.AspNetCore.Mvc.Dapr.EventBus) package, you don't need to install the [Volo.Abp.EventBus.Dapr](https://www.nuget.org/packages/Volo.Abp.EventBus.Dapr) package, because the first one already has a reference to the latter one.**

If your application is not an ASP.NET Core application, you can't receive events from Dapr, at least with ABP's integration packages (see [Dapr's document](https://docs.dapr.io/developing-applications/building-blocks/pubsub/howto-publish-subscribe/) if you want to receive events in a different type of application). However, you can still publish messages using the [Volo.Abp.EventBus.Dapr](https://www.nuget.org/packages/Volo.Abp.EventBus.Dapr) package. In this case, follow the steps below to install that package to your project:

* Install the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) if you haven't installed before.
* Open a command line (terminal) in the directory of the `.csproj` file you want to add the `Volo.Abp.EventBus.Dapr` package.
* Run `abp add-package Volo.Abp.EventBus.Dapr` command.

If you want to do it manually, install the [Volo.Abp.EventBus.Dapr](https://www.nuget.org/packages/Volo.Abp.EventBus.Dapr) NuGet package to your project and add `[DependsOn(typeof(AbpEventBusDaprModule))]` to the [ABP module](Module-Development-Basics.md) class inside your project.

### Configuration

You can configure the `AbpDaprEventBusOptions` [options class](../Options.md) for Dapr configuration:

````csharp
Configure<AbpDaprEventBusOptions>(options =>
{
    options.PubSubName = "test-pubsub";
});
````

Available properties of the `AbpDaprEventBusOptions` class:

* `PubSubName` (optional): The `pubsubName` parameter while publishing messages through `DaprClient.PublishEventAsync` method. Default value: `pubsub`.

### Usage

You can follow [ABP's distributed event bus documentation](../Distributed-Event-Bus.md) to learn how to publish and subscribe to events in the ABP way. No change required in your application code to use Dapr pub-sub.

In addition to ABP's standard distributed event bus system, you can also use Dapr's API to publish events. In that case, just use the `IAbpDaprClientFactory` service to create a `DaprClient` object (or create it yourself by following Dapr's documentation) and use its `PublishEventAsync` method as [documented by Dapr](https://docs.microsoft.com/en-us/dotnet/architecture/dapr-for-net-developers/publish-subscribe).

> If you directly use the Dapr API to publish events, you can not benefit from ABP's standard distributed event bus features, like the outbox/inbox pattern implementation.

## See Also

* [Dapr for .NET Developers](https://docs.microsoft.com/en-us/dotnet/architecture/dapr-for-net-developers/)
* [The Official Dapr Documentation](https://docs.dapr.io/)