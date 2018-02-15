using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Permissions
{
    public interface IPermissionManagementProvider : ISingletonDependency //TODO: Create an abstract base class.
    {
        string Name { get; }

        Task<PermissionValueProviderGrantInfo> CheckAsync(string name, string providerName, string providerKey);
    }
}