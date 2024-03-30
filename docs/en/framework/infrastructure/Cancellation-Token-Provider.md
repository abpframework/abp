# Cancellation Token Provider

A `CancellationToken` enables cooperative cancellation between threads, thread pool work items, or `Task` objects. To handle the possible cancellation of the operation, ABP Framework provides `ICancellationTokenProvider` to obtain the `CancellationToken` itself from the source.

> To get more information about `CancellationToken`, see [Microsoft Documentation](https://docs.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken).

## ICancellationTokenProvider

`ICancellationTokenProvider` is an abstraction to provide `CancellationToken` for different scenarios.

Generally, you should pass the `CancellationToken` as a parameter for your method to use it. With the `ICancellationTokenProvider` you don't need to pass `CancellationToken` for every method. `ICancellationTokenProvider` can be injected with the **dependency injection** and provides the token from it's source.

**Example:**

```csharp
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace MyProject
{
    public class MyService : ITransientDependency
    {
        private readonly ICancellationTokenProvider _cancellationTokenProvider;

        public MyService(ICancellationTokenProvider cancellationTokenProvider)
        {
            _cancellationTokenProvider = cancellationTokenProvider;
        }

        public async Task DoItAsync()
        {
            while (_cancellationTokenProvider.Token.IsCancellationRequested == false)
            {
                // ...
            }
        }
    }
}
```

## Built-in providers

- `NullCancellationTokenProvider`

  The `NullCancellationTokenProvider` is a built in provider and it supply always `CancellationToken.None`.

- `HttpContextCancellationTokenProvider`

  The `HttpContextCancellationTokenProvider` is a built in default provider for ABP Web applications. It simply provides a `CancellationToken` that is source of the web request from the `HttpContext`.

## Implementing the ICancellationTokenProvider

You can easily create your CancellationTokenProvider by creating a class that implements the `ICancellationTokenProvider` interface, as shown below:

```csharp
using System.Threading;

namespace AbpDemo
{
    public class MyCancellationTokenProvider : ICancellationTokenProvider
    {
        public CancellationToken Token { get; }

        private MyCancellationTokenProvider()
        {

        }
    }
}
```
