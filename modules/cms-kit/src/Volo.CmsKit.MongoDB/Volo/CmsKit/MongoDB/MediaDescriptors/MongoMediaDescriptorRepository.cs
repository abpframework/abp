using System;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.MediaDescriptors;

namespace Volo.CmsKit.MongoDB.MediaDescriptors
{
    public class MongoMediaDescriptorRepository : MongoDbRepository<ICmsKitMongoDbContext, MediaDescriptor, Guid>, IMediaDescriptorRepository
    {
        public MongoMediaDescriptorRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}