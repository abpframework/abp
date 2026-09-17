using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Http.Client;
using Volo.Abp.TestApp.Application;
using Xunit;

namespace Volo.Abp.Http.DynamicProxying;

public class PeopleAppServiceClientProxy_ReturnContentTypes_Tests : AbpHttpClientTestBase
{
    private readonly IPeopleAppService _peopleAppService;

    public PeopleAppServiceClientProxy_ReturnContentTypes_Tests()
    {
        _peopleAppService = ServiceProvider.GetRequiredService<IPeopleAppService>();
    }

    [Fact]
    public async Task EchoStatusAsync_Should_Return_Plain_String_Without_Quotes()
    {
        var status = await _peopleAppService.EchoStatusAsync();
        status.ShouldBe("Open");
        status.StartsWith("\"").ShouldBeFalse();
        status.EndsWith("\"").ShouldBeFalse();
    }

    [Fact]
    public async Task EchoStatusWithProducesJsonAsync_Should_Return_Plain_String_Without_Quotes()
    {
        var status = await _peopleAppService.EchoStatusWithProducesJsonAsync();
        status.ShouldBe("Open");
        status.StartsWith("\"").ShouldBeFalse();
        status.EndsWith("\"").ShouldBeFalse();
    }

    [Fact]
    public async Task GetBinaryImageAsync_Should_Return_Real_Binary_Not_Json_Metadata()
    {
        using var content = await _peopleAppService.GetBinaryImageAsync();
        using var ms = new MemoryStream();
        await content.GetStream().CopyToAsync(ms);
        var bytes = ms.ToArray();

        content.FileName.ShouldBe("tiny.png");
        content.ContentType.ShouldStartWith("image/png");
        bytes.Length.ShouldBeGreaterThan(8);

        bytes[0].ShouldBe((byte)0x89);
        bytes[1].ShouldBe((byte)0x50);
        bytes[2].ShouldBe((byte)0x4E);
        bytes[3].ShouldBe((byte)0x47);
    }

    [Fact]
    public async Task ThrowFromStringAsync_Should_Surface_Server_Exception_To_Client()
    {
        await Should.ThrowAsync<AbpRemoteCallException>(
            () => _peopleAppService.ThrowFromStringAsync()
        );
    }

    [Fact]
    public async Task DownloadAsync_Should_Still_Work()
    {
        using var content = await _peopleAppService.DownloadAsync();
        using var reader = new StreamReader(content.GetStream());
        var text = await reader.ReadToEndAsync();
        text.ShouldBe("DownloadAsync");
        content.FileName.ShouldBe("download.rtf");
        content.ContentType.ShouldStartWith("application/rtf");
    }

    [Fact]
    public async Task UploadAsync_String_Return_Should_Stay_Unquoted()
    {
        using var ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("hello"));
        var result = await _peopleAppService.UploadAsync(
            new Content.RemoteStreamContent(ms, "upload.txt", "text/plain"));
        result.ShouldBe("hello:text/plain:upload.txt");
        result.StartsWith("\"").ShouldBeFalse();
    }
}
