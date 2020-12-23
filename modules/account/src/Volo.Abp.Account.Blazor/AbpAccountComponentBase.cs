using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Volo.Abp.Account.Blazor
{
    public abstract class AbpAccountComponentBase : AbpComponentBase
    {
        protected AbpAccountComponentBase()
        {
            LocalizationResource = typeof(AccountResource);
            ObjectMapperContext = typeof(AbpAccountBlazorModule);
        }
    }
}
