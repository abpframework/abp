using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Identity
{
    public interface IIdentityRoleRepository : IRepository<IdentityRole>
    {
        Task<IdentityRole> FindByNormalizedNameAsync(string normalizedRoleName, CancellationToken cancellationToken);

        Task<List<IdentityRole>> GetListAsync(string sorting, int maxResultCount, int skipCount, string filter);
    }
}