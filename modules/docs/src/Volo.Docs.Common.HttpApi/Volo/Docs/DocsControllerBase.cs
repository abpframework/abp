using Volo.Abp.AspNetCore.Mvc;
using Volo.Docs.Localization;

namespace Volo.Docs;

public abstract class DocsControllerBase : AbpControllerBase
{
    protected DocsControllerBase()
    {
        LocalizationResource = typeof(DocsResource);
    }
}
