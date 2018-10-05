using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Identity
{
    public interface IIdentityRoleRepository : IBasicRepository<IdentityRole, Guid>
    {
        Task<IdentityRole> FindByNormalizedNameAsync(
            string normalizedRoleName,
            bool includeDetails = true,
            CancellationToken cancellationToken = default
        );

        Task<List<IdentityRole>> GetListAsync(
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        Task UpdateClaimsAsync(Guid id, List<IdentityRoleClaim> claims);

        Task<List<IdentityRoleClaim>> GetClaimsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(
            CancellationToken cancellationToken = default
        );
    }
}