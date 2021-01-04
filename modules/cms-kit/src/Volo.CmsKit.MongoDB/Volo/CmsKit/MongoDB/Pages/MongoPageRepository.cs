using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Pages;

namespace Volo.CmsKit.MongoDB.Pages
{
    public class MongoPageRepository : MongoDbRepository<ICmsKitMongoDbContext, Page, Guid>, IPageRepository
    {
        public MongoPageRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual Task<int> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
        {
            return (await GetMongoQueryableAsync())
                .WhereIf<Page, IMongoQueryable<Page>>(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.Title.Contains(filter)
                ).CountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual Task<List<Page>> GetListAsync(
            string filter = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0, 
            string sorting = null,
            CancellationToken cancellationToken = default)
        {
            return (await GetMongoQueryableAsync())
                .WhereIf<Page, IMongoQueryable<Page>>(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.Title.Contains(filter)
                )
                .OrderBy(sorting ?? nameof(Page.Title))
                .As<IMongoQueryable<Page>>()
                .PageBy<Page, IMongoQueryable<Page>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual Task<Page> GetByUrlAsync(string url, CancellationToken cancellationToken = default)
        {
            return GetAsync(x => x.Url == url, cancellationToken: GetCancellationToken(cancellationToken));
        }

        public virtual Task<Page> FindByUrlAsync(string url, CancellationToken cancellationToken = default)
        {
            return FindAsync(x => x.Url == url, cancellationToken: GetCancellationToken(cancellationToken));
        }
        
        public virtual Task<bool> ExistsAsync(string url, CancellationToken cancellationToken = default)
        {
            return (await GetMongoQueryableAsync()).AnyAsync(x => x.Url == url, GetCancellationToken(cancellationToken));
        }
    }
}
