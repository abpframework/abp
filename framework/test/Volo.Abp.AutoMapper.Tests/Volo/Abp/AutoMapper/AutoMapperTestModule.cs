using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Settings;

namespace Volo.Abp.AutoMapper
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AbpObjectExtendingTestModule)
    )]
    public class AutoMapperTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<AutoMapperTestModule>();

                options.Configurators.Add(configurationContext =>
                {
                    configurationContext.MapperConfiguration.AddProfile(
                        new BookProfile(configurationContext.ServiceProvider.GetService<ISettingProvider>()));
                });
            });
        }
    }
}
