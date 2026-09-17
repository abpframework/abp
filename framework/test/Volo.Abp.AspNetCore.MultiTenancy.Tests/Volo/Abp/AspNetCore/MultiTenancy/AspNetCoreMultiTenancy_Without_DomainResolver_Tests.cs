using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Xunit;

namespace Volo.Abp.AspNetCore.MultiTenancy;

public class AspNetCoreMultiTenancy_Without_DomainResolver_Tests : AspNetCoreMultiTenancyTestBase
{
    private readonly Guid _testTenantId = Guid.NewGuid();
    private readonly string _testTenantName = "acme";
    private readonly string _testTenantNormalizedName = "ACME";

    private readonly AbpAspNetCoreMultiTenancyOptions _options;

    public AspNetCoreMultiTenancy_Without_DomainResolver_Tests()
    {
        _options = ServiceProvider.GetRequiredService<IOptions<AbpAspNetCoreMultiTenancyOptions>>().Value;
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AbpDefaultTenantStoreOptions>(options =>
        {
            options.Tenants = new[]
            {
                new TenantConfiguration(_testTenantId, _testTenantName, _testTenantNormalizedName)
            };
        });

        base.ConfigureServices(services);
    }

    [Fact]
    public async Task Should_Use_Host_If_Tenant_Is_Not_Specified()
    {
        var result = await GetResponseAsObjectAsync<Dictionary<string, string>>("http://abp.io");
        result["TenantId"].ShouldBe("");
    }

    [Fact]
    public async Task Should_Use_QueryString_Tenant_Id_If_Specified()
    {

        var result = await GetResponseAsObjectAsync<Dictionary<string, string>>($"http://abp.io?{_options.TenantKey}={_testTenantName}");
        result["TenantId"].ShouldBe(_testTenantId.ToString());
    }

    [Fact]
    public async Task Should_Use_Header_Tenant_Id_If_Specified()
    {
        Client.DefaultRequestHeaders.Add(_options.TenantKey, _testTenantId.ToString());

        var result = await GetResponseAsObjectAsync<Dictionary<string, string>>("http://abp.io");
        result["TenantId"].ShouldBe(_testTenantId.ToString());
    }

    [Fact]
    public async Task Should_Use_Cookie_Tenant_Id_If_Specified()
    {
        Client.DefaultRequestHeaders.Add("Cookie", new CookieHeaderValue(_options.TenantKey, _testTenantId.ToString()).ToString());

        var result = await GetResponseAsObjectAsync<Dictionary<string, string>>("http://abp.io");
        result["TenantId"].ShouldBe(_testTenantId.ToString());
    }

    [Fact]
    public async Task Should_Use_Header_Tenant_Id_When_QueryString_Tenant_Is_Empty()
    {
        Client.DefaultRequestHeaders.Add(_options.TenantKey, _testTenantId.ToString());

        var result = await GetResponseAsObjectAsync<Dictionary<string, string>>($"http://abp.io?{_options.TenantKey}=");
        result["TenantId"].ShouldBe(_testTenantId.ToString());
    }

    [Fact]
    public async Task Should_Fallback_To_Host_When_QueryString_Tenant_Is_Empty_And_No_Other_Resolver()
    {
        var result = await GetResponseAsObjectAsync<Dictionary<string, string>>($"http://abp.io?{_options.TenantKey}=");
        result["TenantId"].ShouldBe("");
    }

    [Fact]
    public async Task Should_Use_Header_Tenant_Id_When_QueryString_Tenant_Is_Whitespace()
    {
        Client.DefaultRequestHeaders.Add(_options.TenantKey, _testTenantId.ToString());

        var result = await GetResponseAsObjectAsync<Dictionary<string, string>>($"http://abp.io?{_options.TenantKey}=%20");
        result["TenantId"].ShouldBe(_testTenantId.ToString());
    }

    [Fact]
    public async Task Should_Fallback_To_Host_When_QueryString_Tenant_Is_Whitespace_And_No_Other_Resolver()
    {
        var result = await GetResponseAsObjectAsync<Dictionary<string, string>>($"http://abp.io?{_options.TenantKey}=%20");
        result["TenantId"].ShouldBe("");
    }
}
