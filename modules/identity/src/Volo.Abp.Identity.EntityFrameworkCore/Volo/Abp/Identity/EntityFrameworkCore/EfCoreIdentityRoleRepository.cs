using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Guids;

namespace Volo.Abp.Identity.EntityFrameworkCore
{
    public class EfCoreIdentityRoleRepository : EfCoreRepository<IIdentityDbContext, IdentityRole, Guid>, IIdentityRoleRepository
    {
        private readonly IGuidGenerator _guidGenerator;

        public EfCoreIdentityRoleRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider, IGuidGenerator guidGenerator)
            : base(dbContextProvider)
        {
            _guidGenerator = guidGenerator;
        }

        public virtual async Task<IdentityRole> FindByNormalizedNameAsync(
            string normalizedRoleName, 
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityRole>> GetListAsync(
            string sorting = null, 
            int maxResultCount = int.MaxValue, 
            int skipCount = 0, 
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .OrderBy(sorting ?? nameof(IdentityRole.Name))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task UpdateClaimsAsync(Guid id, List<IdentityRoleClaim> claims)
        {
            var dbSet = DbContext.Set<IdentityRoleClaim>();

            var oldClaims = dbSet.Where(c => c.RoleId == id).ToList();

            foreach (var oldClaim in oldClaims)
            {
                dbSet.Remove(oldClaim);
            }

            foreach (var claim in claims)
            {
                dbSet.Add(new IdentityRoleClaim(_guidGenerator.Create(), id, claim.ClaimType, claim.ClaimValue, CurrentTenant.Id));
            }
        }

        public async Task<List<IdentityRoleClaim>> GetClaimsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var query = from roleClaim in DbContext.Set<IdentityRoleClaim>()
                where roleClaim.RoleId == id
                select roleClaim;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await this.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public override IQueryable<IdentityRole> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }
}