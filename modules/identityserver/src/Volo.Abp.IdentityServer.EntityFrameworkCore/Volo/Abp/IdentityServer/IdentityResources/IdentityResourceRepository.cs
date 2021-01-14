using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Volo.Abp.IdentityServer.IdentityResources
{
    public class IdentityResourceRepository : EfCoreRepository<IIdentityServerDbContext, IdentityResource, Guid>, IIdentityResourceRepository
    {
        public IdentityResourceRepository(IDbContextProvider<IIdentityServerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<List<IdentityResource>> GetListByScopeNameAsync(
            string[] scopeNames,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = from identityResource in (await GetDbSetAsync()).IncludeDetails(includeDetails)
                        where scopeNames.Contains(identityResource.Name)
                        select identityResource;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        [Obsolete("Use WithDetailsAsync method.")]
        public override IQueryable<IdentityResource> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }

        public override async Task<IQueryable<IdentityResource>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).IncludeDetails();
        }

        public virtual async Task<List<IdentityResource>> GetListAsync(string sorting, int skipCount, int maxResultCount,
            string filter, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .IncludeDetails(includeDetails)
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Name.Contains(filter) ||
                         x.Description.Contains(filter) ||
                         x.DisplayName.Contains(filter))
                .OrderBy(sorting ?? "name desc")
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .WhereIf(!filter.IsNullOrWhiteSpace(),
                    x => x.Name.Contains(filter) ||
                         x.Description.Contains(filter) ||
                         x.DisplayName.Contains(filter))
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<IdentityResource> FindByNameAsync(
            string name,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .IncludeDetails(includeDetails)
                .Where(x => x.Name == name)
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<bool> CheckNameExistAsync(string name, Guid? expectedId = null, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync()).AnyAsync(ir => ir.Id != expectedId && ir.Name == name, cancellationToken: cancellationToken);
        }
    }
}
