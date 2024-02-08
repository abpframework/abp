# New Blazor Web App Template in ABP 8.2

ASP.NET Blazor in .NET 8 allows you to use a single powerful component model to handle all of your web UI needs, including server-side rendering, client-side rendering, streaming rendering, progressive enhancement, and much more!

ABP 8.2 supports the new Blazor Web App template, In this guide, we will introduce some new changes and features in the new Blazor Web App template.

## Create a new Blazor Web App

> Please make sure you have installed the 8.2.x version of the ABP CLI.

You can create a new Blazor Web App using the `abp new BookStore -t app -u blazor-webapp` command. The `-u blazor-webapp` option is used to select the Blazor Web App template.

Of course, you can also create Blazor wasm and Blazor Server applications. We have changed them to use the new Blazor Web App mode.

````csharp
abp new BookStore -t app -u blazor
abp new BookStore -t app -u blazor-server
````

## Render modes

The template project use different render modes for different types of projects in the `App.razor` component.

|      Type      |      Render mode
|----------------|------------------
|      WASM      |   InteractiveWebAssembly(prerender: false)
|      Server    |   InteractiveServer
|      WebApp    |   InteractiveAuto

## The key changes of the new Blazor Web App template

The new Web App template has two project, each of them contains a system of abp modules. 

- MyCompanyName.MyProjectName.Blazor.WebApp
- MyCompanyName.MyProjectName.Blazor.WebApp.Client

### MyCompanyName.MyProjectName.Blazor.WebApp

The `Blazor.WebApp` is the startup project, There is a `App.razor` component in the `Blazor.WebApp` project, which is the root component of the Blazor application.

The main differences between it and a regular blazor server project are:

1. You need to `PreConfigure` the `AbpAspNetCoreComponentsWebOptions's IsBlazorWebApp` to `true`.

````csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<AbpAspNetCoreComponentsWebOptions>(options =>
    {
        options.IsBlazorWebApp = true;
    });
}
````

2. Add related services to the container. Add `MyProjectNameBlazorClientModule's` assembly to the `AbpRouterOptions's AdditionalAssemblies`.

````csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    // Add services to the container.
    context.Services.AddRazorComponents()
        .AddInteractiveServerComponents()
        .AddInteractiveWebAssemblyComponents();

    Configure<AbpRouterOptions>(options =>
    {
        options.AppAssembly = typeof(MyProjectNameBlazorModule).Assembly;
        options.AdditionalAssemblies.Add(typeof(MyProjectNameBlazorClientModule).Assembly);
    });
}
````

3. Add `UseAntiforgery` middleware and `MapRazorComponents/AddInteractiveServer/WebAssemblyRenderMode/AddAdditionalAssemblies` in the `OnApplicationInitialization` method. 

````csharp
public override void OnApplicationInitialization(ApplicationInitializationContext context)
{
    var env = context.GetEnvironment();
    var app = context.GetApplicationBuilder();
    // ...

    app.UseAntiforgery();
    app.UseAuthorization();

    app.UseConfiguredEndpoints(builder =>
    {
        builder.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(builder.ServiceProvider.GetRequiredService<IOptions<AbpRouterOptions>>().Value.AdditionalAssemblies.ToArray());
    });
}
````

### MyCompanyName.MyProjectName.Blazor.WebApp.Client

There is a `Routers.razor` component in the `Blazor.WebApp.Client` project, which is used by the `App.razor` component.

The main differences between it and a regular blazor wasm project are:

1. You need to `PreConfigure` the `AbpAspNetCoreComponentsWebOptions's IsBlazorWebApp` to `true`.

````csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<AbpAspNetCoreComponentsWebOptions>(options =>
    {
        options.IsBlazorWebApp = true;
    });
}
````

2. Use `AddBlazorWebAppServices` to replace `Authentication` code. 

````csharp
private static void ConfigureAuthentication(WebAssemblyHostBuilder builder)
{
    builder.Services.AddBlazorWebAppServices();
}
````

3. Remove the `builder.RootComponents.Add<App>("#ApplicationContainer");` code.

### MyCompanyName.MyProjectName.Blazor.WebApp.Tiered and MyCompanyName.MyProjectName.Blazor.WebApp.Tiered.Client

The tiered projects are the same as the WebApp projects, but the authentication configuration is different.

We need share the `access_token` to `Client` project.

`App.rzor` of `MyCompanyName.MyProjectName.Blazor.WebApp.Tiered`:

````csharp
@code{
    [CascadingParameter]
    private HttpContext HttpContext { get; set; } = default!;

    [Inject]
    private PersistentComponentState PersistentComponentState { get; set; }

    private string? Token { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        if (HttpContext.User?.Identity?.IsAuthenticated == true)
        {
            Token = await HttpContext.GetTokenAsync("access_token");
        }

        PersistentComponentState.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
    }

    async Task OnPersistingAsync()
    {
        if (!Token.IsNullOrWhiteSpace())
        {
            PersistentComponentState.PersistAsJson(PersistentAccessToken.Key, new PersistentAccessToken
            {
                AccessToken = Token
            });
        }

        await Task.CompletedTask;
    }
}
````

`MyProjectNameBlazorClientModule` of `MyCompanyName.MyProjectName.Blazor.WebApp.Tiered.Client`:

````csharp
private static void ConfigureAuthentication(WebAssemblyHostBuilder builder)
{
    builder.Services.AddBlazorWebAppTieredServices();
}
````

## ABP Bundle

You need set `IsBlazorWebApp` and `InteractiveAuto` to `true` in the `appsettings.json` file of the `MyCompanyName.MyProjectName.Blazor.WebApp.Client` project.

````json
{
  "AbpCli": {
    "Bundle": {
      "Mode": "BundleAndMinify", /* Options: None, Bundle, BundleAndMinify */
      "Name": "global",
      "IsBlazorWebApp": true,
      "InteractiveAuto": true,
      "Parameters": {

      }
    }
  }
}
````

For Blazor wasm and Blazor Server applications, you need to set `IsBlazorWebApp` to `true` and not need change the `InteractiveAuto`.

````json
{
  "AbpCli": {
    "Bundle": {
      "Mode": "BundleAndMinify", /* Options: None, Bundle, BundleAndMinify */
      "Name": "global",
      "IsBlazorWebApp": true,
      "Parameters": {

      }
    }
  }
}
````

Then run the `abp bundle` command to under the `MyCompanyName.MyProjectName.Blazor.WebApp.Client` project to generate the `global.css` and `global.js` files.

## Troubleshooting

If you encounter any problems during the migration, please create a new template project and compare the differences between the new and old projects.

# References

- [ASP.NET Core Blazor render modes](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/render-modes?view=aspnetcore-8.0)
- [Migrate from ASP.NET Core Blazor 7.0 to 8.0](https://learn.microsoft.com/en-us/aspnet/core/migration/70-80?view=aspnetcore-8.0&tabs=visual-studio#blazor)
- [Full stack web UI with Blazor in .NET 8 | .NET Conf 2023](https://www.youtube.com/watch?v=YwZdtLEtROA)
