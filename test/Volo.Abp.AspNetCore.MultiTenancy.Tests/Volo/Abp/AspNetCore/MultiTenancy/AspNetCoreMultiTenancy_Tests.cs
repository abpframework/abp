using System.Collections.Generic;
using System.Threading.Tasks;
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

            var result = await GetResponseAsObjectAsync<Dictionary<string, string>>($"http://abp.io?{QueryStringTenantResolver.TenantIdKey}={testTenantId}");
            result["TenantId"].ShouldBe(testTenantId);
        }

        //TODO: Specify tenant in the header, cookie, domain, subdomain, route, querystring and so on...
    }
}
