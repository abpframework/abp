using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        Task<PermissionGrantInfoDto> GetAsync([NotNull] string name, [NotNull] string providerName, [NotNull] string providerKey);

        Task<List<PermissionGrantInfoDto>> GetListAsync([NotNull] string providerName, [NotNull] string providerKey);

        Task UpdateAsync([NotNull] string providerName, [NotNull] string providerKey, UpdatePermissionsDto input);
    }
}
