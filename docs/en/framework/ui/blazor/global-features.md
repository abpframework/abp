# Blazor UI: Global Features
`GlobalFeatureManager` allows you to check the global features in your Blazor applications.

## Usage

```html
@using Volo.Abp.GlobalFeatures

@* ... *@

@* Global Feature can be checked with feature name *@
@if(GlobalFeatureManager.Instance.IsEnabled("Ecommerce.Subscription"))
{
    <span>Ecommerce.Subscription is enabled.</span>
}

@* OR it can be checked with type *@

@if(GlobalFeatureManager.Instance.IsEnabled<EcommerceSubscriptionGlobalFeature>())
{
    <span>Ecommerce.Subscription is enabled.</span>
}
```

- You can follow _Check for a Global Feature_ section of the [Global Features document](../../infrastructure/global-features.md) to check global features in your C# code.