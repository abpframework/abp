using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.ContentFormatters
{
    public class RemoteStreamContentTestController_Tests : AspNetCoreMvcTestBase
    {
        [Fact]
        public async Task DownloadAsync()
        {
            var result = await GetResponseAsync("/api/remote-stream-content-test/download");
            result.Content.Headers.ContentType?.ToString().ShouldBe("audio/mpeg");
            (await result.Content.ReadAsStringAsync()).ShouldBe("stream");
        }

        [Fact]
        public async Task UploadAsync()
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/remote-stream-content-test/upload"))
            {
                var memoryStream = new MemoryStream();
                await memoryStream.WriteAsync(Encoding.UTF8.GetBytes("upload"));

                memoryStream.Position = 0;
                requestMessage.Content = new StreamContent(memoryStream);
                requestMessage.Content.Headers.Add("Content-Type", "*/*");

                var response = await Client.SendAsync(requestMessage);

                (await response.Content.ReadAsStringAsync()).ShouldBe("upload");
            }
        }
    }
}
