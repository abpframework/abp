using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Permissions
{
    public class PermissionChecker : IPermissionChecker, ISingletonDependency
    {
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }

        protected List<IPermissionValueProvider> Providers => _lazyProviders.Value;
        private readonly Lazy<List<IPermissionValueProvider>> _lazyProviders;

        protected PermissionOptions Options { get; }

        public PermissionChecker(
            IOptions<PermissionOptions> options,
            IServiceProvider serviceProvider,
            IPermissionDefinitionManager permissionDefinitionManager)
        {
            PermissionDefinitionManager = permissionDefinitionManager;
            Options = options.Value;

            _lazyProviders = new Lazy<List<IPermissionValueProvider>>(
                () => Options
                    .ValueProviders
                    .Select(c => serviceProvider.GetRequiredService(c) as IPermissionValueProvider)
                    .ToList(),
                true
            );
        }

        public Task<PermissionGrantInfo> CheckAsync(string name)
        {
            var permission = PermissionDefinitionManager.Get(name);

            return GetPermissionGrantInfo(permission);
        }

        protected virtual async Task<PermissionGrantInfo> GetPermissionGrantInfo(PermissionDefinition permission)
        {
            foreach (var provider in Providers)
            {
                var result = await provider.CheckAsync(permission);
                if (result.IsGranted)
                {
                    return new PermissionGrantInfo(permission.Name, true, provider.Name, result.ProviderKey);
                }
            }

            return new PermissionGrantInfo(permission.Name, false);
        }
    }
}