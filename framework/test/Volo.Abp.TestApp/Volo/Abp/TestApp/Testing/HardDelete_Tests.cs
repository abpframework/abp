using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
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
        protected readonly IRepository<Person, Guid> _personRepository;
        protected readonly IDataFilter DataFilter;
        protected readonly IUnitOfWorkManager _unitOfWorkManager;
        public HardDelete_Tests()
        {
            _personRepository = GetRequiredService<IRepository<Person, Guid>>();
            DataFilter = GetRequiredService<IDataFilter>();
            _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        }
        [Fact]
        public async Task Should_HardDelete_Entity_With_Collection()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                using (DataFilter.Disable<ISoftDelete>())
                {
                    var douglas = await _personRepository.FindAsync(TestDataBuilder.UserDouglasId);
                    await _personRepository.HardDeleteAsync(x => x.Id == TestDataBuilder.UserDouglasId);
                    await uow.CompleteAsync();
                }

                var deletedDougles = await _personRepository.FindAsync(TestDataBuilder.UserDouglasId);
                deletedDougles.ShouldBeNull();
            }
        }
        [Fact]
        public async Task Should_HardDelete_Soft_Deleted_Entities()
        {
            var douglas = await _personRepository.GetAsync(TestDataBuilder.UserDouglasId);
            await _personRepository.DeleteAsync(douglas);

            douglas = await _personRepository.FindAsync(TestDataBuilder.UserDouglasId);
            douglas.ShouldBeNull();

            using (DataFilter.Disable<ISoftDelete>())
            {
                douglas = await _personRepository.FindAsync(TestDataBuilder.UserDouglasId);
                douglas.ShouldNotBeNull();
                douglas.IsDeleted.ShouldBeTrue();
                douglas.DeletionTime.ShouldNotBeNull();
            }
            using (var uow = _unitOfWorkManager.Begin())
            {
                using (DataFilter.Disable<ISoftDelete>())
                {
                    douglas = await _personRepository.GetAsync(TestDataBuilder.UserDouglasId);
                    await _personRepository.HardDeleteAsync(douglas);
                    await uow.CompleteAsync();
                    var deletedDougles = await _personRepository.FindAsync(TestDataBuilder.UserDouglasId);
                    deletedDougles.ShouldBeNull();
                }
            }
        }
    }
}
