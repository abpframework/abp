using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Volo.Abp.Account.Blazor
{
    public class AbpAccountComponentBase : AbpComponentBase
    {
        public AbpAccountComponentBase()
        {
            LocalizationResource = typeof(AccountResource);
            ObjectMapperContext = typeof(AbpAccountBlazorModule);
        }
    }
}
