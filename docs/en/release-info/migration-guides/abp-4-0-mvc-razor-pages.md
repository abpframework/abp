```json
//[doc-seo]
{
    "Description": "Learn how to easily migrate from MVC/Razor Pages UI 3.3 to 4.0 by updating the namespace for IBrandingProvider in ABP Framework."
}
```

# MVC / Razor Pages UI 3.3 to 4.0 Migration Guide

## Use IBrandingProvider in the Volo.Abp.UI Package

This will be a breaking change for MVC UI, but very easy to fix. `IBrandingProvider` is being moved from `Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components` to `Volo.Abp.Ui.Branding` namespace. So, just update the namespace imports.

