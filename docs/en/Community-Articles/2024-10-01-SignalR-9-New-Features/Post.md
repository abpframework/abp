# ASP.NET Core SignalR New Features (Summary)

In this article, I'll highlight .**NET 9's SignalR updates** for ASP.NET Core 9.0. 

![cover](cover.png)

## Hub Accept a Base Class

In this version, your `Hub` class can now get a base class of a polymorphic class. As you see in the below example; I can send `Animal` to my Hub method. Before we could only pass the derived class like `Cat` and `Dog`.

```csharp
/*** My Base Class is Animal ***/
[JsonPolymorphic]
[JsonDerivedType(typeof(Cat), nameof(Cat))]
[JsonDerivedType(typeof(Dog), nameof(Dog))]
private class Animal
{
    public string Name { get; set; }
}

/*** CAT derived from Animal ***/
private class Cat : Animal
{
    public CatTypes CatType  { get; set; }
}

/*** DOG derived from Animal ***/
private class Dog : Animal
{
    public DogTypes DogType { get; set; }
}


public class MyHub : Hub
{
    /*** We can use the base type Animal here  ***/
    public void Process(Animal animal)
    {
        if (animal is Cat) { ... }
        else if (animal is Dog) { ... }
    }
}

```



## Better Diagnostics and Telemetry

SignalR now integrates more deeply with the .NET Activity API, which is commonly used for distributed tracing. This enhancement is mostly implemented for better monitoring in  [.NET Aspire Dashboard](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/dashboard/overview?tabs=bash#using-the-dashboard-with-net-aspire-projects). To support the improved telemetry feature:

1- Add these packages to your`csproj`:

```xml
<PackageReference Include="OpenTelemetry.Exporter.OpenTelemetryProtocol" Version="1.9.0" />
<PackageReference Include="OpenTelemetry.Extensions.Hosting" Version="1.9.0" />
<PackageReference Include="OpenTelemetry.Instrumentation.AspNetCore" Version="1.9.0" />
```

2- Add the following startup code to your host project:

```csharp
builder.Services.AddSignalR();
/* After AddSignalR use AddOpenTelemetry() */
builder
    .Services
    .AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        if (builder.Environment.IsDevelopment())
        {         
            tracing.SetSampler(new AlwaysOnSampler()); //for dev env monitor all traces
        }

        tracing.AddAspNetCoreInstrumentation();
        tracing.AddSource("Microsoft.AspNetCore.SignalR.Server");
    });

builder.Services.ConfigureOpenTelemetryTracerProvider(tracing => tracing.AddOtlpExporter());
```

And you'll see the SignalR Hub calls at the Aspire Dashboard:

![image-20241002183315780](C:\Users\alper\AppData\Roaming\Typora\typora-user-images\image-20241002183315780.png)



## Trimming and Native AOT Support

In this version, **trimming** and native **Ahead Of Time** compilation are **supported**. It'll improve your app's performance. To support AOT, your serialization object needs to be JSON and you must use the  `System.Text.Json` Source Generator. Also on the server side, [you shouldn't use](https://github.com/dotnet/aspnetcore/issues/56179) `IAsyncEnumerable<T>` and `ChannelReader<T>` where `T` is a ValueType (`struct`) for  Hub method arguments. Besides, [Strongly typed hubs](https://learn.microsoft.com/en-us/aspnet/core/signalr/hubs?view=aspnetcore-8.0#strongly-typed-hubs) aren't supported with Native AOT (`PublishAot`).  And you should use only `Task`, `Task<T>`, `ValueTask`, `ValueTask<T>` for `async` return types.



