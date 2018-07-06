using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Autofac
{
    [DependsOn(typeof(AbpAutofacModule))]
    public class AutofacTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<AbpAutofacModule>();
        }
    }
}