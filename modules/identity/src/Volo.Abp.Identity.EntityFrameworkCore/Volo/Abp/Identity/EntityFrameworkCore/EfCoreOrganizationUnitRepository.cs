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

        public async Task<List<OrganizationUnit>> GetChildrenAsync(Guid? parentId, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(x => x.ParentId == parentId)
                .ToListAsync(GetCancellationToken(cancellationToken)).ConfigureAwait(false);
        }

        public async Task<List<OrganizationUnit>> GetAllChildrenWithParentCodeAsync(string code, Guid? parentId, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(ou => ou.Code.StartsWith(code) && ou.Id != parentId.Value)
                .ToListAsync(GetCancellationToken(cancellationToken)).ConfigureAwait(false);
        }

        public async Task<List<OrganizationUnit>> GetListAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(t => ids.Contains(t.Id)).ToListAsync(GetCancellationToken(cancellationToken)).ConfigureAwait(false);
        }

        public override async Task<List<OrganizationUnit>> GetListAsync(bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .ToListAsync(GetCancellationToken(cancellationToken)).ConfigureAwait(false);
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

        public Task AddRole(OrganizationUnit ou, IdentityRole role, Guid? tenantId, CancellationToken cancellationToken = default)
        {
            var our = new OrganizationUnitRole(tenantId, role.Id, ou.Id);
            DbContext.Set<OrganizationUnitRole>().Add(our);
            return Task.FromResult(0);
        }

        public async Task RemoveRole(OrganizationUnit ou, IdentityRole role, Guid? tenantId, CancellationToken cancellationToken = default)
        {
            var context = DbContext.Set<OrganizationUnitRole>();
            var our = await context.FirstOrDefaultAsync(our =>
                                        our.OrganizationUnitId == ou.Id &&
                                        our.RoleId == role.Id &&
                                        our.TenantId == tenantId
                                        );
            DbContext.Set<OrganizationUnitRole>().Remove(our);
        }

        public override IQueryable<OrganizationUnit> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }

    }
}
