using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Xunit;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class AspNetCoreMultiTenancy_Tests : AspNetCoreMultiTenancyTestBase
    {
        private readonly Guid _testTenantId = Guid.NewGuid();
        private readonly string _testTenantName = "acme";

        private readonly AspNetCoreMultiTenancyOptions _options;

        public AspNetCoreMultiTenancy_Tests()
        {
            _options = ServiceProvider.GetRequiredService<IOptions<AspNetCoreMultiTenancyOptions>>().Value;
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return base.CreateWebHostBuilder().ConfigureServices(services =>
            {
                services.Configure<ConfigurationTenantStoreOptions>(options =>
                {
                    options.Tenants = new[]
                    {
                        new TenantInformation(_testTenantId, _testTenantName)
                    };
                });
            });
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

            var result = await GetResponseAsObjectAsync<Dictionary<string, string>>($"http://abp.io?{_options.TenantIdKey}={_testTenantName}");
            result["TenantId"].ShouldBe(_testTenantId.ToString());
        }

        [Fact]
        public async Task Should_Use_Header_Tenant_Id_If_Specified()
        {
            Client.DefaultRequestHeaders.Add(_options.TenantIdKey, _testTenantId.ToString());

            var result = await GetResponseAsObjectAsync<Dictionary<string, string>>("http://abp.io");
            result["TenantId"].ShouldBe(_testTenantId.ToString());
        }

        [Fact]
        public async Task Should_Use_Domain_If_Specified()
        {
            var result = await GetResponseAsObjectAsync<Dictionary<string, string>>("http://acme.abp.io");
            result["TenantId"].ShouldBe(_testTenantId.ToString());
        }

        [Fact]
        public async Task Should_Use_Domain_As_First_Priority_If_Specified()
        {
            Client.DefaultRequestHeaders.Add(_options.TenantIdKey, Guid.NewGuid().ToString());

            var result = await GetResponseAsObjectAsync<Dictionary<string, string>>("http://acme.abp.io");
            result["TenantId"].ShouldBe(_testTenantId.ToString());
        }

        [Fact]
        public async Task Should_Use_Cookie_Tenant_Id_If_Specified()
        {
            Client.DefaultRequestHeaders.Add("Cookie", new CookieHeaderValue(_options.TenantIdKey, _testTenantId.ToString()).ToString());

            var result = await GetResponseAsObjectAsync<Dictionary<string, string>>("http://abp.io");
            result["TenantId"].ShouldBe(_testTenantId.ToString());
        }
    }
}
