using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;

namespace Volo.Abp.Identity
{
    public class EfCoreIdentityRoleRepository : EfCoreRepository<IdentityDbContext, IdentityRole>, IIdentityRoleRepository
    {
        public EfCoreIdentityRoleRepository(IDbContextProvider<IdentityDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public Task<IdentityRole> FindByNormalizedNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return DbSet.FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, cancellationToken);
        }

        public async Task<List<IdentityRole>> GetListAsync(string sorting, int maxResultCount, int skipCount)
        {
            return await this.OrderBy(sorting ?? nameof(IdentityRole.Name)).PageBy(skipCount, maxResultCount).ToListAsync();
        }
    }
}