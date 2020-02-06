using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.Guids;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB
{
    public class MongoIdentityUserRepository : MongoDbRepository<IAbpIdentityMongoDbContext, IdentityUser, Guid>, IIdentityUserRepository
    {
        private readonly IGuidGenerator _guidGenerator;

        public MongoIdentityUserRepository(IMongoDbContextProvider<IAbpIdentityMongoDbContext> dbContextProvider, IGuidGenerator guidGenerator) 
            : base(dbContextProvider)
        {
            _guidGenerator = guidGenerator;
        }

        public async Task<IdentityUser> FindByNormalizedUserNameAsync(
            string normalizedUserName, 
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .FirstOrDefaultAsync(
                    u => u.NormalizedUserName == normalizedUserName,
                    GetCancellationToken(cancellationToken)
                );
        }

        public async Task<List<string>> GetRoleNamesAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var user = await GetAsync(id, cancellationToken: GetCancellationToken(cancellationToken));
            var roleIds = user.Roles.Select(r => r.RoleId).ToArray();
            return await DbContext.Roles.AsQueryable().Where(r => roleIds.Contains(r.Id)).Select(r => r.Name).ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<IdentityUser> FindByLoginAsync(
            string loginProvider, 
            string providerKey, 
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .Where(u => u.Logins.Any(login => login.LoginProvider == loginProvider && login.ProviderKey == providerKey))
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<IdentityUser> FindByNormalizedEmailAsync(
            string normalizedEmail,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable().FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, GetCancellationToken(cancellationToken));
        }

        public async Task<List<IdentityUser>> GetListByClaimAsync(
            Claim claim,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .Where(u => u.Claims.Any(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<IdentityUser>> GetListByNormalizedRoleNameAsync(
            string normalizedRoleName, 
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var role = await DbContext.Roles.AsQueryable().Where(x => x.NormalizedName == normalizedRoleName).FirstOrDefaultAsync(GetCancellationToken(cancellationToken));

            if (role == null)
            {
                return new List<IdentityUser>();
            }

            return await GetMongoQueryable()
                .Where(u => u.Roles.Any(r => r.RoleId == role.Id))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<IdentityUser>> GetListAsync(
            string sorting = null,
            int maxResultCount = int.MaxValue, 
            int skipCount = 0, 
            string filter = null, 
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .WhereIf<IdentityUser, IMongoQueryable<IdentityUser>>(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.UserName.Contains(filter) ||
                        u.Email.Contains(filter)
                )
                .OrderBy(sorting ?? nameof(IdentityUser.UserName))
                .As<IMongoQueryable<IdentityUser>>()
                .PageBy<IdentityUser, IMongoQueryable<IdentityUser>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<IdentityRole>> GetRolesAsync(
            Guid id,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var user = await GetAsync(id, cancellationToken: GetCancellationToken(cancellationToken));
            var roleIds = user.Roles.Select(r => r.RoleId).ToArray();
            return await DbContext.Roles.AsQueryable().Where(r => roleIds.Contains(r.Id)).ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<long> GetCountAsync(
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .WhereIf<IdentityUser, IMongoQueryable<IdentityUser>>(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.UserName.Contains(filter) ||
                        u.Email.Contains(filter)
                )
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }
    }
}