# Features

ABP Feature system is used to **enable**, **disable** or **change the behavior** of the application features **on runtime**.

The runtime value for a feature is generally a `boolean` value, like `true` (enabled) or `false` (disabled). However, you can get/set **any kind** of value for feature.

Feature system was originally designed to control the tenant features in a **multi-tenant** application. However, it is **extensible** and capable of determining the features by any condition.

> The feature system is implemented with the [Volo.Abp.Features](https://www.nuget.org/packages/Volo.Abp.Features) NuGet package. Most of the times you don't need to manually [install it](https://abp.io/package-detail/Volo.Abp.Features) since it comes pre-installed with the [application startup template](Startup-Templates/Application.md).

## Checking for the Features

Before explaining to define features, let's see how to check a feature value in your application code.

### RequiresFeature Attribute

`[RequiresFeature]` attribute (defined in the `Volo.Abp.Features` namespace) is used to declaratively check if a feature is `true` (enabled) or not. It is a useful shortcut for the `boolean` features.

**Example: Check if the "PDF Reporting" feature enabled**

```csharp
public class ReportingAppService : ApplicationService, IReportingAppService
{
    [RequiresFeature("MyApp.PdfReporting")]
    public async Task<PdfReportResultDto> GetPdfReportAsync()
    {
        //TODO...
    }
}
```

* `RequiresFeature(...)` simply gets a feature name to check if it is enabled or not. If not enabled, an authorization [exception](Exception-Handling.md) is thrown and a proper response is returned to the client side.
* `[RequiresFeature]` can be used for a **method** or a **class**. When you use it for a class, all the methods of that class require the given feature.
* `RequiresFeature` may get multiple feature names, like `[RequiresFeature("Feature1", "Feature2")]`. In this case ABP checks if any of the features enabled. Use `RequiresAll` option, like `[RequiresFeature("Feature1", "Feature2", RequiresAll = true)]` to force to check all of the features to be enabled.
* Multiple usage of `[RequiresFeature]` attribute is supported for a method or class. ABP check checks all of them in that case.

> Feature name can be any arbitrary string. It should be unique for a feature.

#### About the Interception

ABP Framework uses the interception system to make the `[RequiresFeature]` attribute working. So, it can work with any class (application services, controllers...) that is injected from the [dependency injection](Dependency-Injection.md).

However, there are **some rules should be followed** in order to make it working;

* If you are **not injecting** the service over an interface (like `IMyService`), then the methods of the service must be `virtual`. Otherwise, [dynamic proxy / interception](Dynamic-Proxying-Interceptors.md) system can not work.
* Only `async` methods (methods returning a `Task` or `Task<T>`) are intercepted.

> There is an exception for the **controller and razor page methods**. They **don't require** the following the rules above, since ABP Framework uses the action/page filters to implement the feature checking in this case.

### IFeatureChecker Service

`IFeatureChecker` allows to check a feature in your application code.

#### IsEnabledAsync

Returns `true` if the given feature is enabled. So, you can conditionally execute your business flow.

**Example: Check if the "PDF Reporting" feature enabled**

```csharp
public class ReportingAppService : ApplicationService, IReportingAppService
{
    private readonly IFeatureChecker _featureChecker;

    public ReportingAppService(IFeatureChecker featureChecker)
    {
        _featureChecker = featureChecker;
    }

    public async Task<PdfReportResultDto> GetPdfReportAsync()
    {
        if (await _featureChecker.IsEnabledAsync("MyApp.PdfReporting"))
        {
            //TODO...
        }
        else
        {
            //TODO...   
        }
    }
}
```

`IsEnabledAsync` has overloads to check multiple features in one method call.

#### GetOrNullAsync

Gets the current value for a feature. This method returns a `string`, so you store any kind of value inside it, by converting to or from `string`.

**Example: Check the maximum product count allowed**

```csharp
public class ProductController : AbpController
{
    private readonly IFeatureChecker _featureChecker;

    public ProductController(IFeatureChecker featureChecker)
    {
        _featureChecker = featureChecker;
    }

    public async Task<IActionResult> Create(CreateProductModel model)
    {
        var currentProductCount = await GetCurrentProductCountFromDatabase();

        //GET THE FEATURE VALUE
        var maxProductCountLimit =
            await _featureChecker.GetOrNullAsync("MyApp.MaxProductCount");

        if (currentProductCount >= Convert.ToInt32(maxProductCountLimit))
        {
            throw new BusinessException(
                "MyApp:ReachToMaxProductCountLimit",
                $"You can not create more than {maxProductCountLimit} products!"
            );
        }

        //TODO: Create the product in the database...
    }

    private async Task<int> GetCurrentProductCountFromDatabase()
    {
        throw new System.NotImplementedException();
    }
}
```

This example uses a numeric value as a feature limit product counts for a user/tenant in a SaaS application.

Instead of manually converting the value to `int`, you can use the generic overload of the `GetAsync` method:

```csharp
var maxProductCountLimit = await _featureChecker.GetAsync<int>("MyApp.MaxProductCount");
```

#### Extension Methods

There are some useful extension methods for the `IFeatureChecker` interface;

* `Task<T> GetAsync<T>(string name, T defaultValue = default)`: Used to get a value of a feature with the given type `T`. Allows to specify a `defaultValue` that is returned when the feature value is `null`.
* `CheckEnabledAsync(string name)`: Checks if given feature is enabled. Throws an `AbpAuthorizationException` if the feature was not `true` (enabled).

## Defining the Features

TODO

## Feature Management

TODO

## Advanced Topics

TODO

### Feature Value Providers

TODO

### Feature Store

TODO