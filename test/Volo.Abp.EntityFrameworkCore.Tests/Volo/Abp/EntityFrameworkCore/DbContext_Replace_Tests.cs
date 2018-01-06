using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore.TestApp.ThirdDbContext;
using Volo.Abp.TestApp.EntityFrameworkCore;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore
{
    public class DbContext_Replace_Tests : EntityFrameworkCoreTestBase
    {
        private readonly IRepository<ThirdDbContextDummyEntity> _dummyRepository;

        public DbContext_Replace_Tests()
        {
            _dummyRepository = ServiceProvider.GetRequiredService<IRepository<ThirdDbContextDummyEntity>>();
        }

        [Fact]
        public void Should_Replace_DbContext()
        {
            (ServiceProvider.GetRequiredService<IThirdDbContext>() is TestAppDbContext).ShouldBeTrue();

            (_dummyRepository.GetDbContext() is IThirdDbContext).ShouldBeTrue();
            (_dummyRepository.GetDbContext() is TestAppDbContext).ShouldBeTrue();
        }
    }
}
