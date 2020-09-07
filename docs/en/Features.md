# Features

ABP Feature system is used to **enable**, **disable** or **change the behavior** of the application features **on runtime**.

The runtime value for a feature can simply be a `boolean`, like enabled (`true`) or disabled (`false`). However, you can store **any kind** of runtime value for feature.

Feature system was originally designed to control the tenant features in a **multi-tenant** application. However, it is **extensible** and capable of determining features by any condition.

> The feature system is implemented with the [Volo.Abp.Features](https://www.nuget.org/packages/Volo.Abp.Features) NuGet package. Most of the times you don't need to manually [install it](https://abp.io/package-detail/Volo.Abp.Features) since it comes pre-installed with the [application startup template](Startup-Templates/Application.md).

## Checking for the Features

Before starting to explain how to define features, let's see how to check a feature in your application code.

### RequiresFeature Attribute

`[RequiresFeature]` attribute (defined in the `Volo.Abp.Features` namespace) is used to declaratively check if a feature is enabled or not. It is a useful shortcut for the `boolean` features.

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

* If you are **not injecting** the service over an interface (like `IMyService`), then the methods of the service must be `virtual` (otherwise, [dynamic proxy / interception](Dynamic-Proxying-Interceptors.md) system can not work).
* Only `async` methods (methods returning a `Task` or `Task<T>`) are intercepted.

> There is an exception for the controllers and razor pages. They don't require the following rules since ABP Framework uses action/page filters to implement the feature checking in this case.

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

In this way, you can create numeric limits in your SaaS application depending on the current user or tenant.

Instead of manually converting the value to `int`, you can use the generic overload of the `GetAsync` method:

```csharp
var maxProductCountLimit = await _featureChecker.GetAsync<int>("MyApp.MaxProductCount");
```

#### Extension Methods

TODO