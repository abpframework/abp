using System;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Data;

public class DataSeederExtensions_Tests : AbpIntegratedTest<DataSeederExtensions_Tests.TestModule>
{
    private IDataSeeder _dataSeeder;

    protected override void AfterAddApplication(IServiceCollection services)
    {
        _dataSeeder = Substitute.For<IDataSeeder>();
        services.Replace(ServiceDescriptor.Singleton(_dataSeeder));
        base.AfterAddApplication(services);
    }

    [Fact]
    public void SeedInSeparateUowAsync()
    {
        var tenantId = Guid.NewGuid();
        _dataSeeder.SeedInSeparateUowAsync(tenantId, new AbpUnitOfWorkOptions(true, IsolationLevel.Serializable, 888), true);

        _dataSeeder.Received().SeedAsync(Arg.Is<DataSeedContext>(x => x.TenantId == tenantId &&
                                                                      x.Properties[DataSeederExtensions.SeedInSeparateUow].To<bool>() == true &&
                                                                      x.Properties[DataSeederExtensions.SeedInSeparateUowOptions].As<AbpUnitOfWorkOptions>().IsTransactional == true &&
                                                                      x.Properties[DataSeederExtensions.SeedInSeparateUowOptions].As<AbpUnitOfWorkOptions>().IsolationLevel == IsolationLevel.Serializable &&
                                                                      x.Properties[DataSeederExtensions.SeedInSeparateUowOptions].As<AbpUnitOfWorkOptions>().Timeout == 888 &&
                                                                      x.Properties[DataSeederExtensions.SeedInSeparateUowRequiresNew].To<bool>() == true));
    }

    [DependsOn(typeof(AbpDataModule))]
    public class TestModule : AbpModule
    {

    }
}
