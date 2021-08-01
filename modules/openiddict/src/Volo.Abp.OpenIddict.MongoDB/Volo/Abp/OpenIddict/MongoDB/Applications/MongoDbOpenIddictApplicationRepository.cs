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

namespace Volo.Abp.OpenIddict.Applications
{
    public class MongoDbOpenIddictApplicationRepository
        : MongoDbRepository<IOpenIddictMongoDbContext, OpenIddictApplication, Guid>, IOpenIddictApplicationRepository
    {
        public MongoDbOpenIddictApplicationRepository(IMongoDbContextProvider<IOpenIddictMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<OpenIddictApplication> FindByClientIdAsync(
            string clientId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));

            return await query.FirstOrDefaultAsync(a => a.ClientId == clientId, GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OpenIddictApplication>> FindByPostLogoutRedirectUriAsync(
            string address,
            CancellationToken cancellationToken = default)
        {
            var query = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));

            query = query
                .Where(x => x.PostLogoutRedirectUris.Contains(address));

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OpenIddictApplication>> FindByRedirectUriAsync(
            string address,
            CancellationToken cancellationToken = default)
        {
            var query = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));

            query = query
                .Where(x => x.RedirectUris.Contains(address));

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OpenIddictApplication>> GetListAsync(
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

        public override async Task DeleteAsync(
            OpenIddictApplication entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync(GetCancellationToken(cancellationToken));

            await base.DeleteAsync(entity, autoSave, cancellationToken);

            await dbContext.Authorizations
                .DeleteManyAsync(
                    authorization => authorization.ApplicationId == entity.Id,
                    GetCancellationToken(cancellationToken));

            await dbContext.Tokens
                .DeleteManyAsync(
                    token => token.ApplicationId == entity.Id,
                    GetCancellationToken(cancellationToken));
        }
    }
}