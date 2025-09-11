using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs;

[DependsOn(
    typeof(AbpBackgroundJobsDomainSharedModule),
    typeof(AbpBackgroundJobsModule),
    typeof(AbpMapperlyModule)
    )]
public class AbpBackgroundJobsDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<AbpBackgroundJobsDomainModule>();
     
    }
}
