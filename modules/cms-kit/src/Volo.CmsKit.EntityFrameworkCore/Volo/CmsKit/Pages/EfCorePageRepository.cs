using System;
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
    }
}