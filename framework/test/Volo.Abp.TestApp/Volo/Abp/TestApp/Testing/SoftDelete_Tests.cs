﻿using System;
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
            var douglas = await PersonRepository.GetAsync(TestDataBuilder.UserDouglasId).ConfigureAwait(false);
            await PersonRepository.DeleteAsync(douglas).ConfigureAwait(false);

            douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId).ConfigureAwait(false);
            douglas.ShouldBeNull();

            using (DataFilter.Disable<ISoftDelete>())
            {
                douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId).ConfigureAwait(false);
                douglas.ShouldNotBeNull();
                douglas.IsDeleted.ShouldBeTrue();
                douglas.DeletionTime.ShouldNotBeNull();
            }
        }

        [Fact]
        public async Task Should_Handle_Deletion_On_Update_For_Soft_Delete_Entities()
        {
            var douglas = await PersonRepository.GetAsync(TestDataBuilder.UserDouglasId).ConfigureAwait(false);
            douglas.Age = 42;
            douglas.IsDeleted = true;

            await PersonRepository.UpdateAsync(douglas).ConfigureAwait(false);

            douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId).ConfigureAwait(false);
            douglas.ShouldBeNull();

            using (DataFilter.Disable<ISoftDelete>())
            {
                douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId).ConfigureAwait(false);
                douglas.ShouldNotBeNull();
                douglas.IsDeleted.ShouldBeTrue();
                douglas.DeletionTime.ShouldNotBeNull();
                douglas.Age.ShouldBe(42);
            }
        }
    }
}
