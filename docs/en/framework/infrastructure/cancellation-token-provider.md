```json
//[doc-seo]
{
    "Description": "Learn how to utilize the `ICancellationTokenProvider` in ABP Framework for efficient cooperative cancellation in your applications."
}
```

# Cancellation Token Provider

A [`CancellationToken`](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) enables cooperative cancellation between threads, thread pool work items, or `Task` objects. To handle the possible cancellation of the operation, ABP provides `ICancellationTokenProvider` to obtain the `CancellationToken` itself from the source.

## When To Use Manual Cancellation Tokens

**ABP automates cancellation token usage wherever possible**. For example, in ASP.NET Core applications, ABP automatically obtains the `CancellationToken` from the `HttpContext.RequestAborted` and uses it in database queries and other cancellable places. So, most of the times, you don't need to deal with `CancellationToken` objects to pass them between methods.

> You do not need to use the `ICancellationTokenProvider` unless you want to add cancellation support in your own logic or to pass a cancellation token to a method outside of the ABP framework.

## ICancellationTokenProvider Service

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

## Built-in Providers

- `HttpContextCancellationTokenProvider`: The **default provider** for ASP.NET Core applications. It simply provides a `CancellationToken` that is source of the web request from the `HttpContext.RequestAborted`.

- `NullCancellationTokenProvider`: A built in provider and it supply always `CancellationToken.None`. It is used if no other providers can be used.


## Implementing a Custom Cancellation Token Provider

You can easily create your `ICancellationTokenProvider` implementation by creating a class that implements the `ICancellationTokenProvider` interface, as shown below:

```csharp
using System.Threading;

namespace AbpDemo
{
    public class MyCancellationTokenProvider
        : ICancellationTokenProvider, 
          ITransientDependency // Can also be singleton or scoped
    {
        public CancellationToken Token { get; } // TODO: Return a cancellation token
    }
}
```
