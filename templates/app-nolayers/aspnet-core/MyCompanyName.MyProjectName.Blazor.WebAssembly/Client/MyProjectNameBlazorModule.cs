using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyCompanyName.MyProjectName.Menus;
using MyCompanyName.MyProjectName;
using OpenIddict.Abstractions;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Routing;
using Volo.Abp.AspNetCore.Components.WebAssembly.MudBlazorBasicTheme;
using Volo.Abp.Autofac.WebAssembly;
using Volo.Abp.Mapperly;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Blazor.MudBlazor.WebAssembly;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.Blazor.MudBlazor.WebAssembly;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.Blazor.MudBlazor.WebAssembly;
using Volo.Abp.UI.Navigation;

namespace MyCompanyName.MyProjectName;

[DependsOn(
    typeof(MyProjectNameContractsModule),

    // ABP Framework packages
    typeof(AbpAutofacWebAssemblyModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyMudBlazorBasicThemeModule),

    // Account module packages
    typeof(AbpAccountHttpApiClientModule),

    // Identity module packages
    typeof(AbpIdentityHttpApiClientModule),
    typeof(AbpIdentityBlazorMudBlazorWebAssemblyModule),
    typeof(AbpOpenIddictDomainSharedModule),

    // Permission Management module packages
    typeof(AbpPermissionManagementHttpApiClientModule),

    // Tenant Management module packages
    typeof(AbpTenantManagementHttpApiClientModule),
    typeof(AbpTenantManagementBlazorMudBlazorWebAssemblyModule),

    // Feature Management module packages
    typeof(AbpFeatureManagementHttpApiClientModule),

    // Setting Management module packages
    typeof(AbpSettingManagementHttpApiClientModule),
    typeof(AbpSettingManagementBlazorMudBlazorWebAssemblyModule)
)]
public class MyProjectNameBlazorModule : AbpModule
{
    public const string RemoteServiceName = "Default";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var environment = context.Services.GetSingletonInstance<IWebAssemblyHostEnvironment>();
        var builder = context.Services.GetSingletonInstance<WebAssemblyHostBuilder>();

        ConfigureAuthentication(builder);
        ConfigureHttpClient(context, environment);
        ConfigureRouter(context);
        ConfigureMenu(context);
        ConfigureHttpClientProxies(context);

        context.Services.AddMapperlyObjectMapper<MyProjectNameBlazorModule>();
    }

    private void ConfigureRouter(ServiceConfigurationContext context)
    {
        Configure<AbpRouterOptions>(options =>
        {
            options.AppAssembly = typeof(MyProjectNameBlazorModule).Assembly;
        });
    }

    private void ConfigureMenu(ServiceConfigurationContext context)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new MyProjectNameMenuContributor(context.Services.GetConfiguration()));
        });
    }

    private void ConfigureHttpClientProxies(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(MyProjectNameContractsModule).Assembly,
            RemoteServiceName
        );
    }

    private static void ConfigureAuthentication(WebAssemblyHostBuilder builder)
    {
        builder.Services.AddOidcAuthentication(options =>
        {
            builder.Configuration.Bind("AuthServer", options.ProviderOptions);
            options.UserOptions.NameClaim = OpenIddictConstants.Claims.Name;
            options.UserOptions.RoleClaim = OpenIddictConstants.Claims.Role;

            options.ProviderOptions.DefaultScopes.Add("MyProjectName");
            options.ProviderOptions.DefaultScopes.Add("roles");
            options.ProviderOptions.DefaultScopes.Add("email");
            options.ProviderOptions.DefaultScopes.Add("phone");
        });
    }

    private static void ConfigureHttpClient(ServiceConfigurationContext context, IWebAssemblyHostEnvironment environment)
    {
        context.Services.AddTransient(sp => new HttpClient
        {
            BaseAddress = new Uri(environment.BaseAddress)
        });
    }
}
