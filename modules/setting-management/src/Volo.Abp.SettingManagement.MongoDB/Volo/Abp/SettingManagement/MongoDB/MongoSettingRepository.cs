using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.SettingManagement.MongoDB
{
    public class MongoSettingRepository : MongoDbRepository<ISettingManagementMongoDbContext, Setting, Guid>, ISettingRepository
    {
        public MongoSettingRepository(IMongoDbContextProvider<ISettingManagementMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<Setting> FindAsync(string name, string providerName, string providerKey)
        {
            return await (await GetMongoQueryableAsync())
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(s => s.Name == name && s.ProviderName == providerName && s.ProviderKey == providerKey);
        }

        public virtual async Task<List<Setting>> GetListAsync(string providerName, string providerKey)
        {
            return await (await GetMongoQueryableAsync())
                .Where(s => s.ProviderName == providerName && s.ProviderKey == providerKey)
                .ToListAsync();
        }

        public virtual async Task<List<Setting>> GetListAsync(string[] names, string providerName, string providerKey)
        {
            return await (await GetMongoQueryableAsync())
                .Where(s => names.Contains(s.Name) && s.ProviderName == providerName && s.ProviderKey == providerKey)
                .ToListAsync();
        }
    }
}
