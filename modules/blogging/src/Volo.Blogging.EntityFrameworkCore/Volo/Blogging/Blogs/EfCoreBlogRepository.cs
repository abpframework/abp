using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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
            return await DbSet.FirstOrDefaultAsync(p => p.ShortName == shortName);
        }

        public async Task<List<Blog>> GetListAsync(string sorting, int maxResultCount, int skipCount)
        {
            var auditLogs = await DbSet.OrderBy(sorting ?? "creationTime desc")
                .PageBy(skipCount, maxResultCount)
                .ToListAsync();

            return auditLogs;
        }

        public async Task<int> GetTotalCount()
        {
            return await DbSet.CountAsync();
        }
    }
}
