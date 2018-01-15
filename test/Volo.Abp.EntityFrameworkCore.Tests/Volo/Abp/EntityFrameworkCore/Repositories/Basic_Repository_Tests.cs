using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore.TestApp.SecondContext;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.Repositories
{
    public class Basic_Repository_Tests : EntityFrameworkCoreTestBase
    {
        private readonly IRepository<Person, Guid> _personRepository;
        private readonly IRepository<BookInSecondDbContext, Guid> _bookRepository;
        private readonly IRepository<PhoneInSecondDbContext, long> _phoneInSecondDbContextRepository;

        public Basic_Repository_Tests()
        {
            _personRepository = ServiceProvider.GetRequiredService<IRepository<Person, Guid>>();
            _bookRepository = ServiceProvider.GetRequiredService<IRepository<BookInSecondDbContext, Guid>>();
            _phoneInSecondDbContextRepository = ServiceProvider.GetRequiredService<IRepository<PhoneInSecondDbContext, long>>();
        }

        [Fact]
        public void GetPersonList()
        {
            _personRepository.GetList().Any().ShouldBeTrue();
        }

        [Fact]
        public void GetBookList()
        {
            _bookRepository.GetList().Any().ShouldBeTrue();
        }
        
        [Fact]
        public void GetPhoneInSecondDbContextList()
        {
            _phoneInSecondDbContextRepository.GetList().Any().ShouldBeTrue();
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
