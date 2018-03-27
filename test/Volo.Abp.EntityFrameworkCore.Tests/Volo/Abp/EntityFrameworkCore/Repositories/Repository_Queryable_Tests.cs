using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore.TestApp.SecondContext;
using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.Repositories
{
    public class Repository_Queryable_Tests : Repository_Queryable_Tests<AbpEntityFrameworkCoreTestModule>
    {
        private readonly IRepository<BookInSecondDbContext, Guid> _bookRepository;
        private readonly IRepository<PhoneInSecondDbContext> _phoneInSecondDbContextRepository;

        public Repository_Queryable_Tests()
        {
            _bookRepository = ServiceProvider.GetRequiredService<IRepository<BookInSecondDbContext, Guid>>();
            _phoneInSecondDbContextRepository = ServiceProvider.GetRequiredService<IRepository<PhoneInSecondDbContext>>();
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
    }
}
