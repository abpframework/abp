# ABP Dapr Integration

> This document assumes that you are already familiar with [Dapr](https://dapr.io/) and you want to use it in your ABP based applications.

[Dapr](https://dapr.io/) (Distributed Application Runtime) provides APIs that simplify microservice connectivity. It is an open source project that is mainly backed by Microsoft. It is also a CNCF (Cloud Native Computing Foundation) project and trusted by community.

ABP and Dapr has some intersecting features like service-to-service communication, distributed message bus and distributed locking. However their purposes are totally different. ABP's goal is to provide an end-to-end developer experience by offering an opinionated architecture and providing the necessary infrastructure libraries, reusable modules and tools to implement that architecture properly. Dapr's purpose, on the other hand, is to provide a runtime to decouple common microservice communication patterns from your application logic.

ABP and Dapr can perfectly work together in the same application. ABP offers some packages to provide better integration where Dapr features intersect with ABP. You can use other Dapr features with no ABP integration packages based on [its own documentation](https://docs.dapr.io/).

## ABP Dapr Integration Packages

ABP provides the following NuGet packages for the Dapr integration:

* [Volo.Abp.Dapr](https://www.nuget.org/packages/Volo.Abp.Dapr): The main Dapr integration package. All other packages depend on this package.
* [Volo.Abp.EventBus.Dapr](https://www.nuget.org/packages/Volo.Abp.EventBus.Dapr): Implements ABP's distributed event bus with Dapr's [publish & subscribe](https://docs.dapr.io/developing-applications/building-blocks/pubsub/) building block. With this package, you can send events, but can not receive.
* [Volo.Abp.AspNetCore.Mvc.Dapr.EventBus](https://www.nuget.org/packages/Volo.Abp.AspNetCore.Mvc.Dapr.EventBus): Provides the endpoints to receive events from Dapr's [publish & subscribe](https://docs.dapr.io/developing-applications/building-blocks/pubsub/) building block. Use this package to send and receive events.
* [Volo.Abp.Http.Client.Dapr](https://www.nuget.org/packages/Volo.Abp.Http.Client.Dapr): Integration package for ABP's [dynamic](../API/Dynamic-CSharp-API-Clients.md) and [static](../API/Static-CSharp-API-Clients.md) C# API Client Proxies systems with Dapr's [service invocation](https://docs.dapr.io/developing-applications/building-blocks/service-invocation/service-invocation-overview/) building block.
* [Volo.Abp.DistributedLocking.Dapr](https://www.nuget.org/packages/Volo.Abp.DistributedLocking.Dapr): Uses Dapr's [distributed lock](https://docs.dapr.io/developing-applications/building-blocks/distributed-lock/) building block for [distributed locking](Distributed-Locking.md) service of the ABP Framework.

In the following sections, we will see how to use these packages to use Dapr in your ABP based solutions.

## AbpDaprOptions

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

## IAbpDaprClientFactory

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