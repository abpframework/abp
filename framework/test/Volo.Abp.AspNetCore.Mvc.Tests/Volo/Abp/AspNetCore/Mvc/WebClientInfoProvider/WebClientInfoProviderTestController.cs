using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.WebClientInfo;

namespace Volo.Abp.AspNetCore.Mvc.WebClientInfoProvider;

[Route("api/web-client-info")]
public class WebClientInfoProviderTestController : AbpController
{
    private IWebClientInfoProvider _webClientInfoProvider { get; }

    public WebClientInfoProviderTestController(IWebClientInfoProvider webClientInfoProvider)
    {
        _webClientInfoProvider = webClientInfoProvider;
    }

    [HttpGet]
    public Task<WebClientInfoProviderDto> GetAsync()
    {
        return Task.FromResult(new WebClientInfoProviderDto
        {
            BrowserInfo = _webClientInfoProvider.BrowserInfo,
            DeviceInfo = _webClientInfoProvider.DeviceInfo
        });
    }
}
