using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Authorization.Permissions
{
    public class PermissionManager : IPermissionManager, ISingletonDependency
    {
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }

        protected Lazy<List<IPermissionValueProvider>> Providers { get; }

        protected PermissionOptions Options { get; }

        protected IPermissionStore PermissionStore { get; }

        public PermissionManager(
            IOptions<PermissionOptions> options,
            IServiceProvider serviceProvider,
            IPermissionDefinitionManager permissionDefinitionManager,
            IPermissionStore permissionStore)
        {
            PermissionStore = permissionStore;
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

        public virtual Task<bool> IsGrantedAsync(string name)
        {
            Check.NotNull(name, nameof(name));

            return IsGrantedInternalAsync(name, null, null);
        }

        public virtual Task<bool> IsGrantedAsync(string name, string providerName, string providerKey, bool fallback = true)
        {
            Check.NotNull(name, nameof(name));
            Check.NotNull(providerName, nameof(providerName));

            return IsGrantedInternalAsync(name, providerName, providerKey, fallback);
        }

        public virtual async Task<bool> IsGrantedInternalAsync(string name, string providerName, string providerKey, bool fallback = true)
        {
            var permission = PermissionDefinitionManager.Get(name);
            var providers = Enumerable
                .Reverse(Providers.Value);

            if (providerName != null)
            {
                providers = providers.SkipWhile(c => c.Name != providerName);
            }

            if (!fallback)
            {
                providers = providers.TakeWhile(c => c.Name == providerName);
            }

            foreach (var provider in providers)
            {
                var value = await provider.IsGrantedAsync(permission, providerKey);
                if (value != null)
                {
                    return value.Value;
                }
            }

            return false;
        }

        public virtual async Task<List<PermissionGrantInfo>> GetAllAsync()
        {
            var permissionGrantInfos = new Dictionary<string, PermissionGrantInfo>();
            var permissionDefinitions = PermissionDefinitionManager.GetAll();

            foreach (var provider in Providers.Value)
            {
                foreach (var permission in permissionDefinitions)
                {
                    var value = await provider.IsGrantedAsync(permission, null);
                    if (value != null)
                    {
                        permissionGrantInfos[permission.Name] = new PermissionGrantInfo(permission.Name, value.Value);
                    }
                }
            }

            return permissionGrantInfos.Values.ToList();
        }

        public virtual async Task<List<PermissionGrantInfo>> GetAllAsync(string providerName, string providerKey, bool fallback = true)
        {
            Check.NotNull(providerName, nameof(providerName));

            var permissionGrantInfos = new Dictionary<string, PermissionGrantInfo>();
            var permissionDefinitions = PermissionDefinitionManager.GetAll();
            var providers = Enumerable.Reverse(Providers.Value)
                .SkipWhile(c => c.Name != providerName);

            if (!fallback)
            {
                providers = providers.TakeWhile(c => c.Name == providerName);
            }

            var providerList = providers.Reverse().ToList();

            if (providerList.Any())
            {
                foreach (var permission in permissionDefinitions)
                {
                    foreach (var provider in providerList)
                    {
                        var value = await provider.IsGrantedAsync(permission, providerKey);
                        if (value != null)
                        {
                            permissionGrantInfos[permission.Name] = new PermissionGrantInfo(permission.Name, value.Value);
                        }
                    }
                }
            }

            return permissionGrantInfos.Values.ToList();
        }

        public virtual async Task SetAsync(string name, bool? isGranted, string providerName, string providerKey, bool forceToSet = false)
        {
            Check.NotNull(name, nameof(name));
            Check.NotNull(providerName, nameof(providerName));

            var permission = PermissionDefinitionManager.Get(name);

            var providers = Enumerable
                .Reverse(Providers.Value)
                .SkipWhile(p => p.Name != providerName)
                .ToList();

            if (!providers.Any())
            {
                return;
            }

            if (providers.Count > 1 && !forceToSet)
            {
                //Clear the value if it's same as it's fallback value
                var fallbackValue = await IsGrantedInternalAsync(name, providers[1].Name, providerKey);
                if (fallbackValue == isGranted)
                {
                    isGranted = null;
                }
            }

            providers = providers
                .TakeWhile(p => p.Name == providerName)
                .ToList(); //Getting list for case of there are more than one provider with same EntityType

            if (isGranted == null)
            {
                foreach (var provider in providers)
                {
                    await provider.ClearAsync(permission, providerKey);
                }
            }
            else
            {
                foreach (var provider in providers)
                {
                    await provider.SetAsync(permission, isGranted.Value, providerKey);
                }
            }
        }
    }
}