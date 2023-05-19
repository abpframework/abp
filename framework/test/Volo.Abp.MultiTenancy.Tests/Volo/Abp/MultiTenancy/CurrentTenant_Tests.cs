using System;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Xunit;

namespace Volo.Abp.MultiTenancy;

public class CurrentTenant_Tests : MultiTenancyTestBase
{
    private readonly ICurrentTenant _currentTenant;

    private readonly Guid _tenantAId = Guid.NewGuid();
    private readonly Guid _tenantBId = Guid.NewGuid();

    public CurrentTenant_Tests()
    {
        _currentTenant = ServiceProvider.GetRequiredService<ICurrentTenant>();
    }

    [Fact]
    public void CurrentTenant_Should_Be_Null_As_Default()
    {
        //Assert

        _currentTenant.Id.ShouldBeNull();
    }

    protected override void BeforeAddApplication(IServiceCollection services)
    {
        services.Configure<AbpDefaultTenantStoreOptions>(options =>
        {
            options.Tenants = new[]
            {
                    new TenantConfiguration(_tenantAId, "A"),
                    new TenantConfiguration(_tenantAId, "B")
            };
        });
    }

    [Fact]
    public void Should_Get_Null_If_Not_Set()
    {
        _currentTenant.Id.ShouldBeNull();
    }

    [Fact]
    public void Should_Get_Changed_Tenant_If()
    {
        _currentTenant.Id.ShouldBe(null);

        using (_currentTenant.Change(_tenantAId))
        {
            _currentTenant.Id.ShouldBe(_tenantAId);

            using (_currentTenant.Change(_tenantBId))
            {
                _currentTenant.Id.ShouldBe(_tenantBId);
            }

            _currentTenant.Id.ShouldBe(_tenantAId);
        }

        _currentTenant.Id.ShouldBeNull();
    }
}
