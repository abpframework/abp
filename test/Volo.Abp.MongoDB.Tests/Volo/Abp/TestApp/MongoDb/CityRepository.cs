using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.TestApp.MongoDb
{
    public class CityRepository : MongoDbRepository<ITestAppMongoDbContext, City, Guid>, ICityRepository
    {
        public CityRepository(IMongoDatabaseProvider<ITestAppMongoDbContext> databaseProvider)
            : base(databaseProvider)
        {

        }

        public async Task<City> FindByNameAsync(string name)
        {
            return await (await Collection.FindAsync(c => c.Name == name)).FirstOrDefaultAsync();
        }
    }
}
