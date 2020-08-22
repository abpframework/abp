using System;
using System.Net.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.IdentityModel;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName.Blazor
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(MyProjectNameHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
    )]
    public class MyProjectNameBlazorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var environment = context.Services.GetSingletonInstance<IWebAssemblyHostEnvironment>();
            var builder = context.Services.GetSingletonInstance<WebAssemblyHostBuilder>();

            ConfigureAuthentication(context);
            ConfigureRemoteServices(context);
            ConfigureHttpClient(context, environment);
            ConfigureUI(builder);
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context)
        {
            context.Services.Configure<AbpIdentityClientOptions>(options =>
            {
                //TODO: Get from config, automatically!
                options.IdentityClients.Default.Authority = "https://localhost:44305";
                options.IdentityClients.Default.GrantType = "password";
                options.IdentityClients.Default.ClientId = "MyProjectName_App";
                options.IdentityClients.Default.ClientSecret = "1q2w3e*";
                options.IdentityClients.Default.UserName = "admin";
                options.IdentityClients.Default.UserPassword = "1q2w3E*";
                options.IdentityClients.Default.Scope = "MyProjectName";
            });
        }

        private void ConfigureRemoteServices(ServiceConfigurationContext context)
        {
            //TODO: Get from config, automatically!
            Configure<AbpRemoteServiceOptions>(options =>
            {
                options.RemoteServices.Default.BaseUrl = "https://localhost:44305";
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
    }
}
