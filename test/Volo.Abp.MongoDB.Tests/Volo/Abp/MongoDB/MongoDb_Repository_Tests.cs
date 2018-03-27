using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.MongoDB
{
    public class MongoDb_Repository_Tests : MongoDbTestBase
    {
        private readonly IRepository<Person, Guid> _personRepository;

        public MongoDb_Repository_Tests()
        {
            _personRepository = GetRequiredService<IRepository<Person, Guid>>();
        }

        [Fact]
        public async Task GetAsync()
        {
            var person = await _personRepository.GetAsync(TestDataBuilder.UserDouglasId);
            person.Name.ShouldBe("Douglas");
            person.Phones.Count.ShouldBe(2);
        }

        [Fact]
        public async Task FindAsync_Should_Return_Null_For_Not_Found_Entity()
        {
            var person = await _personRepository.FindAsync(Guid.NewGuid());
            person.ShouldBeNull();
        }

        [Fact]
        public void Linq_Queries()
        {
            _personRepository.FirstOrDefault(p => p.Name == "Douglas").ShouldNotBeNull();

            _personRepository.Count().ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            await _personRepository.DeleteAsync(TestDataBuilder.UserDouglasId);

            (await _personRepository.FindAsync(TestDataBuilder.UserDouglasId)).ShouldBeNull();
        }

        [Fact]
        public async Task UpdateAsync()
        {
            var person = await _personRepository.GetAsync(TestDataBuilder.UserDouglasId);

            person.ChangeName("Douglas-Updated");
            person.Phones.Add(new Phone(person.Id, "6667778899", PhoneType.Office));

            await _personRepository.UpdateAsync(person);

            person = await _personRepository.FindAsync(TestDataBuilder.UserDouglasId);
            person.ShouldNotBeNull();
            person.Name.ShouldBe("Douglas-Updated");
            person.Phones.Count.ShouldBe(3);
            person.Phones.Any(p => p.PersonId == person.Id && p.Number == "6667778899" && p.Type == PhoneType.Office).ShouldBeTrue();
        }

        [Fact]
        public async Task InsertAsync()
        {
            var person = new Person(Guid.NewGuid(), "New Person", 35);
            person.Phones.Add(new Phone(person.Id, "1234567890"));

            await _personRepository.InsertAsync(person);

            person = await _personRepository.FindAsync(person.Id);
            person.ShouldNotBeNull();
            person.Name.ShouldBe("New Person");
            person.Phones.Count.ShouldBe(1);
            person.Phones.Any(p => p.PersonId == person.Id && p.Number == "1234567890").ShouldBeTrue();
        }
    }
}
