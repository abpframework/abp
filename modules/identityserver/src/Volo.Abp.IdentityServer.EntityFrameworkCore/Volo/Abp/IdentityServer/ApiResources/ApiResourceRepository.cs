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

namespace Volo.Abp.IdentityServer.ApiResources
{
    public class ApiResourceRepository : EfCoreRepository<IIdentityServerDbContext, ApiResource, Guid>, IApiResourceRepository
    {
        public ApiResourceRepository(IDbContextProvider<IIdentityServerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public async Task<ApiResource> FindByNameAsync(string apiResourceName, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            var query = from apiResource in (await GetDbSetAsync()).IncludeDetails(includeDetails)
                where apiResource.Name == apiResourceName
                orderby apiResource.Id
                select apiResource;

            return await query.FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<ApiResource>> FindByNameAsync(string[] apiResourceNames, bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var query = from apiResource in (await GetDbSetAsync()).IncludeDetails(includeDetails)
                where apiResourceNames.Contains(apiResource.Name)
                orderby apiResource.Name
                select apiResource;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<ApiResource>> GetListByScopesAsync(
            string[] scopeNames,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = from api in (await GetDbSetAsync()).IncludeDetails(includeDetails)
                        where api.Scopes.Any(x => scopeNames.Contains(x.Scope))
                        select api;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<ApiResource>> GetListAsync(
            string sorting, int skipCount,
            int maxResultCount,
            string filter,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
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

        public virtual async Task<bool> CheckNameExistAsync(string name, Guid? expectedId = null, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync()).AnyAsync(ar => ar.Id != expectedId && ar.Name == name, GetCancellationToken(cancellationToken));
        }

        public async override Task DeleteAsync(Guid id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            var resourceClaims = dbContext.Set<ApiResourceClaim>().Where(sc => sc.ApiResourceId == id);
            foreach (var scopeClaim in resourceClaims)
            {
                dbContext.Set<ApiResourceClaim>().Remove(scopeClaim);
            }

            var resourceScopes = dbContext.Set<ApiResourceScope>().Where(s => s.ApiResourceId == id);
            foreach (var scope in resourceScopes)
            {
                dbContext.Set<ApiResourceScope>().Remove(scope);
            }

            var resourceSecrets = dbContext.Set<ApiResourceSecret>().Where(s => s.ApiResourceId == id);
            foreach (var secret in resourceSecrets)
            {
                dbContext.Set<ApiResourceSecret>().Remove(secret);
            }

            var apiResourceProperties = dbContext.Set<ApiResourceProperty>().Where(s => s.ApiResourceId == id);
            foreach (var property in apiResourceProperties)
            {
                dbContext.Set<ApiResourceProperty>().Remove(property);
            }

            await base.DeleteAsync(id, autoSave, cancellationToken);
        }

        [Obsolete("Use WithDetailsAsync method.")]
        public override IQueryable<ApiResource> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }

        public override async Task<IQueryable<ApiResource>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).IncludeDetails();
        }
    }
}
