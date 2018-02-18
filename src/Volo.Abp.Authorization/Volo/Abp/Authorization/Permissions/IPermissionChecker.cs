using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Authorization.Permissions
{
    public interface IPermissionChecker
    {
        Task<PermissionGrantInfo> CheckAsync([NotNull]string name);
    }
}