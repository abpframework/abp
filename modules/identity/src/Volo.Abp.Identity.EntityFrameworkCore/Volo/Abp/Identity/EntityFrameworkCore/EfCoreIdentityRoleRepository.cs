using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Guids;

namespace Volo.Abp.Identity.EntityFrameworkCore
{
    public class EfCoreIdentityRoleRepository : EfCoreRepository<IIdentityDbContext, IdentityRole, Guid>, IIdentityRoleRepository
    {
        public EfCoreIdentityRoleRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<IdentityRole> FindByNormalizedNameAsync(
            string normalizedRoleName, 
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, GetCancellationToken(cancellationToken)).ConfigureAwait(false);
        }

        public virtual async Task<List<IdentityRole>> GetListAsync(
            string sorting = null, 
            int maxResultCount = int.MaxValue, 
            int skipCount = 0, 
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .OrderBy(sorting ?? nameof(IdentityRole.Name))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken)).ConfigureAwait(false);
        }

        public override IQueryable<IdentityRole> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }
}