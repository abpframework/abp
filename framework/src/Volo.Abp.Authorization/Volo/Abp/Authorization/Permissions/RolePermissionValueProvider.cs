using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Authorization.Permissions
{
    public class RolePermissionValueProvider : PermissionValueProvider
    {
        public const string ProviderName = "R";

        public override string Name => ProviderName;

        public RolePermissionValueProvider(IPermissionStore permissionStore)
            : base(permissionStore)
        {

        }

        public override async Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
        {
            var roles = context.Principal?.FindAll(AbpClaimTypes.Role).Select(c => c.Value).ToArray();

            if (roles == null || !roles.Any())
            {
                return PermissionGrantResult.Undefined;
            }

            foreach (var role in roles)
            {
                if (await PermissionStore.IsGrantedAsync(context.Permission.Name, Name, role))
                {
                    return PermissionGrantResult.Granted;
                }
            }

            return PermissionGrantResult.Undefined;
        }

        public override async Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context)
        {
            var permissionNames = context.Permissions.Select(x => x.Name).Distinct().ToArray();
            Check.NotNullOrEmpty(permissionNames, nameof(permissionNames));

            var result = new MultiplePermissionGrantResult(permissionNames);

            var roles = context.Principal?.FindAll(AbpClaimTypes.Role).Select(c => c.Value).ToArray();
            if (roles == null || !roles.Any())
            {
                return result;
            }

            foreach (var role in roles)
            {
                var multipleResult = await PermissionStore.IsGrantedAsync(permissionNames, Name, role);

                foreach (var grantResult in multipleResult.Result.Where(grantResult =>
                    result.Result.ContainsKey(grantResult.Key) &&
                    result.Result[grantResult.Key] == PermissionGrantResult.Undefined &&
                    grantResult.Value != PermissionGrantResult.Undefined))
                {
                    result.Result[grantResult.Key] = grantResult.Value;
                    permissionNames.RemoveAll(x => x == grantResult.Key);
                }

                if (result.AllGranted || result.AllProhibited)
                {
                    break;
                }

                if (permissionNames.IsNullOrEmpty())
                {
                    break;
                }
            }

            return result;
        }
    }
}
