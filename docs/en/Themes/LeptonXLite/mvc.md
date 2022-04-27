# LeptonX Lite MVC UI
LeptonX Lite has implementation for ABP Framework Razor Pages.

## Installation

- Add **Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite** package to your **Web** application.

```bash
dotnet add package Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite
```

- Make sure old theme is removed and LeptonX is added in your Module class.

```diff
[DependsOn(
-    typeof(AbpAspNetCoreMvcUiBasicThemeModule),
+    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule)
)]
```

- Update AbpBundlingOptions

```diff
Configure<AbpBundlingOptions>(options =>
{
    options.StyleBundles.Configure(
-       BasicThemeBundles.Styles.Global,
+       LeptonXLiteThemeBundles.Styles.Global
        bundle =>
        {
            bundle.AddFiles("/global-styles.css");
        }
    );
});
```

---

## Customization

### Toolbars
LeptonX Lite includes separeted toolbars for desktop & mobile. You can manage toolbars independently. Toolbar names can be accessible in **LeptonXLiteToolbars** class.

- `LeptonXLiteToolbars.Main`
- `LeptonXLiteToolbars.MainMobile`

```csharp
public class MyProjectNameMainToolbarContributor : IToolbarContributor
{
    public async Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        if (context.Toolbar.Name == LeptonXLiteToolbars.Main)
        {
            context.Toolbar.Items.Add(new ToolbarItem(typeof(MyDesktopComponent)));
        }

        if (context.Toolbar.Name == LeptonXLiteToolbars.MainMobile)
        {
            context.Toolbar.Items.Add(new ToolbarItem(typeof(MyMobileComponent)));
        }
    }
}
```