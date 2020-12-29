using System;
using System.Collections.Generic;
using System.Linq;
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
            var person = await PersonRepository.GetAsync(TestDataBuilder.UserDouglasId);
            person.Name.ShouldBe("Douglas");
            person.Phones.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetListAsync()
        {
            var persons = await PersonRepository.GetListAsync();
            persons.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task GetPagedListAsync()
        {
            var persons = await PersonRepository.GetPagedListAsync(0, 10, "name");
            persons.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task GetPagedListAsync_Should_Return_Empty()
        {
            var persons = await PersonRepository.GetPagedListAsync(1, 10, "name");
            persons.Count.ShouldBe(0);
        }

        [Fact]
        public async Task GetAsync_With_Predicate()
        {
            var person = await PersonRepository.GetAsync(p => p.Name == "Douglas");
            person.Name.ShouldBe("Douglas");
            person.Phones.Count.ShouldBe(2);
        }

        [Fact]
        public async Task FindAsync_Should_Return_Null_For_Not_Found_Entity()
        {
            var person = await PersonRepository.FindAsync(Guid.NewGuid());
            person.ShouldBeNull();
        }

        [Fact]
        public async Task FindAsync_Should_Return_Null_For_Not_Found_Entity_With_Predicate()
        {
            var randomName = Guid.NewGuid().ToString();
            var person = await PersonRepository.FindAsync(p => p.Name == randomName);
            person.ShouldBeNull();
        }

        [Fact]
        public async Task DeleteAsync()
        {
            await PersonRepository.DeleteAsync(TestDataBuilder.UserDouglasId);

            (await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId)).ShouldBeNull();
        }

        [Fact]
        public async Task Should_Access_To_Other_Collections_In_Same_Context_In_A_Custom_Method()
        {
            var people = await CityRepository.GetPeopleInTheCityAsync("London");
            people.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Custom_Repository_Method()
        {
            var city = await CityRepository.FindByNameAsync("Istanbul");
            city.ShouldNotBeNull();
            city.Name.ShouldBe("Istanbul");
        }

        [Fact]
        public virtual async Task InsertAsync()
        {
            var personId = Guid.NewGuid();

            await PersonRepository.InsertAsync(new Person(personId, "Adam", 42));

            var person = await PersonRepository.FindAsync(personId);
            person.ShouldNotBeNull();
        }

        [Fact]
        public async Task Insert_Should_Set_Guid_Id()
        {
            var person = new Person(Guid.Empty, "New Person", 35);

            await PersonRepository.InsertAsync(person);

            person.Id.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public async Task InserManyAsync()
        {
            var entities = new List<Person>
            {
                new Person(Guid.NewGuid(), "Person 1", 30),
                new Person(Guid.NewGuid(), "Person 2", 31),
                new Person(Guid.NewGuid(), "Person 3", 32),
                new Person(Guid.NewGuid(), "Person 4", 33),
            };

            await PersonRepository.InsertManyAsync(entities);

            foreach (var entity in entities)
            {
                var person = await PersonRepository.FindAsync(entity.Id);
                person.ShouldNotBeNull();
            }
        }

        [Fact]
        public async Task UpdateManyAsync()
        {
            var entities = await PersonRepository.GetListAsync();
            var random = new Random();
            entities.ForEach(f => f.Age = random.Next());

            await PersonRepository.UpdateManyAsync(entities);

            foreach (var entity in entities)
            {
                var person = await PersonRepository.FindAsync(entity.Id);
                person.ShouldNotBeNull();
                person.Age.ShouldBe(entity.Age);
            }
        }

        [Fact]
        public async Task DeleteManyAsync()
        {
            var entities = await PersonRepository.GetListAsync();

            await PersonRepository.DeleteManyAsync(entities);

            foreach (var entity in entities)
            {
                var person = await PersonRepository.FindAsync(entity.Id);
                person.ShouldBeNull();
            }
        }

        [Fact]
        public async Task DeleteManyAsync_WithId()
        {
            var entities = await PersonRepository.GetListAsync();

            var ids = entities.Select(s => s.Id).ToArray();

            await PersonRepository.DeleteManyAsync(ids);

            foreach (var id in ids)
            {
                var person = await PersonRepository.FindAsync(id);
                person.ShouldBeNull();
            }
        }
    }
}
