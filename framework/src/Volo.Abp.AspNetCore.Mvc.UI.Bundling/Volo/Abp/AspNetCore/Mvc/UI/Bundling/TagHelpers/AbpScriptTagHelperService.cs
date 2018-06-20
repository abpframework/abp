namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public class AbpScriptTagHelperService : AbpBundleItemTagHelperService<AbpScriptTagHelper>
    {
        public AbpScriptTagHelperService(AbpTagHelperScriptService resourceService)
            : base(resourceService)
        {
        }
    }
}