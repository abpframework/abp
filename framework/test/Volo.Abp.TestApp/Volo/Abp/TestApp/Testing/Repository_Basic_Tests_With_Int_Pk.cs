using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.TestApp.Testing
{
    public abstract class Repository_Basic_Tests_With_Int_Pk<TStartupModule> : TestAppTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected readonly IRepository<EntityWithIntPk, int> EntityWithIntPkRepository;

        protected Repository_Basic_Tests_With_Int_Pk()
        {
            EntityWithIntPkRepository = GetRequiredService<IRepository<EntityWithIntPk, int>>();
        }

        [Fact]
        public virtual async Task FirstOrDefault()
        {
            await WithUnitOfWorkAsync(() =>
            {
                var entity = EntityWithIntPkRepository.FirstOrDefault(e => e.Name == "Entity1");
                entity.ShouldNotBeNull();
                entity.Name.ShouldBe("Entity1");
                return Task.CompletedTask;
            });
        }

        [Fact]
        public virtual async Task Get()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var entity = await EntityWithIntPkRepository.GetAsync(1);
                entity.ShouldNotBeNull();
                entity.Name.ShouldBe("Entity1");
            });
        }
    }
}
