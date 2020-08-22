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
                options.IdentityClients.Default = new IdentityClientConfiguration
                {
                    Authority = "https://localhost:44305",
                    GrantType = "password",
                    ClientId = "MyProjectName_App",
                    ClientSecret = "1q2w3e*",
                    UserName = "admin",
                    UserPassword = "1q2w3E*",
                    Scope = "MyProjectName"
                };
            });
        }

        private void ConfigureRemoteServices(ServiceConfigurationContext context)
        {
            //TODO: Get from config, automatically!
            Configure<AbpRemoteServiceOptions>(options =>
            {
                options.RemoteServices.Default = new RemoteServiceConfiguration
                {
                    BaseUrl = "https://localhost:44305"
                };
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
