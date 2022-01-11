using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.Specifications;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.TestApp.Testing
{
    public abstract class Repository_Specifications_Tests<TStartupModule> : TestAppTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected readonly IRepository<City, Guid> CityRepository;

        protected Repository_Specifications_Tests()
        {
            CityRepository = GetRequiredService<IRepository<City, Guid>>();
        }

        [Fact]
        public async Task SpecificationWithRepository_Test()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                (await CityRepository.CountAsync(new CitySpecification().ToExpression())).ShouldBe(1);
                return Task.CompletedTask;
            });
        }
    }

    public class CitySpecification : Specification<City>
    {
        public override Expression<Func<City, bool>> ToExpression()
        {
            return city => city.Name == "Istanbul";
        }
    }
}
