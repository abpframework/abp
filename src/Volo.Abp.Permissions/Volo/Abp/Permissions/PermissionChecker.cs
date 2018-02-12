using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Permissions
{
    public class PermissionChecker : IPermissionChecker, ITransientDependency
    {
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }

        protected Lazy<List<IPermissionValueProvider>> Providers { get; }

        protected PermissionOptions Options { get; }

        public PermissionChecker(
            IOptions<PermissionOptions> options,
            IServiceProvider serviceProvider,
            IPermissionDefinitionManager permissionDefinitionManager)
        {
            PermissionDefinitionManager = permissionDefinitionManager;
            Options = options.Value;

            Providers = new Lazy<List<IPermissionValueProvider>>(
                () => Options
                    .ValueProviders
                    .Select(c => serviceProvider.GetRequiredService(c) as IPermissionValueProvider)
                    .ToList(),
                true
            );
        }

        public async Task<bool> IsGrantedAsync(string name)
        {
            var permission = PermissionDefinitionManager.Get(name);

            foreach (var provider in Providers.Value)
            {
                if (await provider.IsGrantedAsync(permission))
                {
                    return true;
                }
            }

            return false;
        }

        public virtual async Task<List<PermissionGrantInfo>> GetAllAsync()
        {
            var permissionDefinitions = PermissionDefinitionManager.GetAll();
            var permissionGrantInfos = new Dictionary<string, PermissionGrantInfo>();

            foreach (var permission in permissionDefinitions)
            {
                permissionGrantInfos[permission.Name] = await GetPermissionGrantInfo(permission);
            }

            return permissionGrantInfos.Values.ToList();
        }

        private async Task<PermissionGrantInfo> GetPermissionGrantInfo(PermissionDefinition permission)
        {
            foreach (var provider in Providers.Value)
            {
                if (await provider.IsGrantedAsync(permission))
                {
                    return new PermissionGrantInfo(permission.Name, true, provider.Name);
                }
            }

            return new PermissionGrantInfo(permission.Name, false);
        }
    }
}