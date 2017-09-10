using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.TestApp
{
    public class TestDataBuilder : ITransientDependency
    {
        private readonly IRepository<Person> _personRepository;

        public TestDataBuilder(IRepository<Person> personRepository)
        {
            _personRepository = personRepository;
        }

        public void Build()
        {
            AddPeople();
        }

        private void AddPeople()
        {
            var douglas = new Person(Guid.NewGuid(), "Douglas", 42);
            douglas.Phones.Add(new Phone(douglas.Id, "123456789"));
            douglas.Phones.Add(new Phone(douglas.Id, "123456780", PhoneType.Home));
            _personRepository.Insert(douglas);
        }
    }
}