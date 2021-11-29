using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.TestApp.Testing
{
    public abstract class SoftDelete_Tests<TStartupModule> : TestAppTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected readonly IRepository<Person, Guid> PersonRepository;
        protected readonly IDataFilter DataFilter;

        protected SoftDelete_Tests()
        {
            PersonRepository = GetRequiredService<IRepository<Person, Guid>>();
            DataFilter = GetRequiredService<IDataFilter>();
        }

        [Fact]
        public async Task Should_Cancel_Deletion_For_Soft_Delete_Entities()
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
        }

        [Fact]
        public async Task Should_Cancel_Deletion_For_Soft_Delete_Entities_ById()
        {
            var douglas = await PersonRepository.GetAsync(TestDataBuilder.UserDouglasId);
            await PersonRepository.DeleteAsync(douglas.Id);

            douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);
            douglas.ShouldBeNull();

            using (DataFilter.Disable<ISoftDelete>())
            {
                douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);
                douglas.ShouldNotBeNull();
                douglas.IsDeleted.ShouldBeTrue();
                douglas.DeletionTime.ShouldNotBeNull();
            }
        }

        [Fact]
        public async Task Should_Cancel_Deletion_For_Soft_Delete_Many_Entities_ById()
        {
            await PersonRepository.DeleteManyAsync(new []{ TestDataBuilder.UserDouglasId });

            var douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);
            douglas.ShouldBeNull();

            using (DataFilter.Disable<ISoftDelete>())
            {
                douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);
                douglas.ShouldNotBeNull();
                douglas.IsDeleted.ShouldBeTrue();
                douglas.DeletionTime.ShouldNotBeNull();
            }
        }
        
        [Fact]
        public async Task Should_Handle_Deletion_On_Update_For_Soft_Delete_Entities()
        {
            var douglas = await PersonRepository.GetAsync(TestDataBuilder.UserDouglasId);
            douglas.Age = 42;
            douglas.IsDeleted = true;

            await PersonRepository.UpdateAsync(douglas);

            douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);
            douglas.ShouldBeNull();

            using (DataFilter.Disable<ISoftDelete>())
            {
                douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);
                douglas.ShouldNotBeNull();
                douglas.IsDeleted.ShouldBeTrue();
                douglas.DeletionTime.ShouldNotBeNull();
                douglas.Age.ShouldBe(42);
            }
        }
        
        [Fact]
        public async Task Cascading_Entities_Should_Not_Be_Deleted_When_Soft_Deleting_Entities()
        {
            var douglas = await PersonRepository.GetAsync(TestDataBuilder.UserDouglasId);
            douglas.Phones.ShouldNotBeEmpty();
            
            await PersonRepository.DeleteAsync(douglas);

            douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);
            douglas.ShouldBeNull();

            using (DataFilter.Disable<ISoftDelete>())
            {
                douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);
                douglas.ShouldNotBeNull();
                douglas.IsDeleted.ShouldBeTrue();
                douglas.DeletionTime.ShouldNotBeNull();
                
                douglas.Phones.ShouldNotBeEmpty();
            }
        }
        
        [Fact]
        public async Task Cascading_Entities_Should_Not_Be_Deleted_When_Soft_Deleting_Entities_ById()
        {
            var douglas = await PersonRepository.GetAsync(TestDataBuilder.UserDouglasId);
            douglas.Phones.ShouldNotBeEmpty();
            
            await PersonRepository.DeleteAsync(douglas.Id);

            douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);
            douglas.ShouldBeNull();

            using (DataFilter.Disable<ISoftDelete>())
            {
                douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);
                douglas.ShouldNotBeNull();
                douglas.IsDeleted.ShouldBeTrue();
                douglas.DeletionTime.ShouldNotBeNull();
                
                douglas.Phones.ShouldNotBeEmpty();
            }
        }
    }
}
