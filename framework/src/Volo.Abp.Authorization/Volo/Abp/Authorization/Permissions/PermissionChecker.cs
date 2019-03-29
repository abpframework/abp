using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Authorization.Permissions
{
    public class PermissionChecker : IPermissionChecker, ISingletonDependency
    {
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }

        protected IReadOnlyList<IPermissionValueProvider> ValueProviders => _lazyProviders.Value;

        protected ICurrentPrincipalAccessor PrincipalAccessor { get; }

        protected PermissionOptions Options { get; }

        private readonly Lazy<List<IPermissionValueProvider>> _lazyProviders;

        public PermissionChecker(
            IOptions<PermissionOptions> options,
            IServiceProvider serviceProvider,
            ICurrentPrincipalAccessor principalAccessor,
            IPermissionDefinitionManager permissionDefinitionManager)
        {
            PrincipalAccessor = principalAccessor;
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

        public virtual Task<bool> IsGrantedAsync(string name)
        {
            return IsGrantedAsync(PrincipalAccessor.Principal, name);
        }

        public virtual async Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name)
        {
            Check.NotNull(name, nameof(name));

            var isGranted = false;

            var permission = PermissionDefinitionManager.Get(name);
            var context = new PermissionValueCheckContext(permission, claimsPrincipal);
            foreach (var provider in ValueProviders)
            {
                if (context.Permission.Providers.Any() &&
                    !context.Permission.Providers.Contains(provider.Name))
                {
                    continue;
                }

                var result = await provider.CheckAsync(context);

                if (result == PermissionGrantResult.Granted)
                {
                    isGranted = true;
                }
                else if (result == PermissionGrantResult.Prohibited)
                {
                    return false;
                }
            }

            return isGranted;
        }
    }
}