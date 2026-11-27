using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Xunit;

namespace Volo.Abp.AspNetCore.MultiTenancy;

public class AspNetCoreMultiTenancy_DomainResolver_HostMatching_Tests : AspNetCoreMultiTenancyTestBase
{
    private readonly Guid _testTenantId = Guid.NewGuid();

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AbpDefaultTenantStoreOptions>(options =>
        {
            options.Tenants = new[]
            {
                new TenantConfiguration(_testTenantId, "acme", "ACME")
            };
        });

        services.Configure<AbpTenantResolveOptions>(options =>
        {
            options.AddDomainTenantResolver("{0}.abp.io");
        });

        base.ConfigureServices(services);
    }

    [Fact]
    public async Task Should_Use_Tenant_When_The_Host_Fully_Fits_The_Domain_Format()
    {
        var result = await GetResponseAsObjectAsync<Dictionary<string, string>>("http://acme.abp.io/");
        result["TenantId"].ShouldBe(_testTenantId.ToString());
    }

    [Fact]
    public async Task Should_Use_Tenant_When_The_Host_Also_Carries_A_Port()
    {
        // The documented domain format does not have to contain the port.
        var result = await GetResponseAsObjectAsync<Dictionary<string, string>>("http://acme.abp.io:8080/");
        result["TenantId"].ShouldBe(_testTenantId.ToString());
    }

    [Fact]
    public async Task Should_Use_Host_When_The_Host_Only_Contains_The_Domain_Format()
    {
        var result = await GetResponseAsObjectAsync<Dictionary<string, string>>("http://acme.abp.io.evil.com/");
        result["TenantId"].ShouldBe("");
    }
}
