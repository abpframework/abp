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
            return await (await GetDbSetAsync())
                .IncludeDetails(includeDetails)
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(
                    u => u.NormalizedUserName == normalizedUserName,
                    GetCancellationToken(cancellationToken)
                );
        }

        public virtual async Task<List<string>> GetRoleNamesAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var query = from userRole in dbContext.Set<IdentityUserRole>()
                        join role in dbContext.Roles on userRole.RoleId equals role.Id
                        where userRole.UserId == id
                        select role.Name;
            var organizationUnitIds = dbContext.Set<IdentityUserOrganizationUnit>().Where(q => q.UserId == id).Select(q => q.OrganizationUnitId).ToArray();

            var organizationRoleIds = await (
                from ouRole in dbContext.Set<OrganizationUnitRole>()
                join ou in dbContext.Set<OrganizationUnit>() on ouRole.OrganizationUnitId equals ou.Id
                where organizationUnitIds.Contains(ouRole.OrganizationUnitId)
                select ouRole.RoleId
            ).ToListAsync(GetCancellationToken(cancellationToken));

            var orgUnitRoleNameQuery = dbContext.Roles.Where(r => organizationRoleIds.Contains(r.Id)).Select(n => n.Name);
            var resultQuery = query.Union(orgUnitRoleNameQuery);
            return await resultQuery.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<string>> GetRoleNamesInOrganizationUnitAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var query = from userOu in dbContext.Set<IdentityUserOrganizationUnit>()
                        join roleOu in dbContext.Set<OrganizationUnitRole>() on userOu.OrganizationUnitId equals roleOu.OrganizationUnitId
                        join ou in dbContext.Set<OrganizationUnit>() on roleOu.OrganizationUnitId equals ou.Id
                        join userOuRoles in dbContext.Roles on roleOu.RoleId equals userOuRoles.Id
                        where userOu.UserId == id
                        select userOuRoles.Name;

            var result = await query.ToListAsync(GetCancellationToken(cancellationToken));

            return result;
        }

        public virtual async Task<IdentityUser> FindByLoginAsync(
            string loginProvider,
            string providerKey,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .IncludeDetails(includeDetails)
                .Where(u => u.Logins.Any(login => login.LoginProvider == loginProvider && login.ProviderKey == providerKey))
                .OrderBy(x=>x.Id)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<IdentityUser> FindByNormalizedEmailAsync(
            string normalizedEmail,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .IncludeDetails(includeDetails)
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityUser>> GetListByClaimAsync(
            Claim claim,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .IncludeDetails(includeDetails)
                .Where(u => u.Claims.Any(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityUser>> GetListByNormalizedRoleNameAsync(
            string normalizedRoleName,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            var role = await dbContext.Roles
                .Where(x => x.NormalizedName == normalizedRoleName)
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));

            if (role == null)
            {
                return new List<IdentityUser>();
            }

            return await dbContext.Users
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
            Guid? roleId = null,
            Guid? organizationUnitId = null,
            string userName = null,
            string phoneNumber = null,
            string emailAddress = null,
            bool? isLockedOut = null,
            bool? notActive = null,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .IncludeDetails(includeDetails)
                .WhereIf(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.UserName.Contains(filter) ||
                        u.Email.Contains(filter) ||
                        (u.Name != null && u.Name.Contains(filter)) ||
                        (u.Surname != null && u.Surname.Contains(filter)) ||
                        (u.PhoneNumber != null && u.PhoneNumber.Contains(filter))
                )
                .WhereIf(roleId.HasValue, identityUser => identityUser.Roles.Any(x => x.RoleId == roleId.Value))
                .WhereIf(organizationUnitId.HasValue, identityUser => identityUser.OrganizationUnits.Any(x => x.OrganizationUnitId == organizationUnitId.Value))
                .WhereIf(!string.IsNullOrWhiteSpace(userName), x => x.UserName == userName)
                .WhereIf(!string.IsNullOrWhiteSpace(phoneNumber), x => x.PhoneNumber == phoneNumber)
                .WhereIf(!string.IsNullOrWhiteSpace(emailAddress), x => x.Email == emailAddress)
                .WhereIf(isLockedOut == true, x => x.LockoutEnabled && x.LockoutEnd > DateTimeOffset.UtcNow)
                .WhereIf(notActive == true, x => !x.IsActive)
                .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(IdentityUser.UserName) : sorting)
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityRole>> GetRolesAsync(
            Guid id,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            var query = from userRole in dbContext.Set<IdentityUserRole>()
                        join role in dbContext.Roles.IncludeDetails(includeDetails) on userRole.RoleId equals role.Id
                        where userRole.UserId == id
                        select role;

            //TODO: Needs improvement
            var userOrganizationsQuery = from userOrg in dbContext.Set<IdentityUserOrganizationUnit>()
                                         join ou in dbContext.OrganizationUnits.IncludeDetails(includeDetails) on userOrg.OrganizationUnitId equals ou.Id
                                         where userOrg.UserId == id
                                         select ou;

            var orgUserRoleQuery = dbContext.Set<OrganizationUnitRole>()
                .Where(q => userOrganizationsQuery
                .Select(t => t.Id)
                .Contains(q.OrganizationUnitId))
                .Select(t => t.RoleId)
                .ToArray();

            var orgRoles = dbContext.Roles.Where(q => orgUserRoleQuery.Contains(q.Id));
            var resultQuery = query.Union(orgRoles);

            return await resultQuery.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<long> GetCountAsync(
            string filter = null,
            Guid? roleId = null,
            Guid? organizationUnitId = null,
            string userName = null,
            string phoneNumber = null,
            string emailAddress = null,
            bool? isLockedOut = null,
            bool? notActive = null,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .WhereIf(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.UserName.Contains(filter) ||
                        u.Email.Contains(filter) ||
                        (u.Name != null && u.Name.Contains(filter)) ||
                        (u.Surname != null && u.Surname.Contains(filter)) ||
                        (u.PhoneNumber != null && u.PhoneNumber.Contains(filter))
                )
                .WhereIf(roleId.HasValue, identityUser => identityUser.Roles.Any(x => x.RoleId == roleId.Value))
                .WhereIf(organizationUnitId.HasValue, identityUser => identityUser.OrganizationUnits.Any(x => x.OrganizationUnitId == organizationUnitId.Value))
                .WhereIf(!string.IsNullOrWhiteSpace(userName), x => x.UserName == userName)
                .WhereIf(!string.IsNullOrWhiteSpace(phoneNumber), x => x.PhoneNumber == phoneNumber)
                .WhereIf(!string.IsNullOrWhiteSpace(emailAddress), x => x.Email == emailAddress)
                .WhereIf(isLockedOut == true, x => x.LockoutEnabled && x.LockoutEnd > DateTimeOffset.UtcNow)
                .WhereIf(notActive == true, x => !x.IsActive)
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OrganizationUnit>> GetOrganizationUnitsAsync(
            Guid id,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            var query = from userOu in dbContext.Set<IdentityUserOrganizationUnit>()
                        join ou in dbContext.OrganizationUnits.IncludeDetails(includeDetails) on userOu.OrganizationUnitId equals ou.Id
                        where userOu.UserId == id
                        select ou;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityUser>> GetUsersInOrganizationUnitAsync(
            Guid organizationUnitId,
            CancellationToken cancellationToken = default
            )
        {
            var dbContext = await GetDbContextAsync();

            var query = from userOu in dbContext.Set<IdentityUserOrganizationUnit>()
                        join user in dbContext.Users on userOu.UserId equals user.Id
                        where userOu.OrganizationUnitId == organizationUnitId
                        select user;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityUser>> GetUsersInOrganizationsListAsync(
            List<Guid> organizationUnitIds,
            CancellationToken cancellationToken = default
            )
        {
            var dbContext = await GetDbContextAsync();

            var query = from userOu in dbContext.Set<IdentityUserOrganizationUnit>()
                        join user in dbContext.Users on userOu.UserId equals user.Id
                        where organizationUnitIds.Contains(userOu.OrganizationUnitId)
                        select user;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityUser>> GetUsersInOrganizationUnitWithChildrenAsync(
            string code,
            CancellationToken cancellationToken = default
            )
        {
            var dbContext = await GetDbContextAsync();

            var query = from userOu in dbContext.Set<IdentityUserOrganizationUnit>()
                        join user in dbContext.Users on userOu.UserId equals user.Id
                        join ou in dbContext.Set<OrganizationUnit>() on userOu.OrganizationUnitId equals ou.Id
                        where ou.Code.StartsWith(code)
                        select user;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        [Obsolete("Use WithDetailsAsync method.")]
        public override IQueryable<IdentityUser> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }

        public override async Task<IQueryable<IdentityUser>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).IncludeDetails();
        }
    }
}
