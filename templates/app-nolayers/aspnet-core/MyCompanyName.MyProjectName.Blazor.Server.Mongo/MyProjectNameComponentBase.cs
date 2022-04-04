using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.AspNetCore.Components;

namespace MyCompanyName.MyProjectName;

public abstract class MyProjectNameComponentBase : AbpComponentBase
{
    protected MyProjectNameComponentBase()
    {
        LocalizationResource = typeof(MyProjectNameResource);
    }
}
