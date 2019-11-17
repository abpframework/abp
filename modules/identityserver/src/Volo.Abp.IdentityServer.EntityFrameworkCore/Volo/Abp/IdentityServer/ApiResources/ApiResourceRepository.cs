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

namespace Volo.Abp.IdentityServer.ApiResources
{
    public class ApiResourceRepository : EfCoreRepository<IIdentityServerDbContext, ApiResource, Guid>, IApiResourceRepository
    {
        public ApiResourceRepository(IDbContextProvider<IIdentityServerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public virtual async Task<ApiResource> FindByNameAsync(
            string name,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var query = from apiResource in DbSet.IncludeDetails(includeDetails)
                where apiResource.Name == name
                select apiResource;

            return await query
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<ApiResource>> GetListByScopesAsync(
            string[] scopeNames, 
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = from api in DbSet.IncludeDetails(includeDetails)
                where api.Scopes.Any(x => scopeNames.Contains(x.Name))
                select api;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<ApiResource>> GetListAsync(string sorting, int skipCount, int maxResultCount, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails).OrderBy(sorting ?? "name desc")
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<ApiResource>> GetListAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<bool> CheckNameExistAsync(string name, Guid? expectedId = null, CancellationToken cancellationToken = default)
        {
            return await DbSet.AnyAsync(ar => ar.Id != expectedId && ar.Name == name, cancellationToken: cancellationToken);
        }

        public override async Task DeleteAsync(Guid id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var scopeClaims = DbContext.Set<ApiScopeClaim>().Where(sc => sc.ApiResourceId == id);

            foreach (var scopeClaim in scopeClaims)
            {
                DbContext.Set<ApiScopeClaim>().Remove(scopeClaim);
            }

            var scopes = DbContext.Set<ApiScope>().Where(s => s.ApiResourceId == id);

            foreach (var scope in scopes)
            {
                DbContext.Set<ApiScope>().Remove(scope);
            }

            await base.DeleteAsync(id, autoSave, cancellationToken);
        }

        public override IQueryable<ApiResource> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }
}