# Dynamic C# API Client Proxies

ABP can dynamically create C# API client proxies to call your remote HTTP services (REST APIs). In this way, you don't need to deal with `HttpClient` and other low level details to call remote services and get results.

Dynamic C# proxies automatically handle the following stuff for you;

* Maps C# **method calls** to remote server **HTTP calls** by considering the HTTP method, route, query string parameters, request payload and other details.
* **Authenticates** the HTTP Client by adding access token to the HTTP header.
* **Serializes** to and deserialize from JSON.
* Handles HTTP API **versioning**.
* Add **correlation id**, current **tenant** id and the current **culture** to the request.
* Properly **handles the error messages** sent by the server and throws proper exceptions.

This system can be used by any type of .NET client to consume your HTTP APIs.

## Service Interface

Your service/controller should implement an interface that is shared between the server and the client. So, first define a service interface in a shared library project, typically in the `Application.Contracts` project if you've created your solution using the startup templates.

Example:

````csharp
public interface IBookAppService : IApplicationService
{
    Task<List<BookDto>> GetListAsync();
}
````

> Your interface should implement the `IRemoteService` interface to be automatically discovered. Since the `IApplicationService` inherits the `IRemoteService` interface, the `IBookAppService` above satisfies this condition.

Implement this class in your service application. You can use [auto API controller system](Auto-API-Controllers.md) to expose the service as a REST API endpoint.

## Client Proxy Generation

First, add [Volo.Abp.Http.Client](https://www.nuget.org/packages/Volo.Abp.Http.Client) nuget package to your client project:

````
Install-Package Volo.Abp.Http.Client
````

Then add `AbpHttpClientModule` dependency to your module:

````csharp
[DependsOn(typeof(AbpHttpClientModule))] //add the dependency
public class MyClientAppModule : AbpModule
{
}
````

Now, it's ready to create the client proxies. Example:

````csharp
[DependsOn(
    typeof(AbpHttpClientModule), //used to create client proxies
    typeof(BookStoreApplicationContractsModule) //contains the application service interfaces
    )]
public class MyClientAppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //Create dynamic client proxies
        context.Services.AddHttpClientProxies(
            typeof(BookStoreApplicationContractsModule).Assembly
        );
    }
}
````

`AddHttpClientProxies` method gets an assembly, finds all service interfaces in the given assembly, creates and registers proxy classes.

> The startup templates already comes pre-configured for the client proxy generation, in the `HttpApi.Client` project.

### Endpoint Configuration

`RemoteServices` section in the `appsettings.json` file is used to get remote service address by default. Simplest configuration is shown below:

````
{
  "RemoteServices": {
    "Default": {
      "BaseUrl": "http://localhost:53929/"
    } 
  } 
}
````

See the "AbpRemoteServiceOptions" section below for more detailed configuration.

## Usage

It's straightforward to use. Just inject the service interface in the client application code:

````csharp
public class MyService : ITransientDependency
{
    private readonly IBookAppService _bookService;

    public MyService(IBookAppService bookService)
    {
        _bookService = bookService;
    }

    public async Task DoIt()
    {
        var books = await _bookService.GetListAsync();
        foreach (var book in books)
        {
            Console.WriteLine($"[BOOK {book.Id}] Name={book.Name}");
        }
    }
}
````

This sample injects the `IBookAppService` service interface defined above. The dynamic client proxy implementation makes an HTTP call whenever a service method is called by the client.

### IHttpClientProxy Interface

While you can inject `IBookAppService` like above to use the client proxy, you could inject `IHttpClientProxy<IBookAppService>` for a more explicit usage. In this case you will use the `Service` property of the `IHttpClientProxy<T>` interface.

## Configuration

### AbpRemoteServiceOptions

`AbpRemoteServiceOptions` is automatically set from the `appsettings.json` by default. Alternatively, you can configure it in the `ConfigureServices` method of your [module](../Module-Development-Basics.md) to set or override it. Example:

````csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.Configure<AbpRemoteServiceOptions>(options =>
    {
        options.RemoteServices.Default =
            new RemoteServiceConfiguration("http://localhost:53929/");
    });
    
    //...
}
````

### Multiple Remote Service Endpoints

The examples above have configured the "Default" remote service endpoint. You may have different endpoints for different services (as like in a microservice approach where each microservice has different endpoints). In this case, you can add other endpoints to your configuration file:

````json
{
  "RemoteServices": {
    "Default": {
      "BaseUrl": "http://localhost:53929/"
    },
    "BookStore": {
      "BaseUrl": "http://localhost:48392/"
    } 
  } 
}
````

`AddHttpClientProxies` method can get an additional parameter for the remote service name. Example:

````csharp
context.Services.AddHttpClientProxies(
    typeof(BookStoreApplicationContractsModule).Assembly,
    remoteServiceConfigurationName: "BookStore"
);
````

`remoteServiceConfigurationName` parameter matches the service endpoint configured via `AbpRemoteServiceOptions`. If the `BookStore` endpoint is not defined then it fallbacks to the `Default` endpoint.

### As Default Services

When you create a service proxy for `IBookAppService`, you can directly inject the `IBookAppService` to use the proxy client (as shown in the usage section). You can pass `asDefaultServices: false` to the `AddHttpClientProxies` method to disable this feature.

````csharp
context.Services.AddHttpClientProxies(
    typeof(BookStoreApplicationContractsModule).Assembly,
    asDefaultServices: false
);
````

Using `asDefaultServices: false` may only be needed if your application has already an implementation of the service and you do not want to override/replace the other implementation by your client proxy.

> If you disable `asDefaultServices`, you can only use `IHttpClientProxy<T>` interface to use the client proxies. See the *IHttpClientProxy Interface* section above.

### Retry/Failure Logic & Polly Integration

If you want to add retry logic for the failing remote HTTP calls for the client proxies, you can configure the `AbpHttpClientBuilderOptions` in the `PreConfigureServices` method of your module class.

**Example: Use the [Polly](https://github.com/App-vNext/Polly) library to re-try 3 times on a failure**

````csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<AbpHttpClientBuilderOptions>(options =>
    {
        options.ProxyClientBuildActions.Add((remoteServiceName, clientBuilder) =>
        {
            clientBuilder.AddTransientHttpErrorPolicy(policyBuilder =>
                policyBuilder.WaitAndRetryAsync(
                    3,
                    i => TimeSpan.FromSeconds(Math.Pow(2, i))
                )
            );
        });
    });
}
````

This example uses the [Microsoft.Extensions.Http.Polly](https://www.nuget.org/packages/Microsoft.Extensions.Http.Polly) package. You also need to import the `Polly` namespace (`using Polly;`) to be able to use the `WaitAndRetryAsync` method.