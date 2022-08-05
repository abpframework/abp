# LeptonX Lite MVC UI
LeptonX Lite has implementation for the ABP Framework Razor Pages. It's a simplified variation of the [LeptonX Theme](https://x.leptontheme.com/).

>   If you are looking for a professional, enterprise ready theme, you can check the [LeptonX Theme](https://x.leptontheme.com/), which is a part of [ABP Commercial](https://commercial.abp.io/).

> See the [Theming document](https://docs.abp.io/en/abp/latest/UI/AspNetCore/Theming) to learn about themes.

## Installation

This theme is **already installed** when you create a new solution using the startup templates. If you are using any other template, you can install this theme by following the steps below:

- Add **Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite** package to your **Web** application.

```bash
dotnet add package Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite --prerelease
```

- Remove **Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic** reference from the project since it's not necessary after switching to LeptonX Lite.

- Make sure the old theme is removed and LeptonX is added in your Module class.

```diff
[DependsOn(
     // Remove BasicTheme module from DependsOn attribute
-    typeof(AbpAspNetCoreMvcUiBasicThemeModule),
     
     // Add LeptonX Lite module to DependsOn attribute
+    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
)]
```

- Replace `BasicThemeBundles` with `LeptonXLiteThemeBundles` in AbpBundlingOptions

```diff
Configure<AbpBundlingOptions>(options =>
{
    options.StyleBundles.Configure(
        // Remove following line
-       BasicThemeBundles.Styles.Global,
        // Add following line instead
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
LeptonX Lite includes separeted toolbars for desktop & mobile. You can manage toolbars independently. Toolbar names can be accessible in the **LeptonXLiteToolbars** class.

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
