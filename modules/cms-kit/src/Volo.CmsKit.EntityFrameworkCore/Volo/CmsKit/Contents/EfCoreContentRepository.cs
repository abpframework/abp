using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Contents
{
    public class EfCoreContentRepository : EfCoreRepository<ICmsKitDbContext, Content, Guid>, IContentRepository
    {
        public EfCoreContentRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}