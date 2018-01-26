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
    public class Repository_Tests : EntityFrameworkCoreTestBase
    {
        private readonly IRepository<Person, Guid> _personRepository;
        private readonly IRepository<BookInSecondDbContext, Guid> _bookRepository;
        private readonly IRepository<PhoneInSecondDbContext> _phoneInSecondDbContextRepository;

        public Repository_Tests()
        {
            _personRepository = ServiceProvider.GetRequiredService<IRepository<Person, Guid>>();
            _bookRepository = ServiceProvider.GetRequiredService<IRepository<BookInSecondDbContext, Guid>>();
            _phoneInSecondDbContextRepository = ServiceProvider.GetRequiredService<IRepository<PhoneInSecondDbContext>>();
        }

        [Fact]
        public void GetPersonList()
        {
            WithUnitOfWork(() =>
            {
                _personRepository.Any().ShouldBeTrue();
            });
        }

        [Fact]
        public void GetBookList()
        {
            WithUnitOfWork(() =>
            {
                _bookRepository.Any().ShouldBeTrue();
            });
        }
        
        [Fact]
        public void GetPhoneInSecondDbContextList()
        {
            WithUnitOfWork(() =>
            {
                _phoneInSecondDbContextRepository.Any().ShouldBeTrue();
            });
        }

        [Fact]
        public async Task InsertAsync()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var personId = Guid.NewGuid();

                await _personRepository.InsertAsync(new Person(personId, "Adam", 42));

                var person = await _personRepository.FindAsync(personId);
                person.ShouldNotBeNull();
            });
        }
    }
}
