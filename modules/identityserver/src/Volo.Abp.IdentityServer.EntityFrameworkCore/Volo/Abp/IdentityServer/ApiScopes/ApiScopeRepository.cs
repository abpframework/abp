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

        public async Task<ApiScope> GetByNameAsync(string scopeName, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .OrderBy(x=>x.Id)
                .FirstOrDefaultAsync(x => x.Name == scopeName, GetCancellationToken(cancellationToken));
        }

        public async Task<List<ApiScope>> GetListByNameAsync(string[] scopeNames, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = from scope in DbSet.IncludeDetails(includeDetails)
                where scopeNames.Contains(scope.Name)
                orderby scope.Id
                select scope;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<ApiScope>> GetListAsync(string sorting, int skipCount, int maxResultCount, string filter = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Name.Contains(filter) ||
                                                            x.Description.Contains(filter) ||
                                                            x.DisplayName.Contains(filter))
                .OrderBy(sorting ?? "name desc")
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<bool> CheckNameExistAsync(string name, Guid? expectedId = null, CancellationToken cancellationToken = default)
        {
            return await DbSet.AnyAsync(x => x.Id != expectedId && x.Name == name, GetCancellationToken(cancellationToken));
        }

        public async override Task DeleteAsync(Guid id, bool autoSave = false, CancellationToken cancellationToken = new CancellationToken())
        {
            var scopeClaims = DbContext.Set<ApiScopeClaim>().Where(sc => sc.ApiScopeId == id);
            foreach (var claim in scopeClaims)
            {
                DbContext.Set<ApiScopeClaim>().Remove(claim);
            }

            var scopeProperties = DbContext.Set<ApiScopeProperty>().Where(s => s.ApiScopeId == id);
            foreach (var property in scopeProperties)
            {
                DbContext.Set<ApiScopeProperty>().Remove(property);
            }

            await base.DeleteAsync(id, autoSave, cancellationToken);
        }

        public override IQueryable<ApiScope> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }
}
