using Shouldly;
using Volo.Abp.DynamicProxy;
using Volo.Abp.TestApp.Application;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc;

public class ConventionalController_Tests : AspNetCoreMvcTestBase
{
    [Fact]
    public void Conventional_Controller_Should_Disable_DynamicProxy()
    {
        // PeopleAppService is a conventional controller
        var peopleAppService = GetRequiredService<PeopleAppService>();
        ProxyHelper.IsProxy(peopleAppService).ShouldBeFalse();

        // ConventionalAppService is not a conventional controller
        var conventionalAppService = GetRequiredService<ConventionalAppService>();
        ProxyHelper.IsProxy(conventionalAppService).ShouldBeTrue();
    }
}
