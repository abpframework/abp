using System.Linq;
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
        public virtual void FirstOrDefault()
        {
            WithUnitOfWork(() =>
            {
                var entity = EntityWithIntPkRepository.FirstOrDefault(e => e.Name == "Entity1");
                entity.ShouldNotBeNull();
                entity.Name.ShouldBe("Entity1");
            });
        }

        [Fact]
        public virtual void Get()
        {
            WithUnitOfWork(() =>
            {
                var entity = EntityWithIntPkRepository.Get(1);
                entity.ShouldNotBeNull();
                entity.Name.ShouldBe("Entity1");
            });
        }
    }
}
