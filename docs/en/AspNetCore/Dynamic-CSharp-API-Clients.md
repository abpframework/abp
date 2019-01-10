# Dynamic C# API Clients

ABP can dynamically create C# API client proxies to call remote HTTP services (REST APIs). In this way, you don't need to deal with `HttpClient` and other low level HTTP features to call remote services and get results.

## Service Interface

Your service/controller should implement an interface that is shared between the server and the client. So, first define a service interface in a shared library project. Example:

````csharp
public interface IBookAppService : IApplicationService
{
    Task<List<BookDto>> GetListAsync();
}
````

Your interface should implement the `IRemoteService` interface to be automatically discovered. Since the `IApplicationService` inherits the `IRemoteService` interface, the `IBookAppService` above satisfies this condition.

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
    typeof(BookStoreApplicationModule) //contains the application service interfaces
    )]
public class MyClientAppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //Create dynamic client proxies
        context.Services.AddHttpClientProxies(
            typeof(BookStoreApplicationModule).Assembly
        );
    }
}
````

`AddHttpClientProxies` method gets an assembly, finds all service interfaces in the given assembly, creates and registers proxy classes.

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

See the "RemoteServiceOptions" section below for more detailed configuration.

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

### RemoteServiceOptions

`RemoteServiceOptions` is automatically set from the `appsettings.json` by default. Alternatively, you can use `Configure` method to set or override it. Example:

````csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.Configure<RemoteServiceOptions>(options =>
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
    typeof(BookStoreApplicationModule).Assembly,
    remoteServiceName: "BookStore"
);
````

`remoteServiceName` parameter matches the service endpoint configured via `RemoteServiceOptions`. If the `BookStore` endpoint is not defined then it fallbacks to the `Default` endpoint.

### As Default Services

When you create a service proxy for `IBookAppService`, you can directly inject the `IBookAppService` to use the proxy client (as shown in the usage section). You can pass `asDefaultServices: false` to the `AddHttpClientProxies` method to disable this feature.

````csharp
context.Services.AddHttpClientProxies(
    typeof(BookStoreApplicationModule).Assembly,
    asDefaultServices: false
);
````

If you disable `asDefaultServices`, you can only use `IHttpClientProxy<T>` interface to use the client proxies (see the related section above).

Using `asDefaultServices: false` may only be needed if your application has multiple implementation of the service, so you want to distinguish the HTTP client proxy and do not want to override the other implementation.