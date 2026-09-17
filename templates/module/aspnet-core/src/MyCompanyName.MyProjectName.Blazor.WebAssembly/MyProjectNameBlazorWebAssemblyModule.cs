using Volo.Abp.AspNetCore.Components.WebAssembly.Theming.MudBlazor;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName.Blazor.WebAssembly;

[DependsOn(
    typeof(MyProjectNameBlazorModule),
    typeof(MyProjectNameHttpApiClientModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingMudBlazorModule)
    )]
public class MyProjectNameBlazorWebAssemblyModule : AbpModule
{

}
