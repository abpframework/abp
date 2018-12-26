using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Blogging.MongoDB;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Volo.Blogging.Blogs
{
    public class MongoBlogRepository : MongoDbRepository<IBloggingMongoDbContext, Blog, Guid>, IBlogRepository
    {
        public MongoBlogRepository(IMongoDbContextProvider<IBloggingMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<Blog> FindByShortNameAsync(string shortName)
        {
            return await GetMongoQueryable().FirstOrDefaultAsync(p => p.ShortName == shortName);
        }

        public async Task<List<Blog>> GetListAsync(string sorting, int maxResultCount, int skipCount)
        {
            var auditLogs =  GetMongoQueryable().OrderBy(sorting ?? "creationTime desc").As<IMongoQueryable<Blog>>()
                .PageBy(skipCount, maxResultCount)
                .ToList();

            return auditLogs;
        }

        public async Task<int> GetTotalCount()
        {
            return await GetMongoQueryable().CountAsync();
        }
    }
}
