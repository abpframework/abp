using System.Threading.Tasks;

namespace Volo.Abp.Authorization.Permissions
{
    public interface IPermissionStateProvider
    {
        Task<bool> IsEnabledAsync(PermissionStateContext context);
    }
}
