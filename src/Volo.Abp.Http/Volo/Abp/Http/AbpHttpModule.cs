using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.ProxyScripting.Configuration;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Modularity;

namespace Volo.Abp.Http
{
    public class AbpHttpModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpHttpModule>();

            services.Configure<AbpApiProxyScriptingOptions>(options =>
            {
                options.Generators[JQueryProxyScriptGenerator.Name] = typeof(JQueryProxyScriptGenerator);
            });
        }
    }
}
