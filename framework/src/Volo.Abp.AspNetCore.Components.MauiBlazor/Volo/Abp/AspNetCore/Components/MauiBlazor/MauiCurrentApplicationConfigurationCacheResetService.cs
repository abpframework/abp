using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Configuration;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor;

[Dependency(ReplaceServices = true)]
public class MauiCurrentApplicationConfigurationCacheResetService :
    ICurrentApplicationConfigurationCacheResetService,
    ITransientDependency
{
    private readonly MauiBlazorCachedApplicationConfigurationClient _mauiBlazorCachedApplicationConfigurationClient;

    public MauiCurrentApplicationConfigurationCacheResetService(MauiBlazorCachedApplicationConfigurationClient mauiBlazorCachedApplicationConfigurationClient)
    {
        _mauiBlazorCachedApplicationConfigurationClient = mauiBlazorCachedApplicationConfigurationClient;
    }

    public async Task ResetAsync()
    {
        await _mauiBlazorCachedApplicationConfigurationClient.InitializeAsync();
    }
}
