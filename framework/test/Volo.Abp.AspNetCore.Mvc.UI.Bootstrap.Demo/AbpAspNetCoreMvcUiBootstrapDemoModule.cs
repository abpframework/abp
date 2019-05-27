using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo
{
    [DependsOn(
        typeof(AspNetCoreMvcUiBasicThemeModule),
        typeof(AutofacModule)
        )]
    public class AbpAspNetCoreMvcUiBootstrapDemoModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseVirtualFiles();

            app.UseMvcWithDefaultRoute();
        }
    }
}