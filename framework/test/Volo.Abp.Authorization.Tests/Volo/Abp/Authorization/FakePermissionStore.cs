using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Authorization
{
    public class FakePermissionStore : IPermissionStore, ITransientDependency
    {
        public Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
        {
            var result = name == "MenuPermission1" || name == "MenuPermission4";
            return Task.FromResult(result);
        }

        public Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names, string providerName, string providerKey)
        {
            var result = new MultiplePermissionGrantResult();
            foreach (var name in names)
            {
                result.Result.Add(name, name == "MenuPermission1" || name == "MenuPermission4"
                    ? PermissionGrantResult.Granted
                    : PermissionGrantResult.Prohibited);
            }

            return Task.FromResult(result);
        }
    }
}
