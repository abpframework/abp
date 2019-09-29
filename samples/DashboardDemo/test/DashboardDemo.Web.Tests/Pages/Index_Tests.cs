using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace DashboardDemo.Pages
{
    public class Index_Tests : DashboardDemoWebTestBase
    {
        [Fact]
        public async Task Welcome_Page()
        {
            var response = await GetResponseAsStringAsync("/");
            response.ShouldNotBeNull();
        }
    }
}
