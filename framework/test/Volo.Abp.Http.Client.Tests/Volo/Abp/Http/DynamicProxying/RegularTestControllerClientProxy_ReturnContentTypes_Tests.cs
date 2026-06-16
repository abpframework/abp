using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.Http.DynamicProxying;

public class RegularTestControllerClientProxy_ReturnContentTypes_Tests : AbpHttpClientTestBase
{
    private readonly IRegularTestController _controller;

    public RegularTestControllerClientProxy_ReturnContentTypes_Tests()
    {
        _controller = ServiceProvider.GetRequiredService<IRegularTestController>();
    }

    [Fact]
    public async Task GetPlainStringAsync_Should_Return_Unwrapped_String()
    {
        var result = await _controller.GetPlainStringAsync();
        result.ShouldBe("Open");
        result.StartsWith("\"").ShouldBeFalse();
    }

    [Fact]
    public async Task GetProducesJsonStringAsync_Should_Strip_Json_Quotes()
    {
        var result = await _controller.GetProducesJsonStringAsync();
        result.ShouldBe("Open");
        result.StartsWith("\"").ShouldBeFalse();
        result.EndsWith("\"").ShouldBeFalse();
    }

    [Fact]
    public async Task GetProducesTextStringAsync_Should_Return_Raw_Text_Body()
    {
        var result = await _controller.GetProducesTextStringAsync();
        result.ShouldBe("Open");
        result.StartsWith("\"").ShouldBeFalse();
    }

    [Fact]
    public async Task GetNullStringAsync_Should_Return_Default_Or_Empty()
    {
        var result = await _controller.GetNullStringAsync();
        (result == null || result == string.Empty).ShouldBeTrue();
    }

    [Fact]
    public async Task GetEmptyStringAsync_Should_Return_Empty()
    {
        var result = await _controller.GetEmptyStringAsync();
        (result == null || result == string.Empty).ShouldBeTrue();
    }

    [Fact]
    public async Task GetProducesJsonNullStringAsync_Should_Not_Return_Literal_Null()
    {
        var result = await _controller.GetProducesJsonNullStringAsync();
        result.ShouldNotBe("null");
        (result == null || result == string.Empty).ShouldBeTrue();
    }

    [Fact]
    public async Task GetEscapedStringAsync_Should_Decode_Escaped_Characters()
    {
        var result = await _controller.GetEscapedStringAsync();
        result.ShouldBe("a\"b\\c\nd");
    }

    [Fact]
    public async Task DownloadIconAsync_Should_Return_Binary_Bytes()
    {
        using var content = await _controller.DownloadIconAsync();
        using var ms = new MemoryStream();
        await content.GetStream().CopyToAsync(ms);
        ms.ToArray().ShouldBe(System.Text.Encoding.UTF8.GetBytes("ICON-BYTES"));
        content.FileName.ShouldBe("icon.bin");
    }

    [Fact]
    public async Task GetReferenceTypeObjectAsync_Should_Not_Be_Wrapped_As_RemoteStreamContent()
    {
        var result = await _controller.GetReferenceTypeObjectAsync();
        result.ShouldNotBeNull();
        result.ShouldNotBeAssignableTo<Volo.Abp.Content.IRemoteStreamContent>();
    }

    [Fact]
    public async Task GetByteArrayAsync_Should_Round_Trip_Bytes()
    {
        var bytes = await _controller.GetByteArrayAsync();
        bytes.ShouldBe(new byte[] { 1, 2, 3, 4 });
    }

    [Fact]
    public async Task Existing_IncrementValueAsync_Regression_Should_Still_Work()
    {
        var result = await _controller.IncrementValueAsync(41);
        result.ShouldBe(42);
    }
}
