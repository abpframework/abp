﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.IdentityModel
{
    [DependsOn(
        typeof(AbpThreadingModule),
        typeof(AbpMultiTenancyModule)
        )]
    public class AbpIdentityModelModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            context.Services.AddHttpClient(IdentityModelAuthenticationService.HttpClientName);

            Configure<AbpIdentityClientOptions>(configuration);
        }
    }
}
