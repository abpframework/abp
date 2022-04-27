# LeptonX Lite MVC UI

````json
//[doc-params]
{
    "UI": ["Blazor", "BlazorServer"]
}
````

LeptonX Lite has implementation for ABP Framework Blazor WebAssembly & Blazor Server.

## Installation


## Installation

{{if UI == "Blazor"}}

- Add **Volo.Abp.AspNetCore.Components.WebAssembly.LeptonXTheme** package to your **Blazor wasm** application.
  ```bash
  dotnet add package Volo.Abp.AspNetCore.Components.WebAssembly.LeptonXLiteTheme
  ```

- Remove old theme from **DependsOn** attribute in your module class and add **AbpAspNetCoreComponentsWebAssemblyLeptonXThemeModule** type to **DependsOn** attribute.

```diff
[DependsOn(
-    typeof(AbpAspNetCoreComponentsWebAssemblyBasicThemeModule),
+    typeof(AbpAspNetCoreComponentsWebAssemblyLeptonXLiteThemeModule)
)]
```

- Change startup App component with LeptonX one.

```csharp
// Make sure the 'App' comes from 'Volo.Abp.AspNetCore.Components.Web.LeptonXLiteTheme.Themes.LeptonXLite' namespace.
builder.RootComponents.Add<App>("#ApplicationContainer");

```

- Run `abp bundle` command in your **Blazor** application folder.

{{end}}


{{if UI == "BlazorServer"}}

- Complete [MVC Razor Pages Installation](mvc.md#installation) first. 

- Add **Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme** package to your **Blazor server** application.
  ```bash
  dotnet add package Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme
  ```

- Remove old theme from **DependsOn** attribute in your module class and add **AbpAspNetCoreComponentsWebAssemblyLeptonXThemeModule** type to **DependsOn** attribute.

  ```diff
  [DependsOn(
  -    typeof(AbpAspNetCoreComponentsServerBasicThemeModule),
  +    typeof(AbpAspNetCoreComponentsServerLeptonXLiteThemeModule)
  )]
  ```

- Update AbpBundlingOptions
  ```diff
  options.StyleBundles.Configure(
  - BlazorBasicThemeBundles.Styles.Global,
  + BlazorLeptonXLiteThemeBundles.Styles.Global,
    bundle =>
    {
        bundle.AddFiles("/blazor-global-styles.css");
        //You can remove the following line if you don't use Blazor CSS isolation for components
        bundle.AddFiles("/MyProjectName.Blazor.styles.css");
    });
  ```

- Update `_Host.cshtml` file. _(located under **Pages** folder by default.)_

  - Add following usings to Locate **App** and **BlazorLeptonXLiteThemeBundles** classes.
    ```csharp
    @using Volo.Abp.AspNetCore.Components.Web.LeptonXLiteTheme.Themes.LeptonXLite
    @using Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme.Bundling
    ```
  - Then replace script & style bunles as following
    ```diff
    - <abp-style-bundle name="@BlazorBasicThemeBundles.Styles.Global" />
    + <abp-style-bundle name="@BlazorLeptonXLiteThemeBundles.Styles.Global" />
    ```

    ```diff
    - <abp-script-bundle name="@BlazorBasicThemeBundles.Scripts.Global" />
    + <abp-script-bundle name="@BlazorLeptonXLiteThemeBundles.Scripts.Global" />
    ```

{{end}}


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