using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AspNetCore.Mvc.Versioning.App;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Versioning.Test;

public class HelloController_Tests : AspNetCoreMvcVersioningTestBase
{
    private readonly IHelloController _helloController;

    public HelloController_Tests()
    {
        _helloController = ServiceProvider.GetRequiredService<IHelloController>();
    }

    [Fact]
    public async Task GetAsync()
    {
        (await _helloController.GetAsync()).ShouldBe("Get-2.0");
    }

    [Fact]
    public async Task PostAsyncV1()
    {
        (await _helloController.PostAsyncV1()).ShouldBe("Post-1.0");
    }

    [Fact]
    public async Task PostAsyncV2()
    {
        (await _helloController.PostAsyncV2()).ShouldBe("Post-2.0");
    }
}
