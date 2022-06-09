using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.CmsKit.Polls;

namespace Volo.CmsKit.Public.Polls;

[RemoteService(Name = CmsKitPublicRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitPublicRemoteServiceConsts.ModuleName)]
[Route("api/cms-kit-public/polls")]
public class PollViewComponentController : CmsKitPublicControllerBase, IPollViewComponentAppService
{
    protected IPollViewComponentAppService PollViewComponentAppService { get; }

    public PollViewComponentController(IPollViewComponentAppService pollViewComponentAppService)
    {
        PollViewComponentAppService = pollViewComponentAppService;
    }

    [HttpGet]
    [Route("{content}")]
    public virtual Task<List<ContentFragment>> ParseAsync(string content)
    {
        return PollViewComponentAppService.ParseAsync(content);
    }
}
