using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.PermissionManagement
{
    public class PermissionManager : IPermissionManager, ISingletonDependency
    {
        protected IPermissionGrantRepository PermissionGrantRepository { get; }

        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }

        protected IGuidGenerator GuidGenerator { get; }

        protected ICurrentTenant CurrentTenant { get; }

        protected IReadOnlyList<IPermissionManagementProvider> ManagementProviders => _lazyProviders.Value;

        protected PermissionManagementOptions Options { get; }
        
        protected IDistributedCache<PermissionGrantCacheItem> Cache { get; }

        private readonly Lazy<List<IPermissionManagementProvider>> _lazyProviders;

        public PermissionManager(
            IPermissionDefinitionManager permissionDefinitionManager,
            IPermissionGrantRepository permissionGrantRepository,
            IServiceProvider serviceProvider,
            IGuidGenerator guidGenerator,
            IOptions<PermissionManagementOptions> options,
            ICurrentTenant currentTenant, 
            IDistributedCache<PermissionGrantCacheItem> cache)
        {
            GuidGenerator = guidGenerator;
            CurrentTenant = currentTenant;
            Cache = cache;
            PermissionGrantRepository = permissionGrantRepository;
            PermissionDefinitionManager = permissionDefinitionManager;
            Options = options.Value;

            _lazyProviders = new Lazy<List<IPermissionManagementProvider>>(
                () => Options
                    .ManagementProviders
                    .Select(c => serviceProvider.GetRequiredService(c) as IPermissionManagementProvider)
                    .ToList(),
                true
            );
        }

        public virtual async Task<PermissionWithGrantedProviders> GetAsync(string permissionName, string providerName, string providerKey)
        {
            return await GetInternalAsync(PermissionDefinitionManager.Get(permissionName), providerName, providerKey);
        }

        public virtual async Task<List<PermissionWithGrantedProviders>> GetAllAsync(string providerName, string providerKey)
        {
            var results = new List<PermissionWithGrantedProviders>();

            foreach (var permissionDefinition in PermissionDefinitionManager.GetPermissions())
            {
                results.Add(await GetInternalAsync(permissionDefinition, providerName, providerKey));
            }

            return results;
        }

        public virtual async Task SetAsync(string permissionName, string providerName, string providerKey, bool isGranted)
        {
            var permission = PermissionDefinitionManager.Get(permissionName);

            if (!permission.IsEnabled)
            {
                //TODO: BusinessException
                throw new ApplicationException($"The permission named '{permission.Name}' is disabled!");
            }

            if (permission.Providers.Any() && !permission.Providers.Contains(providerName))
            {
                //TODO: BusinessException
                throw new ApplicationException($"The permission named '{permission.Name}' has not compatible with the provider named '{providerName}'");
            }

            if (!permission.MultiTenancySide.HasFlag(CurrentTenant.GetMultiTenancySide()))
            {
                //TODO: BusinessException
                throw new ApplicationException($"The permission named '{permission.Name}' has multitenancy side '{permission.MultiTenancySide}' which is not compatible with the current multitenancy side '{CurrentTenant.GetMultiTenancySide()}'");
            }

            var currentGrantInfo = await GetInternalAsync(permission, providerName, providerKey);
            if (currentGrantInfo.IsGranted == isGranted)
            {
                return;
            }

            var provider = ManagementProviders.FirstOrDefault(m => m.Name == providerName);
            if (provider == null)
            {
                //TODO: BusinessException
                throw new AbpException("Unknown permission management provider: " + providerName);
            }

            await provider.SetAsync(permissionName, providerKey, isGranted);
        }
        
        public virtual async Task<PermissionGrant> UpdateProviderKeyAsync(PermissionGrant permissionGrant, string providerKey)
        {
            using (CurrentTenant.Change(permissionGrant.TenantId))
            {
                //Invalidating the cache for the old key
                await Cache.RemoveAsync(
                    PermissionGrantCacheItem.CalculateCacheKey(
                        permissionGrant.Name,
                        permissionGrant.ProviderName,
                        permissionGrant.ProviderKey
                    )
                );
            }
            
            permissionGrant.ProviderKey = providerKey;
            return await PermissionGrantRepository.UpdateAsync(permissionGrant);
        }

        public virtual async Task DeleteAsync(string providerName, string providerKey)
        {
            var permissionGrants = await PermissionGrantRepository.GetListAsync(providerName, providerKey);
            foreach (var permissionGrant in permissionGrants)
            {
                await PermissionGrantRepository.DeleteAsync(permissionGrant);
            }
        }

        protected virtual async Task<PermissionWithGrantedProviders> GetInternalAsync(PermissionDefinition permission, string providerName, string providerKey)
        {
            var result = new PermissionWithGrantedProviders(permission.Name, false);

            if (!permission.IsEnabled)
            {
                return result;
            }

            if (!permission.MultiTenancySide.HasFlag(CurrentTenant.GetMultiTenancySide()))
            {
                return result;
            }

            if (permission.Providers.Any() && !permission.Providers.Contains(providerName))
            {
                return result;
            }

            foreach (var provider in ManagementProviders)
            {
                var providerResult = await provider.CheckAsync(permission.Name, providerName, providerKey);
                if (providerResult.IsGranted)
                {
                    result.IsGranted = true;
                    result.Providers.Add(new PermissionValueProviderInfo(provider.Name, providerResult.ProviderKey));
                }
            }

            return result;
        }
    }
}
