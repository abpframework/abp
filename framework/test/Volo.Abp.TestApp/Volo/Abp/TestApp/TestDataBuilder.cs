using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.TestApp
{
    public class TestDataBuilder : ITransientDependency
    {
        public static Guid TenantId1 { get; } = new Guid("55687dce-595c-41b4-a024-2a5e991ac8f4");
        public static Guid TenantId2 { get; } = new Guid("f522d19f-5a86-4278-98fb-0577319c544a");
        public static Guid UserDouglasId { get; } = new Guid("1fcf46b2-28c3-48d0-8bac-fa53268a2775");
        public static Guid UserJohnDeletedId { get; } = new Guid("1e28ca9f-df84-4f39-83fe-f5450ecbf5d4");

        public static Guid IstanbulCityId { get; } = new Guid("4d734a0e-3e6b-4bad-bb43-ef8cf1b09633");
        public static Guid LondonCityId { get; } = new Guid("27237527-605e-4652-a2a5-68e0e512da36");

        private readonly IBasicRepository<Person, Guid> _personRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IRepository<EntityWithIntPk, int> _entityWithIntPksRepository;

        public TestDataBuilder(
            IBasicRepository<Person, Guid> personRepository, 
            ICityRepository cityRepository,
            IRepository<EntityWithIntPk, int> entityWithIntPksRepository)
        {
            _personRepository = personRepository;
            _cityRepository = cityRepository;
            _entityWithIntPksRepository = entityWithIntPksRepository;
        }

        public void Build()
        {
            AddCities();
            AddPeople();
            AddEntitiesWithPks();
        }

        private void AddCities()
        {
            _cityRepository.Insert(new City(Guid.NewGuid(), "Tokyo"));
            _cityRepository.Insert(new City(Guid.NewGuid(), "Madrid"));
            _cityRepository.Insert(new City(LondonCityId, "London") {ExtraProperties = { { "Population", 10_470_000 } } });
            _cityRepository.Insert(new City(IstanbulCityId, "Istanbul"));
            _cityRepository.Insert(new City(Guid.NewGuid(), "Paris"));
            _cityRepository.Insert(new City(Guid.NewGuid(), "Washington"));
            _cityRepository.Insert(new City(Guid.NewGuid(), "Sao Paulo"));
            _cityRepository.Insert(new City(Guid.NewGuid(), "Berlin"));
            _cityRepository.Insert(new City(Guid.NewGuid(), "Amsterdam"));
            _cityRepository.Insert(new City(Guid.NewGuid(), "Beijing"));
            _cityRepository.Insert(new City(Guid.NewGuid(), "Rome"));
        }

        private void AddPeople()
        {
            var douglas = new Person(UserDouglasId, "Douglas", 42, cityId: LondonCityId);
            douglas.Phones.Add(new Phone(douglas.Id, "123456789"));
            douglas.Phones.Add(new Phone(douglas.Id, "123456780", PhoneType.Home));

            _personRepository.Insert(douglas);

            _personRepository.Insert(new Person(UserJohnDeletedId, "John-Deleted", 33) { IsDeleted = true });

            var tenant1Person1 = new Person(Guid.NewGuid(), TenantId1 + "-Person1", 42, TenantId1);
            var tenant1Person2 = new Person(Guid.NewGuid(), TenantId1 + "-Person2", 43, TenantId1);

            _personRepository.Insert(tenant1Person1);
            _personRepository.Insert(tenant1Person2);
        }

        private void AddEntitiesWithPks()
        {
            _entityWithIntPksRepository.Insert(new EntityWithIntPk("Entity1"));
        }
    }
}