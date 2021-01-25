using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.IdentityResources;

namespace Volo.Abp.IdentityServer
{
    public class IdentityServerCacheItemInvalidator :
        ILocalEventHandler<EntityChangedEventData<Client>>,
        ILocalEventHandler<EntityChangedEventData<IdentityResource>>,
        ILocalEventHandler<EntityChangedEventData<ApiResource>>,
        ILocalEventHandler<EntityChangedEventData<ApiScope>>
    {
        protected IServiceScopeFactory ServiceScopeFactory { get; }

        public IdentityServerCacheItemInvalidator(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
        }

        public virtual async Task HandleEventAsync(EntityChangedEventData<Client> eventData)
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var cache = scope.ServiceProvider.GetRequiredService<ICache<IdentityServer4.Models.Client>>();

                if (cache is IdentityServerDistributedCache<IdentityServer4.Models.Client> identityServerDistributedCache)
                {
                    await identityServerDistributedCache.RemoveAsync(eventData.Entity.ClientId);
                }
                else
                {
                    LogUnCompatibleError<Client>(scope.ServiceProvider);
                }
            }
        }

        public virtual async Task HandleEventAsync(EntityChangedEventData<IdentityResource> eventData)
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var cache = scope.ServiceProvider
                    .GetRequiredService<ICache<IEnumerable<IdentityServer4.Models.IdentityResource>>>();

                if (cache is IdentityServerDistributedCache<IEnumerable<IdentityServer4.Models.IdentityResource>> identityServerDistributedCache)
                {
                    await identityServerDistributedCache.RemoveAllAsync();
                }
                else
                {
                    LogUnCompatibleError<IEnumerable<IdentityServer4.Models.IdentityResource>>(scope.ServiceProvider);
                }
            }
        }

        public virtual async Task HandleEventAsync(EntityChangedEventData<ApiResource> eventData)
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var cache = scope.ServiceProvider
                    .GetRequiredService<ICache<IEnumerable<IdentityServer4.Models.ApiResource>>>();

                if (cache is IdentityServerDistributedCache<IEnumerable<IdentityServer4.Models.ApiResource>> identityServerDistributedCache)
                {
                    await identityServerDistributedCache.RemoveAllAsync();
                }
                else
                {
                    LogUnCompatibleError<IEnumerable<IdentityServer4.Models.ApiResource>>(scope.ServiceProvider);
                }
            }
        }

        public virtual async Task HandleEventAsync(EntityChangedEventData<ApiScope> eventData)
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var cache = scope.ServiceProvider
                    .GetRequiredService<ICache<IEnumerable<IdentityServer4.Models.ApiScope>>>();

                if (cache is IdentityServerDistributedCache<IEnumerable<IdentityServer4.Models.ApiScope>> identityServerDistributedCache)
                {
                    await identityServerDistributedCache.RemoveAllAsync();
                }
                else
                {
                    LogUnCompatibleError<IEnumerable<IdentityServer4.Models.ApiScope>>(scope.ServiceProvider);
                }
            }
        }

        protected void LogUnCompatibleError<T>(IServiceProvider serviceProvider)
            where T : class
        {
            serviceProvider.GetRequiredService<ILogger<IdentityServerCacheItemInvalidator>>()
                .LogError($"The implementation of {nameof(ICache<T>)} is not compatible with {nameof(IdentityServerCacheItemInvalidator)}");
        }
    }
}
