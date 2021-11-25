using System.Collections.Generic;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.FeatureManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.FeatureManagement.JsonConverters;

namespace Volo.Abp.FeatureManagement;

[DependsOn(
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class AbpFeatureManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpFeatureManagementHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpFeatureManagementResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });

        Configure<JsonOptions>(options =>
        {
            var contractsOptions = context.Services.ExecutePreConfiguredActions<AbpFeatureManagementApplicationContractsOptions>();
            options.JsonSerializerOptions.Converters.AddIfNotContains(new StringValueTypeJsonConverter(contractsOptions));
        });
    }
}
