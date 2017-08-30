using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.AspNetCore.App;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class SimpleController_Tests : AspNetCoreMvcTestBase
    {
        [Fact]
        public async Task ActionResult_Return_Type_ContentResult_Return_Value()
        {
            var result = await GetResponseAsStringAsync(
                GetUrl<SimpleController>(nameof(SimpleController.Index))
            );

            result.ShouldBe("Index-Result");
        }
    }
}
