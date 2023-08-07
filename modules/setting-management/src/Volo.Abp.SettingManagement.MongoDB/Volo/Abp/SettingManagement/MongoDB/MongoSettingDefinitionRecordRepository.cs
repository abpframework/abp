using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.SettingManagement.MongoDB;

public class MongoSettingDefinitionRecordRepository : MongoDbRepository<ISettingManagementMongoDbContext, SettingDefinitionRecord, Guid>, ISettingDefinitionRecordRepository
{
    public MongoSettingDefinitionRecordRepository(IMongoDbContextProvider<ISettingManagementMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public virtual async Task<SettingDefinitionRecord> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(s => s.Name == name, GetCancellationToken(cancellationToken));
    }
}
