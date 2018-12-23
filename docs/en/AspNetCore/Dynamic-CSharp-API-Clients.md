# Dynamic C# API Clients

ABP can dynamically create C# API client proxies to call remote HTTP services (REST APIs). In this way, you don't need to deal with `HttpClient` and other low level HTTP features to call remote services and get results.

## Service Interface

Your service/controller should implement an interface that is shared between the server and the client. So, first define a service interface in a shared library project. Example:

````csharp
public interface IBookService : IApplicationService
{
    Task<List<BookDto>> GetListAsync();
}
````

Your interface should implement the `IRemoteService` interface. Since the `IApplicationService` inherits the `IRemoteService` interface, the `IBookService` above satisfies this condition.

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
        //Configure remote end point
        context.Services.Configure<RemoteServiceOptions>(options =>
        {
            options.RemoteServices.Default =
                new RemoteServiceConfiguration("http://localhost:53929/");
        });

        //Create dynamic client proxies
        context.Services.AddHttpClientProxies(
            typeof(BookStoreApplicationModule).Assembly
        );
    }
}
````

`RemoteServiceOptions` is used to configure endpoints for remote services (This example sets the default endpoint while you can have different service endpoints used by different clients. See the "Multiple Remote Service Endpoint" section).

`AddHttpClientProxies` method gets an assembly, finds all service interfaces in the given assembly, creates and registers proxy classes.

## Usage

It's straightforward to use. Just inject the service interface in the client application code:

````csharp
public class MyService : ITransientDependency
{
    private readonly IBookService _bookService;

    public MyService(IBookService bookService)
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

This sample injects the `IBookService` service interface defined above. The dynamic client proxy implementation makes an HTTP call whenever a service method is called by the client.

## Configuration Details

### RemoteServiceOptions

While you can configure `RemoteServiceOptions` as the example shown above, you can let it to read from your `appsettings.json` file. Add a `RemoteServices` section to your `appsettings.json` file:

````json
{
  "RemoteServices": {
    "Default": {
      "BaseUrl": "http://localhost:53929/"
    } 
  } 
}
````

Then you can pass your `IConfigurationRoot` instance directly to the `Configure<RemoteServiceOptions>()` method as shown below:

````csharp
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

context.Services.Configure<RemoteServiceOptions>(configuration);
````

This approach is useful since it's easy to change the configuration later without touching to the code.

#### Multiple Remote Service Endpoint

The examples above have configured the "Default" remote service endpoint. You may have different endpoints for different services (as like in a microservice approach where each microservice has different endpoint). In this case, you can add other endpoints to your configuration file:

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

See the next section to learn how to use this new endpoint.

### AddHttpClientProxies Method

`AddHttpClientProxies` method can get an additional parameter for the remote service name. Example:

````csharp
context.Services.AddHttpClientProxies(
    typeof(BookStoreApplicationModule).Assembly,
    remoteServiceName: "BookStore"
);
````

`remoteServiceName` parameter matches the service endpoint configured via `RemoteServiceOptions`. If the `BookStore` endpoint is not defined then it fallbacks to the `Default` endpoint.