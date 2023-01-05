using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore.TestApp.FifthContext;
using Volo.Abp.EntityFrameworkCore.TestApp.FourthContext;
using Volo.Abp.EntityFrameworkCore.TestApp.ThirdDbContext;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.EntityFrameworkCore;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore;

public class DbContext_Replace_Tests : EntityFrameworkCoreTestBase
{
    private readonly IBasicRepository<ThirdDbContextDummyEntity, Guid> _dummyRepository;
    private readonly IBasicRepository<FourthDbContextDummyEntity, Guid> _fourthDummyRepository;
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IEfCoreDbContextTypeProvider _dbContextTypeProvider;

    public DbContext_Replace_Tests()
    {
        _dummyRepository = GetRequiredService<IBasicRepository<ThirdDbContextDummyEntity, Guid>>();
        _fourthDummyRepository = GetRequiredService<IBasicRepository<FourthDbContextDummyEntity, Guid>>();
        _personRepository = GetRequiredService<IPersonRepository>();
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        _dbContextTypeProvider = GetRequiredService<IEfCoreDbContextTypeProvider>();
    }

    [Fact]
    public async Task Should_Replace_DbContext()
    {
        _dbContextTypeProvider.GetDbContextType(typeof(IThirdDbContext)).ShouldBe(typeof(TestAppDbContext));
        _dbContextTypeProvider.GetDbContextType(typeof(IFourthDbContext)).ShouldBe(typeof(TestAppDbContext));

        (ServiceProvider.GetRequiredService<IThirdDbContext>() is TestAppDbContext).ShouldBeTrue();
        (ServiceProvider.GetRequiredService<IFourthDbContext>() is TestAppDbContext).ShouldBeTrue();

        using (var uow = _unitOfWorkManager.Begin())
        {
            var instance1 = await _dummyRepository.GetDbContextAsync();
            (instance1 is IThirdDbContext).ShouldBeTrue();

            var instance2 = await _dummyRepository.GetDbContextAsync();
            (instance2 is TestAppDbContext).ShouldBeTrue();

            var instance3 = await _personRepository.GetDbContextAsync();
            (instance3 is TestAppDbContext).ShouldBeTrue();

            var instance4 = await _fourthDummyRepository.GetDbContextAsync();
            (instance4 is IFourthDbContext).ShouldBeTrue();

            var instance5 = await _fourthDummyRepository.GetDbContextAsync();
            (instance5 is TestAppDbContext).ShouldBeTrue();

            // All instances should be the same!
            instance3.ShouldBe(instance1);
            instance3.ShouldBe(instance2);
            instance3.ShouldBe(instance4);
            instance3.ShouldBe(instance5);

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task Should_Replace_DbContext_By_Host_And_Tenant()
    {
        _dbContextTypeProvider.GetDbContextType(typeof(IFifthDbContext)).ShouldBe(typeof(HostTestAppDbContext));
        (ServiceProvider.GetRequiredService<IFifthDbContext>() is HostTestAppDbContext).ShouldBeTrue();

        using (var uow = _unitOfWorkManager.Begin())
        {
            var instance1 = await GetRequiredService<IBasicRepository<FifthDbContextDummyEntity, Guid>>().GetDbContextAsync();
            (instance1 is IFifthDbContext).ShouldBeTrue();

            var instance2 = await GetRequiredService<IBasicRepository<FifthDbContextMultiTenantDummyEntity, Guid>>().GetDbContextAsync();
            (instance2 is HostTestAppDbContext).ShouldBeTrue();

            instance1.ShouldBe(instance2);

            await uow.CompleteAsync();
        }

        using (GetRequiredService<ICurrentTenant>().Change(Guid.NewGuid()))
        {
            _dbContextTypeProvider.GetDbContextType(typeof(IFifthDbContext)).ShouldBe(typeof(TenantTestAppDbContext));
            (ServiceProvider.GetRequiredService<IFifthDbContext>() is TenantTestAppDbContext).ShouldBeTrue();

            using (var uow = _unitOfWorkManager.Begin())
            {
                var instance1 = await GetRequiredService<IBasicRepository<FifthDbContextDummyEntity, Guid>>().GetDbContextAsync();
                (instance1 is IFifthDbContext).ShouldBeTrue();

                var instance2 = await GetRequiredService<IFifthDbContextDummyEntityRepository>().GetDbContextAsync();
                (instance2 is HostTestAppDbContext).ShouldBeTrue();

                instance1.ShouldBe(instance2);

                // Multi-tenant entities use tenant DbContext
                var instance3 = await GetRequiredService<IFifthDbContextMultiTenantDummyEntityRepository>().GetDbContextAsync();
                (instance3 is TenantTestAppDbContext).ShouldBeTrue();

                await uow.CompleteAsync();
            }
        }
    }
}
