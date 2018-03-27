using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MemoryDb;
using Volo.Abp.MemoryDb;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.TestApp.MemoryDb
{
    public class CityRepository : MemoryDbRepository<TestAppMemoryDbContext, City, Guid>, ICityRepository
    {
        public CityRepository(IMemoryDatabaseProvider<TestAppMemoryDbContext> databaseProvider) 
            : base(databaseProvider)
        {
        }

        public Task<City> FindByNameAsync(string name)
        {
            return Task.FromResult(Collection.FirstOrDefault(c => c.Name == name));
        }

        public async Task<List<Person>> GetPeopleInTheCityAsync(string cityName)
        {
            var city = await FindByNameAsync(cityName);

            return Database.Collection<Person>().Where(p => p.CityId == city.Id).ToList();
        }
    }
}
