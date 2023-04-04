using System;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.FeatureManagement.MongoDB;

public class MongoFeatureGroupDefinitionRecordRepository :
    MongoDbRepository<IFeatureManagementMongoDbContext, FeatureGroupDefinitionRecord, Guid>,
    IFeatureGroupDefinitionRecordRepository
{
    public MongoFeatureGroupDefinitionRecordRepository(
        IMongoDbContextProvider<IFeatureManagementMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
}
