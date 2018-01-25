using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Identity
{
    public interface IIdentityRoleRepository : IRepository<IdentityRole, Guid>
    {
        Task<IdentityRole> FindByNormalizedNameAsync(string normalizedRoleName, CancellationToken cancellationToken);

        Task<List<IdentityRole>> GetListAsync(string sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0);

        Task<long> GetCountAsync(CancellationToken cancellationToken = default);
    }
}