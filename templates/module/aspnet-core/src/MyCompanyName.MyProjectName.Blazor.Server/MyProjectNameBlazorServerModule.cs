using Volo.Abp.AspNetCore.Components.Server.Theming.MudBlazor;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName.Blazor.Server;

[DependsOn(
    typeof(AbpAspNetCoreComponentsServerThemingMudBlazorModule),
    typeof(MyProjectNameBlazorModule)
    )]
public class MyProjectNameBlazorServerModule : AbpModule
{

}
