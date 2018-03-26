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
        public static Guid UserDouglasId { get; } = Guid.NewGuid();
        public static Guid UserJohnDeletedId { get; } = Guid.NewGuid();

        private readonly IBasicRepository<Person, Guid> _personRepository;

        public TestDataBuilder(IBasicRepository<Person, Guid> personRepository)
        {
            _personRepository = personRepository;
        }

        public void Build()
        {
            AddPeople();
        }

        private void AddPeople()
        {
            var douglas = new Person(UserDouglasId, "Douglas", 42);
            douglas.Phones.Add(new Phone(douglas.Id, "123456789"));
            douglas.Phones.Add(new Phone(douglas.Id, "123456780", PhoneType.Home));

            _personRepository.Insert(douglas);

            _personRepository.Insert(new Person(UserJohnDeletedId, "John-Deleted", 33) { IsDeleted = true });

            var tenant1Person1 = new Person(Guid.NewGuid(), TenantId1 + "-Person1", 42, TenantId1);
            var tenant1Person2 = new Person(Guid.NewGuid(), TenantId1 + "-Person2", 43, TenantId1);

            _personRepository.Insert(tenant1Person1);
            _personRepository.Insert(tenant1Person2);
        }
    }
}