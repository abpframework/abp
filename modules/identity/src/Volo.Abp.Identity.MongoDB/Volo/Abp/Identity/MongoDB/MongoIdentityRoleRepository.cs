using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB;

public class MongoIdentityRoleRepository : MongoDbRepository<IAbpIdentityMongoDbContext, IdentityRole, Guid>, IIdentityRoleRepository
{
    public MongoIdentityRoleRepository(IMongoDbContextProvider<IAbpIdentityMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public virtual async Task<IdentityRole> FindByNormalizedNameAsync(
        string normalizedRoleName,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<IdentityRoleWithUserCount>> GetListWithUserCountAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var roles = await GetListInternalAsync(sorting, maxResultCount, skipCount, filter, includeDetails, cancellationToken: cancellationToken);
        var roleIds = roles.Select(x => x.Id).ToList();
        var userCount = await (await GetMongoQueryableAsync<IdentityUser>(cancellationToken))
            .Where(user => user.Roles.Any(role => roleIds.Contains(role.RoleId)))
            .SelectMany(user => user.Roles)
            .GroupBy(userRole => userRole.RoleId)
            .Select(x => new
            {
                RoleId = x.Key,
                Count = x.Count()
            })
            .ToListAsync(GetCancellationToken(cancellationToken));

        return roles.Select(role => new IdentityRoleWithUserCount(role, userCount.FirstOrDefault(x => x.RoleId == role.Id)?.Count ?? 0)).ToList();
    }

    public virtual async Task<List<IdentityRole>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await GetListInternalAsync(
            sorting,
            maxResultCount,
            skipCount,
            filter,
            includeDetails,
            cancellationToken);
    }

    public virtual async Task<List<IdentityRole>> GetListAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .Where(t => ids.Contains(t.Id))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<IdentityRole>> GetDefaultOnesAsync(
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .Where(r => r.IsDefault)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<long> GetCountAsync(
        string filter = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .WhereIf(!filter.IsNullOrWhiteSpace(),
                x => x.Name.Contains(filter) ||
                     x.NormalizedName.Contains(filter))
            .As<IMongoQueryable<IdentityRole>>()
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task RemoveClaimFromAllRolesAsync(string claimType, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var roles = await (await GetMongoQueryableAsync(cancellationToken))
            .Where(r => r.Claims.Any(c => c.ClaimType == claimType))
            .ToListAsync(GetCancellationToken(cancellationToken));

        foreach (var role in roles)
        {
            role.Claims.RemoveAll(c => c.ClaimType == claimType);
        }

        await UpdateManyAsync(roles, cancellationToken: cancellationToken);
    }

    protected virtual async Task<List<IdentityRole>> GetListInternalAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .WhereIf(!filter.IsNullOrWhiteSpace(),
                x => x.Name.Contains(filter) ||
                     x.NormalizedName.Contains(filter))
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(IdentityRole.Name) : sorting)
            .As<IMongoQueryable<IdentityRole>>()
            .PageBy<IdentityRole, IMongoQueryable<IdentityRole>>(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
