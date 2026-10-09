using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.Http.Client.ClientProxying;

public class RegularTestControllerStaticClientProxy_Tests : AbpHttpClientTestBase
{
    private readonly RegularTestControllerStaticClientProxy _proxy;

    public RegularTestControllerStaticClientProxy_Tests()
    {
        _proxy = ServiceProvider.GetRequiredService<RegularTestControllerStaticClientProxy>();
    }

    [Theory]
    [InlineData("plain")]
    [InlineData("a&b#c")]
    [InlineData("a?b=c")]
    [InlineData("50% off")]
    [InlineData("a+b")]
    [InlineData("a%23b")]
    [InlineData("rvA-a&b#<q>")]
    [InlineData("ä😀")]
    [InlineData("...")]
    [InlineData(".hidden")]
    public async Task GetWithStringPathAsync(string name)
    {
        (await _proxy.GetWithStringPathAsync(name)).ShouldBe(name);
    }

    [Theory]
    [InlineData("../increment")]
    [InlineData("..")]
    public async Task GetWithStringPathAsync_Should_Not_Send_Dot_Segments(string name)
    {
        var exception = await Should.ThrowAsync<AbpException>(() => _proxy.GetWithStringPathAsync(name));
        exception.ShouldBeOfType<AbpException>();
    }

    [Theory]
    [InlineData("a")]
    [InlineData("a/b")]
    [InlineData("a/b#c?d")]
    [InlineData("a b/c&d")]
    public async Task GetWithCatchAllPathAsync(string path)
    {
        (await _proxy.GetWithCatchAllPathAsync(path)).ShouldBe(path);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("a/b")]
    [InlineData("a/b#c?d")]
    public async Task GetWithSingleStarCatchAllPathAsync(string path)
    {
        (await _proxy.GetWithSingleStarCatchAllPathAsync(path)).ShouldBe(path);
    }
}
