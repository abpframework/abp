using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Abstractions;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Volo.Abp.Security;
using Volo.Abp.Settings;

namespace Volo.Abp.MultiTenancy;

[DependsOn(
    typeof(AbpDataModule),
    typeof(AbpSecurityModule),
    typeof(AbpSettingsModule),
    typeof(AbpEventBusAbstractionsModule),
    typeof(AbpMultiTenancyAbstractionsModule)
    )]
public class AbpMultiTenancyModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<ICurrentTenantAccessor>(AsyncLocalCurrentTenantAccessor.Instance);

        var configuration = context.Services.GetConfiguration();
        Configure<AbpDefaultTenantStoreOptions>(configuration);

        Configure<AbpSettingOptions>(options =>
        {
            options.ValueProviders.InsertAfter(t => t == typeof(GlobalSettingValueProvider), typeof(TenantSettingValueProvider));
        });

        Configure<AbpTenantResolveOptions>(options =>
        {
            options.TenantResolvers.Insert(0, new CurrentUserTenantResolveContributor());
        });
    }
}
