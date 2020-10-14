# ABP Framework 3.3 RC Has Been Published

We have released the [ABP Framework](https://abp.io/) (and the [ABP Commercial](https://commercial.abp.io/)) `3.3.0-rc.1` today. This blog post introduces the new features and important changes of this new version.

## What's new with the ABP Framework 3.3

### The Blazor UI

We had released an experimental early preview version of the Blazor UI with the [previous version](https://blog.abp.io/abp/ABP-Framework-ABP-Commercial-3.2-RC-With-The-New-Blazor-UI). In this version, we've completed most of the fundamental infrastructure features and the application modules (like identity and tenant management).

It currently has almost the same functionalities with the other UI types (Angular & MVC / Razor Pages).

**Example screenshot**: User management page of the Blazor UI

![abp-blazor-ui](abp-blazor-ui.png)

> We've adapted the [Lepton Theme](https://commercial.abp.io/themes) for the ABP Commercial, see the related section below.

We are still working on the fundamentals. So, the next version may introduce breaking changes for the Blazor UI, while we will work to keep them with minimal effect on your application code.

#### Breaking Changes on the Blazor UI

TODO...

### Automatic Validation for AntiForgery Token for HTTP APIs

Starting with the version 3.3, all your HTTP API endpoints are **automatically protected** against CSRF attacks, unless you disable it for your application.

[See the documentation](https://github.com/abpframework/abp/blob/dev/docs/en/CSRF-Anti-Forgery.md) to understand why you need it and how ABP Framework solves the problem.

### Rebus Integration Package for the Distributed Event Bus

[Rebus](https://github.com/rebus-org/Rebus) describes itself as "Simple and lean service bus implementation for .NET". There are a lot of integration packages like RabbitMQ and Azure Service Bus for the Rebus. The new [Volo.Abp.EventBus.Rebus](https://www.nuget.org/packages/Volo.Abp.EventBus.Rebus) package allows you to use the Rebus as the [distributed event bus](https://docs.abp.io/en/abp/latest/Distributed-Event-Bus) for the ABP Framework.

See [the documentation](https://github.com/abpframework/abp/blob/dev/docs/en/Distributed-Event-Bus-Rebus-Integration.md) to learn how to use Rebus with the ABP Framework.

### Async Repository LINQ Extension Methods

You have a problem when you want to use **Async LINQ Extension Methods** (e.g. `FirstOrDefaultAsync(...)`) in your **domain** and **application** layers. These async methods are **not included in the standard LINQ extension methods**. They are defined by the [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore) NuGet package (see [the code](https://github.com/dotnet/efcore/blob/main/src/EFCore/Extensions/EntityFrameworkQueryableExtensions.cs)). So, you normally need to reference to the **Microsoft.EntityFrameworkCore** package to be able to use these async methods.

If you don't want to depend on the EF Core in your business layer, then ABP Framework provides the `IAsyncQueryableExecuter` service to execute your queries asynchronously without depending to the EF Core package. You can see [the documentation](https://docs.abp.io/en/abp/latest/Repositories#option-3-iasyncqueryableexecuter) to get more information for this service.

ABP Framework version 3.3 takes this one step further and allows you to directly execute the async LINQ extension methods on the `IRepository` interface.

**Example: Use `CountAsync` and `FirstOrDefaultAsync` methods on the repositories**

````csharp
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace MyCompanyName.MyProjectName
{
    public class BookAppService : ApplicationService, IBookAppService
    {
        private readonly IRepository<Book, Guid> _bookRepository;

        public BookAppService(IRepository<Book, Guid> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task DemoAsync()
        {
            var countAll = await _bookRepository
                .CountAsync();

            var count = await _bookRepository
                .CountAsync(x => x.Name.Contains("A"));

            var book1984 = await _bookRepository
                .FirstOrDefaultAsync(x => x.Name == "1984");            
        }
    }
}
````

All the standard LINQ methods are supported: *AllAsync, AnyAsync, AverageAsync, ContainsAsync, CountAsync, FirstAsync, FirstOrDefaultAsync, LastAsync, LastOrDefaultAsync, LongCountAsync, MaxAsync, MinAsync, SingleAsync, SingleOrDefaultAsync, SumAsync, ToArrayAsync, ToListAsync*.

This approach still has a limitation. You need to execute the extension method directly on the repository object. For example, the usage below is **not supported**:

````csharp
var count = await _bookRepository.Where(x => x.Name.Contains("A")).CountAsync();
````

This is because the object returned from the `Where` method is not a repository object, it is a standard `IQueryable`. In such cases, you can still use the `IAsyncQueryableExecuter`:

````csharp
var count = await AsyncExecuter.CountAsync(
    _bookRepository.Where(x => x.Name.Contains("A"))
);
````

`AsyncExecuter` has all the standard extension methods, so you don't have any restriction here. See [the repository documentation](https://docs.abp.io/en/abp/latest/Repositories#iqueryable-async-operations) for all the options you have.

> ABP Framework does it's best to not depend on the EF Core yet still be able to use the async LINQ extension methods. However, it there is no problem to depend on the EF Core for your application, you can add the Microsoft.EntityFrameworkCore NuGet package and use it natively.

### Stream Support for the Application Service Methods

[Application services](https://docs.abp.io/en/abp/latest/Application-Services) are consumed by clients and the parameters and return values (typically [Data Transfer Objects](https://docs.abp.io/en/abp/latest/Data-Transfer-Objects)). In case of the client is a remote application, then these objects should be serialized & deserialized.

Until the version 3.3, we hadn't suggest to use the `Stream` in the application service contracts, since it is not serializable/deserializable. However, with the version 3.3, ABP Framework properly supports this scenario by introducing the new `IRemoteStreamContent` interface.

Example: An application service that can get or return streams

````csharp
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace MyProject.Test
{
    public interface ITestAppService : IApplicationService
    {
        Task Upload(Guid id, IRemoteStreamContent streamContent);
        Task<IRemoteStreamContent> Download(Guid id);
    }
}
````

The implementation can be like shown below:

````csharp
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace MyProject.Test
{
    public class TestAppService : ApplicationService, ITestAppService
    {
        public Task<IRemoteStreamContent> Download(Guid id)
        {
            var fs = new FileStream("C:\\Temp\\" + id + ".blob", FileMode.OpenOrCreate);
            return Task.FromResult(
                (IRemoteStreamContent) new RemoteStreamContent(fs) {
                    ContentType = "application/octet-stream" 
                }
            );
        }

        public async Task Upload(Guid id, IRemoteStreamContent streamContent)
        {
            using (var fs = new FileStream("C:\\Temp\\" + id + ".blob", FileMode.Create))
            {
                await streamContent.GetStream().CopyToAsync(fs);
                await fs.FlushAsync();
            }
        }
    }
}
````

> This is just a demo code. Do it better on a production code :)

Thanks to [@alexandru-bagu](https://github.com/alexandru-bagu) for the great contribution!

### Other Changes

* Upgraded all the .NET Core / ASP.NET Core related packages to the version 3.1.8. If you have additional dependencies to the .NET Core / ASP.NET Core related packages, we suggest you to updates your packages to the version 3.1.8 to have the latest bug and security fixes published by Microsoft.
* The blogging module now uses the [BLOB Storing](https://docs.abp.io/en/abp/latest/Blob-Storing) system to store images & files of the blog posts. If you are using this module, then you need to manually migrate the local files to the BLOB Storing system after the upgrade.
* The Angular UI is now redirecting to the profile management page of the MVC UI instead of using its own UI, if you've configured the authorization code flow (which is default since the version 3.2.0).

## What's new with the ABP Commercial 3.3

TODO

## Feedback

Please try the ABP Framework 3.3.0 RC and [provide feedback](https://github.com/abpframework/abp/issues/new) to help us to release a more stable version. The planned release date for the [3.3.0 final](https://github.com/abpframework/abp/milestone/44) version is October 27th.