﻿using System.Collections.Generic;
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

        var contractsOptionsActions = context.Services.GetPreConfigureActions<AbpFeatureManagementApplicationContractsOptions>();
        Configure<AbpFeatureManagementApplicationContractsOptions>(options =>
        {
            contractsOptionsActions.Configure(options);
        });

        Configure<AbpNewtonsoftJsonSerializerOptions>(options =>
        {
            options.Converters.Add<NewtonsoftStringValueTypeJsonConverter>();
        });

        Configure<AbpSystemTextJsonSerializerOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.AddIfNotContains(new StringValueTypeJsonConverter(contractsOptionsActions.Configure()));
        });
    }
}
