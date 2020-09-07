# Features

ABP Feature system is used to **enable**, **disable** or **change the behavior** of the application features **on runtime**.

Feature system was originally designed to control the tenant features in a **multi-tenant** application. However, it is **extensible** and capable of determining features by any condition.

> The feature system is implemented with the [Volo.Abp.Features](https://www.nuget.org/packages/Volo.Abp.Features) NuGet package. Most of the times you don't need to manually [install it](https://abp.io/package-detail/Volo.Abp.Features) since it comes pre-installed with the [application startup template](Startup-Templates/Application.md).

## Checking for the Features

Before starting to explain how to define features, let's see how to check a feature in your application code.

### RequiresFeature Attribute

`[RequiresFeature]` attribute (defined in the `Volo.Abp.Features` namespace) is used to declaratively check if a feature is enabled or not.

**Example: Check if the current user/tenant has "PDF Reporting" feature enabled**

```csharp
public class ReportingAppService : ApplicationService, IReportingAppService
{
    public async Task<CsvReportResultDto> GetCsvReportAsync()
    {
        throw new System.NotImplementedException();
    }

    [RequiresFeature("MyApp.PdfReporting")]
    public async Task<PdfReportResultDto> GetPdfReportAsync()
    {
        throw new System.NotImplementedException();
    }
}
```

* `RequiresFeature(...)` simply gets a feature name to check if it is enabled or not. If not enabled, an authorization [exception](Exception-Handling.md) is thrown and a proper response is returned to the client side.
* `[RequiresFeature]` can be used for a **method** or a **class**. When you use it for a class, all the 
* `RequiresFeature` may get multiple feature names, like `[RequiresFeature("Feature1", "Feature2")]`. In this case ABP checks if current user/tenant has any of the features enabled. Use `[RequiresFeature("Feature1", "Feature2", RequiresAll = true)]` to force to allow only if all of the features are enabled.
* Multiple usage of `[RequiresFeature]` attribute is enabled, so it checks all of them.

#### About the Interception

ABP Framework uses the interception system to make the `[RequiresFeature]` attribute working. So, it can work with any class that is injected from the [dependency injection](Dependency-Injection.md).

However, there are **some rules should be followed** in order to make it working;

* If you are **not injecting** the service over an interface (like `IMyService`), then the methods of the service must be `virtual` (otherwise, [dynamic proxy / interception](Dynamic-Proxying-Interceptors.md) system can not work).
* Only `async` methods (methods returning a `Task` or `Task<T>`) are intercepted.
