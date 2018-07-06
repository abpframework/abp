using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Serialization
{
    [DependsOn(typeof(AbpSerializationModule))]
    public class AbpSerializationTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<AbpSerializationTestModule>();
        }
    }
}