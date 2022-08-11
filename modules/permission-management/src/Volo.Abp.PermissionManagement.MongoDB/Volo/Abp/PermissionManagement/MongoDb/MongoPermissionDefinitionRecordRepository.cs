using System;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.PermissionManagement.MongoDB;

public class MongoPermissionDefinitionRecordRepository :
    MongoDbRepository<IPermissionManagementMongoDbContext, PermissionDefinitionRecord, Guid>,
    IPermissionDefinitionRecordRepository
{
    public MongoPermissionDefinitionRecordRepository(
        IMongoDbContextProvider<IPermissionManagementMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
}