using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Permissions
{
    public interface IPermissionChecker
    {
        Task<PermissionGrantInfo> CheckAsync([NotNull]string name);

        Task<List<PermissionGrantInfo>> GetAllAsync();
    }
}