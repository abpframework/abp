using System.Threading.Tasks;
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

    public virtual async Task<GlobalResource> GetGlobalStyleAsync()
    {
        return await GetOrCreateResourceAsync(GlobalResourceConsts.GlobalStyleName);
    }

    public virtual async Task<GlobalResource> GetGlobalScriptAsync()
    {
        return await GetOrCreateResourceAsync(GlobalResourceConsts.GlobalScriptName);
    }

    protected virtual async Task<GlobalResource> SetGlobalResourceAsync(string name, string value)
    {
        var resource = await _globalResourceRepository.FindByNameAsync(name);

        if (resource == null)
        {
            return await CreateResourceAsync(name, value);
        }

        resource.SetValue(value);

        await _globalResourceRepository.UpdateAsync(resource);

        return resource;
    }

    protected virtual async Task<GlobalResource> GetOrCreateResourceAsync(string name)
    {
        var resource = await _globalResourceRepository.FindByNameAsync(name);
        
        if (resource == null)
        {
            return await CreateResourceAsync(name, string.Empty);
        }

        return resource;
    }

    protected virtual  async Task<GlobalResource> CreateResourceAsync(string name, string value)
    {
        return await _globalResourceRepository.InsertAsync(
            new GlobalResource(GuidGenerator.Create(), name, value, CurrentTenant.Id)
        );
    }
}