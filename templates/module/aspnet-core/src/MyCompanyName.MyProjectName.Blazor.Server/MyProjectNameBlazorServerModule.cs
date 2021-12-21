using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName.Blazor.Server;

[DependsOn(
    typeof(AbpAspNetCoreComponentsServerThemingModule),
    typeof(MyProjectNameBlazorModule)
    )]
public class MyProjectNameBlazorServerModule : AbpModule
{

}
