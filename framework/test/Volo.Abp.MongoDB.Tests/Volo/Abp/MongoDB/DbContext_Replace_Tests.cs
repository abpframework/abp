using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MongoDB.TestApp.FifthContext;
using Volo.Abp.MongoDB.TestApp.FourthContext;
using Volo.Abp.MongoDB.TestApp.ThirdDbContext;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TestApp.MongoDb;
using Volo.Abp.TestApp.MongoDB;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.MongoDB;

[Collection(MongoTestCollection.Name)]
public class DbContext_Replace_Tests : MongoDbTestBase
{
    private readonly IMongoDbContextTypeProvider _dbContextTypeProvider;

    public DbContext_Replace_Tests()
    {
        _dbContextTypeProvider = GetRequiredService<IMongoDbContextTypeProvider>();
    }

    [Fact]
    public void Should_Replace_DbContext()
    {
        _dbContextTypeProvider.GetDbContextType(typeof(IThirdDbContext)).ShouldBe(typeof(TestAppMongoDbContext));
        _dbContextTypeProvider.GetDbContextType(typeof(IFourthDbContext)).ShouldBe(typeof(TestAppMongoDbContext));

        (ServiceProvider.GetRequiredService<IThirdDbContext>() is TestAppMongoDbContext).ShouldBeTrue();
        (ServiceProvider.GetRequiredService<IFourthDbContext>() is TestAppMongoDbContext).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Replace_DbContext_By_Host_And_Tenant()
    {
        _dbContextTypeProvider.GetDbContextType(typeof(IFifthDbContext)).ShouldBe(typeof(HostTestAppDbContext));
        (ServiceProvider.GetRequiredService<IFifthDbContext>() is HostTestAppDbContext).ShouldBeTrue();

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            var instance1 = await GetRequiredService<IFifthDbContextDummyEntityRepository>().GetDbContextAsync();
            (instance1 is HostTestAppDbContext).ShouldBeTrue();

            var instance2 = await GetRequiredService<IFifthDbContextMultiTenantDummyEntityRepository>().GetDbContextAsync();
            (instance2 is HostTestAppDbContext).ShouldBeTrue();

            instance1.ShouldBe(instance2);

            await uow.CompleteAsync();
        }

        using (GetRequiredService<ICurrentTenant>().Change(Guid.NewGuid()))
        {
            _dbContextTypeProvider.GetDbContextType(typeof(IFifthDbContext)).ShouldBe(typeof(TenantTestAppDbContext));
            (ServiceProvider.GetRequiredService<IFifthDbContext>() is TenantTestAppDbContext).ShouldBeTrue();

            using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
            {
                var instance1 = await GetRequiredService<IFifthDbContextDummyEntityRepository>().GetDbContextAsync();
                (instance1 is HostTestAppDbContext).ShouldBeTrue();

                // Multi-tenant entities use tenant DbContext
                var instance2 = await GetRequiredService<IFifthDbContextMultiTenantDummyEntityRepository>().GetDbContextAsync();
                (instance2 is TenantTestAppDbContext).ShouldBeTrue();

                await uow.CompleteAsync();
            }
        }
    }
}
