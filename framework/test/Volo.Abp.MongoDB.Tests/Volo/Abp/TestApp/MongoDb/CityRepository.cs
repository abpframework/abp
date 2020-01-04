﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.TestApp.MongoDB
{
    public class CityRepository : MongoDbRepository<ITestAppMongoDbContext, City, Guid>, ICityRepository
    {
        public CityRepository(IMongoDbContextProvider<ITestAppMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<City> FindByNameAsync(string name)
        {
            return await (await Collection.FindAsync(c => c.Name == name).ConfigureAwait(false)).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<List<Person>> GetPeopleInTheCityAsync(string cityName)
        {
            var city = await FindByNameAsync(cityName).ConfigureAwait(false);
            return await DbContext.People.AsQueryable().Where(p => p.CityId == city.Id).ToListAsync().ConfigureAwait(false);
        }
    }
}
