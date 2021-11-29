using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Xunit;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class AspNetCoreMultiTenancy_WithDomainResolver_Tests : AspNetCoreMultiTenancyTestBase
    {
        private readonly Guid _testTenantId = Guid.NewGuid();
        private readonly string _testTenantName = "acme";

        private readonly AbpAspNetCoreMultiTenancyOptions _options;

        public AspNetCoreMultiTenancy_WithDomainResolver_Tests()
        {
            _options = ServiceProvider.GetRequiredService<IOptions<AbpAspNetCoreMultiTenancyOptions>>().Value;
        }

        protected override IHostBuilder CreateHostBuilder()
        {
            return base.CreateHostBuilder().ConfigureServices(services =>
            {
                services.Configure<AbpDefaultTenantStoreOptions>(options =>
                {
                    options.Tenants = new[]
                    {
                        new TenantConfiguration(_testTenantId, _testTenantName)
                    };
                });

                services.Configure<AbpTenantResolveOptions>(options =>
                {
                    options.AddDomainTenantResolver("{0}.abp.io:8080");
                });
            });
        }

        [Fact]
        public async Task Should_Use_Host_If_Tenant_Is_Not_Specified()
        {
            var result = await GetResponseAsObjectAsync<Dictionary<string, string>>("http://abp.io:8080");
            result["TenantId"].ShouldBe("");
        }

        [Fact]
        public async Task Should_Use_Domain_If_Specified()
        {
            var result = await GetResponseAsObjectAsync<Dictionary<string, string>>("http://acme.abp.io:8080");
            result["TenantId"].ShouldBe(_testTenantId.ToString());
        }

        [Fact]
        public async Task Should_Use_Domain_As_First_Priority_If_Specified()
        {
            Client.DefaultRequestHeaders.Add(_options.TenantKey, Guid.NewGuid().ToString());

            var result = await GetResponseAsObjectAsync<Dictionary<string, string>>("http://acme.abp.io:8080");
            result["TenantId"].ShouldBe(_testTenantId.ToString());
        }
    }
}
