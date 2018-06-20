using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Identity.EntityFrameworkCore
{
    public class EfCoreIdentityUserRepository : EfCoreRepository<IIdentityDbContext, IdentityUser, Guid>, IIdentityUserRepository
    {
        public EfCoreIdentityUserRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<IdentityUser> FindByNormalizedUserNameAsync(
            string normalizedUserName, 
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .FirstOrDefaultAsync(
                    u => u.NormalizedUserName == normalizedUserName,
                    GetCancellationToken(cancellationToken)
                );
        }

        public virtual async Task<List<string>> GetRoleNamesAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var query = from userRole in DbContext.Set<IdentityUserRole>()
                        join role in DbContext.Roles on userRole.RoleId equals role.Id
                        where userRole.UserId == id
                        select role.Name;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<IdentityUser> FindByLoginAsync(
            string loginProvider, 
            string providerKey, 
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .Where(u => u.Logins.Any(login => login.LoginProvider == loginProvider && login.ProviderKey == providerKey))
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<IdentityUser> FindByNormalizedEmailAsync(
            string normalizedEmail,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityUser>> GetListByClaimAsync(
            Claim claim,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .Where(u => u.Claims.Any(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityUser>> GetListByNormalizedRoleNameAsync(
            string normalizedRoleName, 
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var role = await DbContext.Roles
                .Where(x => x.NormalizedName == normalizedRoleName)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));

            if (role == null)
            {
                return new List<IdentityUser>();
            }

            return await DbSet
                .IncludeDetails(includeDetails)
                .Where(u => u.Roles.Any(r => r.RoleId == role.Id))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityUser>> GetListAsync(
            string sorting = null, 
            int maxResultCount = int.MaxValue,
            int skipCount = 0, 
            string filter = null, 
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .WhereIf(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.UserName.Contains(filter) ||
                        u.Email.Contains(filter)
                )
                .OrderBy(sorting ?? nameof(IdentityUser.UserName))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityRole>> GetRolesAsync(
            Guid id,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = from userRole in DbContext.Set<IdentityUserRole>()
                        join role in DbContext.Roles.IncludeDetails(includeDetails) on userRole.RoleId equals role.Id
                        where userRole.UserId == id
                        select role;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<long> GetCountAsync(
            string filter = null, 
            CancellationToken cancellationToken = default)
        {
            return await this.WhereIf(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.UserName.Contains(filter) ||
                        u.Email.Contains(filter)
                )
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public override IQueryable<IdentityUser> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }
}
