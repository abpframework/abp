using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        public async Task<List<OrganizationUnit>> GetListAsync(bool includeDetails = true)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .ToListAsync();
        }

        public async Task<OrganizationUnit> GetOrganizationUnit(string displayName, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .FirstOrDefaultAsync(
                    ou => ou.DisplayName == displayName,
                    GetCancellationToken(cancellationToken)
                ).ConfigureAwait(false);
        }

        public async Task AddRole(OrganizationUnit ou, IdentityRole role, Guid? tenantId)
        {
            var our = new OrganizationUnitRole(tenantId, role.Id, ou.Id);
            DbContext.Set<OrganizationUnitRole>().Add(our);
            await DbContext.SaveChangesAsync();
        }

        public async Task RemoveRole(OrganizationUnit ou, IdentityRole role, Guid? tenantId)
        {
            var context = DbContext.Set<OrganizationUnitRole>();
            var our = await context.FirstOrDefaultAsync(our =>
                                        our.OrganizationUnitId == ou.Id &&
                                        our.RoleId == role.Id &&
                                        our.TenantId == tenantId
                                        );
            DbContext.Set<OrganizationUnitRole>().Remove(our);
            await DbContext.SaveChangesAsync();
        }

        public override IQueryable<OrganizationUnit> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }
}
