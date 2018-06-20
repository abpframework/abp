using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Serialization
{
    [DependsOn(typeof(AbpSerializationModule))]
    public class AbpSerializationTestModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpSerializationTestModule>();
        }
    }
}