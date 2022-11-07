# ABP Dapr Integration

> This document assumes that you are already familiar with [Dapr](https://dapr.io/) and you want to use it in your ABP based applications.

[Dapr](https://dapr.io/) (Distributed Application Runtime) provides APIs that simplify microservice connectivity. It is an open source project that is mainly backed by Microsoft. It is also a CNCF (Cloud Native Computing Foundation) project and trusted by the community.

ABP and Dapr have some intersecting features like service-to-service communication, distributed message bus and distributed locking. However their purposes are totally different. ABP's goal is to provide an end-to-end developer experience by offering an opinionated architecture and providing the necessary infrastructure libraries, reusable modules and tools to implement that architecture properly. Dapr's purpose, on the other hand, is to provide a runtime to decouple common microservice communication patterns from your application logic.

ABP and Dapr can perfectly work together in the same application. ABP offers some packages to provide better integration where Dapr features intersect with ABP. You can use other Dapr features with no ABP integration packages based on [its own documentation](https://docs.dapr.io/).

## ABP Dapr Integration Packages

ABP provides the following NuGet packages for the Dapr integration:

* [Volo.Abp.Dapr](https://www.nuget.org/packages/Volo.Abp.Dapr): The main Dapr integration package. All other packages depend on this package.
* [Volo.Abp.Http.Client.Dapr](https://www.nuget.org/packages/Volo.Abp.Http.Client.Dapr): Integration package for ABP's [dynamic](../API/Dynamic-CSharp-API-Clients.md) and [static](../API/Static-CSharp-API-Clients.md) C# API Client Proxies systems with Dapr's [service invocation](https://docs.dapr.io/developing-applications/building-blocks/service-invocation/service-invocation-overview/) building block.
* [Volo.Abp.EventBus.Dapr](https://www.nuget.org/packages/Volo.Abp.EventBus.Dapr): Implements ABP's distributed event bus with Dapr's [publish & subscribe](https://docs.dapr.io/developing-applications/building-blocks/pubsub/) building block. With this package, you can send events, but can not receive.
* [Volo.Abp.AspNetCore.Mvc.Dapr.EventBus](https://www.nuget.org/packages/Volo.Abp.AspNetCore.Mvc.Dapr.EventBus): Provides the endpoints to receive events from Dapr's [publish & subscribe](https://docs.dapr.io/developing-applications/building-blocks/pubsub/) building block. Use this package to send and receive events.
* [Volo.Abp.DistributedLocking.Dapr](https://www.nuget.org/packages/Volo.Abp.DistributedLocking.Dapr): Uses Dapr's [distributed lock](https://docs.dapr.io/developing-applications/building-blocks/distributed-lock/) building block for [distributed locking](../Distributed-Locking.md) service of the ABP Framework.

In the following sections, we will see how to use these packages to use Dapr in your ABP based solutions.

## Basics

### Installation

> This section explains how to add [Volo.Abp.Dapr](https://www.nuget.org/packages/Volo.Abp.Dapr), the core Dapr integration package to your project. If you are using one of the other Dapr integration packages, you can skip this section since this package will be indirectly added.

Use the ABP CLI to add the [Volo.Abp.Dapr](https://www.nuget.org/packages/Volo.Abp.Dapr) NuGet package to your project:

* Install the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) if you haven't installed it before.
* Open a command line (terminal) in the directory of the `.csproj` file you want to add the `Volo.Abp.Dapr` package.
* Run the `abp add-package Volo.Abp.Dapr` command.

If you want to do it manually, install the [Volo.Abp.Dapr](https://www.nuget.org/packages/Volo.Abp.Dapr) NuGet package to your project and add `[DependsOn(typeof(AbpDaprModule))]` to the [ABP module](../Module-Development-Basics.md) class inside your project.

### AbpDaprOptions

`AbpDaprOptions` is the main [options class](../Options.md) that you can configure the global Dapr settings with. **All settings are optional and you mostly don't need to configure them.** If you need, you can configure it in the `ConfigureServices` method of your [module class](../Module-Development-Basics.md):

````csharp
Configure<AbpDaprOptions>(options =>
{
    // ...
});
````

Available properties of the `AbpDaprOptions` class:

* `HttpEndpoint` (optional): HTTP endpoint that is used while creating a `DaprClient` object. If you don't specify, the default value is used.
* `GrpcEndpoint` (optional): The gRPC endpoint that is used while creating a `DaprClient` object. If you don't specify, the default value is used.
* `DaprApiToken` (optional): The [Dapr API token](https://docs.dapr.io/operations/security/api-token/) that is used while sending requests from the application to Dapr. It is filled from the `DAPR_API_TOKEN` environment variable by default (which is set by Dapr once it is configured). See the *Security* section in this document for details.
* `AppApiToken` (optional): The [App API token](https://docs.dapr.io/operations/security/app-api-token/) that is used to validate requests coming from Dapr. It is filled from the `APP_API_TOKEN` environment variable by default (which is set by Dapr once it is configured). See the *Security* section in this document for details.

Alternatively, you can configure the options in the `Dapr` section of your `appsettings.json` file. Example:

````csharp
"Dapr": {
  "HttpEndpoint": "http://localhost:3500/"
}
````

### Injecting DaprClient

ABP registers the `DaprClient` class to the [dependency injection](../Dependency-Injection.md) system. So, you can inject and use it whenever you need:

````csharp
public class MyService : ITransientDependency
{
    private readonly DaprClient _daprClient;

    public MyService(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task DoItAsync()
    {
        // TODO: Use the injected _daprClient object
    }
}
````

Injecting `DaprClient` is the recommended way of using it in your application code. When you inject it, the `IAbpDaprClientFactory` service is used to create it, which is explained in the next section.

### IAbpDaprClientFactory

`IAbpDaprClientFactory` can be used to create `DaprClient` or `HttpClient` objects to perform operations on Dapr. It uses `AbpDaprOptions`, so you can configure the settings in a central place.

**Example usages:**

````csharp
public class MyService : ITransientDependency
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

ABP can [dynamically](../API/Dynamic-CSharp-API-Clients.md) or [statically](../API/Static-CSharp-API-Clients.md) generate proxy classes to invoke your HTTP APIs from a Dotnet client application. It makes perfect sense to consume HTTP APIs in a distributed system. The [Volo.Abp.Http.Client.Dapr](https://www.nuget.org/packages/Volo.Abp.Http.Client.Dapr) package configures the client-side proxies system, so it uses Dapr's service invocation building block for the communication between your applications.

### Installation

Use the ABP CLI to add the [Volo.Abp.Http.Client.Dapr](https://www.nuget.org/packages/Volo.Abp.Http.Client.Dapr) NuGet package to your project (to the client side):

* Install the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) if you haven't installed before.
* Open a command line (terminal) in the directory of the `.csproj` file you want to add the `Volo.Abp.Http.Client.Dapr` package to.
* Run the `abp add-package Volo.Abp.Http.Client.Dapr` command.

If you want to do it manually, install the [Volo.Abp.Http.Client.Dapr](https://www.nuget.org/packages/Volo.Abp.Http.Client.Dapr) NuGet package to your project and add `[DependsOn(typeof(AbpHttpClientDaprModule))]` to the [ABP module](../Module-Development-Basics.md) class inside your project.

### Configuration

Once you install the [Volo.Abp.Http.Client.Dapr](https://www.nuget.org/packages/Volo.Abp.Http.Client.Dapr) NuGet package, all you need to do is to configure ABP's remote services option either in `appsettings.json` or using the `AbpRemoteServiceOptions` [options class](../Options.md).

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

The remote service name (`Default` in this example) should match the remote service name specified in the `AddHttpClientProxies` call for dynamic client proxies or the `AddStaticHttpClientProxies` call for static client proxies. Using `Default` is fine if your client communicates to a single server. However, if your client uses multiple servers, you typically have multiple keys in the `RemoteServices` configuration. Once you configure the remote service endpoints as Dapr application ids, it will automatically work and make the HTTP calls through Dapr when you use ABP's client proxy system.

> See the [dynamic](../API/Dynamic-CSharp-API-Clients.md) and [static](../API/Static-CSharp-API-Clients.md) client proxy documents for details about the ABP's client proxy system.

## Distributed Event Bus Integration

[ABP's distributed event bus](../Distributed-Event-Bus.md) system provides a convenient abstraction to allow applications to communicate asynchronously via events. ABP has integration packages with various distributed messaging systems, like RabbitMQ, Kafka, and Azure. Dapr also has a [publish & subscribe building block](https://docs.dapr.io/developing-applications/building-blocks/pubsub/pubsub-overview/) for the same purpose: distributed messaging / events.

ABP's [Volo.Abp.EventBus.Dapr](https://www.nuget.org/packages/Volo.Abp.EventBus.Dapr) and [Volo.Abp.AspNetCore.Mvc.Dapr.EventBus](https://www.nuget.org/packages/Volo.Abp.AspNetCore.Mvc.Dapr.EventBus) packages make it possible to use the Dapr infrastructure for ABP's distributed event bus.

The [Volo.Abp.EventBus.Dapr](https://www.nuget.org/packages/Volo.Abp.EventBus.Dapr) package can be used by any type of application (e.g., a Console or ASP.NET Core application) to publish events through Dapr. To be able to receive messages (by subscribing to events), you need to have the [Volo.Abp.AspNetCore.Mvc.Dapr.EventBus](https://www.nuget.org/packages/Volo.Abp.AspNetCore.Mvc.Dapr.EventBus) package installed, and your application should be an ASP.NET Core application.

### Installation

If your application is an ASP.NET Core application and you want to send and receive events, you need to install the [Volo.Abp.AspNetCore.Mvc.Dapr.EventBus](https://www.nuget.org/packages/Volo.Abp.AspNetCore.Mvc.Dapr.EventBus) package as described below:

* Install the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) if you haven't installed it before.
* Open a command line (terminal) in the directory of the `.csproj` file you want to add the `Volo.Abp.AspNetCore.Mvc.Dapr.EventBus` package to.
* Run the `abp add-package Volo.Abp.AspNetCore.Mvc.Dapr.EventBus` command.

If you want to do it manually, install the [Volo.Abp.AspNetCore.Mvc.Dapr.EventBus](https://www.nuget.org/packages/Volo.Abp.AspNetCore.Mvc.Dapr.EventBus) NuGet package to your project and add `[DependsOn(typeof(AbpAspNetCoreMvcDaprEventBusModule))]` to the [ABP module](../Module-Development-Basics.md) class inside your project.

> **If you install the [Volo.Abp.AspNetCore.Mvc.Dapr.EventBus](https://www.nuget.org/packages/Volo.Abp.AspNetCore.Mvc.Dapr.EventBus) package, you don't need to install the [Volo.Abp.EventBus.Dapr](https://www.nuget.org/packages/Volo.Abp.EventBus.Dapr) package, because the first one already has a reference to the latter one.**

If your application is not an ASP.NET Core application, you can't receive events from Dapr, at least with ABP's integration packages (see [Dapr's document](https://docs.dapr.io/developing-applications/building-blocks/pubsub/howto-publish-subscribe/) if you want to receive events in a different type of application). However, you can still publish messages using the [Volo.Abp.EventBus.Dapr](https://www.nuget.org/packages/Volo.Abp.EventBus.Dapr) package. In this case, follow the steps below to install that package to your project:

* Install the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) if you haven't installed it before.
* Open a command line (terminal) in the directory of the `.csproj` file you want to add the `Volo.Abp.EventBus.Dapr` package to.
* Run the `abp add-package Volo.Abp.EventBus.Dapr` command.

If you want to do it manually, install the [Volo.Abp.EventBus.Dapr](https://www.nuget.org/packages/Volo.Abp.EventBus.Dapr) NuGet package to your project and add `[DependsOn(typeof(AbpEventBusDaprModule))]` to the [ABP module](../Module-Development-Basics.md) class inside your project.

### Configuration

You can configure the `AbpDaprEventBusOptions` [options class](../Options.md) for Dapr configuration:

````csharp
Configure<AbpDaprEventBusOptions>(options =>
{
    options.PubSubName = "pubsub";
});
````

Available properties of the `AbpDaprEventBusOptions` class:

* `PubSubName` (optional): The `pubsubName` parameter while publishing messages through the `DaprClient.PublishEventAsync` method. Default value: `pubsub`.

### The ABP Subscription Endpoints

ABP provides the following endpoints to receive events from Dapr:

* `dapr/subscribe`: Dapr uses this endpoint to get a list of subscriptions from the application. ABP automatically returns all the subscriptions for your distributed event handler classes and custom controller actions with the `Topic` attribute.
* `api/abp/dapr/event`: The unified endpoint to receive all the events from Dapr. ABP dispatches the events to your event handlers based on the topic name.

> **Since ABP will call `MapSubscribeHandler` internally, you should not manually call it anymore.** You can use the `app.UseCloudEvents()` middleware in your ASP.NET Core pipeline if you want to support the [CloudEvents](https://cloudevents.io/) standard.

### Usage

#### The ABP Way

You can follow [ABP's distributed event bus documentation](../Distributed-Event-Bus.md) to learn how to publish and subscribe to events in the ABP way. No change required in your application code to use Dapr pub-sub. ABP will automatically subscribe to Dapr for your event handler classes (that implement the `IDistributedEventHandler` interface).

ABP provides `api/abp/dapr/event`

**Example: Publish an event using the `IDistributedEventBus` service**

````csharp
public class MyService : ITransientDependency
{
    private readonly IDistributedEventBus _distributedEventBus;

    public MyService(IDistributedEventBus distributedEventBus)
    {
        _distributedEventBus = distributedEventBus;
    }

    public async Task DoItAsync()
    {
        await _distributedEventBus.PublishAsync(new StockCountChangedEto
        {
            ProductCode = "AT837234",
            NewStockCount = 42 
        });
    }
}
````

**Example: Subscribe to an event by implementing the `IDistributedEventHandler` interface**

````csharp
public class MyHandler : 
    IDistributedEventHandler<StockCountChangedEto>,
    ITransientDependency
{
    public async Task HandleEventAsync(StockCountChangedEto eventData)
    {
        var productCode = eventData.ProductCode;
        // ...
    }
}
````

See [ABP's distributed event bus documentation](../Distributed-Event-Bus.md) to learn the details.

#### Using the Dapr API

In addition to ABP's standard distributed event bus system, you can also use Dapr's API to publish events. 

> If you directly use the Dapr API to publish events, you may not benefit from ABP's standard distributed event bus features, like the outbox/inbox pattern implementation.

**Example: Publish an event using `DaprClient`**

````csharp
public class MyService : ITransientDependency
{
    private readonly DaprClient _daprClient;

    public MyService(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task DoItAsync()
    {
        await _daprClient.PublishEventAsync(
            "pubsub", // pubsub name
            "StockChanged", // topic name 
            new StockCountChangedEto // event data
            {
                ProductCode = "AT837234",
                NewStockCount = 42
            }
        );
    }
}
````

**Example: Subscribe to an event by creating an ASP.NET Core controller**

````csharp
public class MyController : AbpController
{
    [HttpPost("/stock-changed")]
    [Topic("pubsub", "StockChanged")]
    public async Task<IActionResult> TestRouteAsync([FromBody] StockCountChangedEto model)
    {
        HttpContext.ValidateDaprAppApiToken();
        
        // Do something with the event
        return Ok();
    }
}
````

`HttpContext.ValidateDaprAppApiToken()` extension method is provided by ABP to check if the request is coming from Dapr. This is optional. You should configure Dapr to send the App API token to your application if you want to enable the validation. If not configured, `ValidateDaprAppApiToken()` does nothing. See [Dapr's App API Token document](https://docs.dapr.io/operations/security/app-api-token/) for more information. Also see the *AbpDaprOptions* and *Security* sections in this document.

See the [Dapr documentation](https://docs.microsoft.com/en-us/dotnet/architecture/dapr-for-net-developers/publish-subscribe) to learn the details of sending & receiving events with the Dapr API.

## Distributed Lock

> Dapr's distributed lock feature is currently in the Alpha stage and may not be stable yet. It is not suggested to replace ABP's distributed lock with Dapr in that point.

ABP provides a [Distributed Locking](../Distributed-Locking.md) abstraction to control access to a shared resource by multiple applications. Dapr also has a [distributed lock building block](https://docs.dapr.io/developing-applications/building-blocks/distributed-lock/). The [Volo.Abp.DistributedLocking.Dapr](https://www.nuget.org/packages/Volo.Abp.DistributedLocking.Dapr) package makes ABP use Dapr's distributed locking system.

### Installation

Use the ABP CLI to add the [Volo.Abp.DistributedLocking.Dapr](https://www.nuget.org/packages/Volo.Abp.DistributedLocking.Dapr) NuGet package to your project (to the client side):

* Install the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) if you haven't installed it before.
* Open a command line (terminal) in the directory of the `.csproj` file you want to add the `Volo.Abp.DistributedLocking.Dapr` package to.
* Run the `abp add-package Volo.Abp.DistributedLocking.Dapr` command.

If you want to do it manually, install the [Volo.Abp.DistributedLocking.Dapr](https://www.nuget.org/packages/Volo.Abp.DistributedLocking.Dapr) NuGet package to your project and add `[DependsOn(typeof(AbpDistributedLockingDaprModule))]` to the [ABP module](../Module-Development-Basics.md) class inside your project.

### Configuration

You can use the `AbpDistributedLockDaprOptions` options class in the `ConfigureServices` method of [your module](../Module-Development-Basics.md) to configure the Dapr distributed lock:

````csharp
Configure<AbpDistributedLockDaprOptions>(options =>
{
    options.StoreName = "mystore";
});
````

The following options are available:

* **`StoreName`** (required): The store name used by Dapr. Lock key names are scoped in the same store. That means different applications can acquire the same lock name in different stores. Use the same store name for the same resources you want to control the access of.
* `Owner` (optional): The `owner` value used by the `DaprClient.Lock` method. If you don't specify, ABP uses a random value, which is fine in general.
* `DefaultExpirationTimeout` (optional): Default value of the time after which the lock gets expired. Default value: 2 minutes.

### Usage

You can inject and use the `IAbpDistributedLock` service, just like explained in the [Distributed Locking document](../Distributed-Locking.md).

**Example:**

````csharp
public class MyService : ITransientDependency
{
    private readonly IAbpDistributedLock _distributedLock;
    
    public MyService(IAbpDistributedLock distributedLock)
    {
        _distributedLock = distributedLock;
    }
    
    public async Task MyMethodAsync()
    {
        await using (var handle = 
                     await _distributedLock.TryAcquireAsync("MyLockName"))
        {
            if (handle != null)
            {
                // your code that access the shared resource
            }
        }   
    }
}
````

There are two points we should mention about the `TryAcquireAsync` method, as different from ABP's standard usage:

* The `timeout` parameter is currently not used (even if you specify it), because Dapr doesn't support waiting to obtain the lock.
* Dapr uses the expiration timeout system (that means the lock is automatically released after that timeout even if you don't release the lock by disposing the handler). However, ABP's `TryAcquireAsync` method has no such a parameter. Currently, you can set `AbpDistributedLockDaprOptions.DefaultExpirationTimeout` as a global value in your application.

As mentioned first, Dapr's distributed lock feature is currently in the Alpha stage and its API is a candidate to change. You should use it as is if you want, but be ready for the changes in the future. For now, we are recommending to use the [DistributedLock](https://github.com/madelson/DistributedLock) library as explained in ABP's [Distributed Locking document](../Distributed-Locking.md).

## Security

If you are using Dapr, most or all the incoming and outgoing requests in your application pass through Dapr. Dapr uses two kinds of API tokens to secure the communication between your application and Dapr.

### Dapr API Token

> This token is automatically set by default and generally you don't care about it.

The [Enable API token authentication in Dapr](https://docs.dapr.io/operations/security/api-token/) document describes what the Dapr API token is and how it is configured. Please read that document if you want to enable it for your application.

If you enable the Dapr API token, you should send that token in every request to Dapr from your application. `AbpDaprOptions` defines a `DaprApiToken` property as a central point to configure the Dapr API token in your application.

The default value of the `DaprApiToken` property is set from the `DAPR_API_TOKEN` environment variable and that environment variable is set by Dapr when it runs. So, most of the time, you don't need to configure `AbpDaprOptions.DaprApiToken` in your application. However, if you need to configure (or override) it, you can do in the `ConfigureServices` method of your module class as shown in the following code block:

````csharp
Configure<AbpDaprOptions>(options =>
{
    options.DaprApiToken = "...";
});
````

Or you can set it in your `appsettings.json` file:

````json
"Dapr": {
  "DaprApiToken": "..."
}
````

Once you set it, it is used when you inject `DaprClient` or use `IAbpDaprClientFactory`. If you need that value in your application, you can inject `IDaprApiTokenProvider` and use its `GetDaprApiToken()` method.

### App API Token

> Enabling App API token validation is strongly recommended. Otherwise, for example, any client can directly call your event subscription endpoint, and your application acts like an event has occurred (if there is no other security policy in your event subscription endpoint).

The [Authenticate requests from Dapr using token authentication](https://docs.dapr.io/operations/security/app-api-token/) document describes what the App API token is and how it is configured. Please read that document if you want to enable it for your application.

If you enable the App API token, you can validate it to ensure that the request is coming from Dapr. ABP provides useful shortcuts to validate it.

**Example: Validate the App API token in an event handling HTTP API**

````csharp
public class MyController : AbpController
{
    [HttpPost("/stock-changed")]
    [Topic("pubsub", "StockChanged")]
    public async Task<IActionResult> TestRouteAsync([FromBody] StockCountChangedEto model)
    {
        // Validate the App API token!
        HttpContext.ValidateDaprAppApiToken();
        
        // Do something with the event
        return Ok();
    }
}
````

`HttpContext.ValidateDaprAppApiToken()` is an extension method provided by the ABP Framework. It throws an `AbpAuthorizationException` if the token was missing or wrong in the HTTP header (the header name is `dapr-api-token`). You can also inject `IDaprAppApiTokenValidator` and use its methods to validate the token in any service (not only in a controller class).

You can configure `AbpDaprOptions.AppApiToken` if you want to set (or override) the App API token value. The default value is set by the `APP_API_TOKEN` environment variable. You can change it in the `ConfigureServices` method of your module class as shown in the following code block:

````csharp
Configure<AbpDaprOptions>(options =>
{
    options.AppApiToken = "...";
});
````

Or you can set it in your `appsettings.json` file:

````json
"Dapr": {
  "AppApiToken": "..."
}
````

If you need that value in your application, you can inject `IDaprApiTokenProvider` and use its `GetAppApiToken()` method.

## See Also

* [Dapr for .NET Developers](https://docs.microsoft.com/en-us/dotnet/architecture/dapr-for-net-developers/)
* [The Official Dapr Documentation](https://docs.dapr.io/)
