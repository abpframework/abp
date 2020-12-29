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
            var query = from apiResource in DbSet.IncludeDetails(includeDetails)
                where apiResource.Name == apiResourceName
                orderby apiResource.Id
                select apiResource;

            return await query.FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<ApiResource>> FindByNameAsync(string[] apiResourceNames, bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var query = from apiResource in DbSet.IncludeDetails(includeDetails)
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
            var query = from api in DbSet.IncludeDetails(includeDetails)
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
            return await DbSet
                .IncludeDetails(includeDetails)
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Name.Contains(filter) ||
                         x.Description.Contains(filter) ||
                         x.DisplayName.Contains(filter))
                .OrderBy(sorting ?? "name desc")
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<bool> CheckNameExistAsync(string name, Guid? expectedId = null, CancellationToken cancellationToken = default)
        {
            return await DbSet.AnyAsync(ar => ar.Id != expectedId && ar.Name == name, GetCancellationToken(cancellationToken));
        }

        public async override Task DeleteAsync(Guid id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var resourceClaims = DbContext.Set<ApiResourceClaim>().Where(sc => sc.ApiResourceId == id);
            foreach (var scopeClaim in resourceClaims)
            {
                DbContext.Set<ApiResourceClaim>().Remove(scopeClaim);
            }

            var resourceScopes = DbContext.Set<ApiResourceScope>().Where(s => s.ApiResourceId == id);
            foreach (var scope in resourceScopes)
            {
                DbContext.Set<ApiResourceScope>().Remove(scope);
            }

            var resourceSecrets = DbContext.Set<ApiResourceSecret>().Where(s => s.ApiResourceId == id);
            foreach (var secret in resourceSecrets)
            {
                DbContext.Set<ApiResourceSecret>().Remove(secret);
            }

            var apiResourceProperties = DbContext.Set<ApiResourceProperty>().Where(s => s.ApiResourceId == id);
            foreach (var property in apiResourceProperties)
            {
                DbContext.Set<ApiResourceProperty>().Remove(property);
            }

            await base.DeleteAsync(id, autoSave, cancellationToken);
        }

        public override IQueryable<ApiResource> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }
}
