# Global Features
Global Feature system is used to enable/disable an application feature on development time. It is done on the development time, because some **services** (e.g. controllers) are removed from the application model and **database tables** are not created for the disabled features, which is not possible on runtime.

Global Features system is especially useful if you want to develop a reusable application module with optional features. If the final application doesn't want to use some of the features, it can disable these features.

> If you are looking for a system to enable/disable features based on current tenant or any other condition, please see the [Features](Features.md) document.

## Installation
> This package is already installed by default with the startup template. So, most of the time, you don't need to install it manually.

Open a command line window in the folder of the project (.csproj file) and type the following command:

```bash
abp add-package Volo.Abp.GlobalFeatures
```

## Defining a Global Feature

A feature class is something like that:

```csharp
[GlobalFeatureName("Shopping.Payment")]
public class PaymentFeature
{
    
}
```

## Enable/Disable Global Features

Use `GlobalFeatureManager.Instance` to enable/disable a global feature.

```csharp
// Able to Enable/Disable with generic type parameter.
GlobalFeatureManager.Instance.Enable<PaymentFeature>();
GlobalFeatureManager.Instance.Disable<PaymentFeature>();

// Also able to Enable/Disable with string feature name.
GlobalFeatureManager.Instance.Enable("Shopping.Payment");
GlobalFeatureManager.Instance.Disable("Shopping.Payment");
```

> Global Features are disabled unless they are explicitly enabled.

### Where to Configure Global Features?

Global Features have to be configured before application startup. Since the `GlobalFeatureManager.Instance` is a singleton object, one-time, static configuration is enough. It is suggested to enable/disable global features in `PreConfigureServices` method of your module. You can use the `OneTimeRunner` utility class to make sure it runs only once:

```csharp
private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();
public override void PreConfigureServices(ServiceConfigurationContext context)
{
  OneTimeRunner.Run(() =>
  {
  	GlobalFeatureManager.Instance.Enable<PaymentFeature>();
  });
}
```

## Check for a Global Feature

```csharp
GlobalFeatureManager.Instance.IsEnabled<PaymentFeature>()
GlobalFeatureManager.Instance.IsEnabled("Shopping.Payment")
```

Both methods return `bool`. So, you can write conditional logic as shown below:

```csharp
if (GlobalFeatureManager.Instance.IsEnabled<PaymentFeature>())
{
    // Some strong payment codes here...
}
```

### RequiresGlobalFeature Attribute

Beside the manual check, there is `[RequiresGlobalFeature]` attribute to check it declaratively for a controller or page. ABP returns HTTP Response `404` if the related feature was disabled.

```csharp
[RequiresGlobalFeature(typeof(PaymentFeature))]
public class PaymentController : AbpController
{

}
```

## Grouping Features of a Module

It is common to group global features of a module to allow the final application developer easily discover and configure the features. Following example shows how to group features of a module.

Assume that we've defined a global feature for `Subscription` feature of an `Ecommerce` module:

```csharp
[GlobalFeatureName("Ecommerce.Subscription")]
public class SubscriptionFeature : GlobalFeature
{
    public SubscriptionFeature(GlobalModuleFeatures module)
        : base(module)
    {
    }
}
```

You can define as many features as you need in your module. Then define a class to group these features together:

```csharp
public class GlobalEcommerceFeatures : GlobalModuleFeatures
{
    public const string ModuleName = "Ecommerce";

    public SubscriptionFeature Subscription => GetFeature<SubscriptionFeature>();
	
    public GlobalEcommerceFeatures(GlobalFeatureManager featureManager)
        : base(featureManager)
    {
        AddFeature(new SubscriptionFeature(this));
    }
}
```

Finally, you can create an extension method on `GlobalModuleFeaturesDictionary`:

```csharp
public static class GlobalModuleFeaturesDictionaryEcommerceExtensions
{
    public static GlobalEcommerceFeatures Ecommerce(
        this GlobalModuleFeaturesDictionary modules)
    {
        return modules.GetOrAdd(
            GlobalEcommerceFeatures.ModuleName,
            _ => new GlobalEcommerceFeatures(modules.FeatureManager)
        ) as GlobalEcommerceFeatures;
  }
```

Then `GlobalFeatureManager.Instance.Modules.Ecommerce()` can be used to access the global features of your module. Examples usages:

```csharp
GlobalFeatureManager.Instance.Modules.Ecommerce().Subscription.Enable();
GlobalFeatureManager.Instance.Modules.Ecommerce().EnableAll();
```

