using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Abp.PermissionManagement
{
    public class PermissionManager : IPermissionManager, ISingletonDependency
    {
        protected IPermissionGrantRepository PermissionGrantRepository { get; }

        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }

        protected IGuidGenerator GuidGenerator { get; }

        protected IReadOnlyList<IPermissionManagementProvider> ManagementProviders => _lazyProviders.Value;

        protected PermissionManagementOptions Options { get; }

        private readonly Lazy<List<IPermissionManagementProvider>> _lazyProviders;

        public PermissionManager(
            IPermissionDefinitionManager permissionDefinitionManager,
            IPermissionGrantRepository permissionGrantRepository,
            IServiceProvider serviceProvider,
            IGuidGenerator guidGenerator,
            IOptions<PermissionManagementOptions> options)
        {
            GuidGenerator = guidGenerator;
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

        public async Task<PermissionWithGrantedProviders> GetAsync(string permissionName, string providerName, string providerKey)
        {
            return await GetInternalAsync(PermissionDefinitionManager.Get(permissionName), providerName, providerKey);
        }

        public async Task<List<PermissionWithGrantedProviders>> GetAllAsync(string providerName, string providerKey)
        {
            var results = new List<PermissionWithGrantedProviders>();

            foreach (var permissionDefinition in PermissionDefinitionManager.GetPermissions())
            {
                results.Add(await GetInternalAsync(permissionDefinition, providerName, providerKey));
            }

            return results;
        }

        public async Task SetAsync(string permissionName, string providerName, string providerKey, bool isGranted)
        {
            var currentGrantInfo = await GetAsync(permissionName, providerName, providerKey);
            if (currentGrantInfo.IsGranted == isGranted)
            {
                return;
            }

            var provider = ManagementProviders.FirstOrDefault(m => m.Name == providerName);
            if (provider == null)
            {
                throw new AbpException("Unknown permission management provider: " + providerName);
            }

            await provider.SetAsync(permissionName, providerKey, isGranted);
        }

        protected virtual async Task<PermissionWithGrantedProviders> GetInternalAsync(PermissionDefinition permissionDefinition, string providerName, string providerKey)
        {
            var result = new PermissionWithGrantedProviders(permissionDefinition.Name, false);

            foreach (var provider in ManagementProviders)
            {
                var providerResult = await provider.CheckAsync(permissionDefinition.Name, providerName, providerKey);
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