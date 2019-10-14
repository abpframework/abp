using Volo.Abp.Application.Services;
using Volo.Docs.Localization;

namespace Volo.Docs
{
    public abstract class DocsAppServiceBase : ApplicationService
    {
        protected DocsAppServiceBase()
        {
            ObjectMapperContext = typeof(DocsApplicationModule);
            LocalizationResource = typeof(DocsResource);
        }
    }
}