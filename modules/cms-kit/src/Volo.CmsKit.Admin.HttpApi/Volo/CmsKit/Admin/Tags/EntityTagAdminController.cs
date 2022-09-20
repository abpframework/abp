using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Tags;

[RequiresGlobalFeature(typeof(TagsFeature))]
[RemoteService(Name = CmsKitAdminRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitAdminRemoteServiceConsts.ModuleName)]
[Route("api/cms-kit-admin/entity-tags")]
public class EntityTagAdminController : CmsKitAdminController, IEntityTagAdminAppService
{
    protected IEntityTagAdminAppService EntityTagAdminAppService { get; }

    public EntityTagAdminController(IEntityTagAdminAppService entityTagAdminAppService)
    {
        EntityTagAdminAppService = entityTagAdminAppService;
    }

    [HttpPost]
    public Task AddTagToEntityAsync(EntityTagCreateDto input)
    {
        return EntityTagAdminAppService.AddTagToEntityAsync(input);
    }

    [HttpDelete]
    public Task RemoveTagFromEntityAsync(EntityTagRemoveDto input)
    {
        return EntityTagAdminAppService.RemoveTagFromEntityAsync(input);
    }

    [HttpPut]
    public Task SetEntityTagsAsync(EntityTagSetDto input)
    {
        return EntityTagAdminAppService.SetEntityTagsAsync(input);
    }
}
