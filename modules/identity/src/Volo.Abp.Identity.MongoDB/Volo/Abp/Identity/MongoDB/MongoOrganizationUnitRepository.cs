using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity.MongoDB;

public class MongoOrganizationUnitRepository
    : MongoDbRepository<IAbpIdentityMongoDbContext, OrganizationUnit, Guid>,
    IOrganizationUnitRepository
{
    public MongoOrganizationUnitRepository(
        IMongoDbContextProvider<IAbpIdentityMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public virtual async Task<List<OrganizationUnit>> GetChildrenAsync(
        Guid? parentId,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .Where(ou => ou.ParentId == parentId)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OrganizationUnit>> GetAllChildrenWithParentCodeAsync(
        string code,
        Guid? parentId,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
                .Where(ou => ou.Code.StartsWith(code) && ou.Id != parentId)
                .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OrganizationUnit>> GetListAsync(
        IEnumerable<Guid> ids,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
                .Where(t => ids.Contains(t.Id))
                .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OrganizationUnit>> GetListByRoleIdAsync(
        Guid roleId,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .Where(x => x.Roles.Any(r => r.RoleId == roleId))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OrganizationUnit>> GetListByDisplayNamesAsync(
        string[] displayNames,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .Where(x => displayNames.AsEnumerable().Contains(x.DisplayName))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OrganizationUnit>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
                .OrderBy(sorting.IsNullOrEmpty() ? nameof(OrganizationUnit.CreationTime) + " desc" : sorting)
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<OrganizationUnit> GetAsync(
        string displayName,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(
                ou => ou.DisplayName == displayName,
                GetCancellationToken(cancellationToken)
            );
    }

    public virtual async Task<List<IdentityRole>> GetRolesAsync(
        OrganizationUnit organizationUnit,
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var roleIds = organizationUnit.Roles.Select(r => r.RoleId).ToList();

        return await (await GetQueryableAsync<IdentityRole>(cancellationToken))
            .Where(r => roleIds.Contains(r.Id))
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(IdentityRole.Name) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<IdentityRole>> GetRolesAsync(
        Guid[] organizationUnitIds,
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var organizationUnits = await (await GetQueryableAsync(cancellationToken))
            .Where(ou => organizationUnitIds.AsEnumerable().Contains(ou.Id))
            .ToListAsync(GetCancellationToken(cancellationToken));

        var roleIds = organizationUnits.SelectMany(ou => ou.Roles.Select(r => r.RoleId)).ToList();

        return await (await GetQueryableAsync<IdentityRole>(cancellationToken))
            .Where(r => roleIds.Contains(r.Id))
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(IdentityRole.Name) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<int> GetRolesCountAsync(
        OrganizationUnit organizationUnit,
        CancellationToken cancellationToken = default)
    {
        var roleIds = organizationUnit.Roles.Select(r => r.RoleId).ToList();

        return await (await GetQueryableAsync<IdentityRole>(cancellationToken)).Where(r => roleIds.Contains(r.Id)).CountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<IdentityRole>> GetUnaddedRolesAsync(
        OrganizationUnit organizationUnit,
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var roleIds = organizationUnit.Roles.Select(r => r.RoleId).ToList();

        return await (await GetQueryableAsync<IdentityRole>(cancellationToken))
            .Where(r => !roleIds.Contains(r.Id))
            .WhereIf(!filter.IsNullOrWhiteSpace(), r => r.Name.Contains(filter))
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(IdentityRole.Name) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<int> GetUnaddedRolesCountAsync(
        OrganizationUnit organizationUnit,
        string filter = null,
        CancellationToken cancellationToken = default)
    {
        var roleIds = organizationUnit.Roles.Select(r => r.RoleId).ToList();

        return await (await GetQueryableAsync<IdentityRole>(cancellationToken))
            .Where(r => !roleIds.Contains(r.Id))
            .WhereIf(!filter.IsNullOrWhiteSpace(), r => r.Name.Contains(filter))
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<IdentityUser>> GetMembersAsync(
        OrganizationUnit organizationUnit,
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeChildren = false,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        var query = await CreateGetMembersFilteredQueryAsync(organizationUnit, filter, includeChildren, cancellationToken);
        return await query
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(IdentityUser.UserName) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<Guid>> GetMemberIdsAsync(Guid id, bool includeChildren = false, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        return await (await GetQueryableAsync<IdentityUser>(cancellationToken))
            .Where(u => u.OrganizationUnits.Any(uou => uou.OrganizationUnitId == id)).Select(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<int> GetMembersCountAsync(
        OrganizationUnit organizationUnit,
        string filter = null,
        bool includeChildren = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        var query = await CreateGetMembersFilteredQueryAsync(organizationUnit, filter, includeChildren, cancellationToken);
        return await query.CountAsync(cancellationToken);
    }

    public virtual async Task<List<IdentityUser>> GetUnaddedUsersAsync(
        OrganizationUnit organizationUnit,
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await
            (await GetQueryableAsync<IdentityUser>(cancellationToken))
            .Where(u => !u.OrganizationUnits.Any(uou => uou.OrganizationUnitId == organizationUnit.Id))
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.UserName.Contains(filter) ||
                    u.Email.Contains(filter) ||
                    (u.PhoneNumber != null && u.PhoneNumber.Contains(filter))
            )
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(IdentityUser.UserName) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<int> GetUnaddedUsersCountAsync(OrganizationUnit organizationUnit, string filter = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync<IdentityUser>(cancellationToken))
            .Where(u => !u.OrganizationUnits.Any(uou => uou.OrganizationUnitId == organizationUnit.Id))
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.UserName.Contains(filter) ||
                    u.Email.Contains(filter) ||
                    (u.PhoneNumber != null && u.PhoneNumber.Contains(filter))
            )
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual Task RemoveAllRolesAsync(OrganizationUnit organizationUnit, CancellationToken cancellationToken = default)
    {
        organizationUnit.Roles.Clear();
        return Task.FromResult(0);
    }

    public virtual async Task RemoveAllMembersAsync(OrganizationUnit organizationUnit, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        var userQueryable = await GetQueryableAsync<IdentityUser>(cancellationToken);
        var dbContext = await GetDbContextAsync(cancellationToken);
        var users = await userQueryable
            .Where(u => u.OrganizationUnits.Any(uou => uou.OrganizationUnitId == organizationUnit.Id))
            .ToListAsync(cancellationToken);

        foreach (var user in users)
        {
            user.RemoveOrganizationUnit(organizationUnit.Id);
            await dbContext.Users.ReplaceOneAsync(u => u.Id == user.Id, user, cancellationToken: cancellationToken);
        }
    }

    protected virtual async Task<IQueryable<IdentityUser>> CreateGetMembersFilteredQueryAsync(
        OrganizationUnit organizationUnit,
        string filter = null,
        bool includeChildren = false,
        CancellationToken cancellationToken = default)
    {
        return (await GetQueryableAsync<IdentityUser>(cancellationToken))
            .Where(u => u.OrganizationUnits.Any(uou => uou.OrganizationUnitId == organizationUnit.Id))
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.UserName.Contains(filter) ||
                    u.Email.Contains(filter) ||
                    (u.PhoneNumber != null && u.PhoneNumber.Contains(filter))
            );
    }
}
