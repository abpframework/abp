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
    }
}
