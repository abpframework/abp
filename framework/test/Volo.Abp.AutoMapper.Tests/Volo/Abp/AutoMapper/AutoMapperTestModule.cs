using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.AutoMapper
{
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class AutoMapperTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<AutoMapperTestModule>();
            });
        }
    }
}