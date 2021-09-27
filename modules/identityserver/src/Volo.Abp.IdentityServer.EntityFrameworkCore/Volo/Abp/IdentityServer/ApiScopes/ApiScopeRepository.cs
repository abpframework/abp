using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;

namespace Volo.Abp.IdentityServer.ApiScopes
{
    public class ApiScopeRepository : EfCoreRepository<IIdentityServerDbContext, ApiScope, Guid>, IApiScopeRepository
    {
        public ApiScopeRepository(IDbContextProvider<IIdentityServerDbContext> dbContextProvider) : base(
            dbContextProvider)
        {
        }

        public async Task<ApiScope> FindByNameAsync(string scopeName, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .OrderBy(x=>x.Id)
                .FirstOrDefaultAsync(x => x.Name == scopeName, GetCancellationToken(cancellationToken));
        }

        public async Task<List<ApiScope>> GetListByNameAsync(string[] scopeNames, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = from scope in (await GetDbSetAsync()).IncludeDetails(includeDetails)
                where scopeNames.Contains(scope.Name)
                orderby scope.Id
                select scope;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<ApiScope>> GetListAsync(string sorting, int skipCount, int maxResultCount, string filter = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .IncludeDetails(includeDetails)
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Name.Contains(filter) ||
                                                            x.Description.Contains(filter) ||
                                                            x.DisplayName.Contains(filter))
                .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(ApiScope.Name) : sorting)
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

        public async Task<bool> CheckNameExistAsync(string name, Guid? expectedId = null, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync()).AnyAsync(x => x.Id != expectedId && x.Name == name, GetCancellationToken(cancellationToken));
        }

        public override async Task DeleteAsync(Guid id, bool autoSave = false, CancellationToken cancellationToken = new CancellationToken())
        {
            var dbContext = await GetDbContextAsync();
            var scopeClaims = dbContext.Set<ApiScopeClaim>().Where(sc => sc.ApiScopeId == id);
            foreach (var claim in scopeClaims)
            {
                dbContext.Set<ApiScopeClaim>().Remove(claim);
            }

            var scopeProperties = dbContext.Set<ApiScopeProperty>().Where(s => s.ApiScopeId == id);
            foreach (var property in scopeProperties)
            {
                dbContext.Set<ApiScopeProperty>().Remove(property);
            }

            await base.DeleteAsync(id, autoSave, cancellationToken);
        }

        [Obsolete("Use WithDetailsAsync method.")]
        public override IQueryable<ApiScope> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }

        public override async Task<IQueryable<ApiScope>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).IncludeDetails();
        }
    }
}
