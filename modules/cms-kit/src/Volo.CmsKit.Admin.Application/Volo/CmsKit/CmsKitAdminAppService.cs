namespace Volo.CmsKit
{
    public abstract class CmsKitAdminAppService : CmsKitAppService
    {
        protected CmsKitAdminAppService()
        {
            ObjectMapperContext = typeof(CmsKitAdminApplicationModule);
        }
    }
}
