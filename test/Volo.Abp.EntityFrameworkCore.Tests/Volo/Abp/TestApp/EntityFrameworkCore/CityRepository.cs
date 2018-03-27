using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.TestApp.EntityFrameworkCore
{
    public class CityRepository : EfCoreRepository<TestAppDbContext, City, Guid>, ICityRepository
    {
        public CityRepository(IDbContextProvider<TestAppDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public async Task<City> FindByNameAsync(string name)
        {
            return await this.FirstOrDefaultAsync(c => c.Name == name);
        }
    }
}
