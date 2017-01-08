using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using Shouldly;
using Volo.Abp.AspNetCore.App;
using Xunit;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class AspNetCoreMultiTenancy_Tests : AppTestBase
    {
        [Fact]
        public async Task Should_Use_Host_If_Tenant_Is_Not_Specified()
        {
            var result = await GetResponseAsObjectAsync<Dictionary<string, string>>("http://abp.io");
            result["TenantId"].ShouldBe("");
        }

        [Fact]
        public async Task Should_Use_QueryString_Tenant_Id_If_Specified()
        {
            const string testTenantId = "42";

            var result = await GetResponseAsObjectAsync<Dictionary<string, string>>($"http://abp.io?{AbpAspNetCoreMultiTenancyConsts.TenantIdKey}={testTenantId}");
            result["TenantId"].ShouldBe(testTenantId);
        }

        [Fact]
        public async Task Should_Use_Header_Tenant_Id_If_Specified()
        {
            const string testTenantId = "42";

            Client.DefaultRequestHeaders.Add(AbpAspNetCoreMultiTenancyConsts.TenantIdKey, testTenantId);

            var result = await GetResponseAsObjectAsync<Dictionary<string, string>>("http://abp.io");
            result["TenantId"].ShouldBe(testTenantId);
        }

        [Fact]
        public async Task Should_Use_Cookie_Tenant_Id_If_Specified()
        {
            const string testTenantId = "42";
            
            Client.DefaultRequestHeaders.Add("Cookie", new CookieHeaderValue(AbpAspNetCoreMultiTenancyConsts.TenantIdKey, testTenantId).ToString());

            var result = await GetResponseAsObjectAsync<Dictionary<string, string>>("http://abp.io");
            result["TenantId"].ShouldBe(testTenantId);
        }
    }
}
