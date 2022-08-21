using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Contents;

[RemoteService(Name = CmsKitAdminRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitAdminRemoteServiceConsts.ModuleName)]
[Route("api/cms-kit-admin/contents")]
public class ContentAdminController : CmsKitAdminController, IContentAdminAppService
{
    protected IContentAdminAppService ContentAdminAppService { get; }

    public ContentAdminController(IContentAdminAppService contentAdminAppService)
    {
        ContentAdminAppService = contentAdminAppService;
    }

    [HttpGet]
    public virtual Task<ListResultDto<ContentWidgetDto>> GetWidgetsAsync()
    {
        return ContentAdminAppService.GetWidgetsAsync();
    }
}
