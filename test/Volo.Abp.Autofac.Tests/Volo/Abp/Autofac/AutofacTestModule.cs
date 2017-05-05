using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Castle;
using Volo.Abp.Castle.DynamicProxy;
using Volo.Abp.Modularity;

namespace Volo.Abp.Autofac
{
    [DependsOn(typeof(AbpAutofacModule), typeof(AbpCastleCoreTestModule))]
    public class AutofacTestModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpCastleCoreTestModule>();
        }
    }
}