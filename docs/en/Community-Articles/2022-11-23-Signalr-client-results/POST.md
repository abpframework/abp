# Signalr Client results

ASP.NET Core 7 supports [requesting a result from a client](https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-7.0?view=aspnetcore-7.0#signalr), in this article, we will show you how to use client results with the ABP framework.

## Create a SignalR hub

```csharp
public class ChatHub : AbpHub
{
    public async Task<string> WaitForMessage(string connectionId)
    {
        var message = await Clients.Client(connectionId).InvokeAsync<string>("GetMessage");
        return message;
    }
}
```

* ChatHub inherits from `AbpHub` that has useful base properties like `CurrentUser`.
* Define the `WaitForMessage` method to call the client's `GetMessage` method and get the return value.

> Using `InvokeAsync` from a Hub method requires setting the [MaximumParallelInvocationsPerClient](https://learn.microsoft.com/en-us/aspnet/core/signalr/configuration?view=aspnetcore-7.0&tabs=dotnet#configure-server-options) option to a value greater than 1.

## Client

Clients should return results in their `.On(...)` handlers.

### .NET Client

```csharp
hubConnection.On("GetMessage", async () =>
{
    Console.WriteLine("Enter message:");
    var message = await Console.In.ReadLineAsync();
    return message;
});
```

### JavaScript client

```js
connection.on("GetMessage", function () {

    const message = prompt("Enter message:");
    return message;
});
```

## Strongly-typed hubs

We can use strongly-typed instead of `InvokeAsync` by inheriting from `AbpHub<T>` or `Hub<T>`:

```csharp
public interface IClient
{
    Task<string> GetMessage();
}

public class ChatHub : AbpHub<IClient>
{
    public async Task<string> WaitForMessage(string connectionId)
    {
        string message = await Clients.Client(connectionId).GetMessage();
        return message;
    }
}
```

## See also:

* [ABP SignalR-Integration documentation](https://docs.abp.io/en/abp/latest/SignalR-Integration)
* [Microsoft client-results documentation](https://learn.microsoft.com/en-us/aspnet/core/signalr/hubs?view=aspnetcore-7.0#client-results)
