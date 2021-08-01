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

namespace Volo.Abp.OpenIddict.Authorizations
{
    public class MongoDbOpenIddictAuthorizationRepository
        : MongoDbRepository<IOpenIddictMongoDbContext, OpenIddictAuthorization, Guid>, IOpenIddictAuthorizationRepository
    {
        public MongoDbOpenIddictAuthorizationRepository(IMongoDbContextProvider<IOpenIddictMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<List<OpenIddictAuthorization>> FindAsync(
            string subject,
            Guid applicationId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));

            return await query
                .Where(authorization => authorization.Subject == subject && authorization.ApplicationId == applicationId)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OpenIddictAuthorization>> FindAsync(
            string subject,
            Guid applicationId,
            string status,
            CancellationToken cancellationToken = default)
        {
            var query = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));

            return await query
                .Where(authorization => authorization.Subject == subject &&
                        authorization.ApplicationId == applicationId &&
                        authorization.Status == status)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OpenIddictAuthorization>> FindAsync(
            string subject,
            Guid applicationId,
            string status,
            string type,
            CancellationToken cancellationToken = default)
        {
            var query = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));

            return await query
                .Where(authorization => authorization.Subject == subject &&
                        authorization.ApplicationId == applicationId &&
                        authorization.Status == status &&
                        authorization.Type == type)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OpenIddictAuthorization>> FindByApplicationIdAsync(
            Guid applicationId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));

            return await query
                .Where(authorization => authorization.ApplicationId == applicationId)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OpenIddictAuthorization>> FindBySubjectAsync(
            string subject,
            CancellationToken cancellationToken = default)
        {
            var query = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));

            return await query
                .Where(authorization => authorization.Subject == subject)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OpenIddictAuthorization>> GetListAsync(
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

        public virtual async Task<List<OpenIddictAuthorization>> GetPruneListAsync(
            DateTime date,
            int maxResultCount = 1_000,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync(GetCancellationToken(cancellationToken));
            var query = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));

            return
                await (from authorization in query
                       join token in dbContext.Tokens
                                  on authorization.Id equals token.AuthorizationId into tokens
                       where authorization.CreationDate < date
                       where authorization.Status != Statuses.Valid ||
                            (authorization.Type == AuthorizationTypes.AdHoc && !tokens.Any())
                       select authorization)
                       .OrderBy(x => x.Id)
                       .Take(maxResultCount)
                       .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public override async Task DeleteAsync(
            OpenIddictAuthorization entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync(GetCancellationToken(cancellationToken));

            await base.DeleteAsync(entity, autoSave, cancellationToken);

            await dbContext.Tokens.DeleteManyAsync(token => token.AuthorizationId == entity.Id);
        }
    }
}