using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Volo.CmsKit.GlobalResources;

public class GlobalResourceManager : DomainService
{
    private readonly IGlobalResourceRepository _globalResourceRepository;

    public GlobalResourceManager(IGlobalResourceRepository globalResourceRepository)
    {
        _globalResourceRepository = globalResourceRepository;
    }

    public virtual async Task<GlobalResource> SetGlobalStyleAsync(string value)
    {
        return await SetGlobalResourceAsync(GlobalResourceConsts.GlobalStyleName, value);
    }

    public virtual async Task<GlobalResource> SetGlobalScriptAsync(string value)
    {
        return await SetGlobalResourceAsync(GlobalResourceConsts.GlobalScriptName, value);
    }

    protected virtual async Task<GlobalResource> SetGlobalResourceAsync(string name, string value)
    {
        var resource = await _globalResourceRepository.FindByName(name);

        if (resource == null)
        {
            return await _globalResourceRepository.InsertAsync(
                new GlobalResource(GuidGenerator.Create(), name, value, CurrentTenant.Id)
            );
        }

        resource.SetValue(value);

        return await _globalResourceRepository.UpdateAsync(resource);
    }
}