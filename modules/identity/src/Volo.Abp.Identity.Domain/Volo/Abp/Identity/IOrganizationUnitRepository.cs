using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Identity;

public interface IOrganizationUnitRepository : IBasicRepository<OrganizationUnit, Guid>
{
    Task<List<OrganizationUnit>> GetChildrenAsync(
        Guid? parentId,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    Task<List<OrganizationUnit>> GetAllChildrenWithParentCodeAsync(
        string code,
        Guid? parentId,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    Task<OrganizationUnit> GetAsync(
        string displayName,
        bool includeDetails = true,
        CancellationToken cancellationToken = default
    );

    Task<List<OrganizationUnit>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    Task<List<OrganizationUnit>> GetListAsync(
        IEnumerable<Guid> ids,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    Task<List<IdentityRole>> GetRolesAsync(
        OrganizationUnit organizationUnit,
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    Task<int> GetRolesCountAsync(
        OrganizationUnit organizationUnit,
        CancellationToken cancellationToken = default
    );

    Task<List<IdentityRole>> GetUnaddedRolesAsync(
        OrganizationUnit organizationUnit,
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    Task<int> GetUnaddedRolesCountAsync(
        OrganizationUnit organizationUnit,
        string filter = null,
        CancellationToken cancellationToken = default
    );

    Task<List<IdentityUser>> GetMembersAsync(
        OrganizationUnit organizationUnit,
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    Task<int> GetMembersCountAsync(
        OrganizationUnit organizationUnit,
        string filter = null,
        CancellationToken cancellationToken = default
    );

    Task<List<IdentityUser>> GetUnaddedUsersAsync(
        OrganizationUnit organizationUnit,
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    Task<int> GetUnaddedUsersCountAsync(
        OrganizationUnit organizationUnit,
        string filter = null,
        CancellationToken cancellationToken = default
    );

    Task RemoveAllRolesAsync(
        OrganizationUnit organizationUnit,
        CancellationToken cancellationToken = default
    );

    Task RemoveAllMembersAsync(
        OrganizationUnit organizationUnit,
        CancellationToken cancellationToken = default
    );
}
