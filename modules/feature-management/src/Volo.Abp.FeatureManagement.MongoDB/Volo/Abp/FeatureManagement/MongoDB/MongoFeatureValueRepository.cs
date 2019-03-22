using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.FeatureManagement.MongoDB
{
    public class MongoFeatureValueRepository : MongoDbRepository<IFeatureManagementMongoDbContext, FeatureValue, Guid>, IFeatureValueRepository
    {
        public MongoFeatureValueRepository(IMongoDbContextProvider<IFeatureManagementMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<FeatureValue> FindAsync(string name, string providerName, string providerKey)
        {
            return await GetMongoQueryable()
                .FirstOrDefaultAsync(s => s.Name == name && s.ProviderName == providerName && s.ProviderKey == providerKey);
        }

        public async Task<List<FeatureValue>> GetListAsync(string providerName, string providerKey)
        {
            return await GetMongoQueryable()
                .Where(s => s.ProviderName == providerName && s.ProviderKey == providerKey)
                .ToListAsync();
        }
    }
}