using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.AspNetCore.App;
using Volo.Abp.UI;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc;

public class SimpleController_Tests : AspNetCoreMvcTestBase
{
    [Fact]
    public async Task ActionResult_ContentResult()
    {
        var result = await GetResponseAsStringAsync(
            GetUrl<SimpleController>(nameof(SimpleController.Index))
        );

        result.ShouldBe("Index-Result");
    }

    [Fact]
    public async Task ActionResult_ViewResult()
    {
        var result = await GetResponseAsStringAsync(
            GetUrl<SimpleController>(nameof(SimpleController.About))
        );

        result.Trim().ShouldBe("<h2>About</h2>");
    }

    [Fact]
    public async Task ActionResult_ViewResult_Exception()
    {
        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await GetResponseAsStringAsync(
                GetUrl<SimpleController>(nameof(SimpleController.ExceptionOnRazor))
            );
        });
    }
}
