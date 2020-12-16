# Blazor UI: Localization

Blazor applications can reuse the same `IStringLocalizer<T>` service that is explained in the [localization document](../../Localization.md).

All the localization resources and texts available in the server side are usable in the Blazor application.

## IStringLocalizer

`IStringLocalizer<T>` (`T` is the localization resource class) can be injected in any service or component to use the localization service.

### Razor Components

Use `@inject IStringLocalizer<MyProjectNameResource>` to use the localization in a razor component.

**Example: Localization in a Razor Component**

````csharp
@page "/"
@using MyCompanyName.MyProjectName.Localization
@using Microsoft.Extensions.Localization
@inject IStringLocalizer<MyProjectNameResource> L

<h1>
    @L["LongWelcomeMessage"]
</h1>
````

> `L` is a name that we love and use as the name of a `IStringLocalizer` instance, while you can give any name.

#### The AbpComponentBase

`AbpComponentBase` is a useful base class that you can derive the components from. It has some useful properties/methods you typically need in a component.

The `AbpComponentBase` already defines a base `L` property (of type `IStringLocalizer`). It only requires to set the resource type (in the constructor of the derived class). If you created your application from the ABP's application startup template, then you should have a *YourProjectComponentBase* class in the Blazor project. Inherit components from this class to have the localizer pre-injected.

**Example: Derive from the base component class**

````csharp
@page "/"
@inherits MyProjectNameComponentBase

<h1>
    @L["LongWelcomeMessage"]
</h1>
````

### Other Services

`IStringLocalizer<T>` can be injected into any service.

**Example**

````csharp
public class MyService : ITransientDependency
{
    private readonly IStringLocalizer<TestResource> _localizer;

    public MyService(IStringLocalizer<TestResource> localizer)
    {
        _localizer = localizer;
    }

    public void Foo()
    {
        var str = _localizer["HelloWorld"];
    }
}
````

### Format Arguments

Format arguments can be passed after the localization key. If your message is `Hello {0}, welcome!`, then you can pass the `{0}` argument to the localizer like `_localizer["HelloMessage", "John"]`.

> Refer to the [Microsoft's localization documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization) for details about using the localization.

## See Also

* [Localization](../../Localization.md)