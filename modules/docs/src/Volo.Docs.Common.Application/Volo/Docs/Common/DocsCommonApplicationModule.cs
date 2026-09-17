using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;

namespace Volo.Docs.Common;

[DependsOn(
    typeof(DocsDomainModule),
    typeof(DocsCommonApplicationContractsModule),
    typeof(AbpMapperlyModule),
    typeof(AbpDddApplicationModule)
)]
public class DocsCommonApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<DocsCommonApplicationModule>();
    }
}