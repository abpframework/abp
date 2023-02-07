using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.FeatureManagement.MongoDB;

public class MongoFeatureDefinitionRecordRepository :
    MongoDbRepository<IFeatureManagementMongoDbContext, FeatureDefinitionRecord, Guid>,
    IFeatureDefinitionRecordRepository
{
    public MongoFeatureDefinitionRecordRepository(
        IMongoDbContextProvider<IFeatureManagementMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public virtual async Task<FeatureDefinitionRecord> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        return await (await GetMongoQueryableAsync(cancellationToken))
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(
                s => s.Name == name,
                cancellationToken
            );
    }
}
