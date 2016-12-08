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
            result["TenantName"].ShouldBe("<host>");
        }

        //TODO: Specify tenant in the header, cookie, url, querystring and so on...
    }
}
