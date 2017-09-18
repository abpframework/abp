using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.ProxyScripting
{
    public class AbpServiceProxiesController_Tests : AspNetCoreMvcTestBase
    {
        [Fact]
        public async Task GetAll()
        {
            var script = await GetResponseAsStringAsync("/AbpServiceProxies/GetAll");
            script.Length.ShouldBeGreaterThan(0);
        }
    }
}
