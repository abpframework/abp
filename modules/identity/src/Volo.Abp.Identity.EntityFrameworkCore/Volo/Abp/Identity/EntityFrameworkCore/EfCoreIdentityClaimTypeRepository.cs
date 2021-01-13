using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Identity.EntityFrameworkCore
{
    public class EfCoreIdentityClaimTypeRepository : EfCoreRepository<IIdentityDbContext, IdentityClaimType, Guid>, IIdentityClaimTypeRepository
    {
        public EfCoreIdentityClaimTypeRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<bool> AnyAsync(
            string name,
            Guid? ignoredId = null,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .WhereIf(ignoredId != null, ct => ct.Id != ignoredId)
                .CountAsync(ct => ct.Name == name, GetCancellationToken(cancellationToken)) > 0;
        }

        public virtual async Task<List<IdentityClaimType>> GetListAsync(
            string sorting,
            int maxResultCount,
            int skipCount,
            string filter,
            CancellationToken cancellationToken = default)
        {
            var identityClaimTypes = await (await GetDbSetAsync())
                .WhereIf(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.Name.Contains(filter)
                )
                .OrderBy(sorting ?? "name desc")
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));

            return identityClaimTypes;
        }

        public virtual async Task<long> GetCountAsync(
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .WhereIf(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.Name.Contains(filter)
                ).LongCountAsync(GetCancellationToken(cancellationToken));
        }
    }
}
