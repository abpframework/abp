using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.TestApp.Domain
{
    public interface ICityRepository : IBasicRepository<City, Guid>
    {
        Task<City> FindByNameAsync(string name);
    }
}
