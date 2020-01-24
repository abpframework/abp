using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.TestApp.Testing
{
    public abstract class Repository_Basic_Tests<TStartupModule> : TestAppTestBase<TStartupModule> 
        where TStartupModule : IAbpModule
    {
        protected readonly IRepository<Person, Guid> PersonRepository;
        protected readonly ICityRepository CityRepository;

        protected Repository_Basic_Tests()
        {
            PersonRepository = GetRequiredService<IRepository<Person, Guid>>();
            CityRepository = GetRequiredService<ICityRepository>();
        }

        [Fact]
        public async Task GetAsync()
        {
            var person = await PersonRepository.GetAsync(TestDataBuilder.UserDouglasId).ConfigureAwait(false);
            person.Name.ShouldBe("Douglas");
            person.Phones.Count.ShouldBe(2);
        }

        [Fact]
        public async Task FindAsync_Should_Return_Null_For_Not_Found_Entity()
        {
            var person = await PersonRepository.FindAsync(Guid.NewGuid()).ConfigureAwait(false);
            person.ShouldBeNull();
        }

        [Fact]
        public async Task DeleteAsync()
        {
            await PersonRepository.DeleteAsync(TestDataBuilder.UserDouglasId).ConfigureAwait(false);

            (await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId).ConfigureAwait(false)).ShouldBeNull();
        }

        [Fact]
        public async Task Should_Access_To_Other_Collections_In_Same_Context_In_A_Custom_Method()
        {
            var people = await CityRepository.GetPeopleInTheCityAsync("London").ConfigureAwait(false);
            people.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Custom_Repository_Method()
        {
            var city = await CityRepository.FindByNameAsync("Istanbul").ConfigureAwait(false);
            city.ShouldNotBeNull();
            city.Name.ShouldBe("Istanbul");
        }

        [Fact]
        public virtual async Task InsertAsync()
        {
            var personId = Guid.NewGuid();

            await PersonRepository.InsertAsync(new Person(personId, "Adam", 42)).ConfigureAwait(false);

            var person = await PersonRepository.FindAsync(personId).ConfigureAwait(false);
            person.ShouldNotBeNull();
        }
    }
}
