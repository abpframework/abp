using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Authorization.Permissions
{
    public class PermissionChecker : IPermissionChecker, ITransientDependency
    {
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
        protected ICurrentPrincipalAccessor PrincipalAccessor { get; }
        protected ICurrentTenant CurrentTenant { get; }
        protected IPermissionValueProviderManager PermissionValueProviderManager { get; }

        public PermissionChecker(
            ICurrentPrincipalAccessor principalAccessor,
            IPermissionDefinitionManager permissionDefinitionManager, 
            ICurrentTenant currentTenant,
            IPermissionValueProviderManager permissionValueProviderManager)
        {
            PrincipalAccessor = principalAccessor;
            PermissionDefinitionManager = permissionDefinitionManager;
            CurrentTenant = currentTenant;
            PermissionValueProviderManager = permissionValueProviderManager;
        }

        public virtual Task<bool> IsGrantedAsync(string name)
        {
            return IsGrantedAsync(PrincipalAccessor.Principal, name);
        }

        public virtual async Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name)
        {
            Check.NotNull(name, nameof(name));

            var permission = PermissionDefinitionManager.Get(name);

            var multiTenancySide = claimsPrincipal?.GetMultiTenancySide()
                                   ?? CurrentTenant.GetMultiTenancySide();

            if (!permission.MultiTenancySide.HasFlag(multiTenancySide))
            {
                return false;
            }

            var isGranted = false;
            var context = new PermissionValueCheckContext(permission, claimsPrincipal);
            foreach (var provider in PermissionValueProviderManager.ValueProviders)
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