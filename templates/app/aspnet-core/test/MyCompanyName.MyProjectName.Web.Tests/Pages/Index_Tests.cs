using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace MyCompanyName.MyProjectName.Pages
{
    [Collection(MyProjectNameTestConsts.CollectionDefinitionName)]
    public class Index_Tests : MyProjectNameWebTestBase
    {
        [Fact]
        public async Task Welcome_Page()
        {
            var response = await GetResponseAsStringAsync("/");
            response.ShouldNotBeNull();
        }
    }
}
