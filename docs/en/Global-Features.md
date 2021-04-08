# Global Features
The purpose of the Global Feature System is to **add a module to your application but disable the features you don't want to use** (or enable only the ones you need). Notice that the features are not determined on runtime, you must select the features **on development time**. Because it will not create database tables, APIs and other stuff for unused features, which is not possible to change then on the runtime.

## Installation
> This package is already installed by default with the startup template. So, most of the time, you don't need to install it manually.

### Using the ABP CLI

Open a command line window in the folder of the project (.csproj file) and type the following command:

```bash
abp add-package Volo.Abp.GlobalFeatures
```

## Implementation

Global Feature system aims module based feature management . A module has to have own Global Features itself.

### Define a Global Feature

A feature class is something like that:

```csharp
[GlobalFeatureName(Name)]
public class PaymentFeature : GlobalFeature
{
    public const string Name = "Shopping.Payment";

    public PaymentFeature(GlobalModuleFeatures module) : base(module)
    {
    }
}
```

### Define Global Module Features

All features of a module have to be defined in a Global Module Features class.

```csharp
public class GlobalShoppingFeatures : GlobalModuleFeatures
{
    public const string ModuleName = "Shopping";

    public GlobalShoppingFeatures(GlobalFeatureManager featureManager) : base(featureManager)
    {
        AddFeature(new PaymentFeature(this));
        // And more features...
    }
}
```

## Usage

### Enable/Disable Features

Global features are managed  by modules. Module Features have to be added to Modules of GlobalFeatureManager.

```csharp
// GerOrAdd might be useful to be sure module features are added.
var shoppingGlobalFeatures = GlobalFeatureManager.Instance.Modules
    .GetOrAdd(
        GlobalShoppingFeatures.ModuleName, 
        ()=> new GlobalShoppingFeatures(GlobalFeatureManager.Instance));

// Able to Enable/Disable with generic type parameter.
shoppingGlobalFeatures.Enable<PaymentFeature>();
shoppingGlobalFeatures.Disable<PaymentFeature>();

// Also able to Enable/Disable with string feature name.
shoppingGlobalFeatures.Enable(PaymentFeature.Name);
shoppingGlobalFeatures.Disable("Shopping.Payment");
```

### Check if a feature is enabled

```csharp
GlobalFeatureManager.Instance.IsEnabled<PaymentFeature>()
GlobalFeatureManager.Instance.IsEnabled("Shopping.Payment")
```

Both methods return `bool`.

```csharp
if (GlobalFeatureManager.Instance.IsEnabled<PaymentFeature>())
{
    // Some strong payment codes here...
}
```

Beside the manual check, there is `[RequiresGlobalFeature]` attribute to check it declaratively for a controller or page. ABP returns 404 if the related feature was disabled.

```csharp
[RequiresGlobalFeature(typeof(CommentsFeature))]
public class PaymentController : AbpController
{
  // ...
}
```

## When to configure Global Features?
Global Features have to be configured before application startup. So best place to configuring it is `PreConfigureServices` with **OneTimeRunner** to make sure it runs one time.

```csharp
private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();
public override void PreConfigureServices(ServiceConfigurationContext context)
{
  OneTimeRunner.Run(() =>
  {
  	GlobalFeatureManager.Instance.Modules.Foo().EnableAll();
  });
}
```

## Features vs Global Features
[Features](Features.md) & [Global Features](Global-Features.md) are totally different systems.

Features are used to switch on/off application feature for each tenant. So Features, only hides disabled ones, but with Global Features, disabled features pretends like never existed in application.