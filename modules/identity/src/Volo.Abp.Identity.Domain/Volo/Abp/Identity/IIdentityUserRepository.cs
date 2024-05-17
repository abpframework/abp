using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Identity;

public interface IIdentityUserRepository : IBasicRepository<IdentityUser, Guid>
{
    Task<IdentityUser> FindByNormalizedUserNameAsync(
        [NotNull] string normalizedUserName,
        bool includeDetails = true,
        CancellationToken cancellationToken = default
    );

    Task<List<string>> GetRoleNamesAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    Task<List<string>> GetRoleNamesInOrganizationUnitAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IdentityUser> FindByLoginAsync(
        [NotNull] string loginProvider,
        [NotNull] string providerKey,
        bool includeDetails = true,
        CancellationToken cancellationToken = default
    );

    Task<IdentityUser> FindByNormalizedEmailAsync(
        [NotNull] string normalizedEmail,
        bool includeDetails = true,
        CancellationToken cancellationToken = default
    );

    Task<List<IdentityUser>> GetListByClaimAsync(
        Claim claim,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    Task RemoveClaimFromAllUsersAsync(
        string claimType,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    );

    Task<List<IdentityUser>> GetListByNormalizedRoleNameAsync(
        string normalizedRoleName,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    Task<List<Guid>> GetUserIdListByRoleIdAsync(
        Guid roleId,
        CancellationToken cancellationToken = default
    );

    Task<List<IdentityUser>> GetListAsync(
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
        CancellationToken cancellationToken = default
    );

    Task<List<IdentityRole>> GetRolesAsync(
        Guid id,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    Task<List<OrganizationUnit>> GetOrganizationUnitsAsync(
        Guid id,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    Task<List<IdentityUser>> GetUsersInOrganizationUnitAsync(
        Guid organizationUnitId,
        CancellationToken cancellationToken = default
        );
    Task<List<IdentityUser>> GetUsersInOrganizationsListAsync(
        List<Guid> organizationUnitIds,
        CancellationToken cancellationToken = default
        );

    Task<List<IdentityUser>> GetUsersInOrganizationUnitWithChildrenAsync(
        string code,
        CancellationToken cancellationToken = default
        );

    Task<long> GetCountAsync(
        string filter = null,
        Guid? roleId = null,
        Guid? organizationUnitId = null,
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
        CancellationToken cancellationToken = default
    );

    Task<IdentityUser> FindByTenantIdAndUserNameAsync(
        [NotNull] string userName,
        Guid? tenantId,
        bool includeDetails = true,
        CancellationToken cancellationToken = default
    );

    Task<List<IdentityUser>> GetListByIdsAsync(
        IEnumerable<Guid> ids,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    Task UpdateRoleAsync(
        Guid sourceRoleId,
        Guid? targetRoleId,
        CancellationToken cancellationToken = default
    );

    Task UpdateOrganizationAsync(
        Guid sourceOrganizationId,
        Guid? targetOrganizationId,
        CancellationToken cancellationToken = default
    );

    Task<List<IdentityUserIdWithRoleNames>> GetRoleNamesAsync(
        IEnumerable<Guid> userIds,
        CancellationToken cancellationToken = default);
}
