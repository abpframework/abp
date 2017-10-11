using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Identity
{
    public interface IIdentityUserRepository : IRepository<IdentityUser>
    {
        Task<IdentityUser> FindByNormalizedUserNameAsync([NotNull] string normalizedUserName, CancellationToken cancellationToken);

        Task<List<string>> GetRoleNamesAsync(Guid userId);

        Task<IdentityUser> FindByLoginAsync([NotNull] string loginProvider, [NotNull] string providerKey, CancellationToken cancellationToken);

        Task<IdentityUser> FindByNormalizedEmailAsync([NotNull] string normalizedEmail, CancellationToken cancellationToken);

        //TODO: Why not return List instead of IList
        Task<IList<IdentityUser>> GetListByClaimAsync(Claim claim, CancellationToken cancellationToken);

        //TODO: Why not return List instead of IList
        Task<IList<IdentityUser>> GetListByNormalizedRoleNameAsync(string normalizedRoleName, CancellationToken cancellationToken);

        //TODO: DTO can be used instead of parameters
        Task<List<IdentityUser>> GetListAsync(string sorting, int maxResultCount, int skipCount, string filter);

        Task<List<IdentityRole>> GetRolesAsync(Guid userId);
    }
}
