using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore.TestApp.ThirdDbContext;
using Volo.Abp.TestApp.EntityFrameworkCore;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore
{
    public class DbContext_Replace_Tests : EntityFrameworkCoreTestBase
    {
        private readonly IBasicRepository<ThirdDbContextDummyEntity, Guid> _dummyRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public DbContext_Replace_Tests()
        {
            _dummyRepository = ServiceProvider.GetRequiredService<IBasicRepository<ThirdDbContextDummyEntity, Guid>>();
            _unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        }

        [Fact]
        public async Task Should_Replace_DbContext()
        {
            (ServiceProvider.GetRequiredService<IThirdDbContext>() is TestAppDbContext).ShouldBeTrue();

            using (_unitOfWorkManager.Begin())
            {
                (_dummyRepository.GetDbContext() is IThirdDbContext).ShouldBeTrue();
                (_dummyRepository.GetDbContext() is TestAppDbContext).ShouldBeTrue();

                await _unitOfWorkManager.Current.CompleteAsync();
            }
        }
    }
}
