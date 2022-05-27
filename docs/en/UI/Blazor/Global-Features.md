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

- You can follow [Check for a Global Feature](../../Global-Features#check-for-a-global-feature) section of Global Features document to check global featues in csharp.