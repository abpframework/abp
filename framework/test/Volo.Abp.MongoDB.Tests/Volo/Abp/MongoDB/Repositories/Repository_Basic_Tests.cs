using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MongoDB.Repositories;

[Collection(MongoTestCollection.Name)]
public class Repository_Basic_Tests : Repository_Basic_Tests<AbpMongoDbTestModule>
{
    [Fact]
    public async Task ToMongoQueryable_Test()
    {
        (await PersonRepository.GetQueryableAsync()).ShouldNotBeNull();
        (await PersonRepository.GetQueryableAsync()).ShouldNotBeNull();
        ((IQueryable<Person>)(await PersonRepository.GetQueryableAsync()).Where(p => p.Name == "Douglas")).ShouldNotBeNull();
        (await PersonRepository.GetQueryableAsync()).Where(p => p.Name == "Douglas").ShouldNotBeNull();
    }

    [Fact]
    public async Task Linq_Queries()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            (await PersonRepository.GetQueryableAsync()).FirstOrDefault(p => p.Name == "Douglas").ShouldNotBeNull();
            (await PersonRepository.GetQueryableAsync()).Count().ShouldBeGreaterThan(0);
            return Task.CompletedTask;
        });
    }

    [Fact]
    public async Task UpdateAsync()
    {
        var person = await PersonRepository.GetAsync(TestDataBuilder.UserDouglasId);

        person.ChangeName("Douglas-Updated");
        person.Phones.Add(new Phone(person.Id, "6667778899", PhoneType.Office));

        await PersonRepository.UpdateAsync(person);

        person = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);
        person.ShouldNotBeNull();
        person.Name.ShouldBe("Douglas-Updated");
        person.Phones.Count.ShouldBe(3);
        person.Phones.Any(p => p.PersonId == person.Id && p.Number == "6667778899" && p.Type == PhoneType.Office).ShouldBeTrue();
    }

    [Fact]
    public async override Task InsertAsync()
    {
        var person = new Person(Guid.NewGuid(), "New Person", 35);
        person.Phones.Add(new Phone(person.Id, "1234567890"));
        person.SetProperty("test-guid-property", person.Id);
        person.SetProperty("test-nullable-guid-property", (Guid?)person.Id);

        await PersonRepository.InsertAsync(person);

        person = await PersonRepository.FindAsync(person.Id);
        person.ShouldNotBeNull();
        person.Name.ShouldBe("New Person");
        person.Phones.Count.ShouldBe(1);
        person.Phones.Any(p => p.PersonId == person.Id && p.Number == "1234567890").ShouldBeTrue();
        person.GetProperty<Guid>("test-guid-property").ShouldBe(person.Id);
        person.GetProperty<Guid?>("test-nullable-guid-property").ShouldBe(person.Id);
    }

    [Fact]
    public async Task Filter_Case_Insensitive()
    {
        (await CityRepository.GetQueryableAsync()).FirstOrDefault(c => c.Name == "ISTANBUL").ShouldBeNull();
        (await CityRepository.GetQueryableAsync()).FirstOrDefault(c => c.Name == "istanbul").ShouldBeNull();
        (await CityRepository.GetQueryableAsync()).FirstOrDefault(c => c.Name == "Istanbul").ShouldNotBeNull();

        (await PersonRepository.GetQueryableAsync()).FirstOrDefault(p => p.Name == "douglas").ShouldNotBeNull();
        (await PersonRepository.GetQueryableAsync()).FirstOrDefault(p => p.Name == "DOUGLAS").ShouldNotBeNull();
        (await PersonRepository.GetQueryableAsync()).FirstOrDefault(p => p.Name == "Douglas").ShouldNotBeNull();
    }
}
