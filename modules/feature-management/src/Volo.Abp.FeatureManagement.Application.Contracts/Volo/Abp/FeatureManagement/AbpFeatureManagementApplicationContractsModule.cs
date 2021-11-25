using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.FeatureManagement.JsonConverters;
using Volo.Abp.Json;
using Volo.Abp.Json.Newtonsoft;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.FeatureManagement;

[DependsOn(
    typeof(AbpFeatureManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationAbstractionsModule),
    typeof(AbpJsonModule)
    )]
public class AbpFeatureManagementApplicationContractsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpFeatureManagementApplicationContractsModule>();
        });

        Configure<AbpFeatureManagementApplicationContractsOptions>(options =>
        {
            context.Services.ExecutePreConfiguredActions(options);
        });
        
        Configure<AbpNewtonsoftJsonSerializerOptions>(options =>
        {
            options.Converters.Add<NewtonsoftStringValueTypeJsonConverter>();
        });

        Configure<AbpSystemTextJsonSerializerOptions>(options =>
        {
            var contractsOptions = context.Services.ExecutePreConfiguredActions<AbpFeatureManagementApplicationContractsOptions>();
            options.JsonSerializerOptions.Converters.AddIfNotContains(new StringValueTypeJsonConverter(contractsOptions));
        });
    }
}
