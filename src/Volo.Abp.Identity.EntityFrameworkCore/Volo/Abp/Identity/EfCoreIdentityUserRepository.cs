using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;

namespace Volo.Abp.Identity
{
    public class EfCoreIdentityUserRepository : EfCoreRepository<IdentityDbContext, IdentityUser>, IIdentityUserRepository
    {
        public EfCoreIdentityUserRepository(IDbContextProvider<IdentityDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public Task<IdentityUser> FindByNormalizedUserNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return DbSet.FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName, cancellationToken);
        }

        public Task<List<string>> GetRoleNamesAsync(Guid userId)
        {
            var query = from userRole in DbContext.UserRoles
                        join role in DbContext.Roles on userRole.RoleId equals role.Id
                        where userRole.UserId.Equals(userId)
                        select role.Name;

            return query.ToListAsync();
        }

        public async Task<IdentityUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            //TODO: This should be changed since loginProvider, providerKey are not PKs.
            var userLogin = await DbContext.UserLogins.FindAsync(new object[] { loginProvider, providerKey }, cancellationToken);
            if (userLogin == null)
            {
                return null;
            }

            return await DbSet.FindAsync(new object[] { userLogin.UserId }, cancellationToken);
        }

        public Task<IdentityUser> FindByNormalizedEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return DbSet.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);
        }

        public async Task<IList<IdentityUser>> GetListByClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            var query = from userclaims in DbContext.UserClaims
                        join user in DbContext.Users on userclaims.UserId equals user.Id
                        where userclaims.ClaimValue == claim.Value && userclaims.ClaimType == claim.Type
                        select user;

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<IList<IdentityUser>> GetListByNormalizedRoleNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            var role = await DbContext.Roles.Where(x => x.NormalizedName == normalizedRoleName).FirstOrDefaultAsync(cancellationToken);

            if (role == null)
            {
                return new List<IdentityUser>();
            }

            var query = from userrole in DbContext.UserRoles
                        join user in DbContext.Users on userrole.UserId equals user.Id
                        where userrole.RoleId.Equals(role.Id)
                        select user;

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<List<IdentityUser>> GetListAsync(string sorting, int maxResultCount, int skipCount)
        {
            return await this.OrderBy(sorting ?? nameof(IdentityUser.UserName)).PageBy(skipCount, maxResultCount).ToListAsync();
        }
    }
}
