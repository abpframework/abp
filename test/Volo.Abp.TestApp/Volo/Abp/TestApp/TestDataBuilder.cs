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
            _personRepository.Insert(new Person(Guid.NewGuid(), "Douglas", 42));
        }
    }
}