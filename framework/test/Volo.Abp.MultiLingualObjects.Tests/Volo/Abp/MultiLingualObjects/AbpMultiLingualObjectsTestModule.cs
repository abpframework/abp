using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Settings;

namespace Volo.Abp.MultiLingualObjects;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpLocalizationModule),
    typeof(AbpSettingsModule),
    typeof(AbpObjectMappingModule),
    typeof(AbpMultiLingualObjectsModule),
    typeof(AbpTestBaseModule),
    typeof(AbpAutoMapperModule)
)]
public class AbpMultiLingualObjectsTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpSettingOptions>(options =>
        {   
            options.DefinitionProviders.Add<LocalizationSettingProvider>();
        });
        context.Services.AddAutoMapperObjectMapper<AbpMultiLingualObjectsTestModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AbpMultiLingualObjectsTestModule>(validate: true);
        });
    }
}
