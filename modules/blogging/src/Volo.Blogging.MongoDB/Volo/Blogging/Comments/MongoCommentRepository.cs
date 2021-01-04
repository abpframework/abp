using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Blogging.MongoDB;

namespace Volo.Blogging.Comments
{
    public class MongoCommentRepository : MongoDbRepository<IBloggingMongoDbContext, Comment, Guid>, ICommentRepository
    {
        public MongoCommentRepository(IMongoDbContextProvider<IBloggingMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<Comment>> GetListOfPostAsync(Guid postId)
        {
            return await (await GetMongoQueryableAsync())
                .Where(a => a.PostId == postId)
                .OrderBy(a => a.CreationTime)
                .ToListAsync();
        }

        public async Task<int> GetCommentCountOfPostAsync(Guid postId)
        {
            return await (await GetMongoQueryableAsync())
                .CountAsync(a => a.PostId == postId);
        }

        public async Task<List<Comment>> GetRepliesOfComment(Guid id)
        {
            return await (await GetMongoQueryableAsync())
                .Where(a => a.RepliedCommentId == id).ToListAsync();
        }

        public async Task DeleteOfPost(Guid id)
        {
            var recordsToDelete = (await GetMongoQueryableAsync()).Where(pt => pt.PostId == id);

            foreach (var record in recordsToDelete)
            {
                await DeleteAsync(record);
            }
        }
    }
}
