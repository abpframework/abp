using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.WebClientInfoProvider;

public class WebClientInfoProviderTestController_Tests : AspNetCoreMvcTestBase
{
    [Theory]
    [InlineData("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/137.0.0.0 Safari/537.36", "Windows 10 Chrome")]
    [InlineData("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/137.0.0.0 Safari/537.36", "Mac OS X Chrome")]
    [InlineData("PostmanRuntime/7.43.4", "PostmanRuntime/7.43.4")]
    [InlineData("curl/7.64.1", "curl/7.64.1")]
    [InlineData("Mozilla/5.0 (compatible; MojeekBot/0.11; +mojeek.com/bot.html)", "MojeekBot")]
    public async Task TestAsync(string ua, string device)
    {
        var clientInfo = await GetWebClientInfoAsync(ua);
        clientInfo.ShouldNotBeNull();
        clientInfo.BrowserInfo.ShouldBe(ua);
        clientInfo.DeviceInfo.ShouldBe(device);
    }

    private async Task<WebClientInfoProviderDto> GetWebClientInfoAsync(string userAgent )
    {
        using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "api/web-client-info"))
        {
            requestMessage.Headers.Add("User-Agent", userAgent);
            var response = await Client.SendAsync(requestMessage);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            return JsonSerializer.Deserialize<WebClientInfoProviderDto>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions.Web);
        }
    }
}
