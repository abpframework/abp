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

        Task<IList<IdentityUser>> GetListByClaimAsync(Claim claim, CancellationToken cancellationToken);

        Task<IList<IdentityUser>> GetListByNormalizedRoleNameAsync(string normalizedRoleName, CancellationToken cancellationToken);
    }
}
