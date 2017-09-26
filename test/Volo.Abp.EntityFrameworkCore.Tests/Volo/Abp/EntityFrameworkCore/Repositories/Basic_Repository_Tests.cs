using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.Repositories
{
    public class Basic_Repository_Tests : EntityFrameworkCoreTestBase
    {
        private readonly IRepository<Person> _personRepository;

        public Basic_Repository_Tests()
        {
            _personRepository = ServiceProvider.GetRequiredService<IRepository<Person>>();
        }

        [Fact]
        public void GetList()
        {
            _personRepository.GetList().Any().ShouldBeTrue();
        }

        [Fact]
        public async Task InsertAsync()
        {
            var personId = Guid.NewGuid();

            await _personRepository.InsertAsync(new Person(personId, "Adam", 42));

            var person = await _personRepository.FindAsync(personId);
            person.ShouldNotBeNull();
        }
    }
}
