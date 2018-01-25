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
        private readonly IQueryableRepository<Person, Guid> _personRepository;
        private readonly IQueryableRepository<BookInSecondDbContext, Guid> _bookRepository;
        private readonly IQueryableRepository<PhoneInSecondDbContext, long> _phoneInSecondDbContextRepository;

        public Basic_Repository_Tests()
        {
            _personRepository = ServiceProvider.GetRequiredService<IQueryableRepository<Person, Guid>>();
            _bookRepository = ServiceProvider.GetRequiredService<IQueryableRepository<BookInSecondDbContext, Guid>>();
            _phoneInSecondDbContextRepository = ServiceProvider.GetRequiredService<IQueryableRepository<PhoneInSecondDbContext, long>>();
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
