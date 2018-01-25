using System;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Domain;
using Xunit;
using System.Linq;

namespace Volo.Abp.MemoryDb.Repositories
{
    public class MemoryDb_Basic_Repository_Tests : MemoryDbTestBase
    {
        private readonly IQueryableRepository<Person, Guid> _personRepository;

        public MemoryDb_Basic_Repository_Tests()
        {
            _personRepository = ServiceProvider.GetRequiredService<IQueryableRepository<Person, Guid>>();
        }

        [Fact]
        public void GetList()
        {
            var people = _personRepository.GetList();
            people.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void Insert()
        {
            //Arrange
            var name = Guid.NewGuid().ToString();

            //Act
            _personRepository.Insert(new Person(Guid.NewGuid(), name, 42));

            //Assert
            _personRepository.FirstOrDefault(p => p.Name == name).ShouldNotBeNull();
        }
    }
}
