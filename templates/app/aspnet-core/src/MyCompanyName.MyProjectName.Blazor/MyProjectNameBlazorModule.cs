using System;
using System.Net.Http;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel.WebAssembly;
using Volo.Abp.Identity.Blazor;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName.Blazor
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(MyProjectNameHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelWebAssemblyModule),
        typeof(AbpAspNetCoreComponentsWebAssemblyModule),
        typeof(AbpIdentityBlazorModule)
    )]
    public class MyProjectNameBlazorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var environment = context.Services.GetSingletonInstance<IWebAssemblyHostEnvironment>();
            var builder = context.Services.GetSingletonInstance<WebAssemblyHostBuilder>();

            ConfigureAuthentication(builder);
            ConfigureHttpClient(context, environment);
            ConfigureBlazorise(context);
            ConfigureUI(builder);
        }

        private void ConfigureBlazorise(ServiceConfigurationContext context)
        {
            context.Services
                .AddBlazorise()
                .AddBootstrapProviders()
                .AddFontAwesomeIcons();
        }

        private static void ConfigureAuthentication(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("AuthServer", options.ProviderOptions);
                options.ProviderOptions.DefaultScopes.Add("MyProjectName");
            });
        }

        private static void ConfigureUI(WebAssemblyHostBuilder builder)
        {
            builder.RootComponents.Add<App>("app");
        }

        private static void ConfigureHttpClient(ServiceConfigurationContext context, IWebAssemblyHostEnvironment environment)
        {
            context.Services.AddTransient(sp => new HttpClient
            {
                BaseAddress = new Uri(environment.BaseAddress)
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            context.ServiceProvider
                .UseBootstrapProviders()
                .UseFontAwesomeIcons();
        }
    }
}
