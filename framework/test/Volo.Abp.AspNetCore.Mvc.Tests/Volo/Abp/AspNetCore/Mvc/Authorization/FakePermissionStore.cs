using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.Authorization
{
    public class FakePermissionStore : IPermissionStore, ITransientDependency
    {
        public Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
        {
            var result = name == "TestPermission1" &&
                         providerName == UserPermissionValueProvider.ProviderName &&
                         providerKey == AuthTestController.FakeUserId.ToString();

            return Task.FromResult(result);
        }

        public Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names, string providerName, string providerKey)
        {
            var result = new MultiplePermissionGrantResult();
            foreach (var name in names)
            {
                result.Result.Add(name, name == "TestPermission1" &&
                                        providerName == UserPermissionValueProvider.ProviderName &&
                                        providerKey == AuthTestController.FakeUserId.ToString()
                    ? PermissionGrantResult.Granted
                    : PermissionGrantResult.Prohibited);
            }

            return Task.FromResult(result);
        }
    }
}
