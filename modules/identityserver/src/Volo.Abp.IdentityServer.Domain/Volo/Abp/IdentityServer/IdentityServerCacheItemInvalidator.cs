using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;
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
        ILocalEventHandler<EntityChangedEventData<ApiScope>>,
        ILocalEventHandler<EntityChangedEventData<ClientCorsOrigin>>
    {
        protected IServiceProvider ServiceProvider { get; }

        public IdentityServerCacheItemInvalidator(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public virtual async Task HandleEventAsync(EntityChangedEventData<Client> eventData)
        {
            var cache = ServiceProvider.GetRequiredService<IdentityServerDistributedCache<IdentityServer4.Models.Client>>();
            await cache.RemoveAsync(eventData.Entity.ClientId);

            var corsCache = ServiceProvider
                .GetRequiredService<IdentityServerDistributedCache<CachingCorsPolicyService<AbpCorsPolicyService>.CorsCacheEntry>>();
            await corsCache.RemoveAllAsync();
        }

        public virtual async Task HandleEventAsync(EntityChangedEventData<IdentityResource> eventData)
        {
            var cache = ServiceProvider.GetRequiredService<IdentityServerDistributedCache<IEnumerable<IdentityServer4.Models.IdentityResource>>>();
            await cache.RemoveAllAsync();

            var resourcesCache = ServiceProvider
                .GetRequiredService<IdentityServerDistributedCache<IdentityServer4.Models.Resources>>();
            await resourcesCache.RemoveAllAsync();
        }

        public virtual async Task HandleEventAsync(EntityChangedEventData<ApiResource> eventData)
        {
            var cache = ServiceProvider
                .GetRequiredService<IdentityServerDistributedCache<IEnumerable<IdentityServer4.Models.ApiResource>>>();
            await cache.RemoveAllAsync();

            var resourcesCache = ServiceProvider
                .GetRequiredService<IdentityServerDistributedCache<IdentityServer4.Models.Resources>>();
            await resourcesCache.RemoveAllAsync();
        }

        public virtual async Task HandleEventAsync(EntityChangedEventData<ApiScope> eventData)
        {
            var cache = ServiceProvider
                .GetRequiredService<IdentityServerDistributedCache<IEnumerable<IdentityServer4.Models.ApiScope>>>();
            await cache.RemoveAllAsync();

            var resourcesCache = ServiceProvider
                .GetRequiredService<IdentityServerDistributedCache<IdentityServer4.Models.Resources>>();
            await resourcesCache.RemoveAllAsync();
        }

        public async Task HandleEventAsync(EntityChangedEventData<ClientCorsOrigin> eventData)
        {
            var cache = ServiceProvider
                .GetRequiredService<IdentityServerDistributedCache<CachingCorsPolicyService<AbpCorsPolicyService>.CorsCacheEntry>>();
            await cache.RemoveAllAsync();
        }
    }
}
