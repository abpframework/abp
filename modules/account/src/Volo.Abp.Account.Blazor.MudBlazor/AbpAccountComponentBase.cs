using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Volo.Abp.Account.Blazor.MudBlazor;

public abstract class AbpAccountComponentBase : AbpComponentBase
{
    protected AbpAccountComponentBase()
    {
        LocalizationResource = typeof(AccountResource);
        ObjectMapperContext = typeof(AbpAccountBlazorMudBlazorModule);
    }
}
