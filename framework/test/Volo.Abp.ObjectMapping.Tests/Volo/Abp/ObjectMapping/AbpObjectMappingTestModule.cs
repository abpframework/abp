using Volo.Abp.Modularity;

namespace Volo.Abp.ObjectMapping
{
    [DependsOn(
        typeof(AbpObjectMappingModule),
        typeof(AbpTestBaseModule)
        )]
    public class AbpObjectMappingTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTest1AutoObjectMappingProvider<MappingContext1>();
            context.Services.AddTest2AutoObjectMappingProvider<MappingContext2>();
        }
    }
}
