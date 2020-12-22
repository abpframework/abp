using System;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.MongoDB.Contents
{
    public class MongoContentRepository : MongoDbRepository<ICmsKitMongoDbContext, Content, Guid>, IContentRepository
    {
        public MongoContentRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}