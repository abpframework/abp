using MyCompanyName.MyProjectName.Blazor.WebAssembly.Shared.Localization;
using Volo.Abp.AspNetCore.Components;

namespace MyCompanyName.MyProjectName.Blazor.WebAssembly.Client;

public abstract class MyProjectNameComponentBase : AbpComponentBase
{
    protected MyProjectNameComponentBase()
    {
        LocalizationResource = typeof(MyProjectNameResource);
    }
}
