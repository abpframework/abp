using System;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Blogging.MongoDB;

namespace Volo.Blogging.Posts
{
    public class MongoPostTagRepository : MongoDbRepository<IBloggingMongoDbContext, PostTag>, IPostTagRepository
    {
        public MongoPostTagRepository(IMongoDbContextProvider<IBloggingMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public void DeleteOfPost(Guid id)
        {
            var recordsToDelete = GetMongoQueryable().Where(pt => pt.PostId == id);
            foreach (var record in recordsToDelete)
            {
                Delete(record);
            }
        }

        public async Task<PostTag> FindByTagIdAndPostIdAsync(Guid postId, Guid tagId)
        {
            return await GetMongoQueryable().FirstOrDefaultAsync(pt => pt.PostId == postId && pt.TagId == tagId);
        }
    }
}
