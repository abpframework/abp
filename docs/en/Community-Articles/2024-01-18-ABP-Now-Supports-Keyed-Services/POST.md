# ABP Now Supports Keyed Services!

In this post, I describe the new **"keyed service"** support for the dependency injection container, which came with .NET 8.0. Then, I'll show you an example of usage within the ABP Framework.

## What Are Keyed Services?

ASP.NET Core ships with a built-in dependency injection container, which is a pretty basic DI container that supports minimal features a dependency injection container is supposed to have. For that reason, most of the .NET users use third-party containers like [Autofac](https://autofac.org/), or Ninject.

> ABP Framework uses Autofac by default in startup templates, and it supports more features. For example, the built-in DI container does not natively support property injection but Autofac does.

[**Keyed dependency injection (DI) services**](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-8.0#keyed-services) were added to the built-in DI container as a new feature with .NET 8.0. This is an important feature, which allows for registering and retrieving DI services using keys/names.

Prior to this version, you could register multiple services as follows:

```csharp
var builder = WebApplication.CreateBuilder(args);

//service registrations
builder.Services.AddTransient<IMyService, MyServiceOne>();
builder.Services.AddTransient<IMyService, MyServiceTwo>();
builder.Services.AddTransient<IMyService, MyServiceThree>();
```

But without keyed services, you could only retrieve all of the services like this:

```csharp
public class BookService(IEnumerable<IMyService> services) {}
```

Or the last registered one (`MyServiceThree` in our case):

```csharp
public class BookService(IMyService service) {}
```

There was not a simple way to retrieve a specific implementation type directly. With keyed services, now we can register services with a key and then resolve them by the key anywhere we want and retrieve the only one that we want to use.

## ABP Framework Now Supports Keyed Services!

Autofac was already supporting the named/keyed DI services for a long time and with v9.0.0, they made it compatible with the `Microsoft.Extensions.DependencyInjection` package (including the keyed service support).

After the v9.0.0 was released for Autofac, then the ABP Framework Core team immediately updated the `Autofac.Extensions.DependencyInjection` package to `v9.0.0`, made the related changes in its own Autofac package and included in the v8.0.2 release.

You can see the related changes in the following PRs:

* https://github.com/abpframework/abp/pull/18775
* https://github.com/abpframework/abp/pull/18777

## Using Keyed Services with ABP Framework

After the quick background of the process and the feature itself, now let's see the keyed services in action.

First, update the ABP CLI to v8.0.2 with the following command (if your CLI version is newer, no need to apply this):

```bash
dotnet tool update -g Volo.Abp.Cli --version 8.0.2
```

Then, we can create an application template (single-layer) with the following command (MVC as the UI option and EF Core as the DB provider):

```bash
abp new KeyedServiceDemo -t app-nolayers -csf --version 8.0.2
```

After the application is created, we can open it in an IDE and start developing...

**Example**:

Let's assume that we have an interface called `IMyService`, which has three implementation types `MyServiceOne`, `MyServiceTwo` and `MyServiceThree`:

```csharp
public interface IMyService
{
    string GetMessage();
}

public class MyServiceOne : IMyService
{
    public string GetMessage() => "MyServiceOne";
}

public class MyServiceTwo : IMyService
{
    public string GetMessage() => "MyServiceTwo";
}

public class MyServiceThree : IMyService
{
    public string GetMessage() => "MyServiceThree";
}
```

After creating our services, now we can register them in our module class. So, open the module class and add the following lines in the `ConfigureServices` method:

```csharp
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //code abbreviation for clarity...

        context.Services.AddKeyedTransient<IMyService, MyServiceOne>("myserviceone");
        context.Services.AddKeyedTransient<IMyService, MyServiceTwo>("myservicetwo");
        context.Services.AddKeyedTransient<IMyService, MyServiceThree>("myservicethree");
    }
```

* To register a keyed service, you can use one of the `AddKeyedTransient`, `AddKeyedScoped`, or `AddKeyedSingleton` overloads, and provide a key for the registration.
* In the example above, I used `AddKeyedTransient` overload, and registered all of these services as in the `Transient` lifetime.

Then, when you want to retrieve a keyed service, you can use the `[FromKeyedServices(object key)]` attribute:

```csharp
public class NotificationService([FromKeyedServices("myserviceone")] IMyService myServiceOne)
{
    public string Notify() => myServiceOne.GetMessage();
}
```

With this kind of use, you can be certain that not the latest registered service is resolved and instead the keyed service will be resolved, and then you can use it.

Also, you can use the `FromKeyedServicesAttribute` with minimal APIs as follows:

```csharp
public override void OnApplicationInitialization(ApplicationInitializationContext context)
{
    //code abbreviation for clarity...

    app.UseConfiguredEndpoints(endpoints =>
    {
        endpoints.MapGet("/my-service", ([FromKeyedServices("myserviceone")] IMyService myservice) => myservice.GetMessage());
    });
}
```

That's how it's easy to register multiple services in a keyed fashion and resolve them by keys!

## Advanced

In this section, I want to briefly mention some advanced topics (relatively 😀), such as what happens if I register more than one service with the same key, or is there a way to get the keyed services from the `ServiceProvider` etc.

### Registering Multiple Services with the Same Key

Let's assume that you mistakenly registered multiple services with the same key:

```csharp
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //code abbreviation for clarity...

        context.Services.AddKeyedTransient<IMyService, MyServiceOne>("myserviceone");
        context.Services.AddKeyedTransient<IMyService, MyServiceTwo>("myserviceone");
    }
```

In that case, when you try to resolve the dependency with the key, then the last registered one will be used without having a problem:

```csharp
//it will resolve the `MyServiceTwo` service!
endpoints.MapGet("/my-service", ([FromKeyedServices("myserviceone")] IMyService myservice) => myservice.GetMessage());
```

Also, since you used the same key for multiple services, then you can inject the `IEnumerable<IMyService>` to get all services for a certain key and use them:

```csharp
//it will resolve both `MyServiceOne` and `MyServiceTwo` services!
endpoints.MapGet("/my-service", ([FromKeyedServices("myserviceone")] IEnumerable<IMyService> myservices) =>
{
    var sb = new StringBuilder();
    foreach (var myService in myservices)
    {
        sb.AppendLine(myService.GetMessage());
    }

    return sb.ToString();
});
```

### Resolving Keyed Services from ServiceProvider & LazyServiceProvider

You can resolve keyed services by using one of the extension methods (`.GetKeyedServices`, `.GetKeyedService<>`, `.GetRequiredKeyedService<>`, ...):

```csharp
//uses the `MyServiceTwo` service!
var myService = ServiceProvider.GetRequiredKeyedService<IMyService>("myserviceone");
```

On the other hand, resolving keyed services from `LazyServiceProvider` is not supported in v8.0.2, but there is a PR, which you can find at https://github.com/abpframework/abp/pull/18792, it will fix this problem and it will be included in the next version.

### Automatically Registering Keyed Services

Currently, if you want to register a keyed service, you need to do it manually as we see in the previous sections by using one of the overloads (`.AddKeyedTransient`, `.AddKeyedScoped` and `.AddKeyedSingleton`). 

It would be good if we could make this process automatically and not need to manually register services, and for that purpose, I have [created an issue](https://github.com/abpframework/abp/issues/18794) that aims to introduce an attribute, which allows us to automatically register multiple services as keyed services. 

You can [follow the issue](https://github.com/abpframework/abp/issues/18794) if you are considering using keyed services in your application and don't want to register them manually.

## Summary

In this post, I described the new **"keyed service"** support added to the built-in dependency injection container that was released in .NET 8, and wanted to announce it's being supported from v8.0.2 for ABP Framework. It's a really good addition to the built-in dependency injection container and can be pretty useful for certain use-cases.
