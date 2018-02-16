using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Abp.Permissions
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

        public async Task<PermissionWithGrantedProviders> GetAsync(string name, string providerName, string providerKey)
        {
            return await GetInternalAsync(PermissionDefinitionManager.Get(name), providerName, providerKey);
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

        public async Task SetAsync(string name, string providerName, string providerKey, bool isGranted)
        {
            var currentGrantInfo = await GetAsync(name, providerName, providerKey);
            if (currentGrantInfo.IsGranted == isGranted)
            {
                return;
            }

            if (currentGrantInfo.IsGranted == false)
            {
                var provider = ManagementProviders.FirstOrDefault(m => m.Name == providerName);
                await provider.GrantAsync(name, providerKey);
            }
            else
            {
                if (currentGrantInfo.Providers.All(p => p.Name != providerName))
                {
                    return;
                }

                var provider = ManagementProviders.FirstOrDefault(m => m.Name == providerName);
                await provider.RevokeAsync(name, providerKey);
            }
        }

        public async Task<PermissionWithGrantedProviders> GetInternalAsync(PermissionDefinition permissionDefinition, string providerName, string providerKey)
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