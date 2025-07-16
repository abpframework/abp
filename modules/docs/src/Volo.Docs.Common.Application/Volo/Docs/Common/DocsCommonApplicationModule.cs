using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Volo.Docs.Common;

[DependsOn(
    typeof(DocsDomainModule),
    typeof(DocsCommonApplicationContractsModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpDddApplicationModule)
)]
public class DocsCommonApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<DocsCommonApplicationModule>();
            
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<DocsCommonApplicationAutoMapperProfile>(validate: true);
        });
    }
}