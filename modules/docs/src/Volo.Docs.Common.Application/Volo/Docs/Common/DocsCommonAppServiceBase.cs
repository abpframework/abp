using Volo.Abp.Application.Services;
using Volo.Docs.Localization;

namespace Volo.Docs.Common
{
    public abstract class DocsCommonAppServiceBase : ApplicationService
    {
        protected DocsCommonAppServiceBase()
        {
            ObjectMapperContext = typeof(DocsCommonApplicationModule);
            LocalizationResource = typeof(DocsResource);
        }
    }
}