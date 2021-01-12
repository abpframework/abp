using System;
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

        public Task<Page> GetByUrlAsync(string url)
        {
            return GetAsync(x => x.Url == url);
        }

        public Task<Page> FindByUrlAsync(string url)
        {
            return FindAsync(x => x.Url == url);
        }

        public async Task<bool> DoesExistAsync(string url)
        {
            return await (await GetDbSetAsync()).AnyAsync(x => x.Url == url);
        }
    }
}
