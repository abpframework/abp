using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.DynamicProxying;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp;

namespace Volo.Abp.Http
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(AbpAspNetCoreMvcTestModule)
        )]
    public class AbpHttpClientTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(typeof(TestAppModule).Assembly);
            context.Services.AddHttpClientProxy<IRegularTestController>();

            Configure<RemoteServiceOptions>(options =>
            {
                options.RemoteServices.Default = new RemoteServiceConfiguration("/");
            });

            //This is needed after ASP.NET Core 3.0 upgrade.
            context.Services.AddMvc()
                .PartManager.ApplicationParts
                .Add(new AssemblyPart(typeof(AbpAspNetCoreMvcModule).Assembly));
        }
    }
}
