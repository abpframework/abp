namespace Volo.Abp.AspNetCore.Components.Web;

public class AbpAspNetCoreComponentsWebOptions
{
    public bool IsBlazorWebApp { get; set; }

    public AbpAspNetCoreComponentsWebOptions()
    {
        IsBlazorWebApp = false;
    }
}
