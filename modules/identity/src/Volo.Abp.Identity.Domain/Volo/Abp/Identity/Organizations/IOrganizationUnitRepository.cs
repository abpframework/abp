using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Identity.Organizations
{
    public interface IOrganizationUnitRepository : IBasicRepository<OrganizationUnit, Guid>
    {
        Task<List<OrganizationUnit>> GetChildrenAsync(Guid? parentId);

        Task<List<OrganizationUnit>> GetAllChildrenWithParentCodeAsync(string code, Guid? parentId);

        Task<List<OrganizationUnit>> GetListAsync(IEnumerable<Guid> ids);

        Task<List<OrganizationUnit>> GetListAsync(bool includeDetails = true);

        Task<OrganizationUnit> GetOrganizationUnit(string displayName, bool includeDetails = false, CancellationToken cancellationToken = default);

        Task AddRole(OrganizationUnit ou, IdentityRole role, Guid? tenantId);

        Task RemoveRole(OrganizationUnit ou, IdentityRole role, Guid? tenantId);
    }
}
