using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo.Components;

public class Card_Tests : AbpAspNetCoreMvcUiBootstrapDemoTestBase
{
    [Fact(Skip = "This test project is not completed yet")]
    public async Task Index()
    {
        var result = await GetResponseAsStringAsync("/Components/Cards");
        result.ShouldNotBeNullOrEmpty();
    }
}
