using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.TestApp.Domain
{
    public interface ICityRepository : IBasicRepository<City, Guid>
    {
        [CanBeNull]
        Task<City> FindByNameAsync(string name);
    }
}
