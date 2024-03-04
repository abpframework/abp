namespace Volo.Abp.AspNetCore.Components.Server;

public class AbpAspNetCoreComponentsWebOptions
{
    public bool IsBlazorWebApp { get; set; }

    public AbpAspNetCoreComponentsWebOptions()
    {
        IsBlazorWebApp = false;
    }
}
