using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;

namespace Volo.Abp.IdentityServer.Grants
{
    public class PersistentGrantRepository : EfCoreRepository<IIdentityServerDbContext, PersistedGrant, Guid>, IPersistentGrantRepository
    {
        public PersistentGrantRepository(IDbContextProvider<IIdentityServerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<List<PersistedGrant>> GetListAsync(string subjectId, string sessionId, string clientId, string type, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await (await FilterAsync(subjectId, sessionId, clientId, type))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<PersistedGrant> FindByKeyAsync(
            string key,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(x => x.Key == key, GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<PersistedGrant>> GetListBySubjectIdAsync(
            string subjectId,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .Where(x => x.SubjectId == subjectId)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<PersistedGrant>> GetListByExpirationAsync(
            DateTime maxExpirationDate,
            int maxResultCount,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .Where(x => x.Expiration != null && x.Expiration < maxExpirationDate)
                .OrderBy(x => x.ClientId)
                .Take(maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task DeleteAsync(
            string subjectId = null,
            string sessionId = null,
            string clientId = null,
            string type = null,
            CancellationToken cancellationToken = default)
        {
            var persistedGrants = await (await FilterAsync(subjectId, sessionId, clientId, type)).ToListAsync(GetCancellationToken(cancellationToken));

            var dbSet = await GetDbSetAsync();

            foreach (var persistedGrant in persistedGrants)
            {
                dbSet.Remove(persistedGrant);
            }
        }

        private async Task<IQueryable<PersistedGrant>> FilterAsync(
            string subjectId,
            string sessionId,
            string clientId,
            string type)
        {
            return (await GetDbSetAsync())
                .WhereIf(!subjectId.IsNullOrWhiteSpace(), x => x.SubjectId == subjectId)
                .WhereIf(!sessionId.IsNullOrWhiteSpace(), x => x.SessionId == sessionId)
                .WhereIf(!clientId.IsNullOrWhiteSpace(), x => x.ClientId == clientId)
                .WhereIf(!type.IsNullOrWhiteSpace(), x => x.Type == type);
        }
    }
}
