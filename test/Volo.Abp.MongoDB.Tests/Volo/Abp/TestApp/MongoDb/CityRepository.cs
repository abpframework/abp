using System;
using System.Collections.Generic;
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

        public async Task<List<Person>> GetPeopleInTheCityAsync(string cityName)
        {
            var city = await FindByNameAsync(cityName);

            throw new NotImplementedException();

            //return await DbContext.People.Where(p => p.CityId == city.Id).ToListAsync();
        }
    }
}
