using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Entities;
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
        return await (await GetQueryableAsync(cancellationToken))
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
            .ToList();

        var organizationUnits = await (await GetQueryableAsync<OrganizationUnit>(cancellationToken))
            .Where(ou => organizationUnitIds.Contains(ou.Id))
            .ToListAsync(cancellationToken: cancellationToken);
        var orgUnitRoleIds = organizationUnits.SelectMany(x => x.Roles.Select(r => r.RoleId)).ToList();
        var roleIds = user.Roles.Select(r => r.RoleId).ToList();
        var allRoleIds = orgUnitRoleIds.Union(roleIds).ToList();
        return await (await GetQueryableAsync<IdentityRole>(cancellationToken)).Where(r => allRoleIds.Contains(r.Id)).Select(r => r.Name).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<string>> GetRoleNamesInOrganizationUnitAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        var user = await GetAsync(id, cancellationToken: cancellationToken);

        var organizationUnitIds = user.OrganizationUnits
            .Select(r => r.OrganizationUnitId)
            .ToList();

        var organizationUnits = await (await GetQueryableAsync<OrganizationUnit>(cancellationToken))
            .Where(ou => organizationUnitIds.Contains(ou.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        var roleIds = organizationUnits.SelectMany(x => x.Roles.Select(r => r.RoleId)).ToList();

        var queryable = await GetQueryableAsync<IdentityRole>(cancellationToken);

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
        return await (await GetQueryableAsync(cancellationToken))
            .Where(u => u.Logins.Any(login => login.LoginProvider == loginProvider && login.ProviderKey == providerKey))
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<IdentityUser> FindByNormalizedEmailAsync(
        string normalizedEmail,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .OrderBy(x => x.Id).FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<IdentityUser>> GetListByClaimAsync(
        Claim claim,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .Where(u => u.Claims.Any(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task RemoveClaimFromAllUsersAsync(string claimType, bool autoSave, CancellationToken cancellationToken = default)
    {
        var users = await (await GetQueryableAsync(cancellationToken))
            .Where(u => u.Claims.Any(c => c.ClaimType == claimType))
            .ToListAsync(GetCancellationToken(cancellationToken));

        foreach (var user in users)
        {
            user.Claims.RemoveAll(c => c.ClaimType == claimType);
        }

        await UpdateManyAsync(users, cancellationToken: cancellationToken);
    }

    public virtual async Task<List<IdentityUser>> GetListByNormalizedRoleNameAsync(
        string normalizedRoleName,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var queryable = await GetQueryableAsync<IdentityRole>(cancellationToken);

        var role = await queryable
            .Where(x => x.NormalizedName == normalizedRoleName)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (role == null)
        {
            return new List<IdentityUser>();
        }

        return await (await GetQueryableAsync(cancellationToken))
            .Where(u => u.Roles.Any(r => r.RoleId == role.Id))
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<Guid>> GetUserIdListByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        return await (await GetQueryableAsync(cancellationToken))
            .Where(u => u.Roles.Any(r => r.RoleId == roleId))
            .Select(x => x.Id)
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
        Guid? id = null,
        string userName = null,
        string phoneNumber = null,
        string emailAddress = null,
        string name = null,
        string surname = null,
        bool? isLockedOut = null,
        bool? notActive = null,
        bool? emailConfirmed = null,
        bool? isExternal = null,
        DateTime? maxCreationTime = null,
        DateTime? minCreationTime = null,
        DateTime? maxModifitionTime = null,
        DateTime? minModifitionTime = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetFilteredQueryableAsync(
            filter,
            roleId,
            organizationUnitId,
            id,
            userName,
            phoneNumber,
            emailAddress,
            name,
            surname,
            isLockedOut,
            notActive,
            emailConfirmed,
            isExternal,
            maxCreationTime,
            minCreationTime,
            maxModifitionTime,
            minModifitionTime,
            cancellationToken
        );

        return await query
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(IdentityUser.CreationTime) + " desc" : sorting)
            .PageBy(skipCount, maxResultCount)
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
            .ToList();

        var organizationUnits = await (await GetQueryableAsync<OrganizationUnit>(cancellationToken))
            .Where(ou => organizationUnitIds.Contains(ou.Id))
            .ToListAsync(cancellationToken: cancellationToken);
        var orgUnitRoleIds = organizationUnits.SelectMany(x => x.Roles.Select(r => r.RoleId)).ToList();
        var roleIds = user.Roles.Select(r => r.RoleId).ToList();
        var allRoleIds = orgUnitRoleIds.Union(roleIds);
        return await (await GetQueryableAsync<IdentityRole>(cancellationToken)).Where(r => allRoleIds.Contains(r.Id)).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<OrganizationUnit>> GetOrganizationUnitsAsync(
        Guid id,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        var user = await GetAsync(id, cancellationToken: cancellationToken);
        var organizationUnitIds = user.OrganizationUnits.Select(r => r.OrganizationUnitId).ToList();

        return await (await GetQueryableAsync<OrganizationUnit>(cancellationToken))
                        .Where(ou => organizationUnitIds.Contains(ou.Id))
                        .ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        string filter = null,
        Guid? roleId = null,
        Guid? organizationUnitId = null,
        Guid? id = null,
        string userName = null,
        string phoneNumber = null,
        string emailAddress = null,
        string name = null,
        string surname = null,
        bool? isLockedOut = null,
        bool? notActive = null,
        bool? emailConfirmed = null,
        bool? isExternal = null,
        DateTime? maxCreationTime = null,
        DateTime? minCreationTime = null,
        DateTime? maxModifitionTime = null,
        DateTime? minModifitionTime = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetFilteredQueryableAsync(
            filter,
            roleId,
            organizationUnitId,
            id,
            userName,
            phoneNumber,
            emailAddress,
            name,
            surname,
            isLockedOut,
            notActive,
            emailConfirmed,
            isExternal,
            maxCreationTime,
            minCreationTime,
            maxModifitionTime,
            minModifitionTime,
            cancellationToken
        );

        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<IdentityUser>> GetUsersInOrganizationUnitAsync(
        Guid organizationUnitId,
        CancellationToken cancellationToken = default)
    {
        var result = await (await GetQueryableAsync(cancellationToken))
                .Where(u => u.OrganizationUnits.Any(uou => uou.OrganizationUnitId == organizationUnitId))
                .ToListAsync(GetCancellationToken(cancellationToken));
        return result;
    }

    public virtual async Task<List<IdentityUser>> GetUsersInOrganizationsListAsync(
        List<Guid> organizationUnitIds,
        CancellationToken cancellationToken = default)
    {
        var result = await (await GetQueryableAsync(cancellationToken))
                .Where(u => u.OrganizationUnits.Any(uou => organizationUnitIds.Contains(uou.OrganizationUnitId)))
                .ToListAsync(GetCancellationToken(cancellationToken));
        return result;
    }

    public virtual async Task<List<IdentityUser>> GetUsersInOrganizationUnitWithChildrenAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var organizationUnitIds = await (await GetQueryableAsync<OrganizationUnit>(cancellationToken))
            .Where(ou => ou.Code.StartsWith(code))
            .Select(ou => ou.Id)
            .ToListAsync(cancellationToken);

        return await (await GetQueryableAsync(cancellationToken))
                 .Where(u => u.OrganizationUnits.Any(uou => organizationUnitIds.Contains(uou.OrganizationUnitId)))
                 .ToListAsync(cancellationToken);
    }

    public virtual async Task<IdentityUser> FindByTenantIdAndUserNameAsync(
        [NotNull] string userName,
        Guid? tenantId,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .FirstOrDefaultAsync(
                u => u.TenantId == tenantId && u.UserName == userName,
                GetCancellationToken(cancellationToken)
            );
    }

    public virtual async Task<List<IdentityUser>> GetListByIdsAsync(IEnumerable<Guid> ids, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task UpdateRoleAsync(Guid sourceRoleId, Guid? targetRoleId, CancellationToken cancellationToken = default)
    {
        var users = await (await GetQueryableAsync(cancellationToken))
            .Where(x => x.Roles.Any(r => r.RoleId == sourceRoleId))
            .ToListAsync(GetCancellationToken(cancellationToken));

        foreach (var user in users)
        {
            user.RemoveRole(sourceRoleId);
            if (targetRoleId.HasValue)
            {
                user.AddRole(targetRoleId.Value);
            }
        }

        await UpdateManyAsync(users, cancellationToken: cancellationToken);
    }

    public virtual async Task UpdateOrganizationAsync(Guid sourceOrganizationId, Guid? targetOrganizationId, CancellationToken cancellationToken = default)
    {
        var sourceOrganizationUnit = await (await GetQueryableAsync<OrganizationUnit>(cancellationToken))
            .Where(x => x.Id == sourceOrganizationId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (sourceOrganizationUnit == null)
        {
            throw new EntityNotFoundException(typeof(OrganizationUnit), sourceOrganizationId);
        }

        var allSourceOrganizationIds = await (await GetQueryableAsync<OrganizationUnit>(cancellationToken))
            .Where(x => x.Code.StartsWith(sourceOrganizationUnit.Code))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        var users = await (await GetQueryableAsync(cancellationToken))
            .Where(x => x.OrganizationUnits.Any(r => allSourceOrganizationIds.Contains(r.OrganizationUnitId)))
            .ToListAsync(GetCancellationToken(cancellationToken));

        foreach (var user in users)
        {
            foreach (var organizationId in allSourceOrganizationIds)
            {
                user.RemoveOrganizationUnit(organizationId);
            }

            if (targetOrganizationId.HasValue)
            {
                user.AddOrganizationUnit(targetOrganizationId.Value);
            }
        }

        await UpdateManyAsync(users, cancellationToken: cancellationToken);
    }

    public virtual async Task<List<IdentityUserIdWithRoleNames>> GetRoleNamesAsync(
        IEnumerable<Guid> userIds,
        CancellationToken cancellationToken = default)
    {
        var users = await GetListByIdsAsync(userIds, cancellationToken: cancellationToken);

        var userAndRoleIds = users.SelectMany(u => u.Roles)
            .Select(userRole => new { userRole.UserId, userRole.RoleId })
            .GroupBy(x => x.UserId).ToDictionary(x => x.Key, x => x.Select(r => r.RoleId).ToList());
        var userAndOrganizationUnitIds = users.SelectMany(u => u.OrganizationUnits)
            .Select(userOrganizationUnit => new { userOrganizationUnit.UserId, userOrganizationUnit.OrganizationUnitId })
            .GroupBy(x => x.UserId).ToDictionary(x => x.Key, x => x.Select(r => r.OrganizationUnitId).ToList());

        var organizationUnitIds = userAndOrganizationUnitIds.SelectMany(x => x.Value).ToList();
        var roleIds = userAndRoleIds.SelectMany(x => x.Value).ToList();

        var organizationUnitAndRoleIds = await (await GetQueryableAsync<OrganizationUnit>(cancellationToken)).Where(ou => organizationUnitIds.Contains(ou.Id))
            .Select(userOrganizationUnit => new
            {
                userOrganizationUnit.Id,
                userOrganizationUnit.Roles
            }).ToListAsync(cancellationToken: cancellationToken);
        var allOrganizationUnitRoleIds = organizationUnitAndRoleIds.SelectMany(x => x.Roles.Select(r => r.RoleId)).ToList();
        var allRoleIds = roleIds.Union(allOrganizationUnitRoleIds).ToList();

        var roles = await (await GetQueryableAsync<IdentityRole>(cancellationToken)).Where(r => allRoleIds.Contains(r.Id)).Select(r => new{ r.Id, r.Name }).ToListAsync(cancellationToken);
        var userRoles = userAndRoleIds.ToDictionary(x => x.Key, x => roles.Where(r => x.Value.Contains(r.Id)).Select(r => r.Name).ToArray());

        var result = userRoles.Select(x => new IdentityUserIdWithRoleNames { Id = x.Key, RoleNames = x.Value }).ToList();

        foreach (var userAndOrganizationUnitId in userAndOrganizationUnitIds)
        {
            var user = result.FirstOrDefault(x => x.Id == userAndOrganizationUnitId.Key);
            var organizationUnitRoleIds = organizationUnitAndRoleIds.Where(x => userAndOrganizationUnitId.Value.Contains(x.Id)).SelectMany(x => x.Roles.Select(r => r.RoleId)).ToList();
            var roleNames = roles.Where(x => organizationUnitRoleIds.Contains(x.Id)).Select(r => r.Name).ToArray();
            if (user != null && roleNames.Any())
            {
                user.RoleNames = user.RoleNames.Union(roleNames).ToArray();
            }
            else if (roleNames.Any())
            {
                result.Add(new IdentityUserIdWithRoleNames { Id = userAndOrganizationUnitId.Key, RoleNames = roleNames });
            }
        }

        return result;
    }

    public virtual async Task<IdentityUser> FindByPasskeyIdAsync(byte[] credentialId, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .Where(u => u.Passkeys.Any(x => x.CredentialId == credentialId))
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<IdentityUser>> GetUsersByNormalizedUserNameAsync(string normalizedUserName, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .OrderBy(x => x.Id)
            .Where(u => u.NormalizedUserName == normalizedUserName)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<List<IdentityUser>> GetUsersByNormalizedUserNamesAsync(string[] normalizedUserNames, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .OrderBy(x => x.Id)
            .Where(u => normalizedUserNames.Contains(u.NormalizedUserName))
            .Distinct()
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<List<IdentityUser>> GetUsersByNormalizedEmailAsync(string normalizedEmail, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .OrderBy(x => x.Id)
            .Where(u => u.NormalizedEmail == normalizedEmail)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<List<IdentityUser>> GetUsersByNormalizedEmailsAsync(string[] normalizedEmails, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .OrderBy(x => x.Id)
            .Where(u => normalizedEmails.Contains(u.NormalizedEmail))
            .Distinct()
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<List<IdentityUser>> GetUsersByLoginAsync(string loginProvider, string providerKey, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .OrderBy(x => x.Id)
            .Where(u => u.Logins.Any(login => login.LoginProvider == loginProvider && login.ProviderKey == providerKey))
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<List<IdentityUser>> GetUsersByPasskeyIdAsync(byte[] credentialId, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .OrderBy(x => x.Id)
            .Where(u => u.Passkeys.Any(x => x.CredentialId == credentialId))
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<IdentityUser> FindByNormalizedUserNameAsync(Guid? tenantId, string normalizedUserName, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(
                u => u.TenantId == tenantId && u.NormalizedUserName == normalizedUserName,
                GetCancellationToken(cancellationToken)
            );
    }

    public virtual async Task<IdentityUser> FindByNormalizedEmailAsync(Guid? tenantId, string normalizedEmail, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .OrderBy(x => x.Id).FirstOrDefaultAsync(
                u => u.TenantId == tenantId && u.NormalizedEmail == normalizedEmail,
                GetCancellationToken(cancellationToken)
            );
    }

    protected virtual async Task<IQueryable<IdentityUser>> GetFilteredQueryableAsync(
        string filter = null,
        Guid? roleId = null,
        Guid? organizationUnitId = null,
        Guid? id = null,
        string userName = null,
        string phoneNumber = null,
        string emailAddress = null,
        string name = null,
        string surname = null,
        bool? isLockedOut = null,
        bool? notActive = null,
        bool? emailConfirmed = null,
        bool? isExternal = null,
        DateTime? maxCreationTime = null,
        DateTime? minCreationTime = null,
        DateTime? maxModifitionTime = null,
        DateTime? minModifitionTime = null,
        CancellationToken cancellationToken = default)
    {
        var upperFilter = filter?.ToUpperInvariant();
        var query = await GetQueryableAsync(cancellationToken);

        if (id.HasValue)
        {
            return query.Where(x => x.Id == id);
        }

        if (roleId.HasValue)
        {
            var organizationUnitIds = (await GetQueryableAsync<OrganizationUnit>(cancellationToken))
                .Where(ou => ou.Roles.Any(r => r.RoleId == roleId.Value))
                .Select(userOrganizationUnit => userOrganizationUnit.Id)
                .ToList();

            query = query.Where(identityUser => identityUser.Roles.Any(x => x.RoleId == roleId.Value) || identityUser.OrganizationUnits.Any(x => organizationUnitIds.Contains(x.OrganizationUnitId)));
        }

        return  query
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.NormalizedUserName.Contains(upperFilter) ||
                    u.NormalizedEmail.Contains(upperFilter) ||
                    (u.Name != null && u.Name.Contains(filter)) ||
                    (u.Surname != null && u.Surname.Contains(filter)) ||
                    (u.PhoneNumber != null && u.PhoneNumber.Contains(filter))
            )
            .WhereIf(organizationUnitId.HasValue, identityUser => identityUser.OrganizationUnits.Any(x => x.OrganizationUnitId == organizationUnitId.Value))
            .WhereIf(!string.IsNullOrWhiteSpace(userName), x => x.UserName == userName)
            .WhereIf(!string.IsNullOrWhiteSpace(phoneNumber), x => x.PhoneNumber == phoneNumber)
            .WhereIf(!string.IsNullOrWhiteSpace(emailAddress), x => x.Email == emailAddress)
            .WhereIf(!string.IsNullOrWhiteSpace(name), x => x.Name == name)
            .WhereIf(!string.IsNullOrWhiteSpace(surname), x => x.Surname == surname)
            .WhereIf(isLockedOut.HasValue && isLockedOut.Value, x => x.LockoutEnabled && x.LockoutEnd != null && x.LockoutEnd > DateTimeOffset.UtcNow)
            .WhereIf(isLockedOut.HasValue && !isLockedOut.Value, x =>  !(x.LockoutEnabled && x.LockoutEnd != null && x.LockoutEnd > DateTimeOffset.UtcNow))
            .WhereIf(notActive.HasValue, x => x.IsActive == !notActive.Value)
            .WhereIf(emailConfirmed.HasValue, x => x.EmailConfirmed == emailConfirmed.Value)
            .WhereIf(isExternal.HasValue, x => x.IsExternal == isExternal.Value)
            .WhereIf(maxCreationTime != null, p => p.CreationTime <= maxCreationTime)
            .WhereIf(minCreationTime != null, p => p.CreationTime >= minCreationTime)
            .WhereIf(maxModifitionTime != null, p => p.LastModificationTime <= maxModifitionTime)
            .WhereIf(minModifitionTime != null, p => p.LastModificationTime >= minModifitionTime);
    }
}
