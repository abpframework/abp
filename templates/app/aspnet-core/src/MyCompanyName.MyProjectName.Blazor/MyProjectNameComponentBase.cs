using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.AspNetCore.Components;

namespace MyCompanyName.MyProjectName.Blazor
{
    public class MyProjectNameComponentBase : AbpComponentBase
    {
        public MyProjectNameComponentBase()
        {
            LocalizationResource = typeof(MyProjectNameResource);
        }
    }
}
