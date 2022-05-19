# Consuming HTTP APIs from a .NET Client Using ABP's Client Proxy System

In this article, I will show how to consume your HTTP APIs from a .NET application using ABP's [dynamic](https://docs.abp.io/en/abp/latest/API/Dynamic-CSharp-API-Clients) and [static](https://docs.abp.io/en/abp/latest/API/Static-CSharp-API-Clients) client-side proxy systems. I will start by creating a new project and consume the HTTP APIs from a .NET console application using dynamic client proxies. Then I will switch to static client proxies. Finally, I will glance at the differences and similarities between static and dynamic generic proxies.

## Create a new ABP application with the ABP CLI
Firstly create a new solution via [ABP CLI](https://docs.abp.io/en/abp/latest/CLI):

````shell
abp new Acme.BookStore
````

> See ABP's [Getting Started document](https://docs.abp.io/en/abp/latest/Getting-Started-Setup-Environment?UI=MVC&DB=EF&Tiered=No) to learn how to create and run your application, if you haven't done it before.

## Create application service interface
I will start by creating an application service and expose it as HTTP API to be consumed by remote clients. First, define an interface for the application service; Create an `IBookAppService` interface in the `Books` folder (namespace) of the `Acme.BookStore.Application.Contracts` project:

````csharp
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Books
{
    public interface IBookAppService : IApplicationService
    {
        Task<PagedResultDto<BookDto>> GetListAsync();
    }
}
````

Also add a `BookDto` class inside the same `Books` folder:

```csharp
using System;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Books
{
    public class BookDto
    {
        public string Name { get; set; }
        public string AuthorName { get; set; }
        public float Price { get; set; }
    }
}
```

## Implement the application service
It is time to implement the `IBookAppService` interface. Create a new class, named `BookAppService` in the `Books` namespace (folder) of the `Acme.BookStore.Application` project:

```csharp
using Acme.BookStore.Permissions;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Books
{
    public class BookAppService : ApplicationService, IBookAppService
    {
        public Task<PagedResultDto<BookDto>> GetListAsync()
        {
            var bookDtos = new List<BookDto>()
            {
                new BookDto(){ Name = "Anna Karenina", AuthorName ="Tolstoy", Price = 50},
                new BookDto(){ Name = "Crime and Punishment", AuthorName ="Dostoevsky", Price = 60},
                new BookDto(){ Name = "Mother", AuthorName ="Gorki", Price = 70}
            };
            return Task.FromResult(new PagedResultDto<BookDto>(
               bookDtos.Count,
               bookDtos
           ));
        }
    }
}
```
It simply returns a list of books. You probably want to get the books from a database, but it doesn't matter for this article. If you want it, you can fully implement [this tutorial](https://docs.abp.io/en/abp/latest/Tutorials/Part-1?UI=MVC&DB=EF).

## Consume the app service from the console application
The startup solution comes with an example .NET console application (`Acme.BookStore.HttpApi.Client.ConsoleTestApp`) that is fully configured to consume your HTTP APIs remotely. Change `ClientDemoService` as shown the following in the `Acme.BookStore.HttpApi.Client.ConsoleTestApp` project (it is under the `test` folder).

```csharp
using Acme.BookStore.Books;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;

namespace Acme.BookStore.HttpApi.Client.ConsoleTestApp;

public class ClientDemoService : ITransientDependency
{
    private readonly IBookAppService _bookAppService;

    public ClientDemoService(IBookAppService bookAppService )
    {
        _bookAppService = bookAppService;
    }

    public async Task RunAsync()
    {
        var listOfBooks = await _bookAppService.GetListAsync(new PagedAndSortedResultRequestDto());
        Console.WriteLine($"Books: {string.Join(", ", listOfBooks.Items.Select(p => p.Name).ToList())}");
    }
}
```

We are basically injecting the `IBookAppService` interface to consume the remote service. ABP handles all the details (performing HTTP request, deserializing the resulting JSON object, etc) for us.

You can run the application to see the output:

````
Books: Anna Karenina, Crime and Punishment, Mother
````

## Convert application to use static client proxies
Before showing you how to use static client proxies instead of dynamic client proxies, I ask you to talk differences between both approaches. Their similarities, advantages and disadvantages to each other.

**Benefits (both valid for dynamic and static proxies):**

* Maps C# method calls to remote server HTTP calls by considering the HTTP method, route, query string parameters, request payload and other details.
* Authenticates the HTTP Client by adding access token to the HTTP header.
* Serializes to and deserialize from JSON.
* Handles HTTP API versioning.
* Add correlation id, current tenant id and the current culture to the request.
* Properly handles the error messages sent by the server and throws proper exceptions.

**Differences of dynamic and static proxies:**

Static generic proxies provide **better performance** because it doesn't need to run on runtime, but you should **re-generate** once changing the API endpoint definition. Dynamic generic proxies don't need re-generate again because it works on the runtime but it has a slight performance penalty.

The [application startup template](https://docs.abp.io/en/abp/latest/Startup-Templates/Application) comes pre-configured for the **dynamic** client proxy generation, in the `HttpApi.Client` project. If you want to switch to the **static** client proxies, you should change `context.Services.AddHttpClientProxies` to `context.Services.AddStaticHttpClientProxies` in the module class of your `HttpApi.Client` project:

```csharp
public class BookStoreHttpApiClientModule : AbpModule
{
    public const string RemoteServiceName = "Default";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
       // Other configurations...

        context.Services.AddStaticHttpClientProxies(
            typeof(BookStoreApplicationContractsModule).Assembly,
            RemoteServiceName
        );
    }
}
```

`AddStaticHttpClientProxies` method gets an assembly, finds all service interfaces in the given assembly, and prepares for static client proxy generation.


Now you're ready to generate the client proxy code by running the following command in the root folder of your client project while your server-side project is running:

````bash
abp generate-proxy -t csharp -u http://localhost:44397/
````

> The URL (`-u` parameter's value) might be different for your application. It should be server's root URL.

You should have seen the generated files under the selected folder:

![files of the static proxy](./static-proxy.png)

Now you can run the console client application again. You should see the same output:

````
Books: Anna Karenina, Crime and Punishment, Mother
````

## Add authorization
ABP Framework provides an [authorization system](https://docs.abp.io/en/abp/latest/Authorization) based on the [ASP.NET Core's authorization infrastructure](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/introduction). We can define permissions and restrict access to some of our application's functionalities, so only the allowed users/clients can use these functionalities. Here, I will define a permission to be able to get the list of books.

Under `Acme.BookStore.Application.Contracts` open `BookStorePermissions` and paste the below code:
```csharp
namespace Acme.BookStore.Permissions;

public static class BookStorePermissions
{
    public const string GroupName = "BookStore";

    public static class Books
    {
        public const string Default = GroupName + ".Books";
    }

}
```
Also need to change `BookStorePermissionDefinitionProvider` under the same folder and project as follows.
```csharp
using Acme.BookStore.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

public class BookStorePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var bookStoreGroup = context.AddGroup(BookStorePermissions.GroupName);
        bookStoreGroup.AddPermission(BookStorePermissions.Books.Default);
    }
}
```
We can now add `[Authorize(BookStorePermissions.Books.Default)]` attribute to the `BookAppService` class:

````csharp
[Authorize(BookStorePermissions.Books.Default)]
public class BookAppService : ApplicationService, IBookAppService
{
    ...
}
````

If you now run the server, then run the console client application, you will see the following error on the console application:

````
Authorization failed. These requirements were not met:
PermissionRequirement: BookStore.Books
AuthenticationScheme: Identity.Application was forbidden.
Request finished HTTP/1.1 GET https://localhost:44397/api/app/book?SkipCount=0&MaxResultCount=10&api-version=1.0 - - - 403 0 - 156.9766ms
````

To fix the problem, we should grant permission for the admin user. We are granting permission to the admin user because the console application is configured to use Resource Owner Password Grant Flow. That means the client application is consuming services on behalf of the admin user. You can see the configuration in the `appsettings.json` file of the console application.

**TODO: Show how to give permission, with screenshots**

### Further Reading
In this tutorial, I explained how you can create an example project and apply static client proxy instead of dynamic client proxy. Also summarized the differences between both approaches. If you want to get more information, you can read the following documents:

* [Static C# API Client Proxies](https://docs.abp.io/en/abp/latest/API/Static-CSharp-API-Clients)
* [Dynamic C# API Client Proxies](https://docs.abp.io/en/abp/latest/API/Dynamic-CSharp-API-Clients)
* [Web Application Development Tutorial](https://docs.abp.io/en/abp/latest/Tutorials/Part-1?UI=MVC&DB=EF)
