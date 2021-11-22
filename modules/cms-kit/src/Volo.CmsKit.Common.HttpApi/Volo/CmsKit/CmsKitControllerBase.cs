using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit;

public abstract class CmsKitControllerBase : AbpControllerBase
{
    protected CmsKitControllerBase()
    {
        LocalizationResource = typeof(CmsKitResource);
    }
}
