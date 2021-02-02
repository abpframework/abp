using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Volo.Abp.Domain;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace Volo.Abp.PermissionManagement
{
    [DependsOn(typeof(AbpAuthorizationModule))]
    [DependsOn(typeof(AbpDddDomainModule))]
    [DependsOn(typeof(AbpPermissionManagementDomainSharedModule))]
    [DependsOn(typeof(AbpCachingModule))]
    [DependsOn(typeof(AbpJsonModule))]
    public class AbpPermissionManagementDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpDistributedEventBusOptions>(options =>
            {
                var serviceProvider = context.Services.GetServiceProviderOrNull();
                if (serviceProvider != null)
                {
                    var abpIdentityOptions = serviceProvider.GetRequiredService<IOptions<PermissionManagementOptions>>().Value;
                    if (!abpIdentityOptions.IsDistributedEventHandlingEnabled)
                    {
                        var identityDomainAssembly = typeof(AbpPermissionManagementDomainModule).Assembly;
                        options.Handlers.RemoveAll(x => x.Assembly == identityDomainAssembly);
                    }
                }
            });
        }
    }
}
