using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using System;

namespace Volo.Abp.Identity
{
    public class EfCoreIdentityRoleRepository : EfCoreRepository<IIdentityDbContext, IdentityRole, Guid>, IIdentityRoleRepository
    {
        public EfCoreIdentityRoleRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public Task<IdentityRole> FindByNormalizedNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return DbSet.FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, cancellationToken);
        }

        public async Task<List<IdentityRole>> GetListAsync(string sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0)
        {
            return await this
                .OrderBy(sorting ?? nameof(IdentityRole.Name))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync();
        }

        public async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await this.LongCountAsync(cancellationToken);
        }
    }
}