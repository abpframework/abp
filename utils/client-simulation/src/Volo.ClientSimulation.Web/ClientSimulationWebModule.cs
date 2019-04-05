using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.ClientSimulation.Web
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiBasicThemeModule),
        typeof(AbpAutofacModule),
        typeof(ClientSimulationModule)
        )]
    public class ClientSimulationWebModule : AbpModule
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
