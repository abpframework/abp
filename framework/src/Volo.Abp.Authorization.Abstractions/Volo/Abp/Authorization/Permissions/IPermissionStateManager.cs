using System.Threading.Tasks;

namespace Volo.Abp.Authorization.Permissions
{
    public interface IPermissionStateManager
    {
        Task<bool> IsEnabledAsync(PermissionDefinition permission);
    }
}
