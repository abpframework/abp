using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Mapster;

[DependsOn(
    typeof(AbpObjectMappingModule)
)]
public class AbpMapsterModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapsterObjectMapper();
    }
}