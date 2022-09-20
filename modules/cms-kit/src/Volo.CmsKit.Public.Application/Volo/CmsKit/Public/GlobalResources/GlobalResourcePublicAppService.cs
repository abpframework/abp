using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.GlobalResources;

namespace Volo.CmsKit.Public.GlobalResources;

[RequiresGlobalFeature(typeof(GlobalResourcesFeature))]
public class GlobalResourcePublicAppService : ApplicationService, IGlobalResourcePublicAppService
{
    public GlobalResourceManager GlobalResourceManager { get; }

    public GlobalResourcePublicAppService(GlobalResourceManager globalResourceManager)
    {
        GlobalResourceManager = globalResourceManager;
    }
    
    public async Task<GlobalResourceDto> GetGlobalScriptAsync()
    {
        var globalScript = await GlobalResourceManager.GetGlobalScriptAsync();
        
        return ObjectMapper.Map<GlobalResource, GlobalResourceDto>(globalScript);
    }
    
    public async Task<GlobalResourceDto> GetGlobalStyleAsync()
    {
        var globalStyle = await GlobalResourceManager.GetGlobalStyleAsync();
        
        return ObjectMapper.Map<GlobalResource, GlobalResourceDto>(globalStyle);
    }
}