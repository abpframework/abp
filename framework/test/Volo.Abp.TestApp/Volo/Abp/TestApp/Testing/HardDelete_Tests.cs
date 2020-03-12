using Shouldly;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.TestApp.Testing
{
    public abstract class HardDelete_Tests<TStartupModule> : TestAppTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected readonly IRepository<Person, Guid> PersonRepository;
        protected readonly IDataFilter DataFilter;
        protected readonly IUnitOfWorkManager UnitOfWorkManager;

        protected HardDelete_Tests()
        {
            PersonRepository = GetRequiredService<IRepository<Person, Guid>>();
            DataFilter = GetRequiredService<IDataFilter>();
            UnitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        }

        [Fact]
        public async Task Should_HardDelete_Entities()
        {
            var douglas = await PersonRepository.GetAsync(TestDataBuilder.UserDouglasId);
            await PersonRepository.HardDeleteAsync(douglas);

            douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);
            douglas.ShouldBeNull();
        }

        [Fact]
        public async Task Should_HardDelete_Soft_Deleted_Entities()
        {
            var douglas = await PersonRepository.GetAsync(TestDataBuilder.UserDouglasId);
            await PersonRepository.DeleteAsync(douglas);

            douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);
            douglas.ShouldBeNull();

            using (DataFilter.Disable<ISoftDelete>())
            {
                douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);
                douglas.ShouldNotBeNull();
                douglas.IsDeleted.ShouldBeTrue();
                douglas.DeletionTime.ShouldNotBeNull();
            }

            using (var uow = UnitOfWorkManager.Begin())
            {
                using (DataFilter.Disable<ISoftDelete>())
                {
                    douglas = await PersonRepository.GetAsync(TestDataBuilder.UserDouglasId);
                }

                await PersonRepository.HardDeleteAsync(douglas);
                await uow.CompleteAsync();
            }

            douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);
            douglas.ShouldBeNull();
        }
    }
}
