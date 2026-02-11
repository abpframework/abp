using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.AspNetCore.Components;

namespace MyCompanyName.MyProjectName.Blazor.WebApp;

public abstract class MyProjectNameComponentBase : AbpComponentBase
{
    protected MyProjectNameComponentBase()
    {
        LocalizationResource = typeof(MyProjectNameResource);
    }
}
