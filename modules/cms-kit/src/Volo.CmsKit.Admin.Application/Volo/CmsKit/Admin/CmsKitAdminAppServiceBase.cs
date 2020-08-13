namespace Volo.CmsKit.Admin
{
    public abstract class CmsKitAdminAppServiceBase : CmsKitAppServiceBase
    {
        protected CmsKitAdminAppServiceBase()
        {
            ObjectMapperContext = typeof(CmsKitAdminApplicationModule);
        }
    }
}
