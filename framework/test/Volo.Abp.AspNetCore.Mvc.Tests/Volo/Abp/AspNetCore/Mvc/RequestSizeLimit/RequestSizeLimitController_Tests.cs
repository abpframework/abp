using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.RequestSizeLimit;

public class RequestSizeLimitController_Tests : AspNetCoreMvcTestBase
{
    [Fact]
    public async Task RequestSizeLimitCheck_Test()
    {
        using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, "api/request-siz-limit/check"))
        {
            var memoryStream = new MemoryStream();
            await memoryStream.WriteAsync(Encoding.UTF8.GetBytes("0123456789"));
            memoryStream.Position = 0;

            var streamContent = new StreamContent(memoryStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            requestMessage.Content = new MultipartFormDataContent { { streamContent, "file", "text.txt" } };

            var response = await Client.SendAsync(requestMessage);

            (await response.Content.ReadAsStringAsync()).ShouldContain("Request content size is greater than the limit size");
        }
    }

    [Fact]
    public async Task DisableRequestSizeLimit_Test()
    {
        using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, "api/request-siz-limit/disable"))
        {
            var memoryStream = new MemoryStream();
            await memoryStream.WriteAsync(Encoding.UTF8.GetBytes("0123456789"));
            memoryStream.Position = 0;

            var streamContent = new StreamContent(memoryStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            requestMessage.Content = new MultipartFormDataContent { { streamContent, "file", "text.txt" } };

            var response = await Client.SendAsync(requestMessage);

            (await response.Content.ReadAsStringAsync()).ShouldContain("ok");
        }
    }
}
