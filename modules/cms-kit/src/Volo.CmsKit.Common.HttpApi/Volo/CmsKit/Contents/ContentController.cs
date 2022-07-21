using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Contents;

[RequiresGlobalFeature(typeof(PagesFeature))]
[RemoteService(Name = CmsKitCommonRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitCommonRemoteServiceConsts.ModuleName)]
[Route("api/cms-kit/contents")]
public class ContentController : CmsKitControllerBase, IContentAppService
{
    protected IContentAppService ContentAppService { get; }

    public ContentController(IContentAppService contentAppService)
    {
        ContentAppService = contentAppService;
    }

    [HttpGet]
    [Route("{parse}")]
    public async Task<List<ContentFragment>> ParseAsync(string content)
    {
        return await ContentAppService.ParseAsync(content);
    }
}
