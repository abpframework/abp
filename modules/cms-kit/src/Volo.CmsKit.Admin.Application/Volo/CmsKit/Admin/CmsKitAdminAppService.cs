namespace Volo.CmsKit.Admin
{
    public abstract class CmsKitAdminAppService : CmsKitAppService
    {
        protected CmsKitAdminAppService()
        {
            ObjectMapperContext = typeof(CmsKitAdminApplicationModule);
        }
    }
}
