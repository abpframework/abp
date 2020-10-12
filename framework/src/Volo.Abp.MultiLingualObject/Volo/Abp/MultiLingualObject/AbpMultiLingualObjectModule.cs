using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Abp.MultiLingualObject
{
    [DependsOn(
        typeof(AbpLocalizationModule))]
    public class AbpMultiLingualObjectModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient(typeof(MultiLingualMappingAction<,,,>));
            context.Services.AddTransient(typeof(RecursiveMultiLingualMappingAction<,,,>));

            Configure<AbpAutoMapperOptions>(options =>
            {
                var sp = context.Services.BuildServiceProvider();
                options.Configurators.Add(ct => ct.MapperConfiguration.ConstructServicesUsing(sp.GetService));
            });
        }
    }
}
