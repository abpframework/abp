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
            var script = await GetResponseAsStringAsync("/Abp/ServiceProxyScript?minify=true");
            script.Length.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task GetAllWithMinify()
        {
            var script = await GetResponseAsStringAsync("/Abp/ServiceProxyScript");
            var minifyScript = await GetResponseAsStringAsync("/Abp/ServiceProxyScript?minify=true");

            script.Length.ShouldBeGreaterThan(0);
            minifyScript.Length.ShouldBeGreaterThan(0);
            minifyScript.Length.ShouldBeLessThan(script.Length);
        }
    }
}
