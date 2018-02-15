using System.Threading.Tasks;

namespace Volo.Abp.Permissions
{
    public interface IPermissionManagementProvider
    {
        string Name { get; }

        Task<PermissionValueProviderGrantInfo> CheckAsync(string name, string providerName, string providerKey);
    }
}