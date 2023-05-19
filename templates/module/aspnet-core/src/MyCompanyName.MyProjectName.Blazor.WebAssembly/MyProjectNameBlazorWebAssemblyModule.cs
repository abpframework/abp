using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName.Blazor.WebAssembly;

[DependsOn(
    typeof(MyProjectNameBlazorModule),
    typeof(MyProjectNameHttpApiClientModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule)
    )]
public class MyProjectNameBlazorWebAssemblyModule : AbpModule
{

}
