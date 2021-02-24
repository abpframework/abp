using AbpBlazorServerDemo.Data;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.AspNetCore.Components.UI.BasicTheme;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.Autofac;
using Volo.Abp.BlazoriseUI;
using Volo.Abp.Modularity;
using Volo.Abp.AspNetCore.Components.UI.Theming.Routing;

namespace AbpBlazorServerDemo
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule),
        typeof(AbpBlazoriseUIModule),
        typeof(AbpAspNetCoreComponentsUiBasicThemeModule)
    )]
    public class AbpBlazorServerDemoModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureBlazorise(context);
            ConfigureRouter(context);
            context.Services.AddServerSideBlazor();
            context.Services.AddSingleton<WeatherForecastService>();
        }
        
        private void ConfigureBlazorise(ServiceConfigurationContext context)
        {
            context.Services
                .AddBootstrapProviders()
                .AddFontAwesomeIcons();
        }
        
        private void ConfigureRouter(ServiceConfigurationContext context)
        {
            Configure<AbpRouterOptions>(options =>
            {
                options.AppAssembly = typeof(AbpBlazorServerDemoModule).Assembly;
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var env = context.GetEnvironment();
            var app = context.GetApplicationBuilder();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseConfiguredEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}