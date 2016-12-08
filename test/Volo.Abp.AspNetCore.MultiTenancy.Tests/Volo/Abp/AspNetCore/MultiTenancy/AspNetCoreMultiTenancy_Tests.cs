using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.AspNetCore.App;
using Xunit;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class AspNetCoreMultiTenancy_Tests : AppTestBase
    {
        private const string FakeUrl = "http://abp.io";

        [Fact]
        public async Task Should_Use_Host_If_Tenant_Is_Not_Specified()
        {
            var result = await GetResponseAsObjectAsync<Dictionary<string, string>>(FakeUrl);
            result["TenantName"].ShouldBe("<host>");
        }

        //TODO: Specify tenant in the header, cookie, url, querystring and so on...
    }
}
