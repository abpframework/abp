using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore.TestApp.ThirdDbContext;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.EntityFrameworkCore;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore
{
    public class DbContext_Replace_Tests : EntityFrameworkCoreTestBase
    {
        private readonly IBasicRepository<ThirdDbContextDummyEntity, Guid> _dummyRepository;
        private readonly IPersonRepository _personRepository; 
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly AbpDbContextOptions _options;

        public DbContext_Replace_Tests()
        {
            _dummyRepository = GetRequiredService<IBasicRepository<ThirdDbContextDummyEntity, Guid>>();
            _personRepository = GetRequiredService<IPersonRepository>();
            _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
            _options = GetRequiredService<IOptions<AbpDbContextOptions>>().Value;
        }

        [Fact]
        public async Task Should_Replace_DbContext()
        {
            _options.GetReplacedTypeOrSelf(typeof(IThirdDbContext)).ShouldBe(typeof(TestAppDbContext));
            
            (ServiceProvider.GetRequiredService<IThirdDbContext>() is TestAppDbContext).ShouldBeTrue();

            using (var uow = _unitOfWorkManager.Begin())
            {
                var instance1 = await _dummyRepository.GetDbContextAsync();
                (instance1 is IThirdDbContext).ShouldBeTrue();

                var instance2 = await _dummyRepository.GetDbContextAsync();
                (instance2 is TestAppDbContext).ShouldBeTrue();

                var instance3 = await _personRepository.GetDbContextAsync();
                (instance3 is TestAppDbContext).ShouldBeTrue();
                
                // All instances should be the same!
                instance3.ShouldBe(instance1);
                instance3.ShouldBe(instance2);
                
                await uow.CompleteAsync();
            }
        }
    }
}
