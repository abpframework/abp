using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.ContentFormatters;

public class RemoteStreamContentTestController_Tests : AspNetCoreMvcTestBase
{
    [Fact]
    public async Task DownloadAsync()
    {
        var result = await GetResponseAsync("/api/remote-stream-content-test/download");
        result.Content.Headers.ContentType?.ToString().ShouldBe("application/rtf");
        result.Content.Headers.ContentDisposition?.FileName.ShouldBe("download.rtf");
        result.Content.Headers.ContentLength.ShouldBe("DownloadAsync".Length);
        (await result.Content.ReadAsStringAsync()).ShouldBe("DownloadAsync");
    }
    
    [Fact]
    public async Task Download_With_Custom_Content_Disposition_Async()
    {
        var result = await GetResponseAsync("/api/remote-stream-content-test/download-with-custom-content-disposition");
        result.Content.Headers.ContentType?.ToString().ShouldBe("application/rtf");
        result.Content.Headers.ContentDisposition?.FileName.ShouldBe("myDownload.rtf");
        result.Content.Headers.ContentLength.ShouldBe("DownloadAsync".Length);
        (await result.Content.ReadAsStringAsync()).ShouldBe("DownloadAsync");
    }
    
    [Fact]
    public async Task Download_With_Chinese_File_Name_Async()
    {
        var result = await GetResponseAsync("/api/remote-stream-content-test/download_with_chinese_file_name");
        result.Content.Headers.ContentType?.ToString().ShouldBe("application/rtf");
        result.Content.Headers.ContentDisposition?.FileNameStar.ShouldBe("下载文件.rtf");
        result.Content.Headers.ContentLength.ShouldBe("DownloadAsync".Length);
        (await result.Content.ReadAsStringAsync()).ShouldBe("DownloadAsync");
    }

    [Fact]
    public async Task UploadAsync()
    {
        using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/remote-stream-content-test/upload"))
        {
            var memoryStream = new MemoryStream();
            await memoryStream.WriteAsync(Encoding.UTF8.GetBytes("UploadAsync"));
            memoryStream.Position = 0;

            var streamContent = new StreamContent(memoryStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/rtf");
            requestMessage.Content = new MultipartFormDataContent { { streamContent, "file", "upload.rtf" } };

            var response = await Client.SendAsync(requestMessage);

            (await response.Content.ReadAsStringAsync()).ShouldBe("UploadAsync:application/rtf:upload.rtf");
        }
    }

    [Fact]
    public async Task UploadRawAsync()
    {
        using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/remote-stream-content-test/upload-raw"))
        {
            var memoryStream = new MemoryStream();
            var text = @"{ ""hello"": ""world"" }";
            await memoryStream.WriteAsync(Encoding.UTF8.GetBytes(text));
            memoryStream.Position = 0;

            var streamContent = new StreamContent(memoryStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            requestMessage.Content = streamContent;

            var response = await Client.SendAsync(requestMessage);
            (await response.Content.ReadAsStringAsync()).ShouldBe($"{text}:application/json");
        }
    }
}
