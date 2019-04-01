﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement
{
    [DependsOn(
        typeof(AbpFeatureManagementApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class AbpFeatureManagementHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "AbpFeatureManagement";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(AbpFeatureManagementApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
