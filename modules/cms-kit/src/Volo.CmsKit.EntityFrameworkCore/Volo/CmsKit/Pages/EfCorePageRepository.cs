using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Pages
{
    public class EfCorePageRepository : EfCoreRepository<ICmsKitDbContext, Page, Guid>, IPageRepository
    {
        public EfCorePageRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<int> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync()).WhereIf(
                    !filter.IsNullOrWhiteSpace(), 
                    x =>
                            x.Title.Contains(filter)
                        ).CountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Page>> GetListAsync(
            string filter = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string sorting = null,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync()).WhereIf(
                            !filter.IsNullOrWhiteSpace(), 
                            x =>
                                x.Title.Contains(filter))
                        .OrderBy(sorting ?? nameof(Page.Title))
                        .PageBy(skipCount, maxResultCount)
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

        public virtual async Task<bool> ExistsAsync(string url, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync()).AnyAsync(x => x.Url == url, GetCancellationToken(cancellationToken));
        }
    }
}
