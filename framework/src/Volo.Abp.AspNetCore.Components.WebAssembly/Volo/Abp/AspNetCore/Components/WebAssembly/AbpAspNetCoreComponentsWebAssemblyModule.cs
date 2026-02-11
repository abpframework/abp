using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.AspNetCore.Components.Web.ExceptionHandling;
using Volo.Abp.AspNetCore.Components.Web.Security;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.UI;

namespace Volo.Abp.AspNetCore.Components.WebAssembly;

[DependsOn(
    typeof(AbpAspNetCoreMvcClientCommonModule),
    typeof(AbpUiModule),
    typeof(AbpAspNetCoreComponentsWebModule)
)]
public class AbpAspNetCoreComponentsWebAssemblyModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var abpHostEnvironment = context.Services.GetSingletonInstance<IAbpHostEnvironment>();
        if (abpHostEnvironment.EnvironmentName.IsNullOrWhiteSpace())
        {
            abpHostEnvironment.EnvironmentName = context.Services.GetWebAssemblyHostEnvironment().Environment;
        }

        PreConfigure<AbpHttpClientBuilderOptions>(options =>
        {
            options.ProxyClientBuildActions.Add((_, builder) =>
            {
                builder.AddHttpMessageHandler<AbpBlazorClientHttpMessageHandler>();
            });
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClient();
        context.Services
            .GetHostBuilder().Logging
            .AddProvider(new AbpExceptionHandlingLoggerProvider(context.Services));
        
        if (!context.Services.ExecutePreConfiguredActions<AbpAspNetCoreComponentsWebOptions>().IsBlazorWebApp)
        {
            Configure<AbpAuthenticationOptions>(options =>
            {
                options.LoginUrl = "authentication/login";
                options.LogoutUrl = "authentication/logout";
            });
        }
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        var msAuthenticationStateProvider = context.Services.FirstOrDefault(x => x.ServiceType == typeof(AuthenticationStateProvider));
        if (msAuthenticationStateProvider is {ImplementationType: not null} &&
            msAuthenticationStateProvider.ImplementationType.IsGenericType &&
            msAuthenticationStateProvider.ImplementationType.GetGenericTypeDefinition() == typeof(RemoteAuthenticationService<,,>))
        {
            var webAssemblyAuthenticationStateProviderType = typeof(WebAssemblyAuthenticationStateProvider<,,>).MakeGenericType(
                    msAuthenticationStateProvider.ImplementationType.GenericTypeArguments[0],
                    msAuthenticationStateProvider.ImplementationType.GenericTypeArguments[1],
                    msAuthenticationStateProvider.ImplementationType.GenericTypeArguments[2]);

            context.Services.Replace(ServiceDescriptor.Scoped(typeof(AuthenticationStateProvider), webAssemblyAuthenticationStateProviderType));
        }
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.ServiceProvider.GetRequiredService<IClientScopeServiceProviderAccessor>().ServiceProvider.GetRequiredService<WebAssemblyCachedApplicationConfigurationClient>().InitializeAsync();
        await context.ServiceProvider.GetRequiredService<IClientScopeServiceProviderAccessor>().ServiceProvider.GetRequiredService<AbpComponentsClaimsCache>().InitializeAsync();
        await SetCurrentLanguageAsync(context.ServiceProvider);
    }

    private async static Task SetCurrentLanguageAsync(IServiceProvider serviceProvider)
    {
        var configurationClient = serviceProvider.GetRequiredService<ICachedApplicationConfigurationClient>();
        var utilsService = serviceProvider.GetRequiredService<IAbpUtilsService>();
        var configuration = await configurationClient.GetAsync();
        var cultureName = configuration.Localization?.CurrentCulture?.CultureName;
        if (!cultureName.IsNullOrEmpty())
        {
            var culture = new CultureInfo(cultureName!);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
        {
            await utilsService.AddClassToTagAsync("body", "rtl");
        }
    }
}
