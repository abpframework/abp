using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;

namespace Volo.Abp.OpenIddict.Applications
{
    public class EfCoreOpenIddictApplicationRepository
        : EfCoreRepository<IOpenIddictDbContext, OpenIddictApplication, Guid>, IOpenIddictApplicationRepository
    {
        public EfCoreOpenIddictApplicationRepository(IDbContextProvider<IOpenIddictDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<OpenIddictApplication> FindByClientIdAsync(
            string clientId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query.FirstOrDefaultAsync(a => a.ClientId == clientId, GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OpenIddictApplication>> FindByPostLogoutRedirectUriAsync(
            string address,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            //https://entityframeworkcore.com/knowledge-base/60969027/how-to-convert-string-to-datetime-in-csharp-ef-core-query
            query = query
                .Where(x => ((string)(object)x.PostLogoutRedirectUris).Contains(address));

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OpenIddictApplication>> FindByRedirectUriAsync(
            string address,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            //https://entityframeworkcore.com/knowledge-base/60969027/how-to-convert-string-to-datetime-in-csharp-ef-core-query
            query = query
                .Where(x => ((string)(object)x.RedirectUris).Contains(address));

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OpenIddictApplication>> GetListAsync(
            int? maxResultCount,
            int? skipCount,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

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
            var dbContext = await GetDbContextAsync();

            var authorizations = await dbContext.Authorizations
                .Where(authorization => authorization.ApplicationId == entity.Id)
                .ToListAsync(GetCancellationToken(cancellationToken));

            var tokens = await dbContext.Tokens
                .Where(token => token.ApplicationId == entity.Id)
                .ToListAsync(GetCancellationToken(cancellationToken));

            dbContext.Tokens.RemoveRange(tokens);

            dbContext.Authorizations.RemoveRange(authorizations);

            await base.DeleteAsync(entity, autoSave, cancellationToken);
        }
    }
}