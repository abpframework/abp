using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.Organizations;

namespace Volo.Abp.Identity.EntityFrameworkCore
{
    public class EfCoreOrganizationUnitRepository : EfCoreRepository<IIdentityDbContext, OrganizationUnit, Guid>, IOrganizationUnitRepository
    {
        public EfCoreOrganizationUnitRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<OrganizationUnit>> GetChildrenAsync(Guid? parentId)
        {
            return await DbSet.Where(x => x.ParentId == parentId)
                .ToListAsync();
        }

        public async Task<List<OrganizationUnit>> GetAllChildrenWithParentCodeAsync(string code, Guid? parentId)
        {
            return await DbSet.Where(ou => ou.Code.StartsWith(code) && ou.Id != parentId.Value)
                .ToListAsync();
        }

        public async Task<List<OrganizationUnit>> GetListAsync(IEnumerable<Guid> ids)
        {
            return await DbSet.Where(t => ids.Contains(t.Id)).ToListAsync();
        }
    }
}
