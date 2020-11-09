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

        public async override Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
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

        public async override Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context)
        {
            var result = new MultiplePermissionGrantResult();
            var permissionNames = context.Permissions.Select(x => x.Name).ToList();
            foreach (var name in permissionNames)
            {
                result.Result.Add(name, PermissionGrantResult.Undefined);
            }

            var roles = context.Principal?.FindAll(AbpClaimTypes.Role).Select(c => c.Value).ToArray();
            if (roles == null || !roles.Any())
            {
                return result;
            }

            foreach (var role in roles)
            {
                foreach (var grantResult in (await PermissionStore.IsGrantedAsync(permissionNames.ToArray(), Name, role)).Result)
                {
                    if (result.Result.ContainsKey(grantResult.Key) &&
                        result.Result[grantResult.Key] == PermissionGrantResult.Undefined &&
                        grantResult.Value != PermissionGrantResult.Undefined)
                    {
                        result.Result[grantResult.Key] = grantResult.Value;
                        permissionNames.Remove(grantResult.Key);
                    }
                }

                if (result.AllGranted)
                {
                    break;
                }
            }

            return result;
        }
    }
}
