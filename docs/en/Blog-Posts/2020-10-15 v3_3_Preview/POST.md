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

TODO

### Automatic Validation for AntiForgery Token for HTTP APIs

Starting with the version 3.3, all your HTTP API endpoints are **automatically protected** against CSRF attacks, unless you disable it for your application.

[See the documentation](https://github.com/abpframework/abp/blob/dev/docs/en/CSRF-Anti-Forgery.md) to understand why you need it and how ABP Framework solves the problem.

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

## What's new with the ABP Commercial 3.3

TODO

## Feedback

Please try the ABP Framework 3.3.0 RC and [provide feedback](https://github.com/abpframework/abp/issues/new) to help us to release a more stable version. The planned release date for the [3.3.0 final](https://github.com/abpframework/abp/milestone/44) version is October 27th.