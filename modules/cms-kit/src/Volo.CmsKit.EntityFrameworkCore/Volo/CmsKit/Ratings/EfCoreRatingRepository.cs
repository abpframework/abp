using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Ratings
{
    public class EfCoreRatingRepository : EfCoreRepository<ICmsKitDbContext, Rating, Guid>, IRatingRepository
    {
        public EfCoreRatingRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}