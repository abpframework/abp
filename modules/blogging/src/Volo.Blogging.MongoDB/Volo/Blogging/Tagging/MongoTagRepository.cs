using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Blogging.MongoDB;

namespace Volo.Blogging.Tagging
{
    public class MongoTagRepository : MongoDbRepository<IBloggingMongoDbContext, Tag, Guid>, ITagRepository
    {
        public MongoTagRepository(IMongoDbContextProvider<IBloggingMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<Tag>> GetListAsync(Guid blogId)
        {
            return await GetMongoQueryable().Where(t=>t.BlogId == blogId).ToListAsync();
        }

        public async Task<Tag> GetByNameAsync(Guid blogId, string name)
        {
            return await GetMongoQueryable().Where(t => t.BlogId == blogId && t.Name == name).FirstAsync();
        }

        public async Task<Tag> FindByNameAsync(Guid blogId, string name)
        {
            return await GetMongoQueryable().Where(t => t.BlogId == blogId && t.Name == name).FirstOrDefaultAsync();
        }

        public async Task<List<Tag>> GetListAsync(IEnumerable<Guid> ids)
        {
            return await GetMongoQueryable().Where(t => ids.Contains(t.Id)).ToListAsync();
        }

        public void DecreaseUsageCountOfTags(List<Guid> ids)
        {
            var tags = GetMongoQueryable().Where(t => ids.Contains(t.Id));

            foreach (var tag in tags)
            {
                tag.DecreaseUsageCount();
            }
        }
    }
}
