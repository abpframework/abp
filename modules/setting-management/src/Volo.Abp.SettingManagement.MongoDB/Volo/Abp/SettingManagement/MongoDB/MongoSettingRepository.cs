using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.SettingManagement.MongoDB
{
    public class MongoSettingRepository : MongoDbRepository<ISettingManagementMongoDbContext, Setting, Guid>,
        ISettingRepository
    {
        public MongoSettingRepository(IMongoDbContextProvider<ISettingManagementMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<Setting> FindAsync(
            string name,
            string providerName,
            string providerKey,
            CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken))
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(
                    s => s.Name == name && s.ProviderName == providerName && s.ProviderKey == providerKey,
                    GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Setting>> GetListAsync(
            string providerName,
            string providerKey,
            CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken))
                .Where(s => s.ProviderName == providerName && s.ProviderKey == providerKey)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Setting>> GetListAsync(
            string[] names,
            string providerName,
            string providerKey,
            CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken))
                .Where(s => names.Contains(s.Name) && s.ProviderName == providerName && s.ProviderKey == providerKey)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
