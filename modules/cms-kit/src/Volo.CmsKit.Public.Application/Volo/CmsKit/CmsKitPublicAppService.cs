namespace Volo.CmsKit
{
    public abstract class CmsKitPublicAppService : CmsKitAppService
    {
        protected CmsKitPublicAppService()
        {
            ObjectMapperContext = typeof(CmsKitPublicApplicationModule);
        }
    }
}
