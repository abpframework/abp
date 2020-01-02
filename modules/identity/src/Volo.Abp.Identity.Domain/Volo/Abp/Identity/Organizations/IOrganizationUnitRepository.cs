using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Identity.Organizations
{
    public interface IOrganizationUnitRepository : IBasicRepository<OrganizationUnit, Guid>
    {
        Task<List<OrganizationUnit>> GetChildrenAsync(Guid? parentId);

        Task<List<OrganizationUnit>> GetAllChildrenWithParentCodeAsync(string code, Guid? parentId);
    }
}
