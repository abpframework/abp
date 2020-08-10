namespace Volo.CmsKit.Public
{
    public abstract class CmsKitPublicAppService : CmsKitAppService
    {
        protected CmsKitPublicAppService()
        {
            ObjectMapperContext = typeof(CmsKitPublicApplicationModule);
        }
    }
}
