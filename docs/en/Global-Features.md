# Global Features

ABP Global Feature system is used to **enable**, **disable**  application & module features **on development time**. Global Features are totally different from [Features](Features.md). 

The purpose of the Global Feature System is to **add a module to your application but disable the features you don't want to use** (or enable only the ones you need). Notice that the features are not determined on runtime, you must select the features **on development time**. Because it will not create database tables, APIs and other stuff for unused features, which is not possible to change then on the runtime.

## Installation

Global Feature system is pre-installed in abp startup templates. No need any installation manually.

## Usage

### Enable/Disable Existing Features

```csharp
//Enable all the CMS Kit Features
GlobalFeatureManager.Instance.Modules.CmsKit().EnableAll();

//Disable all the CMS Kit Features (while it is already disabled by default)
GlobalFeatureManager.Instance.Modules.CmsKit().DisableAll();

//Enable a feature
GlobalFeatureManager.Instance.Modules.CmsKit().Comments.Enable();
GlobalFeatureManager.Instance.Modules.CmsKit().Enable<CommentsFeature>(); //Alternative: use the feature class
GlobalFeatureManager.Instance.Modules.CmsKit().Enable(CommentsFeature.Name); //Alternative: use the feature name
GlobalFeatureManager.Instance.Modules.CmsKit().Enable("CmsKit.Comments"); //Alternative: Use magic string
```

Alternative approach, using a lambda action to control a single module features:

```csharp
GlobalFeatureManager.Instance.Modules.CmsKit(cmsKit =>
{
    cmsKit.EnableAll();
    cmsKit.Reactions.Enable();
    cmsKit.Enable<ReactionsFeature>();
});
```

### Check if a feature is enabled

```csharp
GlobalFeatureManager.Instance.IsEnabled<CommentsFeature>();
GlobalFeatureManager.Instance.IsEnabled(CommentsFeature.Name); //Alternative
```

Both methods return `bool`.

Beside the manual check, there is `[RequiresGlobalFeature]` attribute to check it declaratively for a controller or page. ABP returns 404 if the related feature was disabled.

```csharp
[RequiresGlobalFeature(typeof(CommentsFeature))]
public class CommentController : AbpController
{
  // ...
}
```



## Implementation

Global Feature system aims module based feature management . So, module has to have own Global Features and Global Module Features class to provide all features.

### Define a Global Feature

A feature class is something like that:

```csharp
[GlobalFeatureName(Name)]
public class FooBarFeature : GlobalFeature
{
    public const string Name = "Foo.Bar";

    internal FooBarFeature(
        [NotNull] GlobalFooFeatures features
    ) : base(features)
    {
    }
}
```

### Define Global Module Features

All global features of a module should be placed under o single **GlobalModuleFeatures** class.

```csharp
public class GlobalFooFeatures : GlobalModuleFeatures
{
  public const string ModuleName = "Foo";
  
  public FooBarFeature FooBar => GetFeature<FooBarFeature>();
  
  public GlobalFooFeatures([NotNull] GlobalFeatureManager featureManager)
    : base(featureManager)
    {
    }
}
```



### Define Global Module Features Dictionary Extensions

An extension method is better to configure global fetures easily. 

```csharp
public static class GlobalModuleFeaturesDictionaryFooExtensions
{
  public static GlobalFooFeatures Foo(
    [NotNull] this GlobalModuleFeaturesDictionary modules)
  {
    Check.NotNull(modules, nameof(modules));

    return modules
      .GetOrAdd(
      GlobalFooFeatures.ModuleName,
      _ => new GlobalFooFeatures(modules.FeatureManager)
    )
      as GlobalFooFeatures;
  }
}
```

Accessing module & module features look like:

```csharp
GlobalFeatureManager.Instance.Modules.Foo();
GlobalFeatureManager.Instance.Modules.Foo().FooBar;

GlobalFeatureManager.Instance.Modules.Foo().FooBar.Enable();
GlobalFeatureManager.Instance.Modules.Foo().Enable<FooBarFeature>();
```



## When to configure Global Features?

Global Features have to be configured before application startup. So best place to configuring it is `PreConfigureServices` with **OneTimeRunner**.

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



## See also

- [Differences Between Features & Global Features](Differences-between-features-and-global-features.md)