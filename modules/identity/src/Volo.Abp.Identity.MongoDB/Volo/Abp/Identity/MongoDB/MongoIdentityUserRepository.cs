﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB;

public class MongoIdentityUserRepository : MongoDbRepository<IAbpIdentityMongoDbContext, IdentityUser, Guid>, IIdentityUserRepository
{
    public MongoIdentityUserRepository(IMongoDbContextProvider<IAbpIdentityMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public virtual async Task<IdentityUser> FindByNormalizedUserNameAsync(
        string normalizedUserName,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(
                u => u.NormalizedUserName == normalizedUserName,
                GetCancellationToken(cancellationToken)
            );
    }

    public virtual async Task<List<string>> GetRoleNamesAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        var user = await GetAsync(id, cancellationToken: cancellationToken);
        var organizationUnitIds = user.OrganizationUnits
            .Select(r => r.OrganizationUnitId)
            .ToArray();

        var organizationUnits = await (await GetMongoQueryableAsync<OrganizationUnit>(cancellationToken))
            .Where(ou => organizationUnitIds.Contains(ou.Id))
            .ToListAsync(cancellationToken: cancellationToken);
        var orgUnitRoleIds = organizationUnits.SelectMany(x => x.Roles.Select(r => r.RoleId)).ToArray();
        var roleIds = user.Roles.Select(r => r.RoleId).ToArray();
        var allRoleIds = orgUnitRoleIds.Union(roleIds);
        return await (await GetMongoQueryableAsync<IdentityRole>(cancellationToken)).Where(r => allRoleIds.Contains(r.Id)).Select(r => r.Name).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<string>> GetRoleNamesInOrganizationUnitAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        var user = await GetAsync(id, cancellationToken: cancellationToken);

        var organizationUnitIds = user.OrganizationUnits
            .Select(r => r.OrganizationUnitId)
            .ToArray();

        var organizationUnits = await (await GetMongoQueryableAsync<OrganizationUnit>(cancellationToken))
            .Where(ou => organizationUnitIds.Contains(ou.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        var roleIds = organizationUnits.SelectMany(x => x.Roles.Select(r => r.RoleId)).ToArray();

        var queryable = await GetMongoQueryableAsync<IdentityRole>(cancellationToken);

        return await queryable
            .Where(r => roleIds.Contains(r.Id))
            .Select(r => r.Name)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<IdentityUser> FindByLoginAsync(
        string loginProvider,
        string providerKey,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .Where(u => u.Logins.Any(login => login.LoginProvider == loginProvider && login.ProviderKey == providerKey))
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<IdentityUser> FindByNormalizedEmailAsync(
        string normalizedEmail,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .OrderBy(x => x.Id).FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<IdentityUser>> GetListByClaimAsync(
        Claim claim,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .Where(u => u.Claims.Any(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<IdentityUser>> GetListByNormalizedRoleNameAsync(
        string normalizedRoleName,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var queryable = await GetMongoQueryableAsync<IdentityRole>(cancellationToken);

        var role = await queryable
            .Where(x => x.NormalizedName == normalizedRoleName)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (role == null)
        {
            return new List<IdentityUser>();
        }

        return await (await GetMongoQueryableAsync(cancellationToken))
            .Where(u => u.Roles.Any(r => r.RoleId == role.Id))
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<IdentityUser>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        Guid? roleId = null,
        Guid? organizationUnitId = null,
        string userName = null,
        string phoneNumber = null,
        string emailAddress = null,
        bool? isLockedOut = null,
        bool? notActive = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .WhereIf<IdentityUser, IMongoQueryable<IdentityUser>>(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.UserName.Contains(filter) ||
                    u.Email.Contains(filter) ||
                    (u.Name != null && u.Name.Contains(filter)) ||
                    (u.Surname != null && u.Surname.Contains(filter)) ||
                    (u.PhoneNumber != null && u.PhoneNumber.Contains(filter))
            )
            .WhereIf<IdentityUser, IMongoQueryable<IdentityUser>>(roleId.HasValue, identityUser => identityUser.Roles.Any(x => x.RoleId == roleId.Value))
            .WhereIf<IdentityUser, IMongoQueryable<IdentityUser>>(organizationUnitId.HasValue, identityUser => identityUser.OrganizationUnits.Any(x => x.OrganizationUnitId == organizationUnitId.Value))
            .WhereIf<IdentityUser, IMongoQueryable<IdentityUser>>(!string.IsNullOrWhiteSpace(userName), x => x.UserName == userName)
            .WhereIf<IdentityUser, IMongoQueryable<IdentityUser>>(!string.IsNullOrWhiteSpace(phoneNumber), x => x.PhoneNumber == phoneNumber)
            .WhereIf<IdentityUser, IMongoQueryable<IdentityUser>>(!string.IsNullOrWhiteSpace(emailAddress), x => x.Email == emailAddress)
            .WhereIf<IdentityUser, IMongoQueryable<IdentityUser>>(isLockedOut == true, x => x.LockoutEnabled && x.LockoutEnd > DateTimeOffset.UtcNow)
            .WhereIf<IdentityUser, IMongoQueryable<IdentityUser>>(notActive == true, x => !x.IsActive)
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(IdentityUser.UserName) : sorting)
            .As<IMongoQueryable<IdentityUser>>()
            .PageBy<IdentityUser, IMongoQueryable<IdentityUser>>(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<IdentityRole>> GetRolesAsync(
        Guid id,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        var user = await GetAsync(id, cancellationToken: cancellationToken);
        var organizationUnitIds = user.OrganizationUnits
            .Select(r => r.OrganizationUnitId)
            .ToArray();

        var organizationUnits = await (await GetMongoQueryableAsync<OrganizationUnit>(cancellationToken))
            .Where(ou => organizationUnitIds.Contains(ou.Id))
            .ToListAsync(cancellationToken: cancellationToken);
        var orgUnitRoleIds = organizationUnits.SelectMany(x => x.Roles.Select(r => r.RoleId)).ToArray();
        var roleIds = user.Roles.Select(r => r.RoleId).ToArray();
        var allRoleIds = orgUnitRoleIds.Union(roleIds);
        return await (await GetMongoQueryableAsync<IdentityRole>(cancellationToken)).Where(r => allRoleIds.Contains(r.Id)).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<OrganizationUnit>> GetOrganizationUnitsAsync(
        Guid id,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        var user = await GetAsync(id, cancellationToken: cancellationToken);
        var organizationUnitIds = user.OrganizationUnits.Select(r => r.OrganizationUnitId);

        return await (await GetMongoQueryableAsync<OrganizationUnit>(cancellationToken))
                        .Where(ou => organizationUnitIds.Contains(ou.Id))
                        .ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        string filter = null,
        Guid? roleId = null,
        Guid? organizationUnitId = null,
        string userName = null,
        string phoneNumber = null,
        string emailAddress = null,
        bool? isLockedOut = null,
        bool? notActive = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .WhereIf<IdentityUser, IMongoQueryable<IdentityUser>>(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.UserName.Contains(filter) ||
                    u.Email.Contains(filter) ||
                    (u.Name != null && u.Name.Contains(filter)) ||
                    (u.Surname != null && u.Surname.Contains(filter)) ||
                    (u.PhoneNumber != null && u.PhoneNumber.Contains(filter))
            )
            .WhereIf<IdentityUser, IMongoQueryable<IdentityUser>>(roleId.HasValue, identityUser => identityUser.Roles.Any(x => x.RoleId == roleId.Value))
            .WhereIf<IdentityUser, IMongoQueryable<IdentityUser>>(organizationUnitId.HasValue, identityUser => identityUser.OrganizationUnits.Any(x => x.OrganizationUnitId == organizationUnitId.Value))
            .WhereIf<IdentityUser, IMongoQueryable<IdentityUser>>(!string.IsNullOrWhiteSpace(userName), x => x.UserName == userName)
            .WhereIf<IdentityUser, IMongoQueryable<IdentityUser>>(!string.IsNullOrWhiteSpace(phoneNumber), x => x.PhoneNumber == phoneNumber)
            .WhereIf<IdentityUser, IMongoQueryable<IdentityUser>>(!string.IsNullOrWhiteSpace(emailAddress), x => x.Email == emailAddress)
            .WhereIf<IdentityUser, IMongoQueryable<IdentityUser>>(isLockedOut == true, x => x.LockoutEnabled && x.LockoutEnd > DateTimeOffset.UtcNow)
            .WhereIf<IdentityUser, IMongoQueryable<IdentityUser>>(notActive == true, x => !x.IsActive)
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<IdentityUser>> GetUsersInOrganizationUnitAsync(
        Guid organizationUnitId,
        CancellationToken cancellationToken = default)
    {
        var result = await (await GetMongoQueryableAsync(cancellationToken))
                .Where(u => u.OrganizationUnits.Any(uou => uou.OrganizationUnitId == organizationUnitId))
                .ToListAsync(GetCancellationToken(cancellationToken));
        return result;
    }

    public virtual async Task<List<IdentityUser>> GetUsersInOrganizationsListAsync(
        List<Guid> organizationUnitIds,
        CancellationToken cancellationToken = default)
    {
        var result = await (await GetMongoQueryableAsync(cancellationToken))
                .Where(u => u.OrganizationUnits.Any(uou => organizationUnitIds.Contains(uou.OrganizationUnitId)))
                .ToListAsync(GetCancellationToken(cancellationToken));
        return result;
    }

    public virtual async Task<List<IdentityUser>> GetUsersInOrganizationUnitWithChildrenAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var organizationUnitIds = await (await GetMongoQueryableAsync<OrganizationUnit>(cancellationToken))
            .Where(ou => ou.Code.StartsWith(code))
            .Select(ou => ou.Id)
            .ToListAsync(cancellationToken);

        return await (await GetMongoQueryableAsync(cancellationToken))
                 .Where(u => u.OrganizationUnits.Any(uou => organizationUnitIds.Contains(uou.OrganizationUnitId)))
                 .ToListAsync(cancellationToken);
    }
}
