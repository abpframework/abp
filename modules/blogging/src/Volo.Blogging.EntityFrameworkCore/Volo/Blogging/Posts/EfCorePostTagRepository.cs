using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Blogging.EntityFrameworkCore;

namespace Volo.Blogging.Posts
{
    public class EfCorePostTagRepository : EfCoreRepository<IBloggingDbContext, PostTag>, IPostTagRepository
    {
        public EfCorePostTagRepository(IDbContextProvider<IBloggingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
