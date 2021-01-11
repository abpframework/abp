using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Blogging.EntityFrameworkCore;

namespace Volo.Blogging.Blogs
{
    public class EfCoreBlogRepository : EfCoreRepository<IBloggingDbContext, Blog, Guid>, IBlogRepository
    {
        public EfCoreBlogRepository(IDbContextProvider<IBloggingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<Blog> FindByShortNameAsync(string shortName)
        {
            return await (await GetDbSetAsync()).FirstOrDefaultAsync(p => p.ShortName == shortName);
        }
    }
}
