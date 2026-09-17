using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Identity;

public interface IUserRoleFinder
{
    [Obsolete("Use GetRoleNamesAsync instead.")]
    Task<string[]> GetRolesAsync(Guid userId);

    Task<string[]> GetRoleNamesAsync(Guid userId);

    Task<List<UserFinderResult>> SearchUserAsync(string filter, int page = 1);

    Task<List<RoleFinderResult>> SearchRoleAsync(string filter, int page = 1);

    Task<List<UserFinderResult>> SearchUserByIdsAsync(Guid[] ids);

    Task<List<RoleFinderResult>> SearchRoleByNamesAsync(string[] names);
}
