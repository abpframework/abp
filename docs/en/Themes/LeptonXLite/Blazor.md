# LeptonX Lite Blazor UI

````json
//[doc-params]
{
    "UI": ["Blazor", "BlazorServer"]
}
````

LeptonX Lite has implementation for the ABP Framework Blazor WebAssembly & Blazor Server. It's a simplified variation of the [LeptonX Theme](https://x.leptontheme.com/).

>   If you are looking for a professional, enterprise ready theme, you can check the [LeptonX Theme](https://x.leptontheme.com/), which is a part of [ABP Commercial](https://commercial.abp.io/).

> See the [Theming document](https://docs.abp.io/en/abp/latest/UI/AspNetCore/Theming) to learn about themes.

## Installation

This theme is **already installed** when you create a new solution using the startup templates. If you are using any other template, you can install this theme by following the steps below:

{{if UI == "Blazor"}}
- Complete the [MVC Razor Pages Installation](AspNetCore.md#installation) for the **HttpApi.Host** application first. _If the solution is tiered/micro-service, complete the MVC steps for all MVC applications such as **HttpApi.Host** and if identity server is separated, install to the **OpenIddict**_.


- Add **Volo.Abp.AspNetCore.Components.WebAssembly.LeptonXLiteTheme** package to your **Blazor WebAssembly** application with the following command:

  ```bash
  dotnet add package Volo.Abp.AspNetCore.Components.WebAssembly.LeptonXLiteTheme --prerelease
  ```

- Remove **Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme** reference from the project since it's not necessary after switching to LeptonX Lite.

- Remove the old theme from the **DependsOn** attribute in your module class and add the **AbpAspNetCoreComponentsWebAssemblyLeptonXLiteThemeModule** type to the **DependsOn** attribute.

```diff
[DependsOn(
     // Remove BasicTheme module from DependsOn attribute
-    typeof(AbpAspNetCoreComponentsWebAssemblyBasicThemeModule),

    // Add LeptonX Lite module to DependsOn attribute
+    typeof(AbpAspNetCoreComponentsWebAssemblyLeptonXLiteThemeModule),
)]
```

- Change startup App component with the LeptonX one.

```csharp
// Make sure the 'App' comes from 'Volo.Abp.AspNetCore.Components.Web.LeptonXLiteTheme.Themes.LeptonXLite' namespace.
builder.RootComponents.Add<App>("#ApplicationContainer");
```

- Run the `abp bundle` command in your **Blazor** application folder.

{{end}}


{{if UI == "BlazorServer"}}

- Complete the [MVC Razor Pages Installation](AspNetCore.md#installation) first. _If the solution is tiered/micro-service, complete the MVC steps for all MVC applications such as **HttpApi.Host** and **AuthServer**_.

- Add **Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme** package to your **Blazor server** application with the following command:

  ```bash
  dotnet add package Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme --prerelease
  ```

- Remove **Volo.Abp.AspNetCore.Components.Server.BasicTheme** reference from the project since it's not necessary after switching to LeptonX Lite.


- Remove old theme from the **DependsOn** attribute in your module class and add the **AbpAspNetCoreComponentsServerLeptonXLiteThemeModule** type to the **DependsOn** attribute.

  ```diff
  [DependsOn(
      // Remove BasicTheme module from DependsOn attribute
  -    typeof(AbpAspNetCoreComponentsServerBasicThemeModule),

      // Add LeptonX Lite module to DependsOn attribute
  +    typeof(AbpAspNetCoreComponentsServerLeptonXLiteThemeModule)
  )]
  ```

- Replace `BlazorBasicThemeBundles` with `BlazorLeptonXLiteThemeBundles` in `AbpBundlingOptions`:
  ```diff
  options.StyleBundles.Configure(
    // Remove following line
  - BlazorBasicThemeBundles.Styles.Global,
    // Add following line instead
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
  - Then replace script & style bundles as following:
    ```diff
    // Remove following line
    - <abp-style-bundle name="@BlazorBasicThemeBundles.Styles.Global" />
    // Add following line instead
    + <abp-style-bundle name="@BlazorLeptonXLiteThemeBundles.Styles.Global" />
    ```

    ```diff
    // Remove following line
    - <abp-script-bundle name="@BlazorBasicThemeBundles.Scripts.Global" />
    // Add following line instead
    + <abp-script-bundle name="@BlazorLeptonXLiteThemeBundles.Scripts.Global" />
    ```

{{end}}


---

## Customization

### Toolbars
LeptonX Lite includes separeted toolbars for desktop & mobile. You can manage toolbars independently. Toolbar names can be accessible in the **LeptonXLiteToolbars** class.

- `LeptonXLiteToolbars.Main`
- `LeptonXLiteToolbars.MainMobile`

```csharp
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

    return Task.CompletedTask;
}
```

{{if UI == "BlazorServer"}}

> _You can visit the [Toolbars Documentation](https://docs.abp.io/en/abp/latest/UI/Blazor/Toolbars) for better understanding._

{{end}}
