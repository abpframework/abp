using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.OpenIddict.MongoDB;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Volo.Abp.OpenIddict.Tokens
{
    public class MongoDbOpenIddictTokenRepository
        : MongoDbRepository<IOpenIddictMongoDbContext, OpenIddictToken, Guid>, IOpenIddictTokenRepository
    {
        public MongoDbOpenIddictTokenRepository(IMongoDbContextProvider<IOpenIddictMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<List<OpenIddictToken>> FindAsync(
            string subject,
            Guid applicationId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));

            return await query
                .Where(token => token.Subject == subject && token.ApplicationId == applicationId)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OpenIddictToken>> FindAsync(
            string subject,
            Guid applicationId,
            string status,
            CancellationToken cancellationToken = default)
        {
            var query = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));

            return await query
                .Where(token => token.Subject == subject &&
                        token.ApplicationId == applicationId &&
                        token.Status == status)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OpenIddictToken>> FindAsync(
            string subject,
            Guid applicationId,
            string status,
            string type,
            CancellationToken cancellationToken = default)
        {
            var query = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));

            return await query
                .Where(token => token.Subject == subject &&
                        token.ApplicationId == applicationId &&
                        token.Status == status &&
                        token.Type == type)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OpenIddictToken>> FindByApplicationIdAsync(
            Guid applicationId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));

            return await query
                .Where(token => token.ApplicationId == applicationId)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OpenIddictToken>> FindByAuthorizationIdAsync(
            Guid authorizationId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));

            return await query
                .Where(token => token.AuthorizationId == authorizationId)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<OpenIddictToken> FindByReferenceIdAsync(

            string referenceId, CancellationToken cancellationToken = default)
        {
            var query = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));

            return await query
                .Where(token => token.ReferenceId == referenceId)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OpenIddictToken>> FindBySubjectAsync(
            string subject,
            CancellationToken cancellationToken = default)
        {
            var query = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));

            return await query
                .Where(token => token.Subject == subject)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OpenIddictToken>> GetListAsync(
            int? maxResultCount,
            int? skipCount,
            CancellationToken cancellationToken = default)
        {
            var query = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));

            if (skipCount.HasValue)
            {
                query = query.Skip(skipCount.Value);
            }
            if (maxResultCount.HasValue)
            {
                query = query.Take(maxResultCount.Value);
            }
            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OpenIddictToken>> GetPruneListAsync(
            DateTime date,
            int maxResultCount = 1_000,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync(GetCancellationToken(cancellationToken));
            var query = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));

            return
                await (from token in query
                       join authorization in dbContext.Authorizations
                                          on token.AuthorizationId equals authorization.Id into authorizations
                       where token.CreationDate < date
                       where (token.Status != Statuses.Inactive && token.Status != Statuses.Valid) ||
                              token.ExpirationDate < DateTime.UtcNow ||
                              authorizations.Any(authorization => authorization.Status != Statuses.Valid)
                       select token)
                       .OrderBy(token => token.Id)
                       .Take(maxResultCount)
                       .ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}