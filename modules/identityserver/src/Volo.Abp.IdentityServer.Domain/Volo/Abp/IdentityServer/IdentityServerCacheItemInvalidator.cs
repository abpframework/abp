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
        protected IServiceProvider ServiceProvider { get; }

        public IdentityServerCacheItemInvalidator(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public virtual async Task HandleEventAsync(EntityChangedEventData<Client> eventData)
        {
            var cache = ServiceProvider.GetRequiredService<ICache<IdentityServer4.Models.Client>>();

            if (cache is IdentityServerDistributedCache<IdentityServer4.Models.Client> identityServerDistributedCache)
            {
                await identityServerDistributedCache.RemoveAsync(eventData.Entity.ClientId);
            }
            else
            {
                LogUnCompatibleError<Client>();
            }
        }

        public virtual async Task HandleEventAsync(EntityChangedEventData<IdentityResource> eventData)
        {
            var cache = ServiceProvider
                .GetRequiredService<ICache<IEnumerable<IdentityServer4.Models.IdentityResource>>>();

            if (cache is IdentityServerDistributedCache<IEnumerable<IdentityServer4.Models.IdentityResource>> identityServerDistributedCache)
            {
                await identityServerDistributedCache.RemoveAllAsync();
            }
            else
            {
                LogUnCompatibleError<IEnumerable<IdentityServer4.Models.IdentityResource>>();
            }
        }

        public virtual async Task HandleEventAsync(EntityChangedEventData<ApiResource> eventData)
        {
            var cache = ServiceProvider
                .GetRequiredService<ICache<IEnumerable<IdentityServer4.Models.ApiResource>>>();

            if (cache is IdentityServerDistributedCache<IEnumerable<IdentityServer4.Models.ApiResource>> identityServerDistributedCache)
            {
                await identityServerDistributedCache.RemoveAllAsync();
            }
            else
            {
                LogUnCompatibleError<IEnumerable<IdentityServer4.Models.ApiResource>>();
            }
        }

        public virtual async Task HandleEventAsync(EntityChangedEventData<ApiScope> eventData)
        {
            var cache = ServiceProvider
                .GetRequiredService<ICache<IEnumerable<IdentityServer4.Models.ApiScope>>>();

            if (cache is IdentityServerDistributedCache<IEnumerable<IdentityServer4.Models.ApiScope>> identityServerDistributedCache)
            {
                await identityServerDistributedCache.RemoveAllAsync();
            }
            else
            {
                LogUnCompatibleError<IEnumerable<IdentityServer4.Models.ApiScope>>();
            }
        }

        protected void LogUnCompatibleError<T>()
            where T : class
        {
            ServiceProvider.GetRequiredService<ILogger<IdentityServerCacheItemInvalidator>>()
                .LogError($"The implementation of {nameof(ICache<T>)} is not compatible with {nameof(IdentityServerCacheItemInvalidator)}");
        }
    }
}
